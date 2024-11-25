namespace LemonPlatform.Core.Exceptions
{
    public class LemonException : Exception
    {
        public LemonException() { }

        public LemonException(string message)
            : base(message)
        { }

        public LemonException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}