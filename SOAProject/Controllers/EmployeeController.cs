
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOAProject.Data;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.EmployeeService;

namespace SOAProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployeesAsync()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
            return Ok(employeeDTOs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return Ok(employeeDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            await _employeeService.CreateEmployeeAsync(employeeDTO);
            var newEmployeeDTO = _mapper.Map<EmployeeDTO>(employeeDTO);
            return Ok(newEmployeeDTO);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateEmployeeAsync(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.EmployeeId)
            {
                return NotFound();
            }
            var employee = _mapper.Map<EmployeeDTO>(employeeDTO);
            try
            {
                await _employeeService.UpdateEmployeeAsync(id, employeeDTO);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _employeeService.DeleteEmployeeAsync(employee.EmployeeId);
            return NoContent();
        }
    }
}
