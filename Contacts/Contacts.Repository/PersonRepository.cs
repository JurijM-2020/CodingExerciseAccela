using Contacts.Interfaces;
using Contacts.Model.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Contacts.Repository
{
    public class PersonRepository : RepositoryBase<Person>,  IPersonRepository
    {
        private readonly RepositoryContext _context;
        public PersonRepository(RepositoryContext context): base(context)
        {
            _context = context;
        }

        public void CreatePerson(Person person) => Create(person);

        public void DeletePerson(int personId)
        {
            Delete(personId);
        }

        public IEnumerable<Person> GetPeople(bool trackChanges) =>
            GetAll(trackChanges).OrderBy(p => p.FirstName).ToList();


        public Person GetPerson(int Id, bool trackChanges) =>
            GetByCondition(p => p.Id == Id, trackChanges).SingleOrDefault();

        public IEnumerable<Person> GetPeopleWithAddresses(bool trackChanges)
        {
            return  _context.People.Include(str => str.Addresses).ToList();
        }

        public void UpdatePerson(int personId, Person person)
        {
            Person personToUpdate = GetPerson(personId, false);
            personToUpdate.FirstName = person.FirstName;
            personToUpdate.LastName = person.LastName;
            Update(person);
        }

        public void UpdatePerson(Person person)
        {
            Update(person);
        }
    }
}
