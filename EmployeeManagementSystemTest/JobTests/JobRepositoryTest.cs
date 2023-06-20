using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.JobRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;
using System.Xml.Linq;

namespace EmployeeManagementSystemTest.JobTests
{
    public class JobRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public JobRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetJobByIdAsync_ReturnsExpectedItem()
        {
            //Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var expectedJob = new Job
                {
                    JobId = 1,
                    JobName = "New Job",
                };

                context.Jobs.Add(expectedJob);
                await context.SaveChangesAsync();

                var repository = new JobRepository(context);

                //Act
                var actualJob = await repository.GetJobByIdAsync(1);

                //Assert
                Assert.Equal(expectedJob.JobId, actualJob.JobId);
                Assert.Equal(expectedJob.JobName, actualJob.JobName);
            }

        }

        [Fact]
        public async Task GetAllJobsAsync_ReturnsAllJobs()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);
                var expectedJobs = new List<Job>
                {
                    new Job
                {
                    JobId = 1,
                    JobName = "New Job 1",
                },

                new Job
                {
                    JobId = 2,
                    JobName = "New Job 2",
                },

                new Job
                {
                    JobId = 3,
                    JobName = "New Job 3",
                },
            };
                context.Jobs.AddRange(expectedJobs);
                await context.SaveChangesAsync();

                // Act
                var actualJobs = await repository.GetAllJobsAsync();

                // Assert
                Assert.Equal(expectedJobs.Count, actualJobs.Count());
                foreach (var expectedJob in expectedJobs)
                {
                    Assert.Contains(expectedJob, actualJobs);
                }
            }
        }

        [Fact]
        public async Task AddJobAsync_AddsJobToContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);
                var job = new Job
                {
                    JobId = 1,
                    JobName = "New Job",
                };

                // Act
                await repository.AddJobAsync(job);
                await context.SaveChangesAsync();

                // Assert
                var addedJob = await context.Jobs.FindAsync(job.JobId);
                Assert.NotNull(addedJob);
                Assert.Equal(job.JobId, addedJob.JobId);
                Assert.Equal(job.JobName, addedJob.JobName);
            }
        }

        [Fact]
        public async Task UpdateJob_UpdatesJobInContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);
                var job = new Job
                {
                    JobId = 1,
                    JobName = "New Job",
                };
                context.Jobs.Add(job);
                await context.SaveChangesAsync();

                // Act
                job.JobName = "Updated Job";
                repository.UpdateJob(job);
                await context.SaveChangesAsync();

                // Assert
                var updatedJob = await context.Jobs.FindAsync(job.JobId);
                Assert.Equal(job.JobName, updatedJob.JobName);
            }
        }

        [Fact]
        public async Task DeleteJob_RemovesJobFromContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);
                var job = new Job
                {
                    JobId = 1,
                    JobName = "New Job",
                };
                context.Jobs.Add(job);
                await context.SaveChangesAsync();

                // Act
                repository.DeleteJob(job);
                await context.SaveChangesAsync();

                // Assert
                var deletedJob = await context.Jobs.FindAsync(job.JobId);
                Assert.Null(deletedJob);
            }
        }

        [Fact]
        public async Task GetJobByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);

                // Act
                var result = await repository.GetJobByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllJobsAsync_ReturnsEmptyList_WhenNoJobsExist()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);

                // Act
                var result = await repository.GetAllJobsAsync();

                // Assert
                Assert.Empty(result);
            }
        }
        [Fact]
        public async Task AddJobAsync_DoesNotAddJob_WhenNullJobIsProvided()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);
                var initialCount = await context.Jobs.CountAsync();

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                {
                    await repository.AddJobAsync(null);
                    await context.SaveChangesAsync();
                });

                // Assert
                var finalCount = await context.Jobs.CountAsync();
                Assert.Equal(initialCount, finalCount);
            }
        }


        [Fact]
        public async Task DeleteJob_DoesNotThrowException_WhenDeletingNonExistingJob()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new JobRepository(context);
                var job = new Job { JobId = 1 };

                // Act & Assert
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    repository.DeleteJob(job);
                    await context.SaveChangesAsync();
                });
            }
        }

    }
}