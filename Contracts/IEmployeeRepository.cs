using Entities.Models;
namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees(bool trackChanges);
        Task<IEnumerable<Employee>> GetEmployeesByCompany(Guid companyId, bool trackChanges);
        Task<Employee> GetEmployee(Guid employeeId, bool trackChanges);
        Task<Employee> GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
