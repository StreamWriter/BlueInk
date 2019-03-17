using BlueInk.API.Data;
using BlueInk.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using BlueInk.API.Controllers;

namespace BlueInk.Tests
{
    public class ProjectTypesControllerTests
    {
        [Fact]
        public void GetProjectTypes_ReturnsAllProjectTypesFromDb()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("GetProjectTypes_ReturnsAllProjectTypesFromDb").Options;

            using var context = new BlueInkDbContext(options);
            context.ProjectTypes.Add(new ProjectType { Name = "Software" });
            context.ProjectTypes.Add(new ProjectType { Name = "Music" });
            context.SaveChanges();

            var controller = new ProjectTypesController(context);

            var result = controller.GetProjectTypes() as OkObjectResult;
            Assert.NotNull(result);

            var content = result.Value as IEnumerable<ProjectType>;
            Assert.Equal(2, content.Count());
            Assert.Equal("Software", content.First().Name);
        }

        [Fact]
        public async void GetProjectType_ReturnsProjectType_WhenIdIsValid()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("GetProjectType_ReturnsProjectType_WhenIdIsValid").Options;

            using var context = new BlueInkDbContext(options);
            context.ProjectTypes.Add(new ProjectType { Id = 1, Name = "Software" });
            context.ProjectTypes.Add(new ProjectType { Id = 2, Name = "Music" });
            context.SaveChanges();

            var controller = new ProjectTypesController(context);

            var result = (await controller.GetProjectType(2)) as OkObjectResult;
            Assert.NotNull(result);

            var content = result.Value as ProjectType;
            Assert.Equal(2, content.Id);
            Assert.Equal("Music", content.Name);
        }

        [Fact]
        public async void GetProjectType_ReturnsNotFound_WhenIdIsOutOfRange()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("GetProjectType_ReturnsNotFound_WhenIdIsInvalid").Options;

            using var context = new BlueInkDbContext(options);
            context.ProjectTypes.Add(new ProjectType { Id = 1, Name = "Software" });
            context.SaveChanges();

            var controller = new ProjectTypesController(context);

            var result = (await controller.GetProjectType(2)) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void CreateProjectType_ReturnsNewEntity_WhenDataIsValid()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("CreateProjectType_ReturnsNewEntity_WhenDataIsValid").Options;

            var projectType = new ProjectType { Name = "Software", Description = "This is software I created." };

            using var context = new BlueInkDbContext(options);
            var controller = new ProjectTypesController(context);

            var result = (await controller.CreateProjectType(projectType)) as OkObjectResult;
            Assert.NotNull(result);

            var content = result.Value as ProjectType;
            Assert.Equal(projectType.Name, content.Name);
            Assert.Equal(projectType.Description, content.Description);
        }

        [Fact]
        public void CreateProjectType_DoesNotOverwriteData_WhenIdIsSupplied()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("CreateProjectType_DoesNotOverwriteData_WhenIdIsSupplied").Options;

            using var context = new BlueInkDbContext(options);
            context.ProjectTypes.Add(new ProjectType { Id = 1, Name = "Software" });
            context.SaveChanges();

            var controller = new ProjectTypesController(context);

            var newProjectType = new ProjectType() { Id = 1, Name = "Music" };
            controller.CreateProjectType(newProjectType);

            var projectWithId1 = context.ProjectTypes.Find(1);
            Assert.Equal("Software", projectWithId1.Name);

            var softwareProject = context.ProjectTypes.SingleOrDefault(p => p.Name == "Software");
            Assert.Equal(1, softwareProject.Id);
        }

        [Fact]
        public async void UpdateProjectType_ReturnsUpdatedEntity_WhenDataIsValid()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("UpdateProjectType_ReturnsUpdatedEntity_WhenDataIsValid").Options;

            using var context = new BlueInkDbContext(options);
            var projectType = new ProjectType { Id = 1, Name = "Software" };
            context.ProjectTypes.Add(projectType);
            context.SaveChanges();

            projectType.Name = "Music";
            var controller = new ProjectTypesController(context);

            var response = (await controller.UpdateProjectType(1, projectType)) as OkObjectResult;
            Assert.NotNull(response);

            var content = response.Value as ProjectType;
            Assert.Equal("Music", content.Name);
        }

        [Fact]
        public async void UpdateProjectType_ReturnsNotFound_WhenNonexistentEntityIsSupplied()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("UpdateProjectType_ReturnsNotFound_WhenNonexistentEntityIsSupplied").Options;

            using var context = new BlueInkDbContext(options);
            var projectType = new ProjectType { Id = 1, Name = "Software" };
            context.SaveChanges();

            var entityToSupply = new ProjectType { Id = 2, Name = "Music" };
            var controller = new ProjectTypesController(context);

            var response = (await controller.UpdateProjectType(2, entityToSupply)) as NotFoundResult;
            Assert.NotNull(response);
        }

        [Fact]
        public async void DeleteProjectType_ReturnsOk_WhenEntityIsDeleted()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("DeleteProjectType_ReturnsOk_WhenEntityIsDeleted").Options;

            using var context = new BlueInkDbContext(options);
            var projectType = new ProjectType { Id = 1, Name = "Software" };
            context.ProjectTypes.Add(projectType);
            context.SaveChanges();

            var controller = new ProjectTypesController(context);

            var response = (await controller.DeleteProjectType(1)) as OkResult;
            Assert.NotNull(response);
        }

        [Fact]
        public async void DeleteProjectType_ReturnsNotFound_WhenNonexistentEntityIsSupplied()
        {
            var options = new DbContextOptionsBuilder<BlueInkDbContext>()
                .UseInMemoryDatabase("DeleteProjectType_ReturnsNotFound_WhenNonexistentEntityIsSupplied").Options;

            using var context = new BlueInkDbContext(options);
            var controller = new ProjectTypesController(context);

            var response = (await controller.DeleteProjectType(1)) as NotFoundResult;
            Assert.NotNull(response);
        }

    }
}
