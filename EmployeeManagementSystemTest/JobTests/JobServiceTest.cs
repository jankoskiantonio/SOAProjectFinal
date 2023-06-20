using AutoMapper;
using Moq;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;
using SOAProject.Services.JobService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagementSystemTest.JobTests
{
    public class JobServiceTest
    {
        [Fact]
        public async Task GetAllJobsAsync_ReturnsAllJobs()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var jobs = new List<Job>
            {
                new Job
                {
                    JobId = 1,
                    JobName = "Test Job 1"
                },
                new Job
                {
                    JobId = 2,
                    JobName = "Test Job 2"
                },
                new Job
                {
                    JobId = 3,
                    JobName = "Test Job 3"
                },
            };

            mockUnitOfWork.Setup(uow => uow.JobRepository.GetAllJobsAsync()).ReturnsAsync(jobs);

            var jobDTOs = new List<JobDTO>
            {
                new JobDTO
                {
                    JobId = 1,
                    JobName = "Test Job 1"
                },
                new JobDTO
                {
                    JobId = 2,
                    JobName = "Test Job 2"
                },
                new JobDTO
                {
                    JobId = 3,
                    JobName = "Test Job 3"
                },
            };

            mockMapper.Setup(mapper => mapper.Map<IEnumerable<JobDTO>>(jobs)).Returns(jobDTOs);

            var jobService = new JobService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await jobService.GetAllJobsAsync();

            // Assert
            Assert.Equal(jobDTOs, result);
        }

        [Fact]
        public async Task GetJobByIdAsync_ExistingId_ReturnsJob()
        {
            // Arrange
            int jobId = 1;
            var job = new Job
            {
                JobId = 1,
                JobName = "Test Job 1"
            };
            var jobDTO = new JobDTO
            {
                JobId = 1,
                JobName = "Test Job 1"
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.JobRepository.GetJobByIdAsync(jobId)).ReturnsAsync(job);
            mockMapper.Setup(mapper => mapper.Map<JobDTO>(job)).Returns(jobDTO);

            var jobService = new JobService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await jobService.GetJobByIdAsync(jobId);

            // Assert
            Assert.Equal(jobDTO, result);
        }

        [Fact]
        public async Task CreateJobAsync_ValidJob_ReturnsNewJob()
        {
            // Arrange
            var job = new Job
            {
                JobId = 1,
                JobName = "Test Job 1"
            };
            var jobDTO = new JobDTO
            {
                JobId = 1,
                JobName = "Test Job 1"
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(mapper => mapper.Map<Job>(jobDTO)).Returns(job);
            mockUnitOfWork.Setup(uow => uow.JobRepository.AddJobAsync(job)).Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();
            mockMapper.Setup(mapper => mapper.Map<JobDTO>(job)).Returns(jobDTO);

            var jobService = new JobService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await jobService.CreateJobAsync(jobDTO);

            // Assert
            Assert.Equal(jobDTO, result);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateJobAsync_ExistingId_UpdatesJob()
        {
            // Arrange
            int jobId = 1;
            var existingJob = new Job
            {
                JobId = 1,
                JobName = "Test Job 1"
            };
            var jobDTO = new JobDTO
            {
                JobId = 1,
                JobName = "Test Job 1"
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.JobRepository.GetJobByIdAsync(jobId)).ReturnsAsync(existingJob);
            mockMapper.Setup(mapper => mapper.Map(jobDTO, existingJob));

            var jobService = new JobService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await jobService.UpdateJobAsync(jobId, jobDTO);

            // Assert
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteJobAsync_ExistingId_DeletesJob()
        {
            // Arrange
            int jobId = 1;
            var existingJob = new Job
            {
                JobId = 1,
                JobName = "Test Job 1"
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.JobRepository.GetJobByIdAsync(jobId)).ReturnsAsync(existingJob);
            mockUnitOfWork.Setup(uow => uow.JobRepository.DeleteJob(existingJob));
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();

            var jobService = new JobService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await jobService.DeleteJobAsync(jobId);

            // Assert
            mockUnitOfWork.Verify(uow => uow.JobRepository.DeleteJob(existingJob), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetJobByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int jobId = 1;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.JobRepository.GetJobByIdAsync(jobId)).ReturnsAsync((Job)null);

            var jobService = new JobService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await jobService.GetJobByIdAsync(jobId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateJobAsync_NonExistingId_ThrowsException()
        {
            // Arrange
            int jobId = 1;
            var jobDTO = new JobDTO
            {
                JobId = 1,
                JobName = "TestJob 1"
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.JobRepository.GetJobByIdAsync(jobId)).ReturnsAsync((Job)null);

            var jobService = new JobService(mockUnitOfWork.Object, mockMapper.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => jobService.UpdateJobAsync(jobId, jobDTO));

            mockUnitOfWork.Verify(uow => uow.JobRepository.GetJobByIdAsync(jobId), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}
