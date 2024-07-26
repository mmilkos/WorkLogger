using System.Globalization;
using MediatR;
using WorkLogger.Application.Builders;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Teams;

public class CreateSummaryFileQueryHandler : IRequestHandler<CreateSummaryFileQuery, OperationResult<ExcelFileResponseDto>>
{
    private IWorkLoggerRepository _repository;
    
    public CreateSummaryFileQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<ExcelFileResponseDto>> Handle(CreateSummaryFileQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<ExcelFileResponseDto>()
        {
            Data = new ExcelFileResponseDto()
        };

        var dto = request.Dto;

        var team = await _repository.FindEntityByIdAsync<Team>(dto.TeamId);
        var teamsTasks = await _repository.GetAllEntitiesAsync<UserTask>(
            condition:  task => task.TeamId == dto.TeamId 
            && task.LastUpdateDate >= dto.StartDate 
            && task.LastUpdateDate <= dto.EndDate);
        
        var data = new Dictionary<string, List<string>>
        {
            {"Reported date",teamsTasks.Select(task => task.CreatedDate.ToString().Split(" ")[0]).ToList()},
            {"Task Name",teamsTasks.Select(task => task.Name).ToList()},
            {"Registered Hours", teamsTasks.Select(task => task.LoggedHours.ToString()).ToList()},
        };

        var memoryStream = ExcelBuilder.BuildExcelFile(data);

        result.Data.Stream = memoryStream;
        result.Data.FileName = GenerateFileName(team);

        return result;
    }

    private string GenerateFileName(Team team)
    {
        var date = DateTime.Now.Date.ToString(CultureInfo.CurrentUICulture).Split(" ")[0];
        return $"{team.Name}_Report_{date}";
    }
}