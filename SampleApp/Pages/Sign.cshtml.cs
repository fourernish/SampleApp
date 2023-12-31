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
                _logger.LogWarning($"������ ������������� {user.Name} �� �������");
                _f.Flash(Types.Warning, $"������ ������ ���������", dismissable: true);
                return Page();
            }

            if (!user.IsEmailUnique())
            {
                _logger.LogWarning($"����� ������������ {user.Name} ��� ������������");
                _f.Flash(Types.Warning, $"����� ��� ������������", dismissable: true);
                return Page();
            }

            try
            {

                _db.Users.Add(user);
                _db.SaveChanges();
                _logger.LogInformation($"������������ {user.Name} ������� ��������");
                _f.Flash(Types.Success, $"������������  {user.Name} ���������������!", dismissable: true);
                return RedirectToPage("./Index");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _f.Flash(Types.Danger, $"������������  {user.Name} �� ���������������", dismissable: true);
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
