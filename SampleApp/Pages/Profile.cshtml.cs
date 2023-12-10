using Core.Flash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Models;
using System;

namespace SampleApp.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly SampleAppContext _db;
        private readonly ILogger<SampleAppContext> _logger;
        private readonly IFlasher _f;
        public ProfileModel(SampleAppContext db, ILogger<SampleAppContext> logger, IFlasher f)
        {
            _db = db;
            _logger = logger;
            _f = f;
        }
        
        public User currentUser { get; set; }
        public User profileUser { get; set; }
        public bool IsFollow { get; set; }

        public async Task<IActionResult> OnGetAsync([FromRoute] int? id)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            profileUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == id) as User;
            currentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id.ToString() == sessionId) as User;

            // если текущий пользователь подписан на профиль пользователя
            var result = _db.Relations.Where(r => r.Follower == currentUser && r.Followed == profileUser).FirstOrDefault();

            if (result != null)
            {
                IsFollow = true;
            }
            else
            {
                IsFollow = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromRoute] int? id)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            profileUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == id) as User;
            currentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id.ToString() == sessionId) as User;

            // если текущий пользователь подписан на профиль пользователя
            var result = _db.Relations.Where(r => r.Follower == currentUser && r.Followed == profileUser).FirstOrDefault();

            if (result != null)
            {
                IsFollow = true;
            }
            else
            {
                IsFollow = false;
            }

            if (IsFollow == false)
            {
                try
                {
                    _db.Relations.Add(new Relation() { FollowerId = currentUser.Id, FollowedId = profileUser.Id });
                    _db.SaveChanges();
                    _f.Flash(Types.Success, $"Пользователь {currentUser.Name} подписался на {profileUser.Name}!", dismissable: true);
                }
                catch (Exception ex)
                {
                    _f.Flash(Types.Success, $"{ex.InnerException.Message}", dismissable: true);
                }
            }
            else
            {

                try
                {
                    var result2 = _db.Relations.Where(r => r.Follower == currentUser && r.Followed == profileUser).FirstOrDefault();
                    _db.Relations.Remove(result2);
                    _db.SaveChanges();
                    _f.Flash(Types.Warning, $"Пользователь {currentUser.Name} отписался от {profileUser.Name}!", dismissable: true);
                }
                catch (Exception ex)
                {
                    _f.Flash(Types.Success, $"{ex.Message}", dismissable: true);
                }


            }

            return RedirectToPage();
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
