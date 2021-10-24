namespace Sod.Core
{
    public enum ReceiveStatus
    {
        // Integra responses 
        Ok = 0x00,
        RequestingUserCodeNotFound = 0x01,
        NoAccess = 0x02,
        SelectedUserCodeDoesNotExist = 0x03,
        SelectedUserAlreadyExists = 0x04,
        WrongCodeOrCodeAlreadyExists = 0x05,
        TelephoneCodeAlreadyExists = 0x06,
        ChangedCodeIsTheSame = 0x07,
        OtherError = 0x08,
        CanNotArmButCanUseForceArm = 0x11,
        CanNotArm = 0x12,
        OtherErrors = 0x80,
        CommandAccepted = 0xFF,
        
        // Custom codes
        SuccessfulRead = 0x100,
        NotSupportedCommand = 0x101,
        InvalidFrame = 0x102,
        InvalidCrc = 0x103
    }
}