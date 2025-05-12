namespace Quod.Domain
{
    public interface IBiometryService
    {
        Task<BiometryValidationResult> CheckFacialBiometryAsync(BiometryRequestViewModel request);
        Task<BiometryValidationResult> CheckFingerPrintBiometryAsync(BiometryRequestViewModel request);
    }
}
