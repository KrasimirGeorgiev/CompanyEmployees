using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext)
        {
        }

        public IEnumerable<Employee> GetAllEmployees(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(x => x.Name)
            .ToList();

        public IEnumerable<Employee> GetEmployeesByCompany(Guid companyId, bool trackChanges) =>
            FindByCondition(x => x.CompanyId == companyId, trackChanges)
            .OrderBy(x => x.Name)
            .ToList();

        public Employee GetEmployee(Guid employeeId, bool trackChanges) =>
          FindByCondition(x => x.Id == employeeId, trackChanges)
            .SingleOrDefault();

        public Employee GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges) =>
            FindByCondition(x => x.CompanyId.Equals(companyId) && x.Id.Equals(employeeId), trackChanges)
            .SingleOrDefault();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
            => Delete(employee);
    }
}
