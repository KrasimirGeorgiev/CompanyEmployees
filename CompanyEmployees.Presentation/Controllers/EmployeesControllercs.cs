﻿using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> Get(Guid companyId)
        {
            var employees = await _service.EmployeeService.GetEmployeesByCompany(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var employee = await _service.EmployeeService.GetEmployeeByIdAndCompanyId(companyId, employeeId, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee == null)
                return BadRequest("Employee for creation object is null.");

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var employeeToReturn = await _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee == null)
                return BadRequest($"{nameof(EmployeeForUpdateDto)} object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, compTrackChanges: false, employeeTrackChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PatchEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document is null");

            var result = await _service.EmployeeService.GetEmployeeForPatch(companyId, id, compTrackChanges: false, employeeTrackChanges: true);
            patchDoc.ApplyTo(result.employeeToPatch, ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EmployeeService.SaveChangesForPath(result.employeeToPatch, result.empoyeeEntity);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid companyId, Guid id)
        {
            await _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);

            return NoContent();
        }
    }
}
