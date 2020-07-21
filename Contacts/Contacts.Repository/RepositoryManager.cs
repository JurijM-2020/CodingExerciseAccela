using Contacts.Interfaces;
using Contacts.Model.Entities;

namespace Contacts.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private IPersonRepository _personRepository;
        private IAddressRepository _addressRepository;
        private RepositoryContext _repositoryContext;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }


        public IPersonRepository Person
        {
            get
            {
                if(_personRepository == null)
                {
                    _personRepository = new PersonRepository(_repositoryContext);
                }
                return _personRepository;
            }
        }

        public IAddressRepository Address
        {
            get
            {
                if (_addressRepository == null)
                {
                    _addressRepository = new AddressRepository(_repositoryContext);
                }
                return _addressRepository;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
        
    }
}
