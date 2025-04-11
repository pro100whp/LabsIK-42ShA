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
        public abstract string BuildingArea { get; set; }
        public abstract string BuildingDate { get; set; }

        public Building(string buildingDate)
        {     
        }
            
       
    }
    public class PublicBuilding:Building
    {
        public override string Address { get; set; }
        public override string BuildingArea { get; set; }
        public override string BuildingDate { get; set; }

        public string Ownership {  get; set; }
        
        public PublicBuilding(string buildingDate, string ownership):base(buildingDate) 
        {
            BuildingDate = buildingDate;
            Ownership = ownership;
        }
    }
    public class Cinema:PublicBuilding,ICapacity
    {
        public string Name {  get; set; }
        public int NumberOfCinemaHalls { get; set; }
        public Cinema(string buildingDate, string ownership):base(buildingDate, ownership)
        {
            
        }
        public int GetCapacity()
        {
            //в зале 100 мест 
            return this.NumberOfCinemaHalls * 100;
        }
    }
    public class Hotel:PublicBuilding,ICapacity
    {
        public string Name { get; set; }
        public int NumberOfRoom { get; set; }
        public int NumberOfStars { get; set; }  
        public Hotel(string buildingDate, string ownership) : base(buildingDate, ownership)
        {

        }
        public int GetCapacity()
        {
            //в отеле все номера 2-х местные
            return this.NumberOfRoom * 2;
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
    }
    public class ResidentialBuilding : Building
    {
        public override string Address { get; set; }
        public override string BuildingArea { get; set; }
        public override string BuildingDate { get; set; }


        public ResidentialBuilding(string buildingDate, int numberOfApartments) : base(buildingDate)
        {
            BuildingDate = buildingDate;
        }
    }
    public class FiveStoreyBuilding:ResidentialBuilding,ICapacity
    {
        public int NumberOfApartments { get; set; }



        public FiveStoreyBuilding(string buildingDate, int numberOfApartments) : base(buildingDate, numberOfApartments)
        {
            BuildingDate = buildingDate;
        }
        public int GetCapacity()
        {
            //будем считать что в каждой квартире живет в среднем 2 человека
            return this.NumberOfApartments * 2;
        }
    }
}
