using System;
using System.Xml.Serialization;

namespace WpfApp1
{
    [Serializable]
    public class InformationAirFlight
    {
        [XmlElement("data")]
        private DateTime data;
        [XmlElement("NumberofSeats")]
        private int NumberofSeats { get; set; }
        [XmlElement("AirCompony")]
        private string AirCompony { get; set; }
        [XmlElement("AirportArrive")]
        private string AirportArrive { get; set; }
        [XmlElement("AirportDeparture")]
        private string AirportDeparture { get; set; }
        [XmlElement("NumberAirFlight")]
        private string NumberAirFlight { get; set; }
        public InformationAirFlight() { }
        public InformationAirFlight(int NumberofSeats, string AirCompony,
            string AirportArrive, string AirportDeparture, string NumberAirFlight)
        {
            this.data = DateTime.Now;
            this.AirCompony = AirCompony;
            this.NumberofSeats = NumberofSeats;
            this.AirportArrive = AirportArrive;
            this.NumberAirFlight = NumberAirFlight;
            this.AirportDeparture = AirportDeparture;
        }
    }
    [Serializable]
    public class InformationTickets
    {
        private DateTime data;
        public InformationTickets() { }
        public InformationTickets(int NumberofSeats, string AirCompony,
            string AirportArrive, string AirportDeparture, string NumberAirFlight)
        { }
    }
}
