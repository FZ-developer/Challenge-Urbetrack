namespace NetExam.Abstractions
{
    using System.Collections.Generic;
    using NetExam.Dto;

    public interface IOfficeRental
    {
        void AddLocation(LocationSpecs locationSpecs);

        IEnumerable<ILocation> GetLocations();

        void AddOffice(OfficeSpecs officeSpecs);

        IEnumerable<IOffice> GetOffices(string locationName);

        void BookOffice(BookingRequest bookingRequest);

        IEnumerable<IBooking> GetBookings(string locationName, string officeName);

        IEnumerable<IOffice> GetOfficeSuggestion(SuggestionRequest suggestionRequest);
    }
}