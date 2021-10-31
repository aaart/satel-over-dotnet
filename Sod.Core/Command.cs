namespace Sod.Core
{
    public enum Command
    {
        // read
        ZonesViolation = 0x00, // inputs state (e.g.: PIR, reed switch)
        ArmedPartitionsSuppressed = 0x09,
        OutputsState = 0x17,
        Troubles = 0x1C,
        NewData = 0x7F,
        
        // write
        ArmInMode0 = 0x80,
        OutputsSwitch = 0x91,
        
        // result
        Result = 0xEF
    }
}