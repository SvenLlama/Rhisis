using Rhisis.Core.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Game.Features.AttackArbiters.Participants
{
    public interface IBattleParticipant
    {
        Range<int> CalculateMeleeAttackRange();
        bool HasOneKillMode();
        int CalculateHitRating();
        int CalculateEscapeRating();
        int GetLevel();
        float GetHitRatingNumeratorMultiplier();
        float GetHitRatingDenominatorMultiplier();
        float GetHitRatingLevelNumeratorMultiplier();
        float GetHitRatingLevelDenominatorMultiplier();
        int GetMinHitRating();
        int GetMaxHitRating();
        float GetMagicAttackDamageMultiplier(int magicPower);
        Range<int> CalculateMagicAttackRange();
        int GetBonusMagicAttackDamage();
    }
}
