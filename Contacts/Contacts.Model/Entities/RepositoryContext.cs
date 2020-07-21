using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;

namespace Contacts.Model.Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() : base(GetConnection(), false)
        {
        }

        public static DbConnection GetConnection()
        {
            var connection = ConfigurationManager.ConnectionStrings["default"];
            var factory = DbProviderFactories.GetFactory(connection.ProviderName);
            var dbCon = factory.CreateConnection();
            dbCon.ConnectionString = connection.ConnectionString;
            return dbCon;
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Map(map =>
            {
                map.ToTable("Person");
                map.Property(p => p.Id).HasColumnName("Id");
                map.Property(p => p.FirstName).HasColumnName("FirstName");
                map.Property(p => p.LastName).HasColumnName("LastName");
            }).HasKey(x => x.Id)
                .Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Address>().Map(map =>
            {
                map.ToTable("Address");
                map.Property(p => p.Id).HasColumnName("Id");
                map.Property(p => p.Street).HasColumnName("Street");
                map.Property(p => p.City).HasColumnName("City");
                map.Property(p => p.State).HasColumnName("State");
                map.Property(p => p.PostalCode).HasColumnName("PostalCode");
                map.Property(p => p.PersonId).HasColumnName("PersonId");
            }).HasKey(x => x.Id)
            .Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Addresses)
                .WithRequired()
                .HasForeignKey(a => a.PersonId)
                .WillCascadeOnDelete(true);
        }
    }
}
