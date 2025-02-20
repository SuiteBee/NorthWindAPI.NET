﻿namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() { }
        public ProductNotFoundException(string msg) : base(msg) { }
        public ProductNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
