using System;
using System.Threading.Tasks;
using App.Tests.Infrastructure;
using Initial.Models;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace App.Tests
{
    using Conferences = Initial.Features.Conferences;
    
    public class ConferenceFacts : IntegrationTests
    {
        public ConferenceFacts(ITestOutputHelper testOutputHelper, IntegrationTestsAppFactory factory) : base(testOutputHelper, factory)
        {
        }
        
        [Fact]
        public async Task ShouldCreateConference()
        {
            // Arrange
            var createConferenceCommand = new Conferences.Create.Command
            {
                Name = "Testowa konferencja"
            };
            var getConferenceQuery = new Conferences.Get.Query
            {
                Name = createConferenceCommand.Name
            };
            
            // Act
            var createConferenceResponse = await Send(createConferenceCommand);
            var getConferenceResponse = await Send(getConferenceQuery);
            
            // Assert
            createConferenceResponse.Succeeded.ShouldBeTrue();
            getConferenceResponse.Conference.ShouldNotBeNull();
            getConferenceResponse.Conference.Name.ShouldBe(createConferenceCommand.Name);
            PublishedEvents.ShouldContain<ConferenceCreatedEvent>();
        }
    }
}