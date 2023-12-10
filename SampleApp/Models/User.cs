using System;
using System.Collections.Generic;

namespace SampleApp.Models
{
    public partial class User
    {
        public User()
        {
            Microposts = new HashSet<Micropost>();
            RelationFolloweds = new HashSet<Relation>();
            RelationFollowers = new HashSet<Relation>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordConfirmation { get; set; } = null!;
        public bool IsAdmin { get; set; }

        public virtual ICollection<Micropost> Microposts { get; set; }
        public virtual ICollection<Relation> RelationFolloweds { get; set; }
        public virtual ICollection<Relation> RelationFollowers { get; set; }
    }
}
