using System.ComponentModel;

namespace WorkLogger.Domain.Enums;

public enum ExceptionsEnum
{
    UserAlreadyExistException,
    RoleDoesNotExistException,
    CompanyDoesNotExistException,
    DataBaseDisposeFailureException
}