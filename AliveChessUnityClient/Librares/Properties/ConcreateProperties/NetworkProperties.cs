using Properties.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Properties.ConcreateProperties
{
    public class NetworkProperties : BaseProperties
    {
        public IPAddress Server { get; private set; }
        public int Port { get; private set; }

        public NetworkProperties(IPAddress address, int port)
        {
            Server = address;
            Port = port;
        }

        public NetworkProperties(String address, String port)
            : this(IPAddress.Parse(address), Int32.Parse(port))
        {
        }

        public override Dictionary<string, string> ToMap()
        {
            Dictionary<String, String> res = new Dictionary<string, string>();
            res["Server"] = Server.ToString();
            res["Port"] = Port.ToString();
            return res;
        }

        public NetworkProperties(Dictionary<String, String> data)
        {
            if (!data.ContainsKey("Server") || !data.ContainsKey("Port")) throw new IllegalPropertiesFormatException();
            Server = IPAddress.Parse(data["Server"]);
            Port = Int32.Parse(data["Port"]);
        }
    }
}
