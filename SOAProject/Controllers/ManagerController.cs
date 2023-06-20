using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;
using SOAProject.Services.ManagerService;

namespace SOAProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IMapper _mapper;

        public ManagerController(IManagerService managerService, IMapper mapper)
        {
            _managerService = managerService ?? throw new ArgumentNullException(nameof(managerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ManagerDTO>>> GetAllManagersAsync()
        {
            var managers = await _managerService.GetAllManagersAsync();
            var managerDTOs = _mapper.Map<IEnumerable<ManagerDTO>>(managers);
            return Ok(managerDTOs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ManagerDTO>> GetManagerByIdAsync(int id)
        {
            var manager = await _managerService.GetManagerByIdAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            var managerDTO = _mapper.Map<ManagerDTO>(manager);
            return Ok(managerDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ManagerDTO>> CreateManagerAsync(ManagerDTO managerDTO)
        {
            await _managerService.CreateManagerAsync(managerDTO);
            var newManagerDTO = _mapper.Map<ManagerDTO>(managerDTO);
            return Ok(newManagerDTO);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateManagerAsync(int id, ManagerDTO managerDTO)
        {
            if (id != managerDTO.ManagerId)
            {
                return BadRequest();
            }
            var manager = _mapper.Map<ManagerDTO>(managerDTO);
            try
            {
                await _managerService.UpdateManagerAsync(id, managerDTO);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteManagerAsync(int id)
        {
            var manager = await _managerService.GetManagerByIdAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            await _managerService.DeleteManagerAsync(manager.ManagerId);
            return NoContent();
        }
    }
}
