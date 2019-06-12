using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IdentityUsers.Models
{
    public class UserConnection
    {   
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ConnectionId { get; set; }

        [Required]
        public bool Status { get; set; }

    }
}
