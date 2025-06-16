using Appointment.Application.Services.Interfaces;
using Appointment.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Appointment.Application.Services.Implementations;

public class PdfService : IPdfService
{
	public MemoryStream ToPdf(Result result)
	{
			var stream = new MemoryStream();

			Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Margin(50);
					page.Size(PageSizes.A4);
					page.PageColor(Colors.White);
					page.DefaultTextStyle(x => x.FontSize(14));

					page.Content()
						.Column(column =>
						{
							column.Item().Text($"Complaints: {result.Complaints}");
							column.Item().Text($"Conclusion: {result.Conclusion}");
							column.Item().Text($"Recommendations: {result.Reccomendations}");
						});
				});
			})
			.GeneratePdf(stream);

			stream.Position = 0;

			return stream;

	}
}
