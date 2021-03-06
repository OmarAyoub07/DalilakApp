using System.Collections.Generic;

namespace DalilakAPI.Models
{
    public class Reviewer
    {
        public string user_id { get; set; }
        public bool like { get; set; }
        public List<Review> reviews { get; set; }

    }
    public class Review
    {
        public string comment { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }
}