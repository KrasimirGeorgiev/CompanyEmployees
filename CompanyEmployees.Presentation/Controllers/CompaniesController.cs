using CompanyEmployees.Presentation.ModelBinders;
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
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var createdCompany = _service.CompanyService.CreateCompany(company);

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id, }, createdCompany);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            if (company is null)
                return BadRequest($"{nameof(CompanyForUpdateDto)} object is null");

            _service.CompanyService.UpdateCompany(id, company, trackChanges: true);

            return NoContent();
        }

            

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {
            if(companies is null || !companies.Any())
                return BadRequest("Company collection is null or empty");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var result = _service.CompanyService.CreateCompanyCollection(companies);

            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id)
        {
            _service.CompanyService.DeleteCompany(id, trackChanges: false);
            return NoContent();
        }
    }
}
