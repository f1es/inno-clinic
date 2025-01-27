namespace Shared.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key) 
        : base($"{entityName} with indefier {key} not found")
    {
        
    }
}
