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
        public Dictionary<Action, int> ActionMappings { get; set; } = new Dictionary<Action, int>();
        public int TOGGLE;
    }
    public class ConfigUtil
    {
        public static ConfigData ReadConfigFile(string filePath)
        {
            ConfigData configData = new ConfigData();
            configData.ActionMappings = new Dictionary<Action, int>();

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
                        
                        // Map specific keys to Actions
                        if (property.Name == "FORWARD")
                        {
                            configData.ActionMappings[Action.Up] = (int)value;
                        }
                        else if (property.Name == "BACKWARDS")
                        {
                            configData.ActionMappings[Action.Down] = (int)value;
                        }
                        else if (property.Name == "LEFT")
                        {
                            configData.ActionMappings[Action.Left] = (int)value;
                        }
                        else if (property.Name == "RIGHT")
                        {
                            configData.ActionMappings[Action.Right] = (int)value;
                        }
                        else if (property.Name == "JUMP")
                        {
                            configData.ActionMappings[Action.Jump] = (int)value;
                        }
                        else if (property.Name == "SLIDE")
                        {
                            configData.ActionMappings[Action.Slide] = (int)value;
                        }
                        else if (property.Name == "RELOAD")
                        {
                            configData.ActionMappings[Action.Reload] = (int)value;
                        }
                        else if (property.Name == "INTERACT")
                        {
                            configData.ActionMappings[Action.Interact] = (int)value;
                        }
                        else if (property.Name == "ABILITY1")
                        {
                            configData.ActionMappings[Action.Ability1] = (int)value;
                        }
                        else if (property.Name == "ULTIMATE")
                        {
                            configData.ActionMappings[Action.Ultimate] = (int)value;
                        }
                        else if (property.Name == "SHIELD")
                        {
                            configData.ActionMappings[Action.Shield] = (int)value;
                        }
                        else if (property.Name == "GRENADE")
                        {
                            configData.ActionMappings[Action.Grenade] = (int)value;
                        }
                        else if (property.Name == "PING")
                        {
                            configData.ActionMappings[Action.Ping] = (int)value;
                        }
                        else if (property.Name == "ENABLE")
                        {
                            configData.ActionMappings[Action.EnableMapping] = (int)value;
                        }
                        else if (property.Name == "DISABLE")
                        {
                            configData.ActionMappings[Action.DisableMapping] = (int)value;
                        }
                    }
                    
                    // Add all new action mappings
                    foreach (Action action in Enum.GetValues(typeof(Action)))
                    {
                        if (!configData.ActionMappings.ContainsKey(action))
                        {
                            file.WriteLine($"ACTION_{action}=0");
                            configData.ActionMappings[action] = 0;
                        }
                    }
                    
                    string[] booleanSettings = {"_southpaw", "_streamProof" /*, other boolean settings */ };

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
                else if (key == "_controllerType")
                {
                    if (int.TryParse(value, out int controllerTypeValue))
                    {
                        configData.BooleanSettings[key] = controllerTypeValue == 1;
                    }
                    else if (bool.TryParse(value, out bool boolValue))
                    {
                        configData.BooleanSettings[key] = boolValue;
                    }
                }
                else if (key.StartsWith("ACTION_") && Enum.TryParse(key.Substring(7), out Action action))
                {
                    configData.ActionMappings[action] = int.Parse(value);
                }
                else if (Enum.TryParse<Keys>(value, out var keyValue))
                {
                    configData.KeyBindings[key] = keyValue;
                    
                    // Map ScanCodes to Actions
                    MapScanCodeToAction(key, int.Parse(value), configData.ActionMappings);
                }
                else if (bool.TryParse(value, out var boolValue))
                {
                    configData.BooleanSettings[key] = boolValue;
                }
            }

            return configData;
        }
        
        private static void MapScanCodeToAction(string scanCodeName, int value, Dictionary<Action, int> actionMappings)
        {
            switch (scanCodeName)
            {
                case "FORWARD": 
                    actionMappings[Action.Up] = value; 
                    break;
                case "BACKWARDS": 
                    actionMappings[Action.Down] = value; 
                    break;
                case "LEFT": 
                    actionMappings[Action.Left] = value; 
                    break;
                case "RIGHT": 
                    actionMappings[Action.Right] = value; 
                    break;
                case "JUMP": 
                    actionMappings[Action.Jump] = value; 
                    break;
                case "SLIDE": 
                    actionMappings[Action.Slide] = value; 
                    break;
                case "RELOAD": 
                    actionMappings[Action.Reload] = value; 
                    break;
                case "INTERACT": 
                    actionMappings[Action.Interact] = value; 
                    break;
                case "ABILITY1": 
                    actionMappings[Action.Ability1] = value; 
                    break;
                case "ULTIMATE": 
                    actionMappings[Action.Ultimate] = value; 
                    break;
                case "SHIELD": 
                    actionMappings[Action.Shield] = value; 
                    break;
                case "GRENADE": 
                    actionMappings[Action.Grenade] = value; 
                    break;
                case "PING": 
                    actionMappings[Action.Ping] = value; 
                    break;
                case "ENABLE":
                    actionMappings[Action.EnableMapping] = value; 
                    break;
                case "DISABLE":
                    actionMappings[Action.DisableMapping] = value; 
                    break;
            }
        }
    }
}
