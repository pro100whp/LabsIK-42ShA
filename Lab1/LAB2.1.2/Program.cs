using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2._1._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //обьявляем словарь
            Dictionary<string, string> example = new Dictionary<string, string>();
            example.Add("key 1", "value 1");           
            example.Add("key 2", "value 2");
            example.Add("key 3", "value 3");
            example.Add("key 4", "value 4");
            example.Add("key 5", "value 5");

            //создаем список всех ключей из dictionary 
            List<string> listKeys = new List<string>(example.Count);
            foreach(KeyValuePair<string, string> pair in example)
            {
                listKeys.Add(pair.Key);
            }
            
            //используя список ключей перебираем все значения в словаре
            foreach (string key in listKeys)
            {
                //запоминаем значение value из словаря
                string value = example[key];

                //удаляем елемент словаря 
                example.Remove(key);    
                //удаляем пробелы из значения словаря 
                value = value.Replace(" ", "");
                //удалим пробелы из ключа в словаре 
                string newKey = key.Replace(" ", "");

                //добавляем новый елемент в словарь без пробелов
                example.Add(newKey, value);
            }
            //выводим dictionary на console
            foreach (KeyValuePair<string, string> pair in example)
            {
                listKeys.Add(pair.Key);
                Console.WriteLine(pair.Key + " : " + pair.Value); 
            }



        }
    }
}
