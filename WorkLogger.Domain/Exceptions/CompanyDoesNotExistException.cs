using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.Exceptions;

public class CompanyDoesNotExistException : Exception
{
    public CompanyDoesNotExistException() : base(ExceptionsEnum.CompanyDoesNotExistException.ToString())
    {
        
    }
    
}