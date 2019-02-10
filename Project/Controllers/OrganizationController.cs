using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using Project.Models;
using Project.Utilities;

namespace Project.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IRepository<Organization> _organizationRepository = null;
        private readonly IMapper _mapper = null;

        public OrganizationController(
            IRepository<Organization> organizationRepository,
            IMapper mapper)
        {
            this._organizationRepository = organizationRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrganizationViewModel>> Get()
        {
            var items = this._organizationRepository.GetAllItems(null);
            var mappedItems = this._mapper.Map<IEnumerable<Organization>, IEnumerable<OrganizationViewModel>>(items).ToList();
            return mappedItems;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrganizationDetailsViewModel> GetById(int id)
        {
            var organization = this._organizationRepository.GetItemById(id);

            if ( organization == null )
            {
                return NotFound();
            }

            var mappedOrganization = this._mapper.Map<Organization, OrganizationDetailsViewModel>(organization);
            return mappedOrganization;
        }

        [HttpGet("{id}/countries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CountryViewModel>> GetCountries(int id)
        {
            var organization = this._organizationRepository.GetItemById(
                id,
                sources => sources.Include(o => o.Countries)
            );

            if (organization != null)
            {
                return Ok(_mapper.Map<IEnumerable<Country>, IEnumerable<CountryViewModel>>(organization.Countries).ToList());
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrganizationDetailsViewModel> Create(OrganizationDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var organization = this._mapper.Map<OrganizationDetailsViewModel, Organization>(model);

                this._organizationRepository.Create(organization);
                var mappedOrganization = _mapper.Map<Organization, OrganizationDetailsViewModel>(organization);
                return CreatedAtAction(nameof(GetById), new { id = organization.Id }, organization);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrganizationDetailsViewModel> Update(OrganizationDetailsViewModel model, int id)
        {
            if (ModelState.IsValid && model.Id == id)
            {
                var updatedOrganization = this._organizationRepository.GetItemById(model.Id, null);
                this._mapper.Map(model, updatedOrganization);

                this._organizationRepository.Update(updatedOrganization);
                var mapUpdatedOrganization = this._mapper.Map<Organization, OrganizationDetailsViewModel>(updatedOrganization);
                return Ok(mapUpdatedOrganization);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrganizationDetailsViewModel> Delete(int id)
        {
            var organization = this._organizationRepository.GetItemById(id, null);

            if (organization != null)
            {
                this._organizationRepository.Delete(organization);
                var mappedItem = this._mapper.Map<Organization, OrganizationDetailsViewModel>(organization);

                return Ok(mappedItem);
            }

            return NotFound();
        }
    }
}