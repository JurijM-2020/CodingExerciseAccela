using Contacts.Interfaces;
using Contacts.Model.Entities;
using Contacts.UI.Infrastructure;
using Contacts.UI.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Contacts.UI.ViewModels
{
    public class MainContactsViewModel : INotifyPropertyChanged
    {
        private static readonly object ItemsLock = new object();
        public ObservableCollection<Person> Contacts { get; set; }
        public Person SelectedPerson { get; set; }
        public CommandHandler AddNewCommand { get; set; }
        public CommandHandler EditCommand { get; set; }
        public CommandHandler DeleteCommand { get; set; }

        private readonly IRepositoryManager _repository;

        public MainContactsViewModel(IRepositoryManager repositoryManager)
        {
            _repository = repositoryManager;
            AddNewCommand = new CommandHandler(AddNewAction, CanAddNew);
            EditCommand = new CommandHandler(EditAction, CanEdit);
            DeleteCommand = new CommandHandler(DeleteAction, CanDelete);
            Contacts = GetPeople();
            InfoMessage = $"Total number of persons: {Contacts.Count}";
            BindingOperations.EnableCollectionSynchronization(Contacts, ItemsLock);
        }

        private bool CanDelete()
        {
            return SelectedPerson != null;
        }

        private void DeleteAction()
        {
            if (MessageBox.Show($"Are you sure you want to delete person '{SelectedPerson.FirstName} {SelectedPerson.LastName}' ?","Delete person", MessageBoxButton.YesNo,MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                _repository.Person.DeletePerson(SelectedPerson.Id);

                LoadContactList(GetPeople());
            }
        }

        private ObservableCollection<Person> GetPeople()
        {
            return _repository.Person.GetPeopleWithAddresses(false).ToObservableCollection();
        }

        public bool CanAddNew()
        {
            return true;
        }
        
        public void AddNewAction()
        {

            ContactWindow window = new ContactWindow();
            var viewModel = new EditContactViewModel(_repository, window.Close);
            window.DataContext = viewModel;
    
            window.ShowDialog();

            LoadContactList(GetPeople());
        }

        public bool CanEdit()
        {
            return SelectedPerson != null;
        }

        public void EditAction()
        {
            ContactWindow window = new ContactWindow();
            var viewModel = new EditContactViewModel(_repository, SelectedPerson, window.Close);
            
            window.DataContext = viewModel;
            
            window.ShowDialog();
            LoadContactList(GetPeople());
        }

        private void LoadContactList(IEnumerable<Person> newList)
        {
            Contacts.Clear();
            foreach (var shoe in newList)
            {
                Contacts.Add(shoe);
            }
            InfoMessage = $"Total number of persons: {Contacts.Count}";
        }

        private string _infoMessage;

        public string InfoMessage
        {
            get { return _infoMessage; }
            set
            {
                if (_infoMessage == value)
                {
                    return;
                }

                _infoMessage = value;
                RaisePropertyChanged("InfoMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
