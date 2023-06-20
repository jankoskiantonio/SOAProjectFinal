using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;
using SOAProject.Services.JobService;

namespace SOAProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobController(IJobService jobService, IMapper mapper)
        {
            _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetAllJobsAsync()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            var jobDTOs = _mapper.Map<IEnumerable<JobDTO>>(jobs);
            return Ok(jobDTOs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<JobDTO>> GetJobByIdAsync(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            var jobDTO = _mapper.Map<JobDTO>(job);
            return Ok(jobDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<JobDTO>> CreateJobAsync(JobDTO jobDTO)
        {
            await _jobService.CreateJobAsync(jobDTO);
            var newJobDTO = _mapper.Map<JobDTO>(jobDTO);
            return Ok(newJobDTO);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateJobAsync(int id, JobDTO jobDTO)
        {
            if (id != jobDTO.JobId)
            {
                return BadRequest();
            }
            var job = _mapper.Map<JobDTO>(jobDTO);
            try
            {
                await _jobService.UpdateJobAsync(id, jobDTO);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteJobAsync(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            await _jobService.DeleteJobAsync(job.JobId);
            return NoContent();
        }


    }
}
