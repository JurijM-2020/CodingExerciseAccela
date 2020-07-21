using Contacts.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Transactions;

namespace Contacts.Repository.Tests
{
    [TestClass]
    public class BaseTest
    {
        
        protected static RepositoryContext _context;
        private TransactionScope scope;
        [TestInitialize]
        public void InitializeData()
        {
            try
            {
                scope = new TransactionScope();

               
                SetInitialTest();


            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {

                        Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public virtual void SetInitialTest()
        {

        }

        public virtual void CleanUp() { }

        [TestCleanup]
        public void TestCleanUp()
        {
           scope.Dispose();
           CleanUp();
        }
    }
}
