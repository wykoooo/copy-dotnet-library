using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Silmoon.MySilmoon.Instance;
using Silmoon.Security;

namespace Silmoon.MySilmoon
{
    public class MyConfigure
    {
        public static VersionResult GetVersion(string productString)
        {
            VersionResult result = new VersionResult();
            try
            {
                string url = "https://encrypted.silmoon.com/apps/apis/config?appName=" + productString + "&configName=_version&outType=text/xml";

                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                result.min_exit_version = int.Parse(xml["version"]["version_config_1"]["min_version"]["exit_version"].InnerText);
                result.min_pop_version = int.Parse(xml["version"]["version_config_1"]["min_version"]["pop_version"].InnerText);
                result.latest_version = int.Parse(xml["version"]["version_config_1"]["latest_version"].InnerText);
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }
            return result;
        }
        public static LicenseResult GetLicense(string productString)
        {
            LicenseResult result = new LicenseResult();
            try
            {
                string url = "https://encrypted.silmoon.com/apps/apis/config?appName=" + productString + "&configName=_license&outType=text/xml";

                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                result.unlimited_state = xml["license"]["license_config_1"]["unlimited"]["state"].InnerText;
                result.unlimited_key = xml["license"]["license_config_1"]["unlimited"]["key"].InnerText;
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }
            return result;
        }
        public static int GetLicenseLevelCodeV1(string productString)
        {
            string licol = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\slf.dat";
            string appendKey = "";
            string keyFileContent = "";

            if (File.Exists(licol))
            {
                if (File.Exists(Application.StartupPath + "\\license.slf") && (keyFileContent = File.ReadAllText(Application.StartupPath + "\\license.slf")) != "")
                {
                    string[] lines = File.ReadAllLines(licol);
                    foreach (var item in lines)
                    {
                        string[] lineArr = item.Split('\0');
                        if (lineArr.Length == 2)
                        {
                            if (lineArr[0] == productString)
                            {
                                appendKey = lineArr[1];
                                break;
                            }
                        }
                    }

                    if (appendKey != "")
                    {
                        using (CSEncrypt enc = new CSEncrypt(appendKey))
                        {
                            try
                            {
                                string s = enc.Decrypt(keyFileContent);
                                int level = int.Parse(s);
                                return level;
                            }
                            catch
                            {
                                return 0;
                            }
                        }
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
                return 0;
        }
    }
}