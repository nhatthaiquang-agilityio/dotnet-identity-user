using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityUsers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityUsers.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Models.Contact> Contacts { get; set; }

        #region snippet1
        public async Task<List<Contact>> GetContactsAsync()
        {
            return await Contacts.AsNoTracking().ToListAsync();
        }
        #endregion

        #region snippet2
        public async Task<Contact> AddContactAsync(Contact contact)
        {
            await Contacts.AddAsync(contact);
            await SaveChangesAsync();
            return contact;
        }
        #endregion

        public void Initialize()
        {
            Contacts.AddRange(GetSeedingContacts());
            SaveChanges();
        }

        public static List<Contact> GetSeedingContacts()
        {
            return new List<Contact>()
            {
                new Contact()
                {
                    Name = "Nhat",
                    Address = "Nui Thanh",
                    City = "Da Nang"
                },
            };
        }
    }
}
