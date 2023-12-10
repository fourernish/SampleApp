using Core.Flash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Models;
using SampleApp.Application;

namespace SampleApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly SampleAppContext _db;
        private readonly IFlasher _f;
        private readonly ILogger<EditModel> _log;

        public EditModel(SampleAppContext context, IFlasher f, ILogger<EditModel> log)
        {
            _db = context;
            _f = f;
            _log = log;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            User = await _db.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            //User.IsAdmin = true;
            _db.Attach(User).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserExists(int id)
        {
            return _db.Users.Any(e => e.Id == id);
        }

    }
}
