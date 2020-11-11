using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Game.Features.AttackArbiters.Participants;
using System;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class MagicAttackArbiter : AttackArbiterBase
    {
        private readonly int _magicPower;

        public MagicAttackArbiter(IMover attackerOld, IMover defenderOld, IBattleParticipant attacker, IBattleParticipant defender, int magicPower) 
            : base(attackerOld, defenderOld, attacker, defender)
        {
            _magicPower = magicPower;
        }

        public override AttackResult CalculateDamages()
        {
            var range = Attacker.CalculateMagicAttackRange();

            int damages = RandomHelper.Random(range.Minimum, range.Maximum);// TODO this was weaponAttackResult.Maximum before, instead of range.Maximum, was this intentional of a mistake??

            damages += Attacker.GetBonusMagicAttackDamage();

            damages = (int)(damages * Attacker.GetMagicAttackDamageMultiplier(_magicPower));            

            return AttackResult.Success(damages, AttackFlags.AF_MAGIC);
        }
    }
}
