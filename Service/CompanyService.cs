using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges)
        {
            var companies = await _repository
                .CompanyRepository
                .GetAllCompanies(trackChanges);

            var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companyDto;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesById(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids == null)
                throw new IdParametersBadRequestException();

            var companies = await _repository
                .CompanyRepository
                .GetCompaniesById(ids, trackChanges);

            if (companies.Count() != ids.Count())
                throw new CollectionByIdsBadRequestException();

            var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companyDto;
        }

        public async Task<CompanyDto> GetCompany(Guid companyId, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompany(companyId, trackChanges) 
                ?? throw new CompanyNotFoundException(companyId);
            
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task<CompanyDto> CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.CompanyRepository.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies)
        {
            if (companies == null)
                throw new CompanyCollectionBadRequestException();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companies);
            foreach (var entity in companyEntities)
            {
                _repository.CompanyRepository.CreateCompany(entity);
            }

            await _repository.SaveAsync();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(x => x.Id));
            return (companies: companyCollectionToReturn, ids: ids);
        }

        public async Task DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            _repository.CompanyRepository.DeleteCompany(company);
            await _repository.SaveAsync();
        }

        public async Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyEntity = await _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (companyEntity == null)
                throw new CompanyNotFoundException(companyId);

            _mapper.Map(companyForUpdate, companyEntity);
            await _repository.SaveAsync();
        }
    }
}
