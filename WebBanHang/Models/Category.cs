﻿namespace WebBanHang.Models

{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int DisplayOrder {  get; set; }
    }
}
