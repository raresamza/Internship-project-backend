using Backend.Domain.Models;

namespace Backend.Application.Abstractions;

public interface ISchedulePdfBuilder
{
    byte[] Build(List<ScheduleEntry> schedule, string title);
}