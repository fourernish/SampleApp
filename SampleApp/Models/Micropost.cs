using System;
using System.Collections.Generic;

namespace SampleApp.Models
{
    public partial class Micropost
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
