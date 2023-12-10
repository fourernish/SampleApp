using Core.Flash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Models;

namespace SampleApp.Pages
{
    public class FollowedsModel : PageModel
    {
        #region 
        private readonly SampleAppContext _context;
        private readonly ILogger<FollowedsModel> _logger;
        private readonly IFlasher _f;
        public FollowedsModel(SampleAppContext context, ILogger<FollowedsModel> logger, IFlasher f)
        {
            _context = context;
            _logger = logger;
            _f = f;
        }
        #endregion

        public User ProfileUser { get; set; }
        public IEnumerable<User> Followeds { get; set; }

        public void OnGet(int id)
        {
            ProfileUser = _context.Users.Include(u => u.RelationFollowers)
                                        .ThenInclude(r => r.Followed)
                                        .Where(u => u.Id == id)
                                        .FirstOrDefault();

            Followeds = ProfileUser.RelationFollowers.Select(item => item.Followed).ToList();

        }
    }
}
