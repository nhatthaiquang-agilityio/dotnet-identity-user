using System;
using IdentityUsers.Data;
using Xunit;

namespace RazorPageTest
{
    public class DataAccessLayerTest
    {
        public DataAccessLayerTest()
        { 
            
        }
        [Fact]
        public void Test1()
        {
            using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var expectedMessages = AppDbContext.GetSeedingMessages();
                await db.AddRangeAsync(expectedMessages);
                await db.SaveChangesAsync();

                // Act
                var result = await db.GetMessagesAsync();

                // Assert
                var actualMessages = Assert.IsAssignableFrom<List<Message>>(result);
                Assert.Equal(
                    expectedMessages.OrderBy(m => m.Id).Select(m => m.Text),
                    actualMessages.OrderBy(m => m.Id).Select(m => m.Text));
            }
        }
    }
}
