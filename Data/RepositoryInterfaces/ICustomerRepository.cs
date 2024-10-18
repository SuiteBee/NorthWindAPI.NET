﻿using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer?> FindCustomer(string id);
    }
}
