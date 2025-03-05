namespace Profiles.Core.Repositories;

public interface IUnitOfWork
{
	public IDoctorRepository DoctorRepository { get; }
	public IPatientRepository PatientRepository { get; }
	public IReceptionistRepository ReceptionistRepository { get; }
	public ISpecializationRepository SpecializationRepository { get; }

	public Task SaveAsync();
}
