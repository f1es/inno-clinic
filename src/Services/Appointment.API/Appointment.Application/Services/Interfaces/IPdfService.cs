using Appointment.Core.Models;

namespace Appointment.Application.Services.Interfaces;

public interface IPdfService
{
	public MemoryStream ToPdf(Result result);
}
