using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Leaderboard.Responses;

public class ClassStudentLeaderboardDto
{
    public string ClassName { get; set; } = string.Empty;
    public List<StudentLeaderboardEntryDto> Students { get; set; } = new();
}
