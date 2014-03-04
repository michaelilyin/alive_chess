using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Properties.ConcreateProperties;
using System.IO;

namespace Properties
{
    public class Properties
    {
        public const String USER_DATA = "Alive Chess";
        public const String LOG = "Logs";
        public const String NETWORK_PROPERTIES = "Network properties.ini";

        private static NetworkProperties _networkProperties;
        public static NetworkProperties NetworkProperties 
        { 
            get 
            { 
                if (_networkProperties == null)
                {
                    String userProfilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    userProfilePath = Path.Combine(userProfilePath, USER_DATA);
                    if (!Directory.Exists(userProfilePath)) Directory.CreateDirectory(userProfilePath);
                    String propFile = Path.Combine(userProfilePath, NETWORK_PROPERTIES);
                    if (!File.Exists(propFile))
                    {
                        _networkProperties = new NetworkProperties("127.0.0.1", "22000");
                        FileProvider.SaveFile(propFile, _networkProperties.ToMap());
                    } 
                    else
                    {
                        _networkProperties = new NetworkProperties(FileProvider.ReadFile(propFile));
                    }
                }
                return _networkProperties;
            } 
        }
    }
}
