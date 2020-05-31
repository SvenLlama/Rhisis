﻿using Rhisis.Network.Packets;
using System.Collections.Generic;
using Rhisis.Network;
using Rhisis.Core.IO;
using System.Linq;
using Rhisis.Cluster.Client;
using Rhisis.Cluster.Structures;

namespace Rhisis.Cluster.Packets
{
    /// <summary>
    /// Cluster packet factory.
    /// </summary>
    public class ClusterPacketFactory : IClusterPacketFactory
    {
        /// <inheritdoc />
        public void SendWelcome(IClusterClient client)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(client.SessionId);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendPong(IClusterClient client, int time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PING);
                packet.Write(time);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendQueryTickCount(IClusterClient client, uint time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.QUERYTICKCOUNT);

                packet.Write(time);
                packet.Write(Time.GetElapsedTime());

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendClusterError(IClusterClient client, ErrorType errorType)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write((int)errorType);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendJoinWorld(IClusterClient client)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PRE_JOIN);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendLoginNumPad(IClusterClient client, int loginProtectValue)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.LOGIN_PROTECT_NUMPAD);
                packet.Write(loginProtectValue);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendLoginProtect(IClusterClient client, int loginProtectValue)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.LOGIN_PROTECT_CERT);
                packet.Write(0);
                packet.Write(loginProtectValue);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendPlayerList(IClusterClient client, int authenticationKey, IEnumerable<ClusterCharacter> characters)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PLAYER_LIST);
                packet.Write(authenticationKey);

                packet.Write(characters.Count()); // player count

                foreach (var character in characters)
                {
                    packet.Write(character.Slot);
                    packet.Write(1); // this number represents the selected character in the window
                    packet.Write(character.MapId);
                    packet.Write(0x0B + (byte)character.Gender); // Model id
                    packet.Write(character.Name);
                    packet.Write(character.PositionX);
                    packet.Write(character.PositionY);
                    packet.Write(character.PositionZ);
                    packet.Write(character.Id);
                    packet.Write(0); // Party id
                    packet.Write(0); // Guild id
                    packet.Write(0); // War Id
                    packet.Write(character.SkinSetId);
                    packet.Write(character.HairId);
                    packet.Write(character.HairColor);
                    packet.Write(character.FaceId);
                    packet.Write((byte)character.Gender);
                    packet.Write(character.JobId);
                    packet.Write(character.Level);
                    packet.Write(0); // Job Level (Maybe master or hero ?)
                    packet.Write(character.Strength);
                    packet.Write(character.Stamina);
                    packet.Write(character.Dexterity);
                    packet.Write(character.Intelligence);
                    packet.Write(0); // Mode ??

                    packet.Write(character.EquipedItems.Count());

                    foreach (int equipedItemId in character.EquipedItems)
                    {
                        packet.Write(equipedItemId);
                    }
                }

                packet.Write(0); // Messenger?

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendWorldAddress(IClusterClient client, string address)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.CACHE_ADDR);
                packet.Write(address);

                client.Send(packet);
            }
        }
    }
}
