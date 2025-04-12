using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{

    public abstract class Building
    {
        public abstract string Address { get; set; }
        public abstract double BuildingArea { get; set; }
        public abstract string BuildingDate { get; set; }

        public Building(string buildingDate)
        {     
        }
            
       
    }
    public class PublicBuilding:Building
    {
        public override string Address { get; set; }
        public override double BuildingArea { get; set; }
        public override string BuildingDate { get; set; }

        public string Ownership {  get; set; }
        
        public PublicBuilding(string buildingDate, string ownership):base(buildingDate) 
        {
            BuildingDate = buildingDate;
            Ownership = ownership;
        }
        public override string ToString()
        {
            return $"Громадська споруда за адресою:{this.Address}, площею {this.BuildingArea}кв. метрів, збудованою {this.BuildingDate}";
        }
    }
    public class Cinema:PublicBuilding,ICapacity
    {
        public string Name {  get; set; }
        public int NumberOfCinemaHalls { get; set; }
        public double PriceOfTicket { get; set; }
        public Cinema(string buildingDate, string ownership):base(buildingDate, ownership)
        {
            
        }
        public int GetCapacity()
        {
            //в зале 100 мест 
            return this.NumberOfCinemaHalls * 100;
        }
        public double GetTicketPrice()
        {
            return this.PriceOfTicket;
        }
        public override string ToString()
        {
            return $"Кінотеатр площею {this.BuildingArea}кв. метрів, збудовано {this.BuildingDate}";
        }
    }
    public class Hotel:PublicBuilding,ICapacity,IRentalRate
    {
        public string Name { get; set; }
        public int NumberOfRoom { get; set; }
        public int NumberOfStars { get; set; }  
        public double RoomPrice { get; set; }
        public Hotel(string buildingDate, string ownership) : base(buildingDate, ownership)
        {

        }
        public int GetCapacity()
        {
            //в отеле все номера 2-х местные
            return this.NumberOfRoom * 2;
        }
        public double GetRentalRate()
        {
            return this.RoomPrice;
        }
        public override string ToString()
        {
            return $"Готель площею {this.BuildingArea}кв. метрів, збудовано {this.BuildingDate}";
        }
    }
    public class Restaraunt : PublicBuilding,ICapacity
    {
        public string Name { get; set; }
        public int NumberOfSeats { get; set; }
        public string Cuisine { get; set; }
        public Restaraunt(string buildingDate, string ownership) : base(buildingDate, ownership)
        {

        }
        public int GetCapacity()
        {
            
            return this.NumberOfSeats;
        }
        public override string ToString()
        {
            return $"Ресторан площею {this.BuildingArea}кв. метрів, збудовано {this.BuildingDate}";
        }
    }
    public class ResidentialBuilding : Building
    {
        public override string Address { get; set; }
        public override double BuildingArea { get; set; }
        public override string BuildingDate { get; set; }


        public ResidentialBuilding(string buildingDate) : base(buildingDate)
        {
            BuildingDate = buildingDate;
        }
        public override string ToString()
        {
            return $"Житлова споруда за адресою:{this.Address}, площею {this.BuildingArea}кв. метрів,збудовано {this.BuildingDate}";
        }
        
    }
    public class FiveStoreyBuilding:ResidentialBuilding,ICapacity,IRentalRate
    {
        public int NumberOfApartments { get; set; }
        public double PricePerSqM { get; set; }


        public FiveStoreyBuilding(string buildingDate, int numberOfApartments) : base(buildingDate)
        {
            BuildingDate = buildingDate;
            NumberOfApartments = numberOfApartments;
            
        }
        public int GetCapacity()
        {
            //будем считать что в каждой квартире живет в среднем 2 человека
            return this.NumberOfApartments * 2;
        }
        public override string ToString()
        {
            return $"Житловий будинок 5 поверхів за адресою:{this.Address},площею {this.BuildingArea}кв. метрів ,з {this.NumberOfApartments} квартирами, збудовано {this.BuildingDate}";
        }
        public  double GetRentalRate()
        {
            return this.BuildingArea / this.NumberOfApartments * this.PricePerSqM;
        }

    }
}
