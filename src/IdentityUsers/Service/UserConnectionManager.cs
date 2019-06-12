using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityUsers.Data;
using IdentityUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityUsers.Service
{
    public class UserConnectionManager : IUserConnectionManager
    {
        private readonly AppDbContext _appDbContext;

        public UserConnectionManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task KeepUserConnection(string userId, string connectionId)
        {
            // save connectionId by userId
            var userConnection = new UserConnection()
            {
                UserId = userId,
                ConnectionId = connectionId,
                Status = true
            };
            await _appDbContext.UserConnections.AddAsync(userConnection);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task RemoveUserConnection(string connectionId)
        {
            //Remove the connectionId of user
            var connections = await _appDbContext.UserConnections.AsNoTracking()
            .Where(i => i.ConnectionId == connectionId).ToListAsync();

            foreach (var connection in connections)
            {
                _appDbContext.UserConnections.Remove(connection);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            return _appDbContext.UserConnections
                .Where(i => i.UserId == userId)
                .Select(p => p.ConnectionId).ToList();
        }
    }
}
