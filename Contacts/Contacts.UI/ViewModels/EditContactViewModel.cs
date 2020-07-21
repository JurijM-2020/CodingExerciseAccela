using Contacts.Interfaces;
using Contacts.Model.Entities;
using Contacts.UI.Infrastructure;
using Contacts.UI.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Contacts.UI.ViewModels
{
    public class EditContactViewModel
    {
        public delegate void CloseView();
        public Person PersonToEdit { get; set; }

        public int? PersonId { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public ObservableCollection<Address> Addresses { get; set; }
        public Address SelectedAddress { get; set; }

        public CommandHandler AddAddress { get; set; }
        public CommandHandler SaveAddress { get; set; }
        public CommandHandler RemoveAddress { get; set; }
        public CommandHandler SubmitCommand { get; set; }
        public CommandHandler SavePerson { get; set; }

        private readonly IRepositoryManager _repository;

        private bool IsDirty { get; set; }
        private bool IsAddressUpdating { get; set; }
        private bool IsNewRowAdded { get; set; }
        private readonly EntryType entryType;
        private readonly CloseView closeView;

        private List<Address> addressesToCreate = new List<Address>();
        private List<Address> addressesToUpdate = new List<Address>();
        private List<Address> addressesToDelete = new List<Address>();

        public EditContactViewModel(IRepositoryManager repositoryManager, CloseView closeAction)
        {
            entryType = EntryType.NewEntry;
            _repository = repositoryManager;
            Addresses = new ObservableCollection<Address>();
            BindButtons();
            closeView = closeAction;
        }

        public EditContactViewModel(IRepositoryManager repositoryManager, Person person, CloseView closeAction)
        {
            entryType = EntryType.Existing;
            _repository = repositoryManager;
            PersonToEdit = person;
            PersonFirstName = person.FirstName;
            PersonLastName = person.LastName;
            PersonId = person.Id;
            if (person.Addresses?.Count > 0)
            {
                Addresses = person.Addresses.ToObservableCollection();
            }
            else
            {
                Addresses = new ObservableCollection<Address>();
            }
            
            BindButtons();
            closeView = closeAction;
        }

        private void BindButtons()
        {
            RemoveAddress = new CommandHandler(RemoveAddressAction, CanRemoveAddress);
            SubmitCommand = new CommandHandler(SubmitAction, CanSubmit);
            AddAddress = new CommandHandler(AddAddressAction, CanAddAddress);
            SaveAddress = new CommandHandler(SaveAddressAction, CanSaveÀddress);
            SavePerson = new CommandHandler(SavePersonAction, CanSavePerson);
        }

        private bool CanSavePerson()
        {
            return true;
        }

        private void SavePersonAction()
        {
            if (string.IsNullOrEmpty(PersonFirstName))
            {
                MessageBox.Show("Please enter First name.");
            }

            if (string.IsNullOrEmpty(PersonLastName))
            {
                MessageBox.Show("Please enter Last name.");
            }
            IsDirty = true;
        }

        private bool CanSaveÀddress()
        {
            return SelectedAddress != null;
        }

        private void SaveAddressAction()
        {
            if (SelectedAddress != null)
            {
                if (string.IsNullOrEmpty(SelectedAddress.Street))
                {
                    MessageBox.Show("Please enter Street.");
                    return;
                }

                if (string.IsNullOrEmpty(SelectedAddress.City))
                {
                    MessageBox.Show("Please enter City.");
                    return;
                }
                if (string.IsNullOrEmpty(SelectedAddress.State))
                {
                    MessageBox.Show("Please enter State.");
                    return;
                }

                if (string.IsNullOrEmpty(SelectedAddress.PostalCode))
                {
                    MessageBox.Show("Please enter PostalCode.");
                    return;
                }

                if (SelectedAddress.Id == default(int))
                {
                    addressesToCreate.Add(SelectedAddress);
                }

                if (SelectedAddress.Id > 0)
                {
                    addressesToUpdate.Add(SelectedAddress);
                }

                SelectedAddress = null;
                IsDirty = true;
                IsAddressUpdating = false;
                IsNewRowAdded = false;
            }
        }

        private bool CanAddAddress()
        {
            return !string.IsNullOrEmpty(PersonFirstName) 
                && !string.IsNullOrEmpty(PersonLastName) 
                && !IsAddressUpdating
                && AreAllRowsSaved();
        }

        private void AddAddressAction()
        {
            Addresses.Add(new Address());
            IsAddressUpdating = true;
            IsNewRowAdded = true;
        }

        private bool CanSubmit()
        {
            return (!string.IsNullOrEmpty(PersonFirstName) && !string.IsNullOrEmpty(PersonLastName)) 
                && IsDirty 
                && !IsAddressUpdating
                && AreAllRowsSaved();
        }

        private void SubmitAction()
        {
            if(entryType == EntryType.NewEntry)
            {
                Person personToSave = new Person()
                {
                    FirstName = PersonFirstName,
                    LastName = PersonLastName,
                    Addresses = Addresses
                };
                if (PersonId.HasValue)
                {
                    personToSave.Id = PersonId.Value;
                }
                _repository.Person.CreatePerson(personToSave);
                _repository.Save();
            }
            else
            {
                UpdatePersonRecord();
                SubmitAddresses();
            }
            
            closeView();
        }

        private void SubmitAddresses()
        {
            foreach (var address in addressesToCreate)
            {
                address.PersonId = PersonToEdit.Id;
                _repository.Address.CreateAddress(address);
            }
            _repository.Save();
            foreach (var address in addressesToDelete)
            {
                _repository.Address.DeleteAddress(address.Id);
            }
            _repository.Save();
            foreach (var address in addressesToUpdate)
            {
                _repository.Address.UpdateAddress(address);
            }
            _repository.Save();
        }

        private void UpdatePersonRecord()
        {
            if (PersonFirstName != PersonToEdit.FirstName || PersonLastName != PersonToEdit.LastName)
            {
                var tempPerson = new Person
                {
                    Id = PersonToEdit.Id,
                    FirstName = PersonFirstName,
                    LastName = PersonLastName
                };
                _repository.Person.UpdatePerson(tempPerson);
                _repository.Save();
            }
        }

        private bool CanRemoveAddress()
        {
            return SelectedAddress != null;
        }

        private void RemoveAddressAction()
        {
            if (SelectedAddress.Id > 0)
            {
                addressesToDelete.Add(SelectedAddress);
            }

            Addresses.Remove(SelectedAddress);
            IsDirty = true;
            IsAddressUpdating = false;
            IsNewRowAdded = false;
        }

        public bool AreAllRowsSaved()
        {
            return (Addresses?.Count > 0) ? !Addresses.Any(n => string.IsNullOrEmpty(n.Street)
            || string.IsNullOrEmpty(n.City)
            || string.IsNullOrEmpty(n.State)
            || string.IsNullOrEmpty(n.PostalCode)) : true;
        }
    }
}
