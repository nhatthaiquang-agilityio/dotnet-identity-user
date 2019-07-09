using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityUsers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityUsers.Pages.Account
{
    public class MessageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public string userId;

        [BindProperty]
        public List<ApplicationUser> Users { get; set; }


        public MessageModel(UserManager<ApplicationUser> userManager)
        {
           
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            userId = user.Id;

            // get list connected users(exclude myself)
            Users = _userManager.Users.Where(u => u.Id != userId).ToList();

            return Page();
        }

    }
}
