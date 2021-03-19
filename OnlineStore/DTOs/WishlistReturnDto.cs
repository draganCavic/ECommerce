﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.DTOs
{
    public class WishlistReturnDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
