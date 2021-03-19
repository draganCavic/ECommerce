﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public User User { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }

    }
}
