using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;
using SOAProject.Services.DepartmentService;

namespace SOAProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            var departmentDTOs = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
            return Ok(departmentDTOs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DepartmentDTO>> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(departmentDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<DepartmentDTO>> CreateDepartmentAsync(DepartmentDTO departmentDTO)
        {
            await _departmentService.CreateDepartmentAsync(departmentDTO);
            var newDepartmentDTO = _mapper.Map<DepartmentDTO>(departmentDTO);
            return Ok(newDepartmentDTO);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateDepartmentAsync(int id, DepartmentDTO departmentDTO)
        {
            if (id != departmentDTO.DepartmentId)
            {
                return BadRequest();
            }
            var department = _mapper.Map<DepartmentDTO>(departmentDTO);
            try
            {
                await _departmentService.UpdateDepartmentAsync(id, departmentDTO);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteDepartmentAsync(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            await _departmentService.DeleteDepartmentAsync(department.DepartmentId);
            return NoContent();
        }
    }
}
