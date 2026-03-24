using FluentAssertions;
using Moq;
using TicketSystem.Application.Interfaces;
using TicketSystem.Application.UseCases;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Policies;
using TicketSystem.Domain.ValueObjects;

public class ReopenTicketUseCaseTests
{
    private readonly Mock<ITicketRepository> _repositoryMock;
    private readonly Mock<IReopenPolicy> _policyMock;

    public ReopenTicketUseCaseTests()
    {
        _repositoryMock = new Mock<ITicketRepository>();
        _policyMock = new Mock<IReopenPolicy>();
    }

    [Fact]
    public async Task Should_Reopen_Ticket_When_Status_Is_Closed()
    {
        // Arrange
        var ticket = Ticket.Create(
            "Printer broken",
            "Printer not responding",
            TicketPriority.Medium());

        ticket.StartProgress();
        ticket.Close();

        var ticketId = ticket.Id;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(ticketId))
            .ReturnsAsync(ticket);

        _policyMock
            .Setup(p => p.GetLimit(ticket, It.IsAny<DateTime>()))
            .Returns(ReopenLimit.Unlimited());

        var useCase = new ReopenTicketUseCase(
            _repositoryMock.Object,
            _policyMock.Object);

        // Act
        await useCase.ExecuteAsync(ticketId);

        // Assert
        ticket.Status.Should().Be(TicketStatus.Open());

        //_repositoryMock.Verify(r => r.UpdateAsync(ticket), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}