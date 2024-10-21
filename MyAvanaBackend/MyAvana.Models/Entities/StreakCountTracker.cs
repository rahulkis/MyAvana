using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class StreakCountTracker
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int StreakCount { get; set; }
    }
}
