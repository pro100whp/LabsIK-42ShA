using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using laboratorna_4._2.Classes;

namespace laboratorna_4._2
{
    public class ParcelSorter
    {

        List<Parcel> _parcels;
        public ParcelSorter(List<Parcel> parcels)
        {
            _parcels = parcels;

        }
        public string GetParcelInfo(string trakingNumber)
        {
            Parcel myParcel = null;
            foreach (Parcel parcel in _parcels)
            {
                if (parcel.TrackingNumber == trakingNumber)
                {
                    myParcel = parcel; break;
                }
            }
            if (myParcel == null)
            {
                return "Поссилку з таким номером не знайдено";

            }
            else
            {
                string parcelInfo = $" Місцезнаходження посилки: {myParcel.Status.ToString()}" + Environment.NewLine;
                parcelInfo += $" Відстань транспортування(км): {myParcel.Distance.ToString()}" + Environment.NewLine;
                parcelInfo += $" Вартість пересилки : {myParcel.CalculeteDeliveryPrice().ToString()} грн.";

                return parcelInfo;
            }
        }
        public string GetAllParcelOverSomeWeight(double weight)
        {
            Boolean parcelsFound = false;
            string parcelInfo = string.Empty;
            foreach (Parcel parcel in _parcels)
            {
                if (parcel.Weight > weight)
                {
                    parcelsFound = true;
                    parcelInfo += parcel.ToString() + Environment.NewLine;
                }
            }
            if (!parcelsFound)
            {
                return $"Посилок з вагою більше ніж {weight} не знайдено ";
            }
            else
            {
                return parcelInfo;
            }
        }

        public string GetParcelsBySenderLocation()
        {
            string parcelInfo = string.Empty;
            List<string> fromAddresses = new List<string>();
            List<string> toAddresses = new List<string>();

            foreach (var parcel in _parcels)
            {
                if (!fromAddresses.Contains(parcel.AddresFrom))
                {
                    fromAddresses.Add(parcel.AddresFrom);
                }
                if (!toAddresses.Contains(parcel.AddresTo))
                {
                    toAddresses.Add(parcel.AddresTo);
                }
            }

            fromAddresses.Sort();
            toAddresses.Sort();

            parcelInfo += "Посилки згрупованні за адресою відправника" + Environment.NewLine;
            foreach (string fromAddress in fromAddresses)
            {
                parcelInfo += $">>{fromAddress}" + Environment.NewLine;
                foreach(var parcel in _parcels)
                {
                    if(parcel.AddresFrom == fromAddress)
                    {
                        parcelInfo += parcel.ToString() + Environment.NewLine;
                    }
                }    
            }
            parcelInfo += Environment.NewLine + "Посилки згрупованні за адресою отримувача" + Environment.NewLine;
            foreach (string toAddress in toAddresses)
            {
                parcelInfo += $">>{toAddress}" + Environment.NewLine;
                foreach (var parcel in _parcels)
                {
                    if (parcel.AddresTo == toAddress)
                    {
                        parcelInfo += parcel.ToString() + Environment.NewLine;
                    }
                }
            }
            return parcelInfo;
        }
        public string GetParcelsByType()
        {

            string parcelInfoS = string.Empty;
            string parcelInfoM = string.Empty;
            string parcelInfoL = string.Empty;
            string documentInfo = string.Empty;
            foreach (var parcel in _parcels)
            {
                if (parcel.GetSize() == "s")
                {
                    parcelInfoS += "  " + parcel.ToString() + Environment.NewLine;
                }
                else if (parcel.GetSize() == "m")
                {
                    parcelInfoM += "  " + parcel.ToString() + Environment.NewLine;
                }
                else if (parcel.GetSize() == "l")
                {
                    parcelInfoL += "  " + parcel.ToString() + Environment.NewLine;
                }
                else if (parcel.GetSize() == "document")
                {
                    documentInfo += "  " + parcel.ToString() + Environment.NewLine;
                }
            }
            string AllInfo = string.Empty;
            AllInfo += $"Посилки з типом S:\r\n{parcelInfoS}Посилки з типом M:\r\n{parcelInfoM}Посилки з типом L:\r\n{parcelInfoL}Посилки з типом Document:\r\n{documentInfo}";
            return AllInfo;

        }
    }
}
