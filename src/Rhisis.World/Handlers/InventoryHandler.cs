using Microsoft.Extensions.Logging;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.PlayerData;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    /// <summary>
    /// Handles all inventory packets.
    /// </summary>
    [Handler]
    public class InventoryHandler
    {
        private readonly ILogger<InventoryHandler> _logger;
        private readonly IInventorySystem _inventorySystem;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="InventoryHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="inventorySystem">Inventory System.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public InventoryHandler(ILogger<InventoryHandler> logger, IInventorySystem inventorySystem, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _inventorySystem = inventorySystem;
            _playerDataSystem = playerDataSystem;
        }

        /// <summary>
        /// Handles the move item request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.MOVEITEM)]
        public void OnMoveItem(IWorldClient client, MoveItemPacket packet)
        {
            _inventorySystem.MoveItem(client.Player, packet.SourceSlot, packet.DestinationSlot);
        }

        /// <summary>
        /// Handles the equip/unequip request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.DOEQUIP)]
        public void OnDoEquip(IWorldClient client, EquipItemPacket packet)
        {
            _inventorySystem.EquipItem(client.Player, packet.UniqueId, packet.Part);
            _playerDataSystem.CalculateDefense(client.Player);
        }

        /// <summary>
        /// Handles the drop item request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.DROPITEM)]
        public void OnDropItem(IWorldClient client, DropItemPacket packet)
        {
            _inventorySystem.DropItem(client.Player, packet.ItemUniqueId, packet.ItemQuantity);
        }

        /// <summary>
        /// Handles the delete item request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.REMOVEINVENITEM)]
        public void OnDeleteItem(IWorldClient client, RemoveInventoryItemPacket packet)
        {
            _inventorySystem.DeleteItem(client.Player, packet.ItemUniqueId, packet.ItemQuantity);
        }

        /// <summary>
        /// Handles the use item request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.DOUSEITEM)]
        public void OnUseItem(IWorldClient client, DoUseItemPacket packet)
        {
            if (!string.IsNullOrWhiteSpace(client.Player.PlayerData.CurrentShopName))
            {
                _logger.LogTrace($"Player {client.Player} tried to use an item while visiting a NPC shop.");
                return;
            }

            _inventorySystem.UseItem(client.Player, packet.UniqueItemId, packet.Part);
            _playerDataSystem.CalculateDefense(client.Player);
        }
    }
}
