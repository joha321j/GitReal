namespace TimeRegistrationLibrary
{
    public class Address
    {
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }

        public Address(string streetName, string streetNumber, string postCode, string city)
        {
            StreetName = streetName;
            StreetNumber = streetNumber;
            PostCode = postCode;
            City = city;
        }

        public override string ToString()
        {
            return StreetName + " " + StreetNumber + " " + PostCode + " " + City;
        }
    }
}