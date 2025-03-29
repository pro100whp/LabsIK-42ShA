using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace laboratorna4
{
    internal class Program
    {
        public class Wear //base 
        {

            public string articule { get; set; }
            private int _size;
            public int size
            {
                get { return _size; }
                set
                {

                    if (value > 0 && value < 4)
                    { _size = value; }

                    else
                    { throw new Exception("Допустимі розміри 1,2,3 "); }

                }

            }

            public virtual void PutOn()
            {
                Console.WriteLine("Одягли одежу");
            }

            public Boolean Equals(Wear other)
            {
                return (this.size == other.size && this.articule == other.articule);
            }

            public string ToString()
            {
                string wear = ($"{this.GetName()} ,розмір {this.size} , артикул {this.articule}");
                return wear;

            }

            public virtual string GetName()
            {
                return "одяг";
            }

            public int GetHashCode()
            {
                string SizePlusArticule = this.size.ToString() + this.articule;
                return SizePlusArticule.GetHashCode();
            }

            public static T GetWear<T>() where T : Wear, new()
            {
                return new T();


            }

            public class Jacket : Wear
            {
                public override string GetName()
                {
                    return "Куртка";
                }
                public override void PutOn()
                {
                    Console.WriteLine($"Одягли куртку під розміром {this.size}");
                }


            }

            public class Shoes : Wear
            {
                public override string GetName()
                {
                    return "Взуття";
                }
                public override void PutOn()
                {
                    Console.WriteLine($"Одягли взуття під розміром {this.size}");
                }


            }

            public class Shirt : Wear
            {
                public override string GetName()
                {
                    return "Сорочка";
                }
                public override void PutOn()
                {
                    Console.WriteLine($"Одягли сорочку під розміром {this.size}");
                }


            }

            public class Pants : Wear
            {
                public override string GetName()
                {
                    return "Штани";
                }
                public override void PutOn()
                {
                    Console.WriteLine($"Одягли штани під розміром {this.size}");
                }


            }

            static void Main(string[] args)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine("==ГАРДЕРОБ==");
                Console.WriteLine("Виберіть тип одягу: 1 - Куртка, 2 - взуття, 3 - сорочка , 4 - штани");
                Wear wear;
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 1)
                {
                    wear = Wear.GetWear<Jacket>();
                }
                else if (choice == 2)
                {
                    wear = Wear.GetWear<Shoes>();
                }
                else if (choice == 3)
                {
                    wear = Wear.GetWear<Shirt>();
                }
                else if (choice == 4)
                {
                    wear = Wear.GetWear<Pants>();
                }
                else
                {
                    throw new Exception("Невірний вибір!");
                }

                Console.WriteLine("Введіть розмір  (1, 2 або 3)");
                wear.size = Convert.ToInt32(Console.ReadLine());//TO DO add exception handling

                Console.WriteLine("Введіть Артикул");
                wear.articule = Console.ReadLine();

                wear.PutOn();
                Console.WriteLine("реалізація метода ToString()");
                Console.WriteLine(wear.ToString());

                Console.WriteLine("реалізація метода GetHashCode()");
                Console.WriteLine(wear.GetHashCode());

                Jacket jacket1 = new Jacket() { articule = "a1", size = 1 };
                Jacket jacket2 = new Jacket() { articule = "a1", size = 1 };
                Jacket jacket3 = new Jacket() { articule = "a1", size = 3 };
                Console.WriteLine("Реалізація методу Equals()");
                Console.WriteLine("Jacket jacket1 = new Jacket() { articule = \"a1\", size = 1 };");
                Console.WriteLine("Jacket jacket2 = new Jacket() { articule = \"a1\", size = 1 };");
                Console.WriteLine("Jacket jacket3 = new Jacket() { articule = \"a1\", size = 3 };");
                Console.WriteLine("Console.WriteLine(jacket1.Equals(jacket2));");
                Console.WriteLine("Console.WriteLine(jacket1.Equals(jacket3));");
                Console.WriteLine(jacket1.Equals(jacket2));
                Console.WriteLine(jacket1.Equals(jacket3));





            }
        }
    }
}











