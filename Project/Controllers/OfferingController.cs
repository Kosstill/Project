using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using Project.Utilities;
using Project.Models;

namespace Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class OfferingController : ControllerBase
    {
        private readonly IRepository<Offering> _offeringRepository;
        private readonly IRepository<Family> _familyRepository;
        private readonly IMapper _mapper;

        public OfferingController(
            IRepository<Offering> repository,
            IRepository<Family> fmRepository,
            IMapper mapper)
        {
            _offeringRepository = repository;
            _familyRepository = fmRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OfferingViewModel>> Get()
        {
            var offerings = this._offeringRepository.GetAllItems(null);

            var mappedOfferings = _mapper.Map<IEnumerable<Offering>, IEnumerable<OfferingViewModel>>(offerings);
            return mappedOfferings.ToList();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OfferingViewModel> GetById(int id)
        {
            var offering = _offeringRepository.GetItemById(id, null);

            if ( offering == null )
            {
                return NotFound();
            }

            var mappedOffering = _mapper.Map<Offering, OfferingViewModel>(offering);
            return mappedOffering;
        }

        [AllowAnonymous]
        [HttpGet("{id}/departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DepartmentViewModel>> GetDepartments(int id)
        {
            var offering = this._offeringRepository.GetItemById(
                id,
                sources => sources.Include(o => o.Departments)
            );

            if ( offering != null )
            {
                return Ok(_mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(offering.Departments).ToList());
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OfferingViewModel> Create(OfferingViewModel offering, int familyId)
        {
            if ( ModelState.IsValid )
            {
                var family = _familyRepository.GetItemById(familyId, null);
                if ( family != null )
                {
                    var newOffering = _mapper.Map<OfferingViewModel, Offering>(offering);
                    newOffering.FamilyId = familyId;
                    this._offeringRepository.Create(newOffering);

                    var createdOffering = _mapper.Map<Offering, OfferingViewModel>(newOffering);
                    return CreatedAtAction(nameof(GetById), new { id = newOffering.Id }, createdOffering);
                }

                return NotFound(new { Message = "Family not found!" });
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OfferingViewModel> Update(int id, OfferingViewModel offering)
        {
            if ( ModelState.IsValid && offering.Id == id )
            {
                var updatedOffering = this._offeringRepository.GetItemById(offering.Id);
                _mapper.Map(offering, updatedOffering);
                this._offeringRepository.Update(updatedOffering);

                return Ok(offering);
            }

            return BadRequest(offering);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<OfferingViewModel> Delete(int id)
        {
            var offering = _offeringRepository.GetItemById(id, null);

            if ( offering != null )
            {
                _offeringRepository.Delete(offering);
                return Ok(_mapper.Map<Offering, OfferingViewModel>(offering));
            }

            return NotFound();
        }
    }
}