using Core.Flash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Models;

namespace SampleApp.Pages
{
    public class FollowersModel : PageModel
    {
        #region 
        private readonly SampleAppContext _context;
        private readonly ILogger<FollowersModel> _logger;
        private readonly IFlasher _f;
        public FollowersModel(SampleAppContext context, ILogger<FollowersModel> logger, IFlasher f)
        {
            _context = context;
            _logger = logger;
            _f = f;
        }
        #endregion

        public User ProfileUser { get; set; }
        public IEnumerable<User> Followers { get; set; }

        public void OnGet(int id)
        {
            ProfileUser = _context.Users.Include(u => u.RelationFolloweds)
                                        .ThenInclude(r => r.Follower)
                                        .Where(u => u.Id == id)
                                        .FirstOrDefault();

            Followers = ProfileUser.RelationFolloweds.Select(item => item.Follower).ToList();

        }
    }
}
