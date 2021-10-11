namespace Sod.Core
{
    public enum Command
    {
        // read
        ZonesViolation = 0x00,
        ArmedPartitionsSuppressed = 0x09,
        OutputsState = 0x17,
        Troubles = 0x1C,
        
        // write
        OutputsSwitch = 0x91
    }
}