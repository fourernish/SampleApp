using Core.Flash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Models;

namespace SampleApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SampleAppContext _db;
        private readonly IFlasher _f;

        public IndexModel(ILogger<IndexModel> logger, SampleAppContext db, IFlasher f)
        {
            _logger = logger;
            _db = db;
            _f = f;
        }
        public User currentUser { get; set; }
        public IEnumerable<User> Followeds { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Micropost> Messages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {

            var sessionId = HttpContext.Session.GetString("SampleSession");

            if (sessionId != null)
            {
                var User = await _db.Users.Include(u => u.Microposts)
                                      .Include(u => u.RelationFollowers).ThenInclude(r => r.Followed)
                                      .FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

                Followeds = User.RelationFollowers.Select(item => item.Followed).ToList();

                Users.AddRange(Followeds);
                Users.Add(User);

                foreach (var u in Users)
                {
                    _db.Entry(u).Collection(u => u.Microposts).Load();
                    Messages.AddRange(u.Microposts);
                }
                return Page();
            }
            else
            {
                return RedirectToPage("Auth");
            }

        }
        public async Task<IActionResult> OnPostAsync(string message)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            currentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

            if (!string.IsNullOrWhiteSpace(message))
            {
                var m = new Micropost()
                {
                    Content = message,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    UserId = currentUser.Id,
                    //User = this.User
                };

                try
                {
                    _db.Microposts.Add(m);
                    _db.SaveChanges();
                    _f.Flash(Types.Success, $"Tweet!", dismissable: true);
                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, $"Ошибка создания сообщения: {ex.InnerException.Message}");
                }


                return Page();
            }
            else
            {
                return Page();
            }

        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int messageid)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            currentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

            try
            {
                Micropost m = _db.Microposts.Find(messageid);
                _db.Microposts.Remove(m);
                _db.SaveChanges();
                _logger.Log(LogLevel.Error, $"Удалено сообщение \"{m.Content}\" пользователя {currentUser.Name}!");
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Ошибка удаления сообщения: {ex.InnerException}");
                _logger.Log(LogLevel.Error, $"Модель привязки из маршрута: {messageid}");
            }

            return Page();

        }
    }
}