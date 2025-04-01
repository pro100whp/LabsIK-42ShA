using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using laboratorna_4._2.Classes;
using laboratorna_4._2;

namespace laboratorna_4._2
{
    
    internal class Program
    {

        
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("ПОШТА");
            string selection;
            ParcelSorter parcelSorter = new ParcelSorter(InitParcels());
            do
            {

                Console.WriteLine("Введіть 1 для списку посилок, 2 для інформації про посилку по TrackingNumber, 0 для виходу");
                selection = Console.ReadLine();
                if (selection == "1")
                {
                    Console.WriteLine("Введіть 1 списку посилок більше 20кг  \r\n2 для списку згрупованих посилок по місцю відправлення та прибуття \r\n3 для списку згрупованих посилок за типом \r\n0 для виходу");
                    selection = Console.ReadLine();
                    if(selection == "1")
                    {
                        Console.WriteLine(parcelSorter.GetAllParcelOverSomeWeight(20));
                    }
                    else if (selection == "2")
                    {
                        Console.WriteLine(parcelSorter.GetParcelsBySenderLocation());
                    }
                    else if (selection == "3")
                    {
                        Console.WriteLine(parcelSorter.GetParcelsByType());
                    }
                }
                else if (selection == "2")
                {
                    Console.WriteLine("Для знаходженян поссили введіть ТТН");
                    string answer = Console.ReadLine();
                    Console.WriteLine(parcelSorter.GetParcelInfo(answer));
                }
            }
            while (selection != "0");

            
        }
        static List<Parcel> InitParcels()
        {
            List <Parcel> parcels = new List<Parcel>();

            StandardParcel parcel1 = new StandardParcel();//s
            parcel1.Width = 10;
            parcel1.Height = 10;
            parcel1.Length = 10;
            parcel1.Weight = 1;
            parcel1.TrackingNumber = "AB84274NFK"; 
            parcel1.AddresFrom = "Poltava";
            parcel1.AddresTo = "Sumy";
            parcel1.Sender = "Vasya";
            parcel1.Reciever = "Alex";
            parcel1.Distance = 300;
            parcel1.Price = 5000;
            parcel1.Status = "Poltava";

            StandardParcel parcel2 = new StandardParcel();//m
            parcel2.Width = 22;
            parcel2.Height = 18;
            parcel2.Length = 19;
            parcel2.Weight = 22;
            parcel2.TrackingNumber = "AB44894OLL";
            parcel2.AddresFrom = "Odesa";
            parcel2.AddresTo = "Kharkiv";
            parcel2.Sender = "Anna";
            parcel2.Reciever = "Roman";
            parcel2.Distance = 550;
            parcel2.Price = 5000;
            parcel2.Status = "Kharkiv";

            StandardParcel parcel3 = new StandardParcel();//l
            parcel3.Width = 30;
            parcel3.Height = 35;
            parcel3.Length = 50;
            parcel3.Weight = 25;
            parcel3.TrackingNumber = "AB63894FBS";
            parcel3.AddresFrom = "Kyiv";
            parcel3.AddresTo = "Lviv";
            parcel3.Sender = "Andrej";
            parcel3.Reciever = "Vova";
            parcel3.Distance = 470;
            parcel3.Price = 5000;
            parcel3.Status = "Rivne";

            StandardParcel parcel4 = new StandardParcel();//s
            parcel4.Width = 5;
            parcel4.Height = 6;
            parcel4.Length = 7;
            parcel4.Weight = 0.5;
            parcel4.TrackingNumber = "AB98435FBS";
            parcel4.AddresFrom = "Kharkiv";
            parcel4.AddresTo = "Dnipro";
            parcel4.Sender = "Maksim";
            parcel4.Reciever = "Yaroslav";
            parcel4.Distance = 195;
            parcel4.Price = 5000;
            parcel4.Status = "Pavlohrad";

            Document document = new Document();//document         
            document.Weight = 0;
            document.TrackingNumber = "AB43567ABC";
            document.AddresFrom = "Kyiv";
            document.AddresTo = "Odesa";
            document.Sender = "Vladislav";
            document.Reciever = "Dmitriy";
            document.Distance = 195;
            document.Price = 5000;
            document.Status = "Pavlohrad";

            parcels.Add(parcel1);
            parcels.Add(parcel2);
            parcels.Add(parcel3);
            parcels.Add(parcel4);
            parcels.Add(document);

            return parcels;
        }
    }
}
