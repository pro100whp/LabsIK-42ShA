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
            
            Dictionary<string, string> example = new Dictionary<string, string>();
            example.Add("key 1", "value 1");           
            example.Add("key 2", "value 2");
            example.Add("key 3", "value 3");
            example.Add("key 4", "value 4");
            example.Add("key 5", "value 5");

            
            List<string> listKeys = new List<string>(example.Count);
            foreach(KeyValuePair<string, string> pair in example)
            {
                listKeys.Add(pair.Key);
            }
            
            
            foreach (string key in listKeys)
            {
                
                string value = example[key];

                
                example.Remove(key);    
               
                value = value.Replace(" ", "");
               
                string newKey = key.Replace(" ", "");

                
                example.Add(newKey, value);
            }
            
            foreach (KeyValuePair<string, string> pair in example)
            {
                listKeys.Add(pair.Key);
                Console.WriteLine(pair.Key + " : " + pair.Value); 
            }



        }
    }
}
