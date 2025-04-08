using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;

namespace Backend.Application.Abstractions;

public interface IScheduleRepository
{
    List<TimeSlot> GenerateTimeSlots(TimeSpan start, TimeSpan end, TimeSpan classLength, TimeSpan breakLength);
    List<ScheduleEntry> GenerateSchedule();
}
