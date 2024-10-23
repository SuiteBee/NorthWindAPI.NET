﻿using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        public Task<Employee?> FindEmployee(int id);
        public Task<IEnumerable<Employee>> AllEmployees();
        public Task<Auth> GetUser(string usr);
        public Task<Auth?> UpdateUser(int authId, Auth user);
    }
}
