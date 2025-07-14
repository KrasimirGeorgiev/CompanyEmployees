using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByCompany(Guid companyId, bool trackChanges)
        {
            var company = await _repository
                .CompanyRepository
                .GetCompany(companyId, trackChanges)
                ?? throw new CompanyNotFoundException(companyId);

            var employees = await _repository
                .EmployeeRepository
                .GetEmployeesByCompany(companyId, trackChanges);

            var employeesDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return employeesDtos;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompany(companyId, trackChanges)
                ?? throw new CompanyNotFoundException(companyId);
            var employee = await _repository.EmployeeRepository.GetEmployeeByIdAndCompanyId(companyId, employeeId, trackChanges)
                ?? throw new EmployeeNotFoundException(employeeId);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }

        public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreattionDto, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreattionDto);
            _repository.EmployeeRepository.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;
        }

        public async Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employee = await _repository.EmployeeRepository.GetEmployeeByIdAndCompanyId(companyId, id, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(id);

            _repository.EmployeeRepository.DeleteEmployee(employee);
            await _repository.SaveAsync();
        }

        public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool employeeTrackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompany(companyId, compTrackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = await _repository.EmployeeRepository.GetEmployeeByIdAndCompanyId(companyId, id, employeeTrackChanges);
            if(employeeEntity is null)
                throw new EmployeeNotFoundException(id);


            _mapper.Map(employeeForUpdateDto, employeeEntity);
            await _repository.SaveAsync();
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee empoyeeEntity)> GetEmployeeForPatch(Guid companyId, Guid employeeId, bool compTrackChanges, bool employeeTrackChanges)
        {
            var companyEntity = await _repository.CompanyRepository.GetCompany(companyId, compTrackChanges);
            if (companyEntity is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = await _repository.EmployeeRepository.GetEmployee(employeeId, employeeTrackChanges);
            if(employeeEntity is null)
                throw new EmployeeNotFoundException(employeeId);

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch, employeeEntity);
        }

        public async Task SaveChangesForPath(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }
    }
}
