using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId:guid}/employees")]
    public class EmployeesControllercs : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmployeesControllercs(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get(Guid companyId) 
        {
            var employees = _service.EmployeeService.GetEmployeesByCompany(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{employeeId:guid}")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var employee = _service.EmployeeService.GetEmployeeByIdAndCompanyId(companyId, employeeId, trackChanges: false);
            return Ok(employee);
        }
    }
}
