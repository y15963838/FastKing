using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FastKing
{
    public static class LocalConfig
    {
        static StreamReader reader;
        static string basePath;

        static LocalConfig()
        {
            basePath = Directory.GetCurrentDirectory() + @"\config";
        } 

        public static UDPInfo GetUDP()
        {
            try
            {
                reader = new StreamReader(basePath + @"\UDP.ini");
                var line = reader.ReadLine();
                reader.Close();

                string ip = line.Split(':')[0];
                int port = Convert.ToInt32(line.Split(':')[1]);
                return new UDPInfo {ip = ip, port = port };
            }
            catch
            {
                return null;
            }
        }

        public static void Test()
        {
            string path = "http://baidu.com";
        }
    }

    public class UDPInfo
    {
        public string ip;
        public int port;
    }
}
