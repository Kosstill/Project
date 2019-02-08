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
        private readonly IRepository<Country> _countryRepository = null;
        private readonly IMapper _mapper = null;

        public OrganizationController(
            IRepository<Organization> organizationRepository,
            IRepository<Country> countryRepository,
            IMapper mapper)
        {
            this._organizationRepository = organizationRepository;
            this._countryRepository = countryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
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
            var organization = this._organizationRepository.GetItemById(id, null);

            if (organization == null)
            {
                return NotFound();
            }

            var mappedOrganization = this._mapper.Map<Organization, OrganizationDetailsViewModel>(organization);

            return mappedOrganization;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrganizationDetailsViewModel> Create(OrganizationDetailsViewModel model, [FromQuery]int[] countriesId)
        {
            if (ModelState.IsValid)
            {
                var organization = this._mapper.Map<OrganizationDetailsViewModel, Organization>(model);

                if (countriesId.Length != 0)
                {
                    var countries = this._countryRepository.GetAllItems(null).Where(c => countriesId.Contains(c.Id));
                    if (countries != null)
                        organization.Countries = countries.ToList();
                }

                this._organizationRepository.Create(organization);
                return CreatedAtAction(nameof(GetById), new { id = organization.Id }, organization);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<OrganizationDetailsViewModel> Update(
            OrganizationDetailsViewModel model, 
            int id,
            [FromQuery]int[] countriesId)
        {
            if (ModelState.IsValid && model.Id == id)
            {
                var updatedOrganization = this._organizationRepository.GetItemById(model.Id, (sources) => sources.Include(o => o.Countries));
                this._mapper.Map(model, updatedOrganization);
                
                if (countriesId.Length != 0)
                {
                    var countries = this._countryRepository.GetAllItems(null).Where(c => countriesId.Contains(c.Id));
                    if (countries != null)
                    {
                        updatedOrganization.Countries = countries.ToList();
                    }
                }

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
            var item = this._organizationRepository.GetItemById(id, null);

            if (item != null)
            {
                this._organizationRepository.Delete(item);
                var mappedItem = this._mapper.Map<Organization, OrganizationDetailsViewModel>(item);

                return Ok(mappedItem);
            }

            return NotFound();
        }
    }
}