using System;
using System.Collections.Generic;
using System.IO;

namespace TerWoord.NetI2C
{
    public class I2CControllerInfo
    {
        public string Name
        {
            get;
        }

        public string Description
        {
            get;
        }

        public int Number
        {
            get;
        }

        public I2CControllerInfo(string name, string description, int number)
        {
            Name = name;
            Description = description;
            Number = number;
        }

        public static I2CControllerInfo[] GetControllers()
        {
            var xResult = GetControllersSysfs("/sys");
            if (xResult == null)
            {
                throw new InvalidOperationException(@"No way to find controllers!");
            }
            return xResult;
        }

        private static I2CControllerInfo[] GetControllersSysfs(string baseDir)
        {
            var xBaseDir = Path.Combine(baseDir, "class/i2c-dev");
            if (!Directory.Exists(xBaseDir))
            {
                return null;
            }

            var xResult = new List<I2CControllerInfo>();
            foreach (var xItem in Directory.GetDirectories(xBaseDir))
            {
                var xNameFile = Path.Combine(xItem, "name");
                if (File.Exists(xNameFile))
                {
                    var xItemName = Path.GetFileNameWithoutExtension(xItem);
                    if (xItemName.IndexOf('-') == -1)
                    {
                        throw new Exception($"Unsupported name '{xItemName}'!");
                    }

                    var xItemNumberStr = xItemName.Substring(xItemName.LastIndexOf('-') + 1);
                    xResult.Add(new I2CControllerInfo(xItemName, File.ReadAllText(xNameFile),int.Parse(xItemNumberStr)));
                }
            }
            return xResult.ToArray();
        }
    }
}
