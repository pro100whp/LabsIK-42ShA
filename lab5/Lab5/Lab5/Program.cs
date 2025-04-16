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
            Multiplex.PriceOfTicket = 150;
            Multiplex.BuildingArea = 2000;
            buildings.Add(Multiplex);

            Cinema OdessaKino = new Cinema("08.12.2015", "Комунальна власність");
            OdessaKino.Name = "OdessaKino";
            OdessaKino.NumberOfCinemaHalls = 5;
            OdessaKino.PriceOfTicket = 180;
            OdessaKino.BuildingArea = 2500;
            buildings.Add(OdessaKino);

            Hotel Eleon = new Hotel("01.01.2007", "Приватна власність");
            Eleon.Name = "Eleon";
            Eleon.NumberOfStars = 5;
            Eleon.NumberOfRoom = 25;
            Eleon.RoomPrice = 1000;
            Eleon.BuildingArea = 3000;
            buildings.Add(Eleon);

            Restaraunt Gaga = new Restaraunt("02.03.2020", "Приватна власність");
            Gaga.Name = "Gaga";
            Gaga.NumberOfSeats = 50;
            Gaga.Cuisine = "Georgian";
            Gaga.BuildingArea = 200;
            buildings.Add(Gaga);

            FiveStoreyBuilding fiveStoreyBuilding = new FiveStoreyBuilding("30.10.1985", 50);
            fiveStoreyBuilding.Address = "Kiyv, Zsustret 5";
            fiveStoreyBuilding.PricePerSqM = 350;
            fiveStoreyBuilding.BuildingArea = 2500;
            buildings.Add (fiveStoreyBuilding);





            foreach (Building building in buildings)
            {
                //выведем описание обьекта 
                Console.WriteLine(building.ToString());

                if (building is Cinema)
                {
                    Cinema cinema = (Cinema)building;
                    Console.WriteLine($"Кінотеатр '{cinema.Name}' має місткість {cinema.GetCapacity()}");
                    Console.WriteLine($"Ціна квитка до 14:00: {cinema.GetTicketPrice(10)}");
                    Console.WriteLine($"Ціна квитка після 14:00: {cinema.GetTicketPrice(20)}");
                }
                else if (building is Hotel)
                {
                    Hotel hotel = (Hotel)building;
                    Console.WriteLine($"Готель '{hotel.Name}' має місткість {hotel.GetCapacity()}");
                    Console.WriteLine($"Ціна оренди одного номеру: {hotel.GetRentalRate()} ");
                }
                else if (building is Restaraunt)
                {
                    Restaraunt restaraunt = (Restaraunt)building;
                    Console.WriteLine($"Ресторан '{restaraunt.Name}' має місткість {restaraunt.GetCapacity()}");
                    
                }
                else if (building is FiveStoreyBuilding)
                {
                    FiveStoreyBuilding storeyBuilding5 = (FiveStoreyBuilding)building;
                    Console.WriteLine($"П'ятиповерхівка має місткість {storeyBuilding5.GetCapacity()}");
                    Console.WriteLine($"Ціна оренди одного помешкання: {storeyBuilding5.GetRentalRate()} ");
                }
                Console.WriteLine();
            }
        }
    }
}
