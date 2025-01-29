namespace Shared.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message)
        :base(message)
    {
    }

    public NotFoundException(string name, object key)
        :base($"Entity: {name} with id ({key}) was not found")
    {
    }
}
