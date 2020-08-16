using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Attributes
{
    public interface IAttributeSystem
    {
        void IncreaseAttribute(ILivingEntity entity, DefineAttributes attribute, int value, bool sendToEntity = true);

        void DecreaseAttribute(ILivingEntity entity, DefineAttributes attribute, int value, bool sendToEntity = true);
    }
}
