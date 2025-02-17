using Microsoft.AspNetCore.Mvc;

namespace NorthWindAPI.Infrastructure.Exceptions.Service
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() { }
        public CustomerNotFoundException(string msg) : base(msg) { }
        public CustomerNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
