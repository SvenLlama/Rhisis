using Rhisis.World.Game.Common;

namespace Rhisis.World.Systems.Battle
{
    public class AttackResult
    {
        public int Damages { get; set; }

        public AttackFlags Flags { get; set; }

        public AttackResult()
            : this(default, AttackFlags.AF_GENERIC)
        {
        }

        public AttackResult(int damages, AttackFlags flags)
        {
            Damages = damages;
            Flags = flags;
        }
    }
}
