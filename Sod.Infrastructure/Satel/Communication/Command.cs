namespace Sod.Infrastructure.Satel.Communication
{
    public enum Command
    {
        // read
        ZonesViolation = 0x00, // inputs state (e.g.: PIR, reed switch)
        ZonesAlarm = 0x02,
        ArmedPartitionsSuppressed = 0x09,
        ArmedPartitionsReally = 0x0A,
        OutputsState = 0x17,
        Troubles = 0x1C,
        NewData = 0x7F,
        
        // write
        ArmInMode0 = 0x80,
        DisArm = 0x84,
        ForceArmInMode0 = 0xA0,
        OutputsOn = 0x88,
        OutputsOff = 0x89,
        OutputsSwitch = 0x91,
        
        // result
        Result = 0xEF
    }
}