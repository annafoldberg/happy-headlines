namespace ProfanityService.Persistence.Repositories;

public interface IProfanityRepository
{
    Task<IReadOnlyCollection<string>> GetAllTermsAsync(CancellationToken ct);
}