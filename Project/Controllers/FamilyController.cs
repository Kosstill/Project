using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.Utilities;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly IRepository<Family> _familyRepository;
        private readonly IRepository<Business> _businessRepository;
        private readonly IMapper _mapper;

        public FamilyController(
            IRepository<Family> repository,
            IRepository<Business> bsRepository,
            IMapper mapper)
        {
            _familyRepository = repository;
            _businessRepository = bsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<FamilyViewModel>> Get()
        {
            var families = this._familyRepository.GetAllItems(null);

            var mappedFamilies = _mapper.Map<IEnumerable<Family>, IEnumerable<FamilyViewModel>>(families);
            return mappedFamilies.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<FamilyViewModel> GetById(int id)
        {
            var family = _familyRepository.GetItemById(id, null);

            if ( family == null )
            {
                return NotFound();
            }

            var mappedFamily = _mapper.Map<Family, FamilyViewModel>(family);
            return mappedFamily;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FamilyViewModel> Create(FamilyViewModel family, int businessId)
        {
            if ( ModelState.IsValid )
            {
                var business = _businessRepository.GetItemById(businessId, null);
                if ( business != null )
                {
                    var newFamily = _mapper.Map<FamilyViewModel, Family>(family);
                    newFamily.BusinessId = businessId;
                    this._familyRepository.Create(newFamily);

                    var createdFamily = _mapper.Map<Family, FamilyViewModel>(newFamily);
                    return CreatedAtAction(nameof(GetById), new { id = newFamily.Id }, createdFamily);
                }

                return NotFound(new { Message = "Business not found!" });
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FamilyViewModel> Update(int id, FamilyViewModel family)
        {
            if ( ModelState.IsValid && family.Id == id )
            {
                var updatedFamily = this._familyRepository.GetItemById(family.Id, null);
                _mapper.Map(family, updatedFamily);
                this._familyRepository.Update(updatedFamily);

                return Ok(family);
            }

            return BadRequest(family);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<FamilyViewModel> Delete(int id)
        {
            var family = _familyRepository.GetItemById(id, null);

            if ( family != null )
            {
                _familyRepository.Delete(family);
                return Ok(_mapper.Map<Family, FamilyViewModel>(family));
            }

            return NotFound();
        }
    }
}