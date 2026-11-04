namespace SubscriberService.Exceptions;

public class DuplicateSubscriberException : Exception
{
    public DuplicateSubscriberException() : base("A subscriber with this email already exists.") { }
}