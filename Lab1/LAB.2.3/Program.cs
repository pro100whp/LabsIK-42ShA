using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2._3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Dictionary<string, int> months = new Dictionary<string, int>();
            months.Add("Січень", 31);
            months.Add("Лютий", 28);
            months.Add("Березень", 31);
            months.Add("Квітень", 30);
            months.Add("Травень", 31);
            months.Add("Червень", 30);
            months.Add("Липень", 31);
            months.Add("Серпень", 31);
            months.Add("Вересень", 30);
            months.Add("Жовтень", 31);
            months.Add("Листопад", 30);
            months.Add("Грудень", 31);

            List<string> monthsWith30Days = months.Where(m => m.Value == 30).Select(m => m.Key).ToList();

            Console.WriteLine("Місяці, що мають 30 днів:");
            foreach (string month in monthsWith30Days)
            {
                Console.WriteLine(month);
            }
        }
    }
}

