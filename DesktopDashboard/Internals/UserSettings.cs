using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDashboard.Internals
{
    internal class UserSettings
    {
        const string UserSettingsDirectory = "UserSettings";

        public enum SettingType
        {
            PluginState,
            WindowState
        }

        private static void CreateSettingsDirectoryIfNotExists()
        {
            if (!Directory.Exists(UserSettingsDirectory))
                Directory.CreateDirectory(UserSettingsDirectory);
        }

        private static string GetSettingFilePath(SettingType settingType)
        {
            return Path.Combine(UserSettingsDirectory, $"{settingType.ToString()}.json");
        }

        public static void SaveSetting(SettingType settingType, object setting)
        {
            try
            {
                CreateSettingsDirectoryIfNotExists();
                string serializedSetting = JsonConvert.SerializeObject(setting);
                File.WriteAllText(GetSettingFilePath(settingType), serializedSetting);
            }
            catch(Exception)
            {
                //ToDo Log
            }
        }

        public static T LoadSetting<T>(SettingType settingType)
        {
            try
            {
                CreateSettingsDirectoryIfNotExists();
                string serializedSetting = File.ReadAllText(GetSettingFilePath(settingType));
                if (String.IsNullOrWhiteSpace(serializedSetting))
                    return default(T);
                return JsonConvert.DeserializeObject<T>(serializedSetting);
            }
            catch (Exception ex)
            {
                //ToDo Log
                return default(T);
            }
        }
    }
}
