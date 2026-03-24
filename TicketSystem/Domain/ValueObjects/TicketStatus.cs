using TicketSystem.Domain.Exceptions;

namespace TicketSystem.Domain.ValueObjects
{
    public sealed class TicketStatus
    {
        public string Value { get; }
        private TicketStatus(string value)
        {
            Value = value;
        }

        public static TicketStatus Open() => new TicketStatus("Open");
        public static TicketStatus InProgress() => new TicketStatus("InProgress");
        public static TicketStatus Closed() => new TicketStatus("Closed");

        public static TicketStatus FromValue(string value)
        {
            return value switch
            {
                "Open" => Open(),
                "InProgress" => InProgress(),
                "Closed" => Closed(),
                _ => throw new DomainException($"Invalid TicketStatus: {value}")
            };
        }

        public override bool Equals(object? obj)
        {
            if(obj is not TicketStatus other)
                return false;

            return Value == other.Value;
        }
        public override int GetHashCode() 
        { 
            return Value.GetHashCode();
        }

        public static bool operator ==(TicketStatus a, TicketStatus b)
            => a?.Equals(b) ?? b is null;

        public static bool operator !=(TicketStatus a, TicketStatus b)
            => !(a == b);

        public override string ToString() => Value;
    }
}
