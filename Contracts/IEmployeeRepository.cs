using Entities.Models;
namespace Contracts
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);
        public IEnumerable<Employee> GetEmployeesByCompany(Guid companyId, bool trackChanges);
        Employee GetEmployee(Guid employeeId, bool trackChanges);
        Employee GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);
    }
}
