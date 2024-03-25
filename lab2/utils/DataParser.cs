using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.utils
{
    public class DataParser
    {
        public MyData? Parse(string filePath)
        {
            try
            {
                // Чтение всего содержимого файла в строку
                string json = File.ReadAllText(filePath);
                if (json != null)
                {
                    MyData? data = JsonConvert.DeserializeObject<MyData>(json);
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге файла: {ex.Message}");
                return null;
            }
        }
    }
    public class MyData
    {
        public List<double> Numbers { get; set; }
    }
}
