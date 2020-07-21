namespace Contacts.Model.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}