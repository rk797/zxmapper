using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zxmapper
{
    public class ConfigData
    {
        public Dictionary<string, Keys> KeyBindings { get; set; } = new Dictionary<string, Keys>();
        public Dictionary<string, bool> BooleanSettings { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, double> SensitiviySettings { get; set; } = new Dictionary<string, double>();
        public int TOGGLE;
    }
    public class ConfigUtil
    {
        public static ConfigData ReadConfigFile(string filePath)
        {
            ConfigData configData = new ConfigData();

            if (!File.Exists(filePath))
            {
                using (StreamWriter file = new StreamWriter(filePath))
                {
                    var scanCodeProperties = typeof(ScanCodes).GetProperties(BindingFlags.Public | BindingFlags.Static);

                    foreach (var property in scanCodeProperties)
                    {
                        var value = property.GetValue(null);
                        file.WriteLine($"{property.Name}={value}");

                        if (Enum.TryParse<Keys>(value.ToString(), out var keyValue))
                        {
                            configData.KeyBindings[property.Name] = keyValue;
                        }
                    }
                    string[] booleanSettings = {"_southpaw", "_streamProof" /*, other boolean settings here */ };

                    foreach (var setting in booleanSettings)
                    {
                        file.WriteLine($"{setting}=false");

                        configData.BooleanSettings[setting] = false;
                    }

                    //sensitivity settings
                    int sensX = 36;
                    int sensY = 36;
                    double scaleFactor = 70;
                    double expFactor = 105;
                    configData.SensitiviySettings["scaleFactor"] = scaleFactor;
                    configData.SensitiviySettings["expFactor"] = expFactor;
                    configData.SensitiviySettings["sensX"] = sensX;
                    configData.SensitiviySettings["sensY"] = sensY;
                    file.WriteLine($"sensX={sensX}");
                    file.WriteLine($"sensY={sensY}");
                    file.WriteLine($"scaleFactor={scaleFactor}");
                    file.WriteLine($"expFactor={expFactor}");

                    //toggle setting
                    configData.TOGGLE = 256;
                    file.WriteLine("TOGGLE=256");
                }

                return configData;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (parts.Length != 2) continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (key == "sensX")
                {
                    configData.SensitiviySettings["sensX"] = int.Parse(value);
                }
                else if (key == "sensY")
                {
                    configData.SensitiviySettings["sensY"] = int.Parse(value);
                }
                else if (key == "scaleFactor")
                {
                    configData.SensitiviySettings["scaleFactor"] = double.Parse(value);
                }
                else if (key == "expFactor")
                {
                    configData.SensitiviySettings["expFactor"] = double.Parse(value);
                }
                else if (key == "TOGGLE")
                {
                    configData.TOGGLE = int.Parse(value);
                }
                else if (Enum.TryParse<Keys>(value, out var keyValue))
                {
                    configData.KeyBindings[key] = keyValue;
                }
                else if (bool.TryParse(value, out var boolValue))
                {
                    configData.BooleanSettings[key] = boolValue;
                }
            }

            return configData;
        }
    }
}
