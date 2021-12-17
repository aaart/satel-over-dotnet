namespace Sod.Infrastructure.Satel.Communication
{
    public enum IntegraResponse
    {
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
        NotApplicable = 0x100
    }
}