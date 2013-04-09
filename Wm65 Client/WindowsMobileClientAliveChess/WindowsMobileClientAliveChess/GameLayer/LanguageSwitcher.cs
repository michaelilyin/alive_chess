using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace WindowsMobileClientAliveChess.GameLayer
{
    public static class LanguageSwitcher
    {
        private static XmlDocument _langDoc;

        public static XmlDocument LangDoc
        {
            get { return LanguageSwitcher._langDoc; }
            set { LanguageSwitcher._langDoc = value; }
        }

        public static string getCurrentPath()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            int index = path.LastIndexOf("\\");
            string rez = "";
            for (int i = 0; i < index; i++)
            {
                rez += path[i];
            }
            return rez;
        }

        public static void Initialize()
        {
            StreamReader sr = new StreamReader(getCurrentPath() + "/XML/Preferences.xml");
            XmlReader _xmR = XmlNodeReader.Create(sr);
            sr.Close();
            sr.Dispose();
            while (_xmR.Read())
            {
                if (_xmR.Name == "language" && _xmR.GetAttribute("selected") == "true")
                {
                    _xmR.Read();
                    _langDoc = new XmlDocument();
                    _langDoc.Load(getCurrentPath() + "/XML/" + _xmR.Value + ".xml");
                    break;
                }
            }
        }

        public static WindowsMobileClientAliveChess.GameLayer.Forms.PreferencesForm.Preferences 
            ReadPreferenses(WindowsMobileClientAliveChess.GameLayer.Forms.PreferencesForm.Preferences pref)
        {
            XmlDocument _xmD = new XmlDocument();
            FileStream fs = new FileStream(getCurrentPath() + "/XML/Preferences.xml", FileMode.Open);
            _xmD.Load(fs);
            fs.Close();
            fs.Dispose();
            XmlNodeList list = _xmD.GetElementsByTagName("connection");
            pref.Privilegies= new string[list.Item(0).ChildNodes.Count-4];
            for (int i = 0, j=0; i < list.Item(0).ChildNodes.Count; i++)
            {
                
                XmlNode value = list.Item(0).ChildNodes.Item(i);
                switch (value.Name)
                {
                    case "IP": pref.IP = value.InnerText;
                        break;
                    case "Port": pref.Port = value.InnerText;
                        break;
                    case "Login": pref.Login = value.InnerText;
                        break;
                    case "Password": pref.Password = value.InnerText;
                        break;
                    case "Priviliges": pref.Privilegies[j] = value.InnerText;
                        if (list.Item(0).ChildNodes.Item(i).Attributes.Item(0).Value == "true")
                            pref.priv_selected = i-4;
                        j++;
                        break;
                    default: break;
                }
            }
            list = _xmD.GetElementsByTagName("languages");
            pref.Languages = new string[list.Item(0).ChildNodes.Count];
            for (int i = 0; i < list.Item(0).ChildNodes.Count; i++)
            {
                pref.Languages[i] = list.Item(0).ChildNodes.Item(i).Attributes.Item(0).Value;
                if (list.Item(0).ChildNodes.Item(i).Attributes.Item(1).Value == "true")
                    pref.lang_selected = i;
            }
            return pref;
        }

        public static void 
            SetNewPreferences(WindowsMobileClientAliveChess.GameLayer.Forms.PreferencesForm.Preferences pref)
        {
            XmlDocument _xmD = new XmlDocument();
            FileStream fs = new FileStream(getCurrentPath() + "/XML/Preferences.xml", FileMode.Open);
            _xmD.Load(fs);
            fs.Close();
            fs.Dispose();
            XmlNodeList list = _xmD.GetElementsByTagName("connection");
            for (int i = 0, j = 0; i < list.Item(0).ChildNodes.Count; i++)
            {
                XmlNode value = list.Item(0).ChildNodes.Item(i);
                switch (value.Name)
                {
                    case "IP": value.InnerText=pref.IP;
                        break;
                    case "Port": value.InnerText = pref.Port;
                        break;
                    case "Login": value.InnerText = pref.Login;
                        break;
                    case "Password": value.InnerText = pref.Password;
                        break;
                    case "Priviliges": value.InnerText = pref.Privilegies[j];
                        if (pref.priv_selected == j)
                            value.Attributes.Item(0).Value = "true";
                        else
                            value.Attributes.Item(0).Value = "false";
                        j++;
                        break;
                    default: break;
                }
            }
            list = _xmD.GetElementsByTagName("languages");
            for (int i = 0; i < list.Item(0).ChildNodes.Count; i++)
            {
                if (i == pref.lang_selected)
                    list.Item(0).ChildNodes.Item(i).Attributes.Item(1).Value = "true";
                else
                    list.Item(0).ChildNodes.Item(i).Attributes.Item(1).Value = "false";
            }
            _xmD.Save(getCurrentPath() + "/XML/Preferences.xml");
        }

        public static string GetElementName(Type parentForm, string elementName)
        {
            XmlNodeList list = _langDoc.GetElementsByTagName(elementName);
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Item(i).ParentNode.Name == parentForm.Name)
                    return list.Item(i).InnerText;
            }
            return "";
        }

        public static string GetExceptionMessage(string elementName)
        {
            return _langDoc.GetElementsByTagName(elementName).Item(0).InnerText;
        }

        public static string GetFormName(Type T)
        {
            return _langDoc.GetElementsByTagName(T.Name).Item(0).Attributes.Item(0).Value;
        }
    }
}
