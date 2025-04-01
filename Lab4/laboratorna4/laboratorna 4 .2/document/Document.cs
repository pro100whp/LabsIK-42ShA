using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using laboratorna_4._2.Classes;

namespace laboratorna_4._2
{
    public class Document:Parcel
    {
        public override double CalculeteDeliveryPrice()
        {
            return 3.5 * Distance;
        }
        public override double Length 
        {
            get { return 0; }
        }
        public override double Width 
        {
            get { return 0; }
        }
        public override double Height 
        {
            get { return 0; }
        }
        public override string ToString()
        {
            return $"Поссилка з номером {this.TrackingNumber} і типом {this.GetSize()}";
        }
    }

}
