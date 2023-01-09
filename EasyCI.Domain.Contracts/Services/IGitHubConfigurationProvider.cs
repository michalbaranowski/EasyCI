using EasyCI.Domain.Contracts.Models;

namespace EasyCI.Domain.Contracts.Services
{
    public interface IGitHubConfigurationProvider
    {
        GitHubConfiguration Get();
    }
}
