
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


            foreach (var i in cfg.Messages)
            {
                ThreadPool.QueueUserWorkItem((p) =>
                {
                    while (Run && cfg.Enabled)
                    {
                        foreach (var z in Context.LevelManager.Levels)
                        {
                            z.BroadcastMessage(i.Text);
                        }

                        Thread.Sleep(1000 * i.DelayInSconds);
                    }
                });
            }

            base.OnEnable();
        }

        public override void OnDisable()
        {
            Run = false;
            base.OnDisable();
        }

    }
}
