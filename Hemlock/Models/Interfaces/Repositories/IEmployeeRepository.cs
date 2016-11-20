using System;

namespace Hemlock.Models.Interfaces.Repositories
{
    public interface IEmployeeRepository: IDisposable
    {
        Employee GetEmployeeByEmail(string email);
        Employee GetEmployeeByID(Guid id);
        void AddStaff(Employee e);
        void UpdateStaff(Employee e);
        void Save();
    }
}
