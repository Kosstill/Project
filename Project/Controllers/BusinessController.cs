using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using Project.Utilities;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IRepository<Business> _businessRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IMapper _mapper;

        public BusinessController(
            IRepository<Business> repository,
            IRepository<Country> cntRepository,
            IMapper mapper)
        {
            _businessRepository = repository;
            _countryRepository = cntRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<BusinessViewModel>> Get()
        {
            var businesses = this._businessRepository.GetAllItems(null);

            var mappedBusinesses = _mapper.Map<IEnumerable<Business>, IEnumerable<BusinessViewModel>>(businesses);
            return mappedBusinesses.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BusinessViewModel> GetById(int id)
        {
            var business = _businessRepository.GetItemById(id, null);

            if ( business == null )
            {
                return NotFound();
            }

            var mappedBusiness = _mapper.Map<Business, BusinessViewModel>(business);
            return mappedBusiness;
        }

        [HttpGet("{id}/families")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<FamilyViewModel>> GetFamilies(int id)
        {
            var business = this._businessRepository.GetItemById(
                id,
                sources => sources.Include(b => b.Families)
            );

            if ( business != null )
            {
                return Ok(_mapper.Map<IEnumerable<Family>, IEnumerable<FamilyViewModel>>(business.Families).ToList());
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BusinessViewModel> Create(BusinessViewModel business, int countryId)
        {
            if ( ModelState.IsValid )
            {
                var country = _countryRepository.GetItemById(countryId, null);
                if ( country != null )
                {
                    var newBusiness = _mapper.Map<BusinessViewModel, Business>(business);
                    newBusiness.CountryId = countryId;
                    this._businessRepository.Create(newBusiness);

                    var createdBusiness = _mapper.Map<Business, BusinessViewModel>(newBusiness);
                    return CreatedAtAction(nameof(GetById), new { id = newBusiness.Id }, createdBusiness);
                }

                return NotFound(new { Message = "Country not found!" });
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BusinessViewModel> Update(int id, BusinessViewModel business)
        {
            if ( ModelState.IsValid && business.Id == id )
            {
                var updatedBusiness = _businessRepository.GetItemById(business.Id, null);
                _mapper.Map(business, updatedBusiness);
                this._businessRepository.Update(updatedBusiness);

                return Ok(business);
            }

            return BadRequest(business);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<BusinessViewModel> Delete(int id)
        {
            var business = _businessRepository.GetItemById(id, null);

            if ( business != null )
            {
                _businessRepository.Delete(business);
                return Ok(_mapper.Map<Business, BusinessViewModel>(business));
            }

            return NotFound();
        }
    }
}