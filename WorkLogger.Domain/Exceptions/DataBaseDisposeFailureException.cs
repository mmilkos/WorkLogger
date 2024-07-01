using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.Exceptions;

public class DataBaseDisposeFailureException : Exception
{
    public DataBaseDisposeFailureException() : base(ExceptionsEnum.DataBaseDisposeFailureException.ToString())
    {
        
    }
  
}