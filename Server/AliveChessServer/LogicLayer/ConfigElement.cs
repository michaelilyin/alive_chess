using System.Configuration;

namespace AliveChessServer.LogicLayer
{
    public class ConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("Enabled", IsRequired = true, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)base["Enabled"]; }
            set { base["Enabled"] = value; }
        }
    }
}
