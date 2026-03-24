using TicketSystem.Domain.Exceptions;

namespace TicketSystem.Domain.ValueObjects
{
    public class ReopenLimit
    {
        public int? Value { get; }

        private ReopenLimit( int? value)
        {
            Value = value;
        }

        public static ReopenLimit Of(int value)
        {
            if (value < 0)
                throw new DomainException("Reopen limit cannot be negative");

            return new ReopenLimit(value);
        }

        public static ReopenLimit Unlimited()
        {
            return new ReopenLimit(null);
        }

        public bool IsUnlimited => Value == null;
    }
}
