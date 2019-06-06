using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityUsers.Data;
using IdentityUsers.Models;
using Xunit;

namespace RazorPageTest
{
    public class DataAccessLayerTest
    {
        [Fact]
        public async Task GetContactsAsync()
        {
            using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
            {

                //// Arrange
                var expectContacts = AppDbContext.GetSeedingContacts();
                await db.AddRangeAsync(expectContacts);
                await db.SaveChangesAsync();

                // Act
                var result = await db.GetContactsAsync();

                // Assert
                Assert.IsAssignableFrom<List<Contact>>(result);
            }
        }


        [Fact]
        public async Task AddContactAsync()
        {
            using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
            {

                // Arrange
                var expectedContact = new Contact() 
                { 
                    Name = "John", City = "Chicago" 
                };

                // Act
                var savedContact = await db.AddContactAsync(expectedContact);

                // Assert
                var actualContact = await db.FindAsync<Contact>(savedContact.ContactId);
                Assert.Equal(expectedContact, actualContact);
            }
        }
    }
}
