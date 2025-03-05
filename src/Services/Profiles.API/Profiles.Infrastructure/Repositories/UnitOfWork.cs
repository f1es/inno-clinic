using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;

namespace Profiles.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly Lazy<IDoctorRepository> _doctorRepository;
	private readonly Lazy<IPatientRepository> _patientRepository;
	private readonly Lazy<IReceptionistRepository> _receptionistRepository;
	private readonly Lazy<ISpecializationRepository> _specializationRepository; 
	private readonly ProfilesDbContext _context;

	public IDoctorRepository DoctorRepository => _doctorRepository.Value;
	public IPatientRepository PatientRepository => _patientRepository.Value;
	public IReceptionistRepository ReceptionistRepository => _receptionistRepository.Value;
	public ISpecializationRepository SpecializationRepository => _specializationRepository.Value;

    public UnitOfWork(ProfilesDbContext context)
    {
        _context = context;

		_doctorRepository = new Lazy<IDoctorRepository>(() => new DoctorRepository(context));

		_patientRepository = new Lazy<IPatientRepository>(() => new PatientRepository(context));

		_receptionistRepository = new Lazy<IReceptionistRepository>(() => new ReceptionistRepository(context));

		_specializationRepository = new Lazy<ISpecializationRepository>(() => new SpecializationRepository(context));
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
