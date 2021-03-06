﻿using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Skills
{
    public interface ISkillSystem
    {
        /// <summary>
        /// Gets the skills for a given job.
        /// </summary>
        /// <param name="job">Job.</param>
        /// <returns>Collection of <see cref="Skill"/> for the given job.</returns>
        IEnumerable<Skill> GetSkillsByJob(DefineJob.Job job);

        /// <summary>
        /// Updates the player skill tree.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="skillsToUpdate">Dictionary of skill tree, with skill id as key and skill level as value.</param>
        /// <returns>True if the skills have been updated; false in case of any error.</returns>
        bool UpdateSkills(IPlayerEntity player, IReadOnlyDictionary<int, int> skillsToUpdate);

        /// <summary>
        /// Use a skill.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="skill">Skill to use.</param>
        /// <param name="targetObjectId">Target object id.</param>
        /// <param name="skillUseType">Skill usage type.</param>
        void UseSkill(IPlayerEntity player, Skill skill, uint targetObjectId, SkillUseType skillUseType);

        /// <summary>
        /// Checks if the player can use the skill.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="target">Skill target.</param>
        /// <param name="skill">Skill to use.</param>
        /// <returns>True if the player can use the skill; false otherwise.</returns>
        bool CanUseSkill(IPlayerEntity player, ILivingEntity target, Skill skill);

        /// <summary>
        /// Resets player skills.
        /// </summary>
        /// <param name="player">Current player.</param>
        void Reskill(IPlayerEntity player);

        /// <summary>
        /// Add skill points to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="skillPoints">Skill points amount to give.</param>
        void AddSkillPoints(IPlayerEntity player, ushort skillPoints);
    }
}
