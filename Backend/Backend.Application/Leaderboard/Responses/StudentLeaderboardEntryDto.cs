using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Leaderboard.Responses;

public class StudentLeaderboardEntryDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public float AverageGrade { get; set; }
    public float ParticipationPoints { get; set; }
    public string ClassName { get; set; } = string.Empty;
}