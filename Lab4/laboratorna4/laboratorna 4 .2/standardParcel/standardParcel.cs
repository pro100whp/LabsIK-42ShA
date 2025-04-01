using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using laboratorna_4._2.Classes;

namespace laboratorna_4._2
{
    public class StandardParcel: Parcel
    {
        public override double CalculeteDeliveryPrice()
        {
            return 10 * Distance;
        }

    }
}
