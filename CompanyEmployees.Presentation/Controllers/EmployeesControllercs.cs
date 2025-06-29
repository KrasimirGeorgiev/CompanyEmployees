using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var employee = _service.EmployeeService.GetEmployeeByIdAndCompanyId(companyId, employeeId, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateAnEmployeeForCompany(Guid companyId, [FromBody]EmployeeForCreationDto employee)
        {
            if (employee == null)
                return BadRequest("Employee for creation object is null.");

            var employeeToReturn = _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]EmployeeForUpdateDto employee)
        {
            if (employee == null)
                return BadRequest($"{nameof(EmployeeForUpdateDto)} object is null");

            _service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, compTrackChanges: false, employeeTrackChanges: true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid companyId, Guid id) 
        {
            _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);

            return NoContent();
        }
    }
}
