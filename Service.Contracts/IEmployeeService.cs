using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetEmployeesByCompany(Guid companyId, bool trackChanges);
        EmployeeDto GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges);
        EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreattionDto, bool trackChanges);
        void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
        void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool employeeTrackChanges);
        (EmployeeForUpdateDto employeeToPatch, Employee empoyeeEntity) 
            GetEmployeeForPatch(Guid companyId, Guid employeeId, bool compTrackChanges, bool employeeTrackChanges);

        void SaveChangesForPath(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}