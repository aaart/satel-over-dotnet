namespace Sod.Infrastructure.Satel.State.Events
{
    public record FailedOutgoingChange : OutgoingChange
    {
        public FailedOutgoingChange(OutgoingChange outgoingChange) 
            : base(outgoingChange.Type, outgoingChange.Reference, outgoingChange.Value)
        {
        }
    }
}