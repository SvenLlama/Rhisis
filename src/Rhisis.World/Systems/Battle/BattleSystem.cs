using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Attributes;
using Rhisis.World.Systems.Battle.Arbiters;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.Projectile;
using System;

namespace Rhisis.World.Systems.Battle
{
    [Injectable]
    public class BattleSystem : IBattleSystem
    {
        private readonly ILogger<BattleSystem> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IProjectileSystem _projectileSystem;
        private readonly IInventorySystem _inventorySystem;
        private readonly IAttributeSystem _attributeSystem;
        private readonly IBattlePacketFactory _battlePacketFactory;
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        /// <summary>
        /// Creates a new <see cref="BattleSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="projectileSystem">Projectile system.</param>
        /// <param name="inventorySystem">Inventory system.</param>
        /// <param name="attributeSystem">Attribute system.</param>
        /// <param name="battlePacketFactory">Battle packet factory.</param>
        /// <param name="moverPacketFactory">Mover packet factory.</param>
        public BattleSystem(ILogger<BattleSystem> logger, IServiceProvider serviceProvider,
            IProjectileSystem projectileSystem, IInventorySystem inventorySystem, IAttributeSystem attributeSystem, 
            IBattlePacketFactory battlePacketFactory, IMoverPacketFactory moverPacketFactory, ITextPacketFactory textPacketFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _projectileSystem = projectileSystem;
            _inventorySystem = inventorySystem;
            _attributeSystem = attributeSystem;
            _battlePacketFactory = battlePacketFactory;
            _moverPacketFactory = moverPacketFactory;
            _textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public AttackResult DamageTarget(ILivingEntity attacker, ILivingEntity defender, AttackDescriptor attackDescriptor)
        {
            if (!CanAttack(attacker, defender))
            {
                ClearBattleTargets(defender);
                ClearBattleTargets(attacker);
                return null;
            }

            if (defender is IPlayerEntity undyingPlayer)
            {
                if (undyingPlayer.PlayerData.Mode == ModeType.MATCHLESS_MODE)
                {
                    _logger.LogDebug($"{attacker.Object.Name} wasn't able to inflict any damages to {defender.Object.Name} because he is in undying mode");
                    return null;
                }
            }

            AttackResult attackResult;

            if (attacker is IPlayerEntity player && player.PlayerData.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                attackResult = new AttackResult(defender.Attributes[DefineAttributes.HP], AttackFlags.AF_GENERIC);
            }
            else if (defender is IMonsterEntity monster && monster.Moves.ReturningToOriginalPosition)
            {
                attackResult = new AttackResult(0, AttackFlags.AF_MISS);
            }
            else
            {
                attackResult = new AttackResult(attackDescriptor.Damages, attackDescriptor.Flags);
            }

            if (attackResult.Flags.HasFlag(AttackFlags.AF_MISS) || attackResult.Damages <= 0)
            {
                _battlePacketFactory.SendAddDamage(defender, attacker, attackResult.Flags, attackResult.Damages);

                return attackResult;
            }

            attacker.Battle.Target = defender;
            defender.Battle.Target = attacker;

            if (attackDescriptor.Flags.HasFlag(AttackFlags.AF_FLYING))
            {
                BattleHelper.KnockbackEntity(defender);
            }

            _battlePacketFactory.SendAddDamage(defender, attacker, attackDescriptor.Flags, attackDescriptor.Damages);
            _attributeSystem.DecreaseAttribute(defender, DefineAttributes.HP, attackDescriptor.Damages);
            CheckIfDefenderIsDead(attacker, defender, attackDescriptor.ObjectMessageType);

            return attackResult;
        }

        /// <inheritdoc />
        public void MeleeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, float attackSpeed)
        {
            //MeleeAttackArbiter meleeAttackArbiter = _serviceProvider.CreateInstance<MeleeAttackArbiter>();
            //AttackResult attackResult = meleeAttackArbiter.CalculateDamages(attacker, defender);

            MeleeAttackArbiter meleeAttackArbiter = new MeleeAttackArbiter(attacker, defender);
            AttackResult attackResult = meleeAttackArbiter.CalculateDamages();

            AttackResult meleeAttackResult = DamageTarget(attacker, defender, new AttackDescriptor(attackResult.Damages, attackResult.Flags, attackType));

            if (meleeAttackResult != null)
            {
                _battlePacketFactory.SendMeleeAttack(attacker, attackType, defender.Id, unknwonParam: 0, meleeAttackResult.Flags);
            }
        }

        /// <inheritdoc />
        public void MagicAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, int magicAttackPower, int projectileId)
        {
            var projectile = new MagicProjectile(attacker, defender, magicAttackPower, onArrived: () =>
            {
                MagicAttackArbiter magicAttackArbiter = new MagicAttackArbiter(attacker, defender, magicAttackPower);
                AttackResult attackResult = magicAttackArbiter.CalculateDamages();

                DamageTarget(attacker, defender, new AttackDescriptor(attackResult.Damages, attackResult.Flags, ObjectMessageType.OBJMSG_ATK_MAGIC1));
            });

            _projectileSystem.CreateProjectile(projectileId, projectile);
            _battlePacketFactory.SendMagicAttack(attacker, ObjectMessageType.OBJMSG_ATK_MAGIC1, defender.Id, magicAttackPower, projectileId);
        }

        /// <inheritdoc />
        public void RangeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, int power, int projectileId)
        {
            if (attacker is IPlayerEntity player)
            {
                Item equipedItem = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

                if (equipedItem == null || equipedItem.Data.WeaponType != WeaponType.RANGE_BOW)
                {
                    return;
                }

                InventoryItem bulletItem = player.Inventory.GetEquipedItem(ItemPartType.Bullet);

                if (bulletItem == null || bulletItem.Data.ItemKind3 != ItemKind3.ARROW)
                {
                    return;
                }

                _inventorySystem.DeleteItem(player, bulletItem, 1);
            }

            var projectile = new RangeArrowProjectile(attacker, defender, power, onArrived: () =>
            {
                //MeleeAttackArbiter meleeAttackArbiter = _serviceProvider.CreateInstance<MeleeAttackArbiter>();
                //AttackResult attackResult = meleeAttackArbiter.CalculateDamages(attacker, defender, AttackFlags.AF_GENERIC | AttackFlags.AF_RANGE, power);

                MeleeAttackArbiter meleeAttackArbiter = new MeleeAttackArbiter(attacker, defender, AttackFlags.AF_GENERIC | AttackFlags.AF_RANGE, power);
                AttackResult attackResult = meleeAttackArbiter.CalculateDamages();

                DamageTarget(attacker, defender, new AttackDescriptor(attackResult.Damages, attackResult.Flags, attackType));
            });

            _projectileSystem.CreateProjectile(projectileId, projectile);
            _battlePacketFactory.SendRangeAttack(attacker, ObjectMessageType.OBJMSG_ATK_RANGE1, defender.Id, power, projectileId);
        }

        /// <inheritdoc />
        public bool MeleeSkillAttack(ILivingEntity attacker, ILivingEntity defender, Skill skill, int castingTime)
        {
            if (skill.Data.SpellRegionType == SpellRegionType.Around)
            {
                // TODO: AoE
                _textPacketFactory.SendFeatureNotImplemented(attacker as IPlayerEntity, "AoE skills");

                return false;
            }
            else
            {
                attacker.Delayer.DelayAction(TimeSpan.FromMilliseconds(castingTime), () =>
                {
                    var meleeSkillAttack = new MeleeSkillAttackArbiter(attacker, defender, skill);
                    AttackResult attackResult = meleeSkillAttack.CalculateDamages();

                    DamageTarget(attacker, defender, new AttackDescriptor(attackResult.Damages, attackResult.Flags, ObjectMessageType.OBJMSG_MELEESKILL));
                    skill.SetCoolTime(skill.LevelData.CooldownTime);

                    _attributeSystem.DecreaseAttribute(attacker, DefineAttributes.FP, skill.LevelData.RequiredFP);
                    _attributeSystem.DecreaseAttribute(attacker, DefineAttributes.MP, skill.LevelData.RequiredMP);
                });
            }

            return true;
        }

        /// <inheritdoc />
        public bool MagicSkillAttack(ILivingEntity attacker, ILivingEntity defender, Skill skill, int castingTime)
        {
            if (skill.Data.SpellRegionType == SpellRegionType.Around)
            {
                // TODO: AoE
                _textPacketFactory.SendFeatureNotImplemented(attacker as IPlayerEntity, "AoE skills");

                return false;
            }
            else
            {
                attacker.Delayer.DelayAction(TimeSpan.FromMilliseconds(castingTime), () =>
                {
                    var magicSkillAttack = new MagicSkillAttackArbiter(attacker, defender, skill);
                    AttackResult attackResult = magicSkillAttack.CalculateDamages();

                    DamageTarget(attacker, defender, new AttackDescriptor(attackResult.Damages, attackResult.Flags, ObjectMessageType.OBJMSG_MAGICSKILL));
                    skill.SetCoolTime(skill.LevelData.CooldownTime);

                    _attributeSystem.DecreaseAttribute(attacker, DefineAttributes.FP, skill.LevelData.RequiredFP);
                    _attributeSystem.DecreaseAttribute(attacker, DefineAttributes.MP, skill.LevelData.RequiredMP);
                });
            }

            return true;
        }

        /// <inheritdoc />
        public bool MagicSkillShotAttack(ILivingEntity attacker, ILivingEntity defender, Skill skill, int castingTime)
        {
            return true;
        }

        /// <summary>
        /// Check if the attacker entity can attack a defender entity.
        /// </summary>
        /// <param name="attacker">Attacker living entity.</param>
        /// <param name="defender">Defender living entity.</param>
        /// <returns></returns>
        private bool CanAttack(ILivingEntity attacker, ILivingEntity defender)
        {
            if (attacker == defender)
            {
                _logger.LogError($"{attacker} cannot attack itself.");
                return false;
            }

            if (attacker.IsDead)
            {
                _logger.LogError($"{attacker} cannot attack because its dead.");
                return false;
            }

            if (defender.IsDead)
            {
                _logger.LogError($"{attacker} cannot attack {defender} because target is already dead.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the defender has been killed by the attacker.
        /// </summary>
        /// <param name="attacker">Attacker living entity.</param>
        /// <param name="defender">Defender living entity.</param>
        /// <param name="attackType">Killing attack type.</param>
        private void CheckIfDefenderIsDead(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType)
        {
            if (defender.IsDead)
            {
                _logger.LogDebug($"{attacker.Object.Name} killed {defender.Object.Name}.");
                defender.Attributes[DefineAttributes.HP] = 0;
                _moverPacketFactory.SendUpdatePoints(defender, DefineAttributes.HP, defender.Attributes[DefineAttributes.HP]);
                ClearBattleTargets(defender);
                ClearBattleTargets(attacker);

                if (defender is IMonsterEntity && attacker is IPlayerEntity player)
                {
                    _battlePacketFactory.SendDie(player, defender, attacker, attackType);
                }
                else if (defender is IPlayerEntity && attacker is IPlayerEntity)
                {
                    // TODO: PVP
                }
                else if (defender is IPlayerEntity deadPlayer)
                {
                    _battlePacketFactory.SendDie(deadPlayer, defender, attacker, attackType);
                }

                attacker.Behavior.OnTargetKilled(defender);
                defender.Behavior.OnKilled(attacker);
            }
        }

        /// <summary>
        /// Clears the battle targets.
        /// </summary>
        /// <param name="entity">Current entity.</param>
        private void ClearBattleTargets(ILivingEntity entity)
        {
            entity.Follow.Reset();
            entity.Object.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
            entity.Object.MovingFlags |= ObjectState.OBJSTA_STAND;
            entity.Moves.DestinationPosition.Reset();

            entity.Battle.Reset();
        }
    }
}
