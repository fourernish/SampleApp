using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Models;

namespace SampleApp.Pages
{
    public class UsersModel : PageModel
    {
        private readonly SampleAppContext _db;
        private readonly ILogger<SampleAppContext> _logger;

        public UsersModel(SampleAppContext db, ILogger<SampleAppContext> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IList<User> Users { get; set; }
        public User User { get; set; }
        public string sessionId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _db.Users.ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnGetRemoveAsync([FromQuery] int id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                _db.Users.Remove(user);
                _db.SaveChanges();
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
            return Page();
        }
    }
}
