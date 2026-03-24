using TicketSystem.Domain.Exceptions;

namespace TicketSystem.Domain.ValueObjects
{
    public sealed class TicketPriority
    {
        public string Value { get; }

        public TicketPriority(string value)
        {
            Value = value; 
        }

        public static TicketPriority Low() => new TicketPriority("Low");
        public static TicketPriority Medium() => new TicketPriority("Medium");
        public static TicketPriority High() => new TicketPriority("High");
        public static TicketPriority FromValue(string value)
        {
            return value switch
            {
                "Low" => Low(),
                "Medium" => Medium(),
                "High" => High(),
                _ => throw new DomainException($"Invalid TicketPriority value.")
            };
        }

        public override bool Equals(object obj)
              => obj is TicketPriority other && Value == other.Value;
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public static bool operator ==(TicketPriority a, TicketPriority b)
            => a?.Equals(b) ?? b is null;

        public static bool operator !=(TicketPriority a, TicketPriority b)
            => !(a == b);

        public override string ToString() => Value;
    }
}
