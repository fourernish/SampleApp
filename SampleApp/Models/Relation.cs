using System;
using System.Collections.Generic;

namespace SampleApp.Models
{
    public partial class Relation
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public int FollowedId { get; set; }

        public virtual User Followed { get; set; } = null!;
        public virtual User Follower { get; set; } = null!;
    }
}
