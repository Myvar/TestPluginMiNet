
using MiNET;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
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
    public class TestPlugin : Plugin
    {
        public bool Run { get; set; }
        public Config cfg { get; set; } = new Config();

        protected override void OnEnable()
        {
            Run = true;

            Console.WriteLine("Test Plugin Started");

            if (File.Exists("config.json"))
            {
                cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            }
            else
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(cfg, Formatting.Indented));
            }

            ThreadPool.QueueUserWorkItem((p) =>
            {

                while (Run && cfg.Enabled)
                {
                    for (int i = 0; i < cfg.Messages.Count; i++)
                    {
                        var msg = cfg.Messages[i];

                        if (msg.Counter == msg.DelayInSconds)
                        {
                            foreach (var z in Context.LevelManager.Levels)
                            {
                                z.BroadcastMessage(msg.Text);
                            }
                            msg.Counter = 0;
                        }
                        msg.Counter++;

                    }

                    Thread.Sleep(1000);
                }
            });

            base.OnEnable();
        }

        public override void OnDisable()
        {
            Run = false;
            base.OnDisable();
        }

    }
}
