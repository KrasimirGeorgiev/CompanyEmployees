using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployeesByCompany(Guid companyId, bool trackChanges);
        Task<EmployeeDto> GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreattionDto, bool trackChanges);
        Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
        Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool employeeTrackChanges);
        Task<(EmployeeForUpdateDto employeeToPatch, Employee empoyeeEntity)>
            GetEmployeeForPatch(Guid companyId, Guid employeeId, bool compTrackChanges, bool employeeTrackChanges);
        Task SaveChangesForPath(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}