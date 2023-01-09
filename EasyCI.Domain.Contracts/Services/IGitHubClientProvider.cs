using Octokit;

namespace EasyCI.Domain.Contracts.Services
{
    public interface IGitHubClientProvider
    {
        IGitHubClient Get();
    }
}
