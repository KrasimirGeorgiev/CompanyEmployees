using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesByCompany(Guid companyId, bool trackChanges) =>
            await FindByCondition(x => x.CompanyId == companyId, trackChanges)
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<Employee> GetEmployee(Guid employeeId, bool trackChanges) =>
          await FindByCondition(x => x.Id == employeeId, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<Employee> GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges) =>
            await FindByCondition(x => x.CompanyId.Equals(companyId) && x.Id.Equals(employeeId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);
    }
}
