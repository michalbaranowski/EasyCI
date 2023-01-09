namespace EasyCI.Domain.Contracts.Services
{
    public interface IGitHubRepositoryStatusProvider
    {
        string GetLastCommitSha();
    }
}
