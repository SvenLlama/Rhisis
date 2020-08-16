﻿using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Attributes;
using Rhisis.World.Systems.Battle;
using Rhisis.World.Systems.Battle.Arbiters;
using Rhisis.World.Systems.Buff;
using Rhisis.World.Systems.Projectile;
using Rhisis.World.Systems.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Skills
{
    [Injectable]
    internal class SkillSystem : ISkillSystem
    {
        /// <summary>
        /// Skill points usage based on the job type.
        /// </summary>
        private static readonly IReadOnlyDictionary<DefineJob.JobType, int> SkillPointUsage = new Dictionary<DefineJob.JobType, int>
        {
            { DefineJob.JobType.JTYPE_BASE, 1 },
            { DefineJob.JobType.JTYPE_EXPERT, 2 },
            { DefineJob.JobType.JTYPE_PRO, 3 },
            { DefineJob.JobType.JTYPE_MASTER, 3 },
            { DefineJob.JobType.JTYPE_HERO, 3 }
        };

        private readonly ILogger<SkillSystem> _logger;
        private readonly IGameResources _gameResources;
        private readonly IBattleSystem _battleSystem;
        private readonly IAttributeSystem _attributeSystem;
        private readonly IBuffSystem _buffSystem;
        private readonly IStatisticsSystem _statisticsSystem;
        private readonly ISkillPacketFactory _skillPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;
        private readonly IPlayerPacketFactory _playerPacketFactory;

        public SkillSystem(ILogger<SkillSystem> logger, IGameResources gameResources,
            IBattleSystem battleSystem, IAttributeSystem attributeSystem, IBuffSystem buffSystem, IStatisticsSystem statisticsSystem,
            ISkillPacketFactory skillPacketFactory, ITextPacketFactory textPacketFactory, IPlayerPacketFactory playerPacketFactory)
        {
            _logger = logger;
            _gameResources = gameResources;
            _battleSystem = battleSystem;
            _attributeSystem = attributeSystem;
            _buffSystem = buffSystem;
            _statisticsSystem = statisticsSystem;
            _skillPacketFactory = skillPacketFactory;
            _textPacketFactory = textPacketFactory;
            _playerPacketFactory = playerPacketFactory;
        }

        public IEnumerable<Skill> GetSkillsByJob(DefineJob.Job job)
        {
            var skillsList = new List<Skill>();

            if (_gameResources.Jobs.TryGetValue(job, out JobData jobData) && jobData.Parent != null)
            {
                skillsList.AddRange(GetSkillsByJob(jobData.Parent.Id));
            }

            IEnumerable<Skill> jobSkills = from x in _gameResources.Skills.Values
                                           where x.Job == jobData.Id &&
                                                 x.JobType != DefineJob.JobType.JTYPE_COMMON &&
                                                 x.JobType != DefineJob.JobType.JTYPE_TROUPE
                                           select new Skill(x.Id, -1, x);

            skillsList.AddRange(jobSkills);

            return skillsList;
        }

        /// <inheritdoc />
        public bool UpdateSkills(IPlayerEntity player, IReadOnlyDictionary<int, int> skillsToUpdate)
        {
            int requiredSkillPoints = 0;

            foreach (KeyValuePair<int, int> skill in skillsToUpdate)
            {
                int skillId = skill.Key;
                int skillLevel = skill.Value;
                Skill playerSkill = player.SkillTree.GetSkill(skillId);

                if (playerSkill == null)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot find skill with id '{skillId}' for player '{player}'.");
                    return false;
                }

                if (playerSkill.Level == skillLevel)
                {
                    // Skill hasn't change
                    continue;
                }

                if (player.Object.Level < playerSkill.Data.RequiredLevel)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Player need to be level '{playerSkill.Data.RequiredLevel}' to learn this skill.");
                    return false;
                }

                if (!CheckRequiredSkill(playerSkill.Data.RequiredSkillId1, playerSkill.Data.RequiredSkillLevel1, skillsToUpdate))
                {
                    SkillData requiredSkill1 = _gameResources.Skills[playerSkill.Data.RequiredSkillId1];

                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Skill '{requiredSkill1.Name}' must be at least Lv.{requiredSkill1.RequiredSkillLevel1}");
                    return false;
                }

                if (!CheckRequiredSkill(playerSkill.Data.RequiredSkillId2, playerSkill.Data.RequiredSkillLevel2, skillsToUpdate))
                {
                    SkillData requiredSkill2 = _gameResources.Skills[playerSkill.Data.RequiredSkillId2];

                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Skill '{requiredSkill2.Name}' must be at least Lv.{requiredSkill2.RequiredSkillLevel1}");
                    return false;
                }

                if (skillLevel < 0 || skillLevel < playerSkill.Level || skillLevel > playerSkill.Data.MaxLevel)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. The skill level is out of bounds.");
                    return false;
                }

                if (!SkillPointUsage.TryGetValue(playerSkill.Data.JobType, out int requiredSkillPointAmount))
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Cannot find required skill point for job type '{playerSkill.Data.JobType}'.");
                    return false;
                }

                requiredSkillPoints += (skillLevel - playerSkill.Level) * requiredSkillPointAmount;
            }

            if (player.SkillTree.SkillPoints < requiredSkillPoints)
            {
                _logger.LogError($"Cannot update skills for player '{player}'. Not enough skill points.");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                return false;
            }

            player.SkillTree.SkillPoints -= (ushort)requiredSkillPoints;

            foreach (KeyValuePair<int, int> skill in skillsToUpdate)
            {
                int skillId = skill.Key;
                int skillLevel = skill.Value;

                player.SkillTree.SetSkillLevel(skillId, skillLevel);
            }

            _skillPacketFactory.SendSkillTreeUpdate(player);

            return true;
        }

        /// <inheritdoc />
        public void UseSkill(IPlayerEntity player, Skill skill, uint targetObjectId, SkillUseType skillUseType)
        {
            if (skill == null)
            {
                _skillPacketFactory.SendSkillCancellation(player);
                throw new ArgumentNullException(nameof(skill), $"Cannot use undefined skill for player {player} on target {targetObjectId}.");
            }

            ILivingEntity target;

            if (player.Id == targetObjectId)
            {
                target = player;
            }
            else
            {
                target = player.FindEntity<ILivingEntity>(targetObjectId);

                if (target == null)
                {
                    _skillPacketFactory.SendSkillCancellation(player);
                    throw new ArgumentNullException(nameof(target), $"Failed to find target with id: {targetObjectId}");
                }
            }

            if (CanUseSkill(player, target, skill))
            {
                int castingTime = GetSkillCastingTime(player, skill);
                bool castSkillResult = skill.Data.ExecuteTarget switch
                {
                    SkillExecuteTargetType.MeleeAttack => _battleSystem.MeleeSkillAttack(player, target, skill, castingTime),
                    SkillExecuteTargetType.MagicAttack => _battleSystem.MagicSkillAttack(player, target, skill, castingTime),
                    SkillExecuteTargetType.MagicAttackShot => _battleSystem.MagicSkillShotAttack(player, target, skill, castingTime),
                    SkillExecuteTargetType.AnotherWith => CastBuffSkill(player, target, skill, castingTime),
                    _ => false
                };

                if (castSkillResult)
                {
                    _skillPacketFactory.SendUseSkill(player, target, skill, castingTime, skillUseType);
                }
                else
                {
                    _logger.LogWarning($"Failed to cast skill {skill.Name} with target {skill.Data.ExecuteTarget} for {player}.");
                    _skillPacketFactory.SendSkillCancellation(player);
                }
            }
            else
            {
                _logger.LogWarning($"{player} cannot use {skill.Data.Name}.");
                _skillPacketFactory.SendSkillCancellation(player);
            }
        }

        /// <inheritdoc />
        public bool CanUseSkill(IPlayerEntity player, ILivingEntity target, Skill skill)
        {
            if (skill.Level <= 0 || skill.Level > skill.Data.SkillLevels.Count)
            {
                _logger.LogWarning($"{player} cannot use skill: {skill.Name}. Incorrect skill level.");
                return false;
            }

            if (!skill.IsCoolTimeElapsed())
            {
                _logger.LogWarning($"{player} cannot use skill: {skill.Name}. Skill cooltime.");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_SKILLWAITTIME);
                return false;
            }

            if (skill.LevelData.RequiredMP > 0 && player.Attributes[DefineAttributes.MP] < skill.LevelData.RequiredMP)
            {
                _logger.LogWarning($"{player} cannot use skill: {skill.Name}. Not enought MP to cast skill.");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REQMP);
                return false;
            }

            if (skill.LevelData.RequiredFP > 0 && player.Attributes[DefineAttributes.FP] < skill.LevelData.RequiredFP)
            {
                _logger.LogWarning($"{player} cannot use skill: {skill.Name}. Not enought FP to cast skill.");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REQFP);
                return false;
            }

            Item rightWeapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

            if (skill.Data.LinkKind.HasValue)
            {
                bool playerHasCorrectWeapon;
                ItemKind3? rightWeaponKind = rightWeapon?.Data?.ItemKind3;

                switch (skill.Data.LinkKind)
                {
                    case ItemKind3.MAGICBOTH:
                        playerHasCorrectWeapon = !rightWeaponKind.HasValue ||
                            rightWeaponKind != ItemKind3.WAND || rightWeaponKind != ItemKind3.STAFF;
                        break;

                    case ItemKind3.YOBO:
                        playerHasCorrectWeapon = !rightWeaponKind.HasValue ||
                            rightWeaponKind != ItemKind3.YOYO || rightWeaponKind != ItemKind3.BOW;
                        break;

                    case ItemKind3.SHIELD:
                        Item leftWeapon = player.Inventory.GetEquipedItem(ItemPartType.LeftWeapon);

                        playerHasCorrectWeapon = leftWeapon == null || leftWeapon.Data?.ItemKind3 != ItemKind3.SHIELD;
                        break;

                    default:
                        playerHasCorrectWeapon = skill.Data.LinkKind == rightWeaponKind;
                        break;
                }

                if (!playerHasCorrectWeapon)
                {
                    _logger.LogWarning($"{player} cannot use skill: {skill.Name}. Incorrect equiped weapon.");
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_WRONGITEM);
                    return false;
                }
            }

            if (skill.Data.Type == SkillType.Magic)
            {
                BuffSkill buffSkill = target.Buffs.OfType<BuffSkill>().FirstOrDefault(x => x.SkillId == skill.SkillId);

                if (buffSkill != null && buffSkill.SkillLevel > skill.Level)
                {
                    _logger.LogWarning($"{player} cannot use skill: {skill.Name}. Target skill level is higher than caster skill level.");
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_DONOTUSEBUFF);
                    return false;
                }
            }

            if (skill.Data.Handed.HasValue)
            {
                // TODO: handle dual weapons and two handed weapons
                return false;
            }

            if (skill.Data.BulletLinkKind.HasValue)
            {
                Item bulletItem = player.Inventory.GetEquipedItem(ItemPartType.Bullet);

                if (bulletItem.Id == -1 || bulletItem.Data.ItemKind2 != skill.Data.BulletLinkKind)
                {
                    DefineText errorText = skill.Data.LinkKind == ItemKind3.BOW ? DefineText.TID_TIP_NEEDSATTACKITEM : DefineText.TID_TIP_NEEDSKILLITEM;

                    _logger.LogWarning($"{player} cannot use skill: {skill.Name}. Missing or incorrect item bullet type.");
                    _textPacketFactory.SendDefinedText(player, errorText);
                    return false;
                }
            }

            // TODO: more skill checks

            return true;
        }

        public void Reskill(IPlayerEntity player)
        {
            foreach (Skill skill in player.SkillTree.Skills)
            {
                player.SkillTree.SkillPoints += (ushort)(skill.Level * SkillPointUsage[skill.Data.JobType]);
                skill.Level = 0;
            }

            _skillPacketFactory.SendSkillReset(player, player.SkillTree.SkillPoints);
        }

        public void GiveSkillPoints(IPlayerEntity player, ushort skillPoints)
        {
            if (skillPoints > 0)
            {
                player.SkillTree.SkillPoints += skillPoints;
                _playerPacketFactory.SendPlayerExperience(player);
            }
        }

        /// <summary>
        /// Check if the required skill condition matches.
        /// </summary>
        /// <param name="requiredSkillId">Required skill id.</param>
        /// <param name="requiredSkillLevel">Required skill level.</param>
        /// <param name="skillsToUpdate">Dictionary of skills to update.</param>
        /// <returns>True if the requirement matches; false otherwise.</returns>
        private bool CheckRequiredSkill(int requiredSkillId, int requiredSkillLevel, IReadOnlyDictionary<int, int> skillsToUpdate)
        {
            if (requiredSkillId == -1)
            {
                return true;
            }

            if (skillsToUpdate.TryGetValue(requiredSkillId, out int skillLevel))
            {
                return skillLevel >= requiredSkillLevel;
            }

            return false;
        }

        /// <summary>
        /// Gets the skill casting time.
        /// </summary>
        /// <param name="caster">Entity casting the skill.</param>
        /// <param name="skill">Skill to be cast.</param>
        /// <returns>Skill casting time in milliseconds.</returns>
        private int GetSkillCastingTime(ILivingEntity caster, Skill skill)
        {
            if (skill.Data.Type == SkillType.Skill)
            {
                return 1 * 1000;
            }
            else
            {
                int castingTime = (int)((skill.LevelData.CastingTime / 1000f) * (60 / 4));

                castingTime -= castingTime * (caster.Attributes[DefineAttributes.SPELL_RATE] / 100);

                return Math.Max(castingTime, 0);
            }
        }

        /// <summary>
        /// Casts a buff skill on the given target.
        /// </summary>
        /// <param name="caster">Living entity casting the skill.</param>
        /// <param name="target">Target living entity.</param>
        /// <param name="skill">Skill to be casted as projectile.</param>
        /// <param name="castingTime">Skill casting time.</param>
        private bool CastBuffSkill(ILivingEntity caster, ILivingEntity target, Skill skill, int castingTime)
        {
            _logger.LogTrace($"{caster} is buffing {target} with skill {skill}");

            if (target.Type == WorldEntityType.Monster)
            {
                _skillPacketFactory.SendSkillCancellation(caster as IPlayerEntity);
                return false;
            }

            if (skill.LevelData.DestParam1 == DefineAttributes.HP)
            {
                if (skill.LevelData.DestParam2 == DefineAttributes.RECOVERY_EXP)
                {
                    // Resurection skill
                    // TODO: process resurection
                }
                else
                {
                    _logger.LogTrace($"{caster} is healing {target} in {castingTime}...");
                    caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                    {
                        _logger.LogTrace($"{target} healed by {caster} !");
                        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                    });
                }
            }

            if (skill.LevelData.DestParam2 == DefineAttributes.HP)
            {
                caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                {
                    ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam2, skill.LevelData.DestParam2Value);
                });
            }

            var timeBonusValues = new int[]
            {
                skill.Data.ReferTarget1 == SkillReferTargetType.Time ? GetTimeBonus(caster, skill.Data.ReferStat1, skill.Data.ReferValue1, skill.Level) : default,
                skill.Data.ReferTarget2 == SkillReferTargetType.Time ? GetTimeBonus(caster, skill.Data.ReferStat2, skill.Data.ReferValue2, skill.Level) : default
            };

            int buffTime = skill.LevelData.SkillTime + timeBonusValues.Sum();

            if (buffTime > 0)
            {
                var attributes = new Dictionary<DefineAttributes, int>();

                if (skill.LevelData.DestParam1 > 0)
                {
                    attributes.Add(skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                }
                if (skill.LevelData.DestParam2 > 0)
                {
                    attributes.Add(skill.LevelData.DestParam2, skill.LevelData.DestParam2Value);
                }

                caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                {
                    var buff = new BuffSkill(skill.SkillId, skill.Level, buffTime, attributes);
                    bool isBuffAdded = _buffSystem.AddBuff(target, buff);

                    if (isBuffAdded)
                    {
                        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam2, skill.LevelData.DestParam2Value);

                        _skillPacketFactory.SendSkillState(target, buff.SkillId, buff.SkillLevel, buff.RemainingTime);
                    }
                });
            }

            skill.SetCoolTime(skill.LevelData.CooldownTime);

            return true;
        }

        public int GetTimeBonus(ILivingEntity entity, DefineAttributes attribute, int value, int skillLevel)
            => GetReferBonus(entity, attribute, value, skillLevel);

        public int GetReferBonus(ILivingEntity entity, DefineAttributes attribute, int value, int skillLevel)
        {
            int attributeValue = attribute switch
            {
                DefineAttributes.STA => _statisticsSystem.GetTotalStrength(entity),
                DefineAttributes.DEX => _statisticsSystem.GetTotalDexterity(entity),
                DefineAttributes.INT => _statisticsSystem.GetTotalIntelligence(entity),
                _ => 1
            };

            return (int)(((value / 10f) * attributeValue) + (skillLevel * (attributeValue / 50f)));
        }

        private void ApplySkillParameters(ILivingEntity caster, ILivingEntity target, Skill skill, DefineAttributes attribute, int value)
        {
            switch (attribute)
            {
                case DefineAttributes.HP:
                    if (skill.Data.ReferTarget1 == SkillReferTargetType.Heal || skill.Data.ReferTarget2 == SkillReferTargetType.Heal)
                    {
                        var hpValues = new int[]
                        {
                            skill.Data.ReferTarget1 == SkillReferTargetType.Heal ? GetReferBonus(caster, skill.Data.ReferStat1, skill.Data.ReferValue1, skill.Level) : 0,
                            skill.Data.ReferTarget2 == SkillReferTargetType.Heal ? GetReferBonus(caster, skill.Data.ReferStat2, skill.Data.ReferValue2, skill.Level) : 0
                        };

                        int recoveredHp = skill.LevelData.DestParam1Value + hpValues.Sum();

                        if (recoveredHp > 0)
                        {
                            _attributeSystem.IncreaseAttribute(target, attribute, recoveredHp);
                        }
                    }
                    break;
                default:
                    _attributeSystem.IncreaseAttribute(target, attribute, value);
                    break;
            }
        }


    }
}
