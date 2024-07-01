using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.Exceptions;

public class RoleDoesNotExistException : Exception
{
    public RoleDoesNotExistException() : base(ExceptionsEnum.RoleDoesNotExistException.ToString())
    {
    }
}