using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>>GetAllCompanies(bool trackChanges);
        Task<IEnumerable<CompanyDto>> GetCompaniesById(IEnumerable<Guid> ids, bool trackChanges);
        Task<CompanyDto> GetCompany(Guid companyId, bool trackChanges);
        Task<CompanyDto> CreateCompany(CompanyForCreationDto company);
        Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies);
        Task DeleteCompany(Guid companyId, bool trackChanges);
        Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);
    }
}
