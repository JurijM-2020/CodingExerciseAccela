using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Contacts.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contacts.Repository.Tests
{
    [TestClass]
    public class EFMappingTests
    {

        private static RepositoryContext _context;
        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            _context = new RepositoryContext();
            _context.Database.Log = s => Debug.WriteLine(s);

        }

        [TestMethod]
        public void PeopleMappingTest_ShouldReturnPeopleResultSet()
        {
            var result = _context.People.ToList();
            Assert.IsInstanceOfType(result, typeof(List<Person>));

        }

        [TestMethod]
        public void AddressesMappingTest_ShouldReturnAddressResultSet()
        {
            var result = _context.Addresses.ToList();
            Assert.IsInstanceOfType(result, typeof(List<Address>));
        }
    }
}
