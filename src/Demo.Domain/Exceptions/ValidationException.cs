namespace Demo.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public readonly string[] Errors;

        public ValidationException(string error)
        {
            Errors = [error];
        }

        public ValidationException(string[] errors)
        {
            Errors = errors;
        }
    }
}
