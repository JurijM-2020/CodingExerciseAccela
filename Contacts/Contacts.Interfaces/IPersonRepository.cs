using Contacts.Model.Entities;
using System.Collections.Generic;

namespace Contacts.Interfaces
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetPeople(bool trackChanges);
        IEnumerable<Person> GetPeopleWithAddresses(bool trackChanges);
        Person GetPerson(int Id, bool trackChanges);
        void CreatePerson(Person person);
        void UpdatePerson(int personId, Person person);
        void UpdatePerson(Person person);
        void DeletePerson(int personId);
    }
}
