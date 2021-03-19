using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.DTOs
{
    public class ReviewDto
    {
        public int Rate { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
