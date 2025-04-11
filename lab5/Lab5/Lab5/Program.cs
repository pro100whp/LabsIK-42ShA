using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Building> buildings = new List<Building>();
            
            Cinema Multiplex = new Cinema("15.02.2010", "Комунальна власність");
            Multiplex.Name = "Multiplex";
            Multiplex.NumberOfCinemaHalls = 2;
            buildings.Add(Multiplex);

            Hotel Eleon = new Hotel("01.01.2007", "Приватна власність");
            Eleon.Name = "Eleon";
            Eleon.NumberOfStars = 5;
            Eleon.NumberOfRoom = 25;
            buildings.Add(Eleon);

            foreach (Building building in buildings)
            {
                if (building is Cinema)
                {
                    Cinema cinema = (Cinema)building;
                    Console.WriteLine($"Кінотеатр '{cinema.Name}' має місткість {cinema.GetCapacity()}");
                }
                else if (building is Hotel)
                {
                    Hotel hotel = (Hotel)building;
                    Console.WriteLine($"Готель '{hotel.Name}' має місткість {hotel.GetCapacity()}");
                }
            }
        }
    }
}
