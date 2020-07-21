using Contacts.Model.Entities;
using Contacts.UI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace Contacts.Repository.Tests.ViewModelTests
{
    [TestClass]
    public class EditContactViewModelTests
    {
        [TestMethod]
        public void AreAllRowsSavedTest_Pass2ValidAddresses_ShouldReturnTrue()
        {
            EditContactViewModel vm = new EditContactViewModel(null, null)
            {
                Addresses = new ObservableCollection<Address>()
                {
                    new Address { Street="a", City="s", State = "d", PostalCode="d"},
                    new Address { Street="a", City="s", State = "d", PostalCode="d"},
                }
            };

            var result = vm.AreAllRowsSaved();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreAllRowsSavedTest_Pass1InvalidAddress_ShouldReturnFalse()
        {
            EditContactViewModel vm = new EditContactViewModel(null, null)
            {
                Addresses = new ObservableCollection<Address>()
                {
                    new Address { Street="a", City="s", State = "d", PostalCode="d"},
                    new Address { Street="a", City="s", State = "d", PostalCode="d"},
                    new Address { Street="a", City="s"},
                }
            };

            var result = vm.AreAllRowsSaved();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreAllRowsSavedTest_Pass1EmptyAddress_ShouldReturnFalse()
        {
            EditContactViewModel vm = new EditContactViewModel(null, null)
            {
                Addresses = new ObservableCollection<Address>()
                {
                    new Address { Street="a", City="s", State = "d", PostalCode="d"},
                    new Address { Street="a", City="s", State = "d", PostalCode="d"},
                    new Address { Street=string.Empty, City=string.Empty, State = string.Empty, PostalCode=string.Empty},
                }
            };

            var result = vm.AreAllRowsSaved();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreAllRowsSavedTest_PassNoAddresses_ShouldReturnTrue()
        {
            EditContactViewModel vm = new EditContactViewModel(null, null);

            var result = vm.AreAllRowsSaved();

            Assert.IsTrue(result);
        }
    }
}
