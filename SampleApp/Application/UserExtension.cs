using SampleApp.Models;

namespace SampleApp.Application
{
    public static class UserExtension
    {
        public static bool IsPasswordConfirmation(this User user)
        {
            return (user.Password == user.PasswordConfirmation) ? true : false;
        }
        public static bool IsEmailUnique(this User newUser)
        {
            using(SampleAppContext db = new SampleAppContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Email == newUser.Email);
                return user != null ? false : true;
            }
        }
    }
}
