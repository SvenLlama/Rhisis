using Rhisis.Core.Data;
using Rhisis.World.Game.Common;

namespace Rhisis.World.Systems.Battle
{
    public struct AttackDescriptor
    {
        public int Damages { get; }
        
        public AttackFlags Flags { get; }

        public ObjectMessageType ObjectMessageType { get; }

        public bool IgnoreDefense { get; set; }

        public AttackDescriptor(int damages, AttackFlags flags, ObjectMessageType objectMessageType)
            : this(damages, flags, objectMessageType, false)
        {
        }

        public AttackDescriptor(int damages, AttackFlags flags, ObjectMessageType objectMessageType, bool ignoreDefense)
        {
            Damages = damages;
            Flags = flags;
            ObjectMessageType = objectMessageType;
            IgnoreDefense = ignoreDefense;
        }
    }
}
