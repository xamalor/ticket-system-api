using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Exceptions;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Tests.Domain
{
    public class TicketTests
    {
        // ============================
        // TEST DE CREACION DE TICKET
        // ============================
        [Fact]
        public void Should_Create_Ticket_With_Open_Status()
        {
            //Arrange
            var ticket = Ticket.Create(
                "Printer broken",
                "Printer not responding",
                TicketPriority.Medium());

            //Assert
            ticket.Status.Should().Be(TicketStatus.Open());
            ticket.ReopenCount.Should().Be(0);
        }

        // ===============================
        // TEST DE TRANSICION DE ESTADO
        // ===============================
        [Fact]
        public void Should_Move_To_InProgress_When_StartProgress()
        {
            var ticket = Ticket.Create(
                "Printer broken",
                "Printer not responding",
                TicketPriority.Medium());

            ticket.StartProgress();

            ticket.Status.Should().Be(TicketStatus.InProgress());
        }

        // ===============================
        // TEST DE CIERRE DE TICKET
        // ===============================
        [Fact]
        public void Should_Close_Ticket_When_InProgress()
        {
            var ticket = Ticket.Create(
                "Printer broken",
                "Printer not responding",
                TicketPriority.Medium());

            ticket.StartProgress();
            ticket.Close();

            ticket.Status.Should().Be(TicketStatus.Closed());
        }

        // ===============================
        // TEST DE REGLA DE NEGOCIO
        // Solo tickets InProgress pueden cerrarse
        // ===============================
        [Fact]
        public void Should_Throw_When_Closing_An_Open_Ticket()
        {
            var ticket = Ticket.Create(
                "Printer broken",
                "Printer not responding",
                TicketPriority.Medium());

            var action = () => ticket.Close();

            action.Should().Throw<DomainException>();
        }

        // ===============================
        // TEST DE REOPEN 
        // ===============================
        [Fact]
        public void Should_Reopen_Closed_Ticket()
        {
            var ticket = Ticket.Create(
                "Printer broken",
                "Printer not responding",
                TicketPriority.Medium());

            ticket.StartProgress();
            ticket.Close();

            ticket.Reopen(ReopenLimit.Unlimited());

            ticket.Status.Should().Be(TicketStatus.Open());
            ticket.ReopenCount.Should().Be(1);
        }

        // ===============================
        // TEST DE DOMAIN EVENT
        // ===============================
        [Fact]
        public void Should_Add_Domain_Event_When_Reopenend()
        {
            var ticket = Ticket.Create(
                "Printer broken",
                "Printer not responding",
                TicketPriority.Medium());

            ticket.StartProgress();
            ticket.Close();

            ticket.Reopen(ReopenLimit.Unlimited());
            ticket.DomainEvents.Should().ContainSingle();
        }

    }
}
