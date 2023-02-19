using Xunit;
using FakeItEasy;
using FluentAssertions;
using SilentApp.Domain.Entities;
using SilentApp.FunctionsApp.Services.Commands;
using SilentApp.FunctionsApp.Services.Commands.Handlers;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.FunctionsApp.Tests
{
    public class RegisterSubscriptionCommandHandlerTests
    {
        private readonly IAzureTableDataProvider _dataProvider;
        private readonly RegisterSubscriptionCommandHandler _handler;

        public RegisterSubscriptionCommandHandlerTests()
        {
            _dataProvider = A.Fake<IAzureTableDataProvider>();
            _handler = new RegisterSubscriptionCommandHandler(_dataProvider);
        }

        [Fact]
        public async Task HandleAsync_InsertsNewSubscription_WhenSubscriptionDoesNotExist()
        {
            // Arrange
            var command = new RegisterSubscriptionCommand(123, "location1");

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            A.CallTo(() => _dataProvider.UpsertRecord(A<AlertNotificationSubscription>._)).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<RequestResult>();
        }

        [Fact]
        public async Task HandleAsync_UpdatesExistingSubscription_WhenSubscriptionExists()
        {
            // Arrange
            var command = new RegisterSubscriptionCommand(456, "location2");

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            A.CallTo(() => _dataProvider.UpsertRecord(A<AlertNotificationSubscription>._)).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<RequestResult>();
        }

        [Fact]
        public async Task HandleAsync_ReturnsError_WhenDataProviderThrowsException()
        {
            // Arrange
            A.CallTo(() => _dataProvider.UpsertRecord(A<AlertNotificationSubscription>._)).Throws<Exception>();
            var command = new RegisterSubscriptionCommand(789, "location3");

            // Act
            RequestResult result;
            try
            {
                result = await _handler.HandleAsync(command);
            }
            catch (Exception ex)
            {
                result = new RequestResult(new Error(ErrorType.InternalError, "EXCEPTION", ex.Message));
            }

            // Assert
            result.Should().BeOfType<RequestResult>().Which.IsSuccessful.Should().BeFalse();
        }

    }

}