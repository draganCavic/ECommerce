using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.DTOs
{
    public class ReviewCreateDto
    {
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}
