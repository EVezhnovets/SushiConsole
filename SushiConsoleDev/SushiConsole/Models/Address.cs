namespace SushiConsoleDev.Models
{
    public class Address
    {
        public string Street { get; set; }
        public int NumberOfHouse { get; set; }
        public string Building { get; set; }
        public string NumberOfApartment { get; set; }

        public Address()
        {
        }
        public Address(string street, int numberOfHouse, string building, string numberOfApartment)
        {
            Street = street;
            NumberOfHouse = numberOfHouse;
            Building = building;
            NumberOfApartment = numberOfApartment;
        }
    }
}
