using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests
{
    public class BallastLaneAppArchitectureTest
    {
        private const string DomainNamespace = "BallastLane.Domain";
        private const string ApplicationNamespace = "BallastLane.Application";
        private const string InfrastructureNamespace = "BallastLane.Infrastructure";
        private const string PersistenceNamespace = "BallastLane.Persistence";
        private const string PresentationNamespace = "BallastLane.Presentation";
        private const string AppNamespace = "BallastLane.App";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(BallastLane.Domain.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                ApplicationNamespace,
                InfrastructureNamespace,
                PersistenceNamespace,
                PresentationNamespace,
                AppNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(BallastLane.Application.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                InfrastructureNamespace,
                PersistenceNamespace,
                PresentationNamespace,
                AppNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(BallastLane.Infrastructure.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                PersistenceNamespace,
                PresentationNamespace,
                AppNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(BallastLane.Persistence.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                InfrastructureNamespace,
                PresentationNamespace,
                AppNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Presentation_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(BallastLane.Presentation.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                InfrastructureNamespace,
                PersistenceNamespace,
                AppNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }
    }
}