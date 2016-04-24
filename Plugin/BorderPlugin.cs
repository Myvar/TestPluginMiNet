using MiNET;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPlugin
{
    [Plugin]
    public class BorderPlugin : Plugin
    {
        public int Boder = 10;

        protected override void OnEnable()
        {
            Console.WriteLine("Boder Plugin Loaded");
            base.OnEnable();
        }

        [PacketHandler, Receive]
        public Package HandleIncomingMove(McpeMovePlayer packet, Player player)
        {            
            var spawn = player.Level.SpawnPoint;
            if(packet.x <= spawn.X + Boder && packet.x >= spawn.X - Boder)
            {
                if (packet.z <= spawn.Z + Boder && packet.z >= spawn.Z - Boder)
                {
                    return packet;
                }
            }
            
           
                player.SetPosition(new PlayerLocation(
                    player.KnownPosition.X  - (packet.x - player.KnownPosition.X),
                    player.KnownPosition.Y, 
                    player.KnownPosition.Z - (packet.z - player.KnownPosition.Z)) {
                    HeadYaw = player.KnownPosition.HeadYaw,
                    Pitch = player.KnownPosition.Pitch,
                    Yaw = player.KnownPosition.Yaw
                });
           

            player.SendMessage("Out of bounce");
            return packet;
        }
    }
}
