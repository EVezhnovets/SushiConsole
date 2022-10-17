namespace SushiConsoleDev.Models
{
    public class Client
    {
        private string _name;
        private Guid _id = Guid.NewGuid();
        private string _email;
        private string _adress;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }

        public Client()
        {
            Id = _id;
        }
        public Client(string name, string email)
        {
            Id = _id;
            Name = name;
            Email = email;
        }
    }
}