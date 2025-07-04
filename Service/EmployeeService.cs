﻿using AutoMapper;
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

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreattionDto, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreattionDto);
            _repository.EmployeeRepository.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;
        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employee = _repository.EmployeeRepository.GetEmployeeByIdAndCompanyId(companyId, id, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(id);

            _repository.EmployeeRepository.DeleteEmployee(employee);
            _repository.Save();
        }

        public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool employeeTrackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, compTrackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _repository.EmployeeRepository.GetEmployeeByIdAndCompanyId(companyId, id, employeeTrackChanges);
            if(employeeEntity is null)
                throw new EmployeeNotFoundException(id);


            _mapper.Map(employeeForUpdateDto, employeeEntity);
            _repository.Save();
        }
    }
}
