﻿using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _service
                .CompanyService
                .GetAllCompanies(trackChanges: false);

            return Ok(companies);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] 
            IEnumerable<Guid> ids)
        {
            var companies = _service.CompanyService.GetCompaniesById(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _service.CompanyService.GetCompany(id, trackChanges: false);
            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreatCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyDto object is null");

            var createdCompany = _service.CompanyService.CreateCompany(company);

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id, }, createdCompany);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {
            var result = _service.CompanyService.CreateCompanyCollection(companies);

            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }
    }
}
