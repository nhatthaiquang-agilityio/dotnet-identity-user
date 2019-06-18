using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUsers.Data;
using IdentityUsers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IdentityUsers.Service
{
    public class UserConnectionManager : IUserConnectionManager
    {
        private static readonly SigningCredentials SigningCreds = new SigningCredentials(
            Startup.SecurityKey, SecurityAlgorithms.HmacSha256);
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        private readonly AppDbContext _appDbContext;

        public UserConnectionManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task KeepUserConnection(string userId, string connectionId)
        {
            // save connectionId by userId
            var userConnection = new UserConnection
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

        public string Token(string userId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name, userId)
                        }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = SigningCreds
            };
            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }
    }
}
