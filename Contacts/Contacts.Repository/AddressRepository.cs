using System.Collections.Generic;
using System.Linq;
using Contacts.Interfaces;
using Contacts.Model.Entities;

namespace Contacts.Repository
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(RepositoryContext context) : base (context)
        { }

        public void CreateAddress(Address address) => Create(address);

        public void DeleteAddress(int addressId) => Delete(addressId);
        

        public Address GetAddress(int personId, int id, bool trackChanges) =>
            GetByCondition(a => a.PersonId == personId && a.Id==id, trackChanges).SingleOrDefault();


        public IEnumerable<Address> GetAddresses(int personId, bool trackChanges) =>
            GetByCondition(a => a.PersonId == personId, trackChanges)
            .OrderBy(s => s.State).ToList();


        public void UpdateAddress(Address address)
        {
            Update(address);
        }
    }
}
