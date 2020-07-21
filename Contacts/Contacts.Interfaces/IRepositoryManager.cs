namespace Contacts.Interfaces
{
    public interface IRepositoryManager
    {
        IPersonRepository Person { get; }
        IAddressRepository Address { get; }
        void Save();
    }
}
