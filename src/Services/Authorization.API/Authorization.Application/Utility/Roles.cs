namespace Authorization.Application.Utility;

public static class Roles
{
	public const string Doctor = "doctor";
	public const string Receptionist = "receptionist";
	public static readonly string[] AllRoles = [Doctor, Receptionist];
}
