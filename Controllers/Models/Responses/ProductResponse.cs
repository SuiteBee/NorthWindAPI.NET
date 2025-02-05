﻿using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class ProductResponse
    {
        /// <summary>
        /// Unique product identifier
        /// </summary>
        /// <example>1</example>
        public int ProductId { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        /// <example>Chai</example>
        public required string ProductName { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        /// <example>Beverages</example>
        public required string CategoryName { get; set; }
        /// <summary>
        /// Category Description
        /// </summary>
        /// <example>Soft drinks, coffees, teas, beers, and ales</example>
        public required string CategoryDescription { get; set; }
        /// <summary>
        /// Product cost
        /// </summary>
        /// <example>18.50</example>
        public decimal ItemPrice { get; set; }
        /// <summary>
        /// Quantity of product left in stock
        /// </summary>
        /// <example>99</example>
        public int StockAmt { get; set; }
        /// <summary>
        /// Is product currently in stock
        /// </summary>
        /// <example>true</example>
        public bool InStock { get; set; }
        /// <summary>
        /// Has product been marked as discontinued
        /// </summary>
        /// <example>false</example>
        public bool Discontinued { get; set; }
        public required SupplierResponse SuppliedBy { get; set; }
    }
}
