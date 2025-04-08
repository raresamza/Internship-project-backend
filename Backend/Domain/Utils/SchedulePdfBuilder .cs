using Backend.Application.Abstractions;
using Backend.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Backend.Infrastructure.Utils;

public class SchedulePdfBuilder : ISchedulePdfBuilder
{
    public byte[] Build(List<ScheduleEntry> schedule, string title)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header()
                    .Text(title)
                    .FontSize(20)
                    .SemiBold().FontColor(Colors.Blue.Medium);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(100); // Day
                        columns.ConstantColumn(120); // Time
                        columns.RelativeColumn();   // Course
                        columns.RelativeColumn();   // Classroom
                        columns.RelativeColumn();   // Teacher
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Day").Bold();
                        header.Cell().Text("Time").Bold();
                        header.Cell().Text("Course").Bold();
                        header.Cell().Text("Classroom").Bold();
                        header.Cell().Text("Teacher").Bold();
                    });

                    foreach (var entry in schedule.OrderBy(e => e.TimeSlot.Day).ThenBy(e => e.TimeSlot.StartTime))
                    {
                        table.Cell().Text(entry.TimeSlot.Day.ToString());
                        table.Cell().Text($"{entry.TimeSlot.StartTime:hh\\:mm} - {entry.TimeSlot.EndTime:hh\\:mm}");
                        table.Cell().Text(entry.Course.Name);
                        table.Cell().Text(entry.Classroom.Name);
                        table.Cell().Text(entry.Course.Teacher?.Name ?? "N/A");
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

        using var stream = new MemoryStream();
        document.GeneratePdf(stream);
        return stream.ToArray();
    }
}
