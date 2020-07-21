using Contacts.Model.Entities;
using System.Collections.Generic;

namespace Contacts.Interfaces
{
    public interface IAddressRepository
    {
        IEnumerable<Address> GetAddresses(int personId, bool trackChanges);
        Address GetAddress(int personId, int id,  bool trackChanges);
        void CreateAddress(Address address);
        void UpdateAddress(Address address);
        void DeleteAddress(int addressId);
    }
}
