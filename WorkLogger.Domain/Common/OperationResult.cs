using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.Common;

public class OperationResult<T>
{
    public bool Success { get; private set; } = true;
    public List<string> ErrorsList { get; }
    
    public T Data { get; set; }
    
    public ErrorTypesEnum ErrorType { get; set; }
    
    public OperationResult()
    {
        ErrorsList = [];
    }

    public void AddError(string error)
    {
        ErrorsList.Add(error);
        Success = false;
    }
}