using Client.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Client.Helper
{
    public class JsonFileReadHelper
    {
        private static JsonFileReadHelper _instance = null;
        public static JsonFileReadHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JsonFileReadHelper();
                }
                return _instance;
            }
        }

        public static List<Key_Value> AllKey_Values = new List<Key_Value>(0);

        public string FIndValue(string Key)
        {
            string Res = null;
            if (AllKey_Values == null || AllKey_Values.Count == 0)
            {
                AllKey_Values = ReadSettingsJsonFile();
            }
            var ResObj = AllKey_Values.Find(z => z.Key == Key);
            if (ResObj != null)
                Res = ResObj.Value;
            return Res;
        }
        public List<Key_Value> ReadSettingsJsonFile()
        {
            List<Key_Value> Res = new List<Key_Value>();
            string JsonStr = "";
            using (FileStream fs = new FileStream("" + System.AppDomain.CurrentDomain.BaseDirectory + "\\OtherSettings.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    JsonStr = sr.ReadToEnd().ToString();
                }
            }
            Res = JsonConvert.DeserializeObject<List<Key_Value>>(JsonStr);
            return Res;
        }
    }
}
