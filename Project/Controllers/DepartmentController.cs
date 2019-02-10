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
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Offering> _offeringRepository;
        private readonly IMapper _mapper;

        public DepartmentController(
            IRepository<Department> repository,
            IRepository<Offering> ofRepository,
            IMapper mapper)
        {
            _departmentRepository = repository;
            _offeringRepository = ofRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DepartmentViewModel>> Get()
        {
            var departments = this._departmentRepository.GetAllItems(null);

            var mappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return mappedDepartments.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DepartmentViewModel> GetById(int id)
        {
            var department = _departmentRepository.GetItemById(id, null);

            if ( department == null )
            {
                return NotFound();
            }

            var mappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return mappedDepartment;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DepartmentViewModel> Create(DepartmentViewModel department, int offeringId)
        {
            if ( ModelState.IsValid )
            {
                var offering = _offeringRepository.GetItemById(offeringId, null);
                if ( offering != null )
                {
                    var newDepartment = _mapper.Map<DepartmentViewModel, Department>(department);
                    newDepartment.OfferingId = offeringId;
                    this._departmentRepository.Create(newDepartment);

                    var createdDepartment = _mapper.Map<Department, DepartmentViewModel>(newDepartment);
                    return CreatedAtAction(nameof(GetById), new { id = newDepartment.Id }, createdDepartment);
                }

                return NotFound(new { Message = "Offering not found!" });
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DepartmentViewModel> Update(int id, DepartmentViewModel department)
        {
            if ( ModelState.IsValid && department.Id == id )
            {
                var updatedDepartment = this._departmentRepository.GetItemById(department.Id);
                _mapper.Map(department, updatedDepartment);
                this._departmentRepository.Update(updatedDepartment);

                return Ok(department);
            }

            return BadRequest(department);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<DepartmentViewModel> Delete(int id)
        {
            var department = _departmentRepository.GetItemById(id, null);

            if ( department != null )
            {
                _departmentRepository.Delete(department);
                return Ok(_mapper.Map<Department, DepartmentViewModel>(department));
            }

            return NotFound();
        }
    }
}