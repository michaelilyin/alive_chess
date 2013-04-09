using System.Configuration;

namespace AliveChessServer.LogicLayer
{
    public class ConfigHandler : ConfigurationSection
    {
        [ConfigurationProperty("Network")]
        public ConfigElement Network
        {
            get { return (ConfigElement) base["Network"]; }
            set { base["Network"] = value; }
        }

        [ConfigurationProperty("Logic")]
        public ConfigElement Logic
        {
            get { return (ConfigElement)base["Logic"]; }
            set { base["Logic"] = value; }
        }

        [ConfigurationProperty("Executing")]
        public ConfigElement Executing
        {
            get { return (ConfigElement)base["Executing"]; }
            set { base["Executing"] = value; }
        }

        [ConfigurationProperty("AI")]
        public ConfigElement AI
        {
            get { return (ConfigElement)base["AI"]; }
            set { base["AI"] = value; }
        }
    }
}
