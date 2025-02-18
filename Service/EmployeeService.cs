using AutoMapper;
using Contracts;
using Entities.Exceptions;
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

        public IEnumerable<EmployeeDto> GetEmployeesByCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository
                .CompanyRepository
                .GetCompany(companyId, trackChanges)
                ?? throw new CompanyNotFoundException(companyId);

            var employees = _repository
                .EmployeeRepository
                .GetEmployeesByCompany(companyId, trackChanges);

            var employeesDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return employeesDtos;
        }

        EmployeeDto IEmployeeService.GetEmployeeByIdAndCompanyId(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges) 
                ?? throw new CompanyNotFoundException(companyId);
            var employee = _repository.EmployeeRepository.GetEmployeeByIdAndCompanyId(companyId, employeeId, trackChanges) 
                ?? throw new EmployeeNotFoundException(employeeId);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }
    }
}
