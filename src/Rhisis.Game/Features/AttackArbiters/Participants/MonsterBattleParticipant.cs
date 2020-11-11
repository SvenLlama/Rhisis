using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Game.Features.AttackArbiters.Participants
{
    class MonsterBattleParticipant : IBattleParticipant
    {
        private IMonster _monster;

        public MonsterBattleParticipant(IMonster monster)
        {
            this._monster = monster;
        }

        public Range<int> CalculateMeleeAttackRange()
        {
            var attackRange = new Range<int>(_monster.Data.AttackMin, _monster.Data.AttackMax);
            return attackRange;
        }

        public int CalculateEscapeRating()
        {
            return _monster.Data.EscapeRating;
        }

        public int CalculateHitRating()
        {
            return _monster.Data.HitRating;
        }

        public bool HasOneKillMode()
        {
            return false;
        }

        public int GetLevel()
        {
            return _monster.Level;
        }

        public float GetHitRatingNumeratorMultiplier()
        {
            return 1.5f;
        }

        public float GetHitRatingDenominatorMultiplier()
        {
            return 2.0f;
        }

        public float GetHitRatingLevelNumeratorMultiplier()
        {
            return 0.5f;
        }

        public float GetHitRatingLevelDenominatorMultiplier()
        {
            return 0.3f;
        }

        public int GetMinHitRating()
        {
            return 20;
        }

        public int GetMaxHitRating()
        {
            return 96;
        }

        public float GetMagicAttackDamageMultiplier(int magicPower)
        {
            return 1;
        }

        public Range<int> CalculateMagicAttackRange()
        {
            throw new NotImplementedException();
        }

        public int GetBonusMagicAttackDamage()
        {
            return 0;
        }
    }
}
