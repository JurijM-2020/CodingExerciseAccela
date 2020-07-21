using Contacts.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Contacts.Repository.Tests
{
    [TestClass]
    public class RepositoryTests : BaseTest
    {
        private static RepositoryManager _repo;
        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            _context = new RepositoryContext();
            _context.Database.Log = s => Debug.WriteLine(s);
            _repo = new RepositoryManager(_context);
        }

        [TestMethod]
        public void InsertPersonAndAddressData_ShouldAdd1PersonAnd1Address()
        {
            CreateXPeopleAndYAddressRecords();

            var result1 = _context.People.ToList();
            Assert.IsInstanceOfType(result1, typeof(List<Person>));
            Assert.IsTrue(result1.Count > 0);

            var result2 = _context.Addresses.ToList();
            Assert.IsInstanceOfType(result2, typeof(List<Address>));
            Assert.IsTrue(result2.Count > 0);
        }

        [TestMethod]
        public void PersonRepositoryManagerTest_GetPeople_ShouldReturnPersonRecord()
        {
            CreateXPeopleAndYAddressRecords(2,1);

            var result = _repo.Person.GetPeople(false);
            Assert.IsInstanceOfType(result, typeof(List<Person>));
            Assert.AreEqual(2, result.Count(),"Should return 2 resords");
        }

        [TestMethod]
        public void PersonRepositoryManagerTest_GetPeopleWithAddresses_ShouldReturnPeopleWithAddressesRecords()
        {
            CreateXPeopleAndYAddressRecords(2, 2);

            var result = _repo.Person.GetPeopleWithAddresses(false);
            Assert.IsInstanceOfType(result, typeof(List<Person>));
            Assert.AreEqual(2, result.Count(), "Should return 2 resords");
        }

        [TestMethod]
        public void PersonRepositoryManagerTest_GetSinglePerson_ShouldReturn1PersonRecord()
        {
            CreateXPeopleAndYAddressRecords(3, 1);
            var allPeople = _repo.Person.GetPeople(false).ToList();

            var singlePerson = _repo.Person.GetPerson(allPeople[0].Id, false);

            Assert.IsInstanceOfType(singlePerson, typeof(Person));
        }


        [TestMethod]
        public void PersonRepositoryManagerTest_PassNonExistingPersonId_ShouldReturnNull()
        {
            var singlePerson = _repo.Person.GetPerson(0, false);

            Assert.IsNull(singlePerson);
        }

        [TestMethod]
        public void AddressRepositoryManagerTest_2People3AndAddresses_ShouldReturn3Records()
        {
            CreateXPeopleAndYAddressRecords(2, 3);
            var allPeople = _repo.Person.GetPeople(false).ToList();

            var result = _repo.Address.GetAddresses(allPeople[0].Id, false);

            Assert.IsInstanceOfType(result, typeof(List<Address>));
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void AddressRepositoryManagerTest_PassNonExistingPersonId_ShouldReturnEmptyResultSet()
        {
            CreateXPeopleAndYAddressRecords(2, 3);

            var result = _repo.Address.GetAddresses(0, false);

            Assert.IsInstanceOfType(result, typeof(List<Address>));
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void AddressRepositoryManagerTest_PassNonExistingPersonIdAndAddressId_ShouldReturnNull()
        {
            CreateXPeopleAndYAddressRecords(2, 3);

            var result = _repo.Address.GetAddress(0, 0, false);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CreatePersonTest_PassPersonEntry_ShouldInsertPersonRecord()
        {
            var peopleBefore = _repo.Person.GetPeople(false).Count();

            var p = new Person
            {
                FirstName = "aaa",
                LastName = "aaa"
            };

            _repo.Person.CreatePerson(p);
            _repo.Save();
            var peopleAfter = _repo.Person.GetPeople(false).Count();

            Assert.AreEqual(1, peopleAfter - peopleBefore);
        }

        [TestMethod]
        public void CreatePersonTest_PassPersonAnd2Addresses_ShouldInsertPersonAnd2AddressesRecord()
        {
            var p = new Person
            {
                FirstName = "paa",
                LastName = "laa",
                Addresses = new[]
                {
                    new Address{Street="s1",City="c1",State="st1",PostalCode="p1"},
                    new Address{Street="s2",City="c2",State="st2",PostalCode="p2"},
                }
            };

            _repo.Person.CreatePerson(p);
            _repo.Save();

            var people = _repo.Person.GetPeople(false).ToList();
            var addresses = _repo.Address.GetAddresses(people[people.Count-1].Id, false).ToList();

            Assert.AreEqual(1, people.Count);
            Assert.AreEqual(2, addresses.Count);
            Assert.AreEqual(people[0].FirstName, "paa");
            Assert.AreEqual(people[0].LastName, "laa");
            Assert.IsTrue(addresses.Any(s => s.Street.Equals("s1", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(addresses.Any(s => s.Street.Equals("s2", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void DeletePesonTest_Create2PesonRecords_ShouldDelete1Record()
        {
            CreateXPeopleAndYAddressRecords(2, 0);
            var people = _repo.Person.GetPeople(false).ToList();
            Assert.AreEqual(2, people.Count, "Should be 2 records created.");

            _repo.Person.DeletePerson(people[0].Id);
            _repo.Save();

            var peopleAfterDelete = _repo.Person.GetPeople(false).ToList();

            Assert.AreEqual(1, people.Count - peopleAfterDelete.Count,"1 record should be deleted.");
        }

        [TestMethod]
        public void DeletePesonTest_Create2PesonAnd2AddressesRecords_ShouldDelete1PersonAnd2AddressesRecords()
        {
            CreateXPeopleAndYAddressRecords(2, 2);
            var people = _repo.Person.GetPeopleWithAddresses(false).ToList();
            Assert.AreEqual(2, people.Count, "Should be 2 people records created.");

            var p1AddressesCount = _repo.Address.GetAddresses(people[0].Id, false).Count();
            var p2AddressesCount = _repo.Address.GetAddresses(people[1].Id, false).Count();

            Assert.AreEqual(2, p1AddressesCount, "Should be 2 Address records for person1 created.");
            Assert.AreEqual(2, p2AddressesCount, "Should be 2 Address records for person2 created.");

            _repo.Person.DeletePerson(people[0].Id);
            _repo.Save();

            var peopleAfterDelete = _repo.Person.GetPeopleWithAddresses(false).ToList();

            Assert.AreEqual(1, people.Count - peopleAfterDelete.Count, "1 record should be deleted.");
            var p1AddressesAfterCount = _repo.Address.GetAddresses(people[0].Id, false).Count();
            var p2AddressesAfterCount = _repo.Address.GetAddresses(people[1].Id, false).Count();

            Assert.AreEqual(2, p1AddressesCount - p1AddressesAfterCount,"Should be 2 deleted records");
            Assert.AreEqual(0, p2AddressesCount - p2AddressesAfterCount, "Should be 0 deleted records");
        }

        [TestMethod]
        public void UpdatePersonTest_PassFirstNameAndLastName_ShouldUpdateRecord()
        {
            CreateXPeopleAndYAddressRecords(1, 0);

            var people = _repo.Person.GetPeople(true).ToList();
            Assert.AreEqual(1, people.Count);

            Person p = people[0];
            p.FirstName = "TestTest";
            p.LastName = "UnitTest";

            _repo.Person.UpdatePerson(p);
            _repo.Save();

            var result = _repo.Person.GetPerson(p.Id, false);

            Assert.AreEqual("TestTest", result.FirstName);
            Assert.AreEqual("UnitTest", result.LastName);
        }

        [TestMethod]
        public void CreateAddressTest_Pass1Address_ShouldCreate1AaddressRecord()
        {
            CreateXPeopleAndYAddressRecords(1, 0);

            var people = _repo.Person.GetPeopleWithAddresses(false).ToList();

            Assert.AreEqual(1, people.Count, "Should be 1 person");
            Assert.AreEqual(0, people[0].Addresses.Count, "Should be no addresses");

            var newAddress = new Address
            {
                Street = "teststreet",
                City = "testcity",
                State = "teststate",
                PostalCode = "testpc",
                PersonId = people[0].Id
            };

            _repo.Address.CreateAddress(newAddress);
            _repo.Save();

            var peopleAfter = _repo.Person.GetPeopleWithAddresses(false).ToList();

            Assert.AreEqual(1, peopleAfter.Count, "Should be 1 person");
            Assert.AreEqual(1, peopleAfter[0].Addresses.Count, "Should be 1 address");
            Assert.AreEqual("teststreet", peopleAfter[0].Addresses.First().Street);
            Assert.AreEqual("testcity", peopleAfter[0].Addresses.First().City);
            Assert.AreEqual("teststate", peopleAfter[0].Addresses.First().State);
            Assert.AreEqual("testpc", peopleAfter[0].Addresses.First().PostalCode);
        }

        [TestMethod]
        public void DeleteAddressTest_Create1PersonWith2Addresses_ShoulDelete1Address()
        {
            CreateXPeopleAndYAddressRecords(1, 2);

            var people = _repo.Person.GetPeopleWithAddresses(false).ToList();

            Assert.AreEqual(1, people.Count, "Should be 1 person");
            Assert.AreEqual(2, people[0].Addresses.Count, "Should be 2 addresses");

            var addressToDelete = people[0].Addresses.First();

            _repo.Address.DeleteAddress(addressToDelete.Id);
            _repo.Save();
        }

        [TestMethod]
        public void UpdateAddressTest_Create1PersonWith2Addresses_ShoulUpdate1Address()
        {
            CreateXPeopleAndYAddressRecords(1, 2);

            var people = _repo.Person.GetPeopleWithAddresses(false).ToList();

            Assert.AreEqual(1, people.Count, "Should be 1 person");
            Assert.AreEqual(2, people[0].Addresses.Count, "Should be 2 addresses");

            var addressToUpdate = people[0].Addresses.First();
            addressToUpdate.Street = "test_street";
            addressToUpdate.City = "test_city";
            addressToUpdate.State = "test_state";
            addressToUpdate.PostalCode = "test_pc";

            _repo.Address.UpdateAddress(addressToUpdate);
            _repo.Save();

            var peopleAfterUpdate = _repo.Person.GetPeopleWithAddresses(false).ToList();

            Assert.AreEqual(1, peopleAfterUpdate.Count, "Should be 1 person");
            Assert.AreEqual(2, peopleAfterUpdate[0].Addresses.Count, "Should be 2 addresses");

            var updatedAddress = peopleAfterUpdate[0].Addresses.SingleOrDefault(a => a.Id == addressToUpdate.Id);

            Assert.IsNotNull(updatedAddress);
            Assert.AreEqual("test_street", updatedAddress.Street);
            Assert.AreEqual("test_city",   updatedAddress.City);
            Assert.AreEqual("test_state",  updatedAddress.State);
            Assert.AreEqual("test_pc", updatedAddress.PostalCode);

        }

        #region Helper

        private void CreateXPeopleAndYAddressRecords(int numOfPeople=1, int numOfAddressesPerPerson=1)
        {
            for (int i = 0; i < numOfPeople; i++)
            {
                var person = new Person
                {
                    FirstName = "aaa",
                    LastName = "aaa"
                };
                _context.People.Add(person);
                _context.SaveChanges();
                if (numOfAddressesPerPerson == 0) continue;
                for (int y = 0; y < numOfAddressesPerPerson; y++)
                {
                    var address = new Address
                    {
                        Street = "123",
                        City = "aaa",
                        State = "sss",
                        PostalCode = "a12",
                        PersonId = person.Id
                    };
                    _context.Addresses.Add(address);
                    _context.SaveChanges();
                }
            }

        }
#endregion

    }
}

