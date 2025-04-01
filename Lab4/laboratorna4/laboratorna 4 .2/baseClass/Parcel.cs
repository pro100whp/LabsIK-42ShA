using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratorna_4._2.Classes
{
    public  class Parcel
    {
        //задаем общие свойства для вссех посылок 
        public double Weight { get; set; }
        public virtual double Width { get; set; }   
        public virtual double Height { get; set; }
        public virtual double Length { get; set; }
        public string TrackingNumber { get; set; }
        public string AddresFrom { get; set; }
        public string AddresTo { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public double Distance { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }


        public virtual double CalculeteDeliveryPrice()
        {
           
            return 5 * this.Distance; 
            
        }
        public string GetSize()
        {
            if (this.Length == 0 && this.Width == 0 && this.Height ==  0)
                return "document";
            if (this.Length < 15 && this.Width < 15 && this.Height < 15)
                return "s";
            else if (this.Length < 25 && this.Width < 25 && this.Height < 25)
                return "m";
            else
                return "l";

        }
        public override string ToString()
        {
            return $"Поссилка з номером {this.TrackingNumber} вагою {this.Weight} і розміром {this.GetSize()}";
        }
    }
}
