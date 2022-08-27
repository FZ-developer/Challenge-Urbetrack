namespace NetExam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetExam.Abstractions;
    using NetExam.Dto;

    public class OfficeRental : IOfficeRental
    {
        public List<LocationSpecs> Locations { get; set; }
        public List<OfficeSpecs> Offices { get; set; }
        public List<BookingRequest> Bookings { get; set; }
        public List<SuggestionRequest> Suggestions { get; set; }

        public OfficeRental()
        {

            Locations = new List<LocationSpecs>();
            Offices = new List<OfficeSpecs>();
            Bookings = new List<BookingRequest>();  
            
        }



        public void AddLocation(LocationSpecs locationSpecs)
        {

            if (Locations.Any(x => x.Name == locationSpecs.Name)) throw new Exception("This location already exists.");

            Locations.Add(locationSpecs);

        }


        public void AddOffice(OfficeSpecs officeSpecs)
        {

            if (!Locations.Exists(x => x.Name == officeSpecs.LocationName)) throw new Exception("This location doesn´t exists.");

            if (Offices.Exists(x => x.Name == officeSpecs.Name && x.LocationName == officeSpecs.LocationName)) throw new Exception("This office already exists.");

            Offices.Add(officeSpecs);

        }


        public void BookOffice(BookingRequest bookingRequest)
        {
            
            if (!Locations.Exists(x => x.Name == bookingRequest.LocationName)) throw new Exception("This location doesn´t exists.");

            if (!Offices.Exists(x => x.Name == bookingRequest.OfficeName)) throw new Exception("This office doesn´t exists.");

            var bookingsLst = Bookings.Where(x => x.LocationName == bookingRequest.LocationName && x.OfficeName == bookingRequest.OfficeName).ToList(); 
                                       
            if (bookingsLst.Exists(x => x.DateTime.AddHours(x.Hours) > bookingRequest.DateTime && x.DateTime < bookingRequest.DateTime))
                throw new Exception($"We are sorry, but the selected office, {bookingRequest.OfficeName}, located in {bookingRequest.LocationName} is already reserved for the chosen date and time.");

            Bookings.Add(bookingRequest);      
            
        }


        public IEnumerable<IBooking> GetBookings(string locationName, string officeName)
        {

            return Bookings.Where(x => x.LocationName == locationName && x.OfficeName == officeName);

        }


        public IEnumerable<ILocation> GetLocations()
        {

            return Locations;       
            
        }


        public IEnumerable<IOffice> GetOffices(string locationName)
        {

            return Offices.Where(x => x.LocationName == locationName);

        }


        public IEnumerable<IOffice> GetOfficeSuggestion(SuggestionRequest suggestionRequest)
        {
            var offices = Offices.Where(x => x.MaxCapacity >= suggestionRequest.CapacityNeeded);

            if (!string.IsNullOrWhiteSpace(suggestionRequest.PreferedNeigborHood))
                offices.Where(x => Locations.Where(y => y.Neighborhood == suggestionRequest.PreferedNeigborHood).Select(y => y.Name).Contains(x.LocationName));

            return OfficesResourcesNeeded(suggestionRequest.ResourcesNeeded, offices).OrderBy(x => x.MaxCapacity).OrderBy(x => x.AvailableResources.Count()).ToList();
        }


        private IEnumerable<OfficeSpecs> OfficesResourcesNeeded(IEnumerable<string> resourcesNeeded, IEnumerable<OfficeSpecs> offices)
        {
            var recommendedOffices = new List<OfficeSpecs>();

            if (resourcesNeeded.Any())
                offices.ToList().ForEach(office =>
                {

                    var resource = true;
                    resourcesNeeded.ToList().ForEach(resourNeeded =>
                    {
                        if (!Array.Exists(office.AvailableResources.ToArray(), x => x == resourNeeded))
                            resource = false;
                    });

                    if (resource)
                        recommendedOffices.Add(office);

                });

            else recommendedOffices.AddRange(offices);

            return recommendedOffices;
        }










    }
}