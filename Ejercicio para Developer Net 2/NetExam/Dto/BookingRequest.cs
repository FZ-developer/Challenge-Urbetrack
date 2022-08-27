namespace NetExam.Dto
{
    using NetExam.Abstractions;
    using System;

    public class BookingRequest : IBooking
    {
        public string LocationName { get; }
        public string OfficeName { get; }
        public DateTime DateTime { get; }
        public int Hours { get; }
        public string UserName { get; }

        public BookingRequest(string locationName, string officeName, DateTime dateTime, int hours, string userName)
        {
            if (string.IsNullOrWhiteSpace(locationName)) throw new ArgumentException(nameof(locationName));
            if (string.IsNullOrWhiteSpace(officeName)) throw new ArgumentException(nameof(officeName));
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException(nameof(userName));
            if (hours <= 0) throw new ArgumentOutOfRangeException(nameof(hours));

            LocationName = locationName;
            OfficeName = officeName;
            DateTime = dateTime;
            Hours = hours;
            UserName = userName;
        }
    }
}