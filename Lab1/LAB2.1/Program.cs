using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LAB2._1
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<double> example = new List<double>() { 1, 2, 3, -3, -5, -8, -8 };
            double ArithmeticMean = 0;
            int numberOfNegattiveValues = 0;
            double minValue = double.MaxValue;
            

            foreach (double value in example)
            {
                if (value < 0)
                {
                    ArithmeticMean += value;
                    numberOfNegattiveValues++;

                    if(value < minValue)
                    {
                        minValue = value;
                        

                    }
                }

            }
            if (numberOfNegattiveValues > 0)
            {
                ArithmeticMean /= numberOfNegattiveValues;

                for (int i = 0; i < example.Count;i++)
                {
                    if (example[i] == minValue)
                    {
                        example[i] = ArithmeticMean;
                    }
                }
                foreach (double value in example)
                {
                    Console.WriteLine(value);
                }

            }
            else
            {
                Console.WriteLine("Негативных значений не найдено");
            }
            
        }
    }
}
