using Rhisis.Game.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Game.Features.AttackArbiters.Participants
{
    public class BattleParticipantFactory
    {
        public IBattleParticipant Create(IMover mover)
        {
            if( mover == null)
            {
                throw new ArgumentNullException(nameof(mover));
            }

            if (mover is IPlayer player)
            {
                return new PlayerBattleParticipant(player);
            }
            else if (mover is IMonster monster)
            {
                return new MonsterBattleParticipant(monster);
            }
            else
            {
                throw new NotSupportedException($"the {nameof(BattleParticipantFactory)} does not support creating a combat participant from the type {mover.GetType().FullName}");
            }
        }
    }
}
