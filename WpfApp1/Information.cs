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
        public InformationAirFlight(int NumberofSeats, string AirCompony, 
            string AirportArrive, string AirportDeparture, string NumberAirFlight)
        {
            this.data = DateTime.Now;
            this.AirCompony = AirCompony;
            this.AirportArrive = AirportArrive;
            this.NumberofSeats = NumberofSeats;
            this.NumberAirFlight = NumberAirFlight;
            this.AirportDeparture = AirportDeparture;
        }
    }
}
