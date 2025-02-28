﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Shared.Models;

namespace Webshop.Shared.Models
{
    public class Product
    {
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }
		public decimal Price { get; set; }
		public required string ImageUrl { get; set; }
		public int Stock { get; set; }
	}

    
}
