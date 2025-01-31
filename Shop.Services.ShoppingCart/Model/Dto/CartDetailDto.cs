﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Shop.Services.ShoppingCart.Model.Dto
{
    public class CartDetailDto
    {
    
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
    
        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
       
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
