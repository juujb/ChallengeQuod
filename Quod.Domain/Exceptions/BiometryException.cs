namespace Quod.Domain
{
    [Serializable]
    public class BiometryException : Exception
    {
        public string? ErrorCode { get; set; }

        public BiometryException()
        {

        }

        public BiometryException(
            string errorCode,
            string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BiometryException(
            string errorCode,
            string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public BiometryException(string message) : base(message)
        {

        }

        public BiometryException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }
}
