using EasyCI.Domain.Contracts.Services;

namespace EasyCI.Domain.Logic.Services
{
    public class GitHubRepositoryStatusProvider : IGitHubRepositoryStatusProvider
    {
        private readonly IGitHubClientProvider _clientProvider;
        private readonly IGitHubConfigurationProvider _configProvider;

        public GitHubRepositoryStatusProvider(
            IGitHubClientProvider clientProvider,
            IGitHubConfigurationProvider configProvider)
        {
            _clientProvider = clientProvider;
            _configProvider = configProvider;
        }

        public string GetLastCommitSha()
        {
            var config = _configProvider.Get();
            var client = _clientProvider.Get();

            var repository = client.Repository.Get(config.Owner, config.RepositoryName).Result;
            var commit = client.Repository.Commit.Get(repository.Id, config.BranchName ?? repository.DefaultBranch).Result;

            return commit.Sha;
        }
    }
}
