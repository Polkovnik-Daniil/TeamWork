using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    [Serializable]
    class InformationAirFlight
    {
        private DateTime data;
        private int NumberofSeats;
        private string AirCompony;
        private string AirportArrive;
        private string AirportDeparture;
        private string NumberAirFlight;
        public InformationAirFlight() { }
        public InformationAirFlight(object[] Result/*int NumberofSeats, string AirCompony, 
            string AirportArrive, string AirportDeparture, string NumberAirFlight*/)
        {
            this.data = DateTime.Now;
            this.NumberofSeats = (int)Result[0];
            this.AirCompony = (string)Result[1];
            this.AirportArrive = (string)Result[2];
            this.NumberAirFlight = (string)Result[4];
            this.AirportDeparture = (string)Result[3];
        }
    }
}
