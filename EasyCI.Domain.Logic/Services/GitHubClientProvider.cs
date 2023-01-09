using EasyCI.Domain.Contracts.Services;
using Octokit;

namespace EasyCI.Domain.Logic.Services
{
    public class GitHubClientProvider : IGitHubClientProvider
    {
        private readonly IGitHubConfigurationProvider _gitHubConfigurationProvider;

        public GitHubClientProvider(IGitHubConfigurationProvider provider)
        {
            _gitHubConfigurationProvider = provider;
        }

        public IGitHubClient Get()
        {
            var config = _gitHubConfigurationProvider.Get();
            return new GitHubClient(new ProductHeaderValue("MyGitHubAPI"))
            {
                Credentials = new Credentials(config.AccessToken)
            };
        }
    }
}
