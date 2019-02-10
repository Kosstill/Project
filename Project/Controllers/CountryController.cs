using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Utilities;
using AutoMapper;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IMapper _mapper;

        public CountryController(
            IRepository<Country> repository,
            IRepository<Organization> orgRepository,
            IMapper mapper)
        {
            _countryRepository = repository;
            _organizationRepository = orgRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CountryViewModel>> Get()
        {
            var countries = this._countryRepository.GetAllItems(null);

            var mappedCountries = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryViewModel>>(countries);
            return mappedCountries.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CountryViewModel> GetById(int id)
        {
            var country = _countryRepository.GetItemById(id, null);

            if (country == null)
            {
                return NotFound();
            }

            var mappedCountry = _mapper.Map<Country, CountryViewModel>(country);
            return mappedCountry;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CountryDetailsViewModel> Create(CountryDetailsViewModel country, int organizationId)
        {
            var organization = _organizationRepository.GetItemById(organizationId, null);
            if (ModelState.IsValid)
            {
                if (organization != null)
                {
                    var newCountry = _mapper.Map<CountryDetailsViewModel, Country>(country);
                    newCountry.OrganizationId = organizationId;
                    this._countryRepository.Create(newCountry);

                    var createdCountry = _mapper.Map<Country, CountryDetailsViewModel>(newCountry);
                    return CreatedAtAction(nameof(GetById), new { id = newCountry.Id }, createdCountry);
                }

                return NotFound(new { Message = "Organization not found!" });
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CountryViewModel> Update(int id, CountryViewModel country)
        {
            if (ModelState.IsValid && country.Id == id)
            {
                var updatedCountry = _countryRepository.GetItemById(country.Id, null);
                _mapper.Map(country, updatedCountry);
                this._countryRepository.Update(updatedCountry);

                return Ok(country);
            }

            return BadRequest(country);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Country> Delete(int id)
        {
            var country = _countryRepository.GetItemById(id, null);

            if (country != null)
            {
                _countryRepository.Delete(country);
                return Ok(_mapper.Map<Country, CountryViewModel>(country));
            }

            return NotFound();
        }
    }
}
