﻿using HetDepot.People.Model;
using HetDepot.Views.Interface;
using HetDepot.Views.Parts;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace HetDepot.Tours.Model
{
    public class Tour : IListableObject
    {
        private List<Visitor> _reservations;
        private List<Visitor> _admissions;
        private int _maxReservations;

        [JsonConstructor]
		public Tour(DateTime startTime, Guide guide, int maxReservations, List<Visitor> reservations, List<Visitor> admissions)
		{
			StartTime = startTime;
            _reservations = reservations;
            _admissions = admissions;
            _maxReservations = maxReservations;
			Guide = guide;
		}

        public DateTime StartTime { get; private set; }
        public Guide Guide { get; set; }
        public int MaxReservations { get { return _maxReservations; } }
        public ReadOnlyCollection<Visitor> Reservations
        { 
            get { return _reservations.AsReadOnly(); }
        }

        public ReadOnlyCollection<Visitor> Admissions
        {
            get { return _admissions.AsReadOnly(); }
        }

        public string GetTime() => StartTime.ToString("H:mm");
        public int FreeSpaces() => Math.Max(0, _maxReservations - Reservations.Count);
        
        public override string ToString() => GetTime();

        public bool AddReservation(Visitor visitor)
        {
            _reservations.Add(visitor);
            return true;
        }

        public bool RemoveReservation(Visitor visitor)
        {
            var reservationToRemove = _reservations.FirstOrDefault(v => v.Id == visitor.Id);
            return _reservations.Remove(reservationToRemove);
        }

        public bool AddAdmission(Visitor visitor)
        {
            _admissions.Add(visitor);
            //Schrijf visitor info weg naar json:
            WriteAdmissionToFile(visitor);
            
            return true;
        }
        public void WriteAdmissionToFile(Visitor visitor)
        {
            //serialize visitor info. Dit zou visitor info moeten toevoegen aan ademissions array zoals in de Example`tours.json
            string toursJson = JsonSerializer.Serialize(visitor);
            // heet deze json tours.json?
            File.WriteAllText("ExampleTours.json", toursJson);

        }

        public bool RemoveAdmission(Visitor visitor)
        {
			var admissionToRemove = _admissions.FirstOrDefault(v => v.Id == visitor.Id);
			return _admissions.Remove(admissionToRemove);
        }

        public ListableItem ToListableItem()
        {
            /*
             * Geef een ListViewPartedItem terug met tijd en aantal plaatsen
             * Wanneer er geen vrije plaatsen zijn zet Disabled op true. Dit zorgt ervoor dat de optie niet gekozen mag
             * worden.
             */
            return new ListViewItem(new List<ListViewItemPart>
                {
                    new (GetTime(), 10),
                    new (FreeSpaces() > 0 ? FreeSpaces() + " plaatsen" : "Vol")
                },
                this, FreeSpaces() <= 0);
        }
    }
}