using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityUsers.Service
{
    public interface IUserConnectionManager
    {
        Task KeepUserConnection(string userId, string connectionId);
        Task RemoveUserConnection(string connectionId);
        List<string> GetUserConnections(string userId);
    }
}
