using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab2.utils
{
    public static class SettingsManager
    {
        private const string SettingsFileName = "../startup_settings.txt";

        public static bool LoadShowStartupMessageSetting()
        {
            // Проверяем существование файла
            if (File.Exists(SettingsFileName))
            {
                // Если файл существует, читаем настройки из него
                string content = File.ReadAllText(SettingsFileName);
                return bool.Parse(content);
            }
            else
            {
                // Если файл не существует, возвращаем значение по умолчанию (true)
                return true;
            }
        }

        public static void SaveShowStartupMessageSetting(bool value)
        {
            // Сохраняем значение в файл
            File.WriteAllText(SettingsFileName, value.ToString());
        }
    }


}
