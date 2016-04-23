using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin
{
    public class Config
    {
        public bool Enabled { get; set; } = true;
        public List<Message> Messages { get; set; } = new List<Message>()
        {
            new Message() { DelayInSconds = 60 * 60 , Text = "Example Message"}
        };
    }

    public class Message
    {
        public string Text { get; set; }
        public int DelayInSconds { get; set; }
    }
}
