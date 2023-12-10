using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SampleApp.Models;
using Core.Flash;
using SampleApp.Application;

namespace SampleApp.Pages
{
    public class SignModel : PageModel
    {
        private readonly SampleAppContext _db;
        private readonly ILogger<SampleAppContext> _logger;
        private readonly IFlasher _f;
        public SignModel(SampleAppContext db, ILogger<SampleAppContext> logger, IFlasher f)
        {
            _db = db;
            _logger = logger;
            _f = f;
        }
        
        public void OnGet()
        {
        }
        public IActionResult OnPost(User user)
        {
            if(!user.IsPasswordConfirmation())
            {
                _logger.LogWarning($"Пароли пользователья {user.Name} не совпали");
                _f.Flash(Types.Warning, $"Пароли должны совпадать", dismissable: true);
                return Page();
            }

            if (!user.IsEmailUnique())
            {
                _logger.LogWarning($"Почта пользователя {user.Name} уже используется");
                _f.Flash(Types.Warning, $"Почта уже используется", dismissable: true);
                return Page();
            }

            try
            {

                _db.Users.Add(user);
                _db.SaveChanges();
                _logger.LogInformation($"Пользователь {user.Name} успешно сохранён");
                _f.Flash(Types.Success, $"Пользователь  {user.Name} зарегистрирован!", dismissable: true);
                return RedirectToPage("./Index");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _f.Flash(Types.Danger, $"Пользователь  {user.Name} не зарегистрирован", dismissable: true);
                return RedirectToPage("./Sign");
            }

            return Page();
        }
        public IActionResult YourAction()
        {
            _f.Flash(Types.Success, "Flash message system for ASP.NET MVC Core", dismissable: true);
            _f.Flash(Types.Danger, "Flash message system for ASP.NET MVC Core", dismissable: false);
            return RedirectToAction("AnotherAction");
        }
    }
}
