namespace WorkLogger.Domain.Exceptions;
using WorkLogger.Domain.Enums;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException() : base(ExceptionsEnum.UserAlreadyExistException.ToString()) { }
}