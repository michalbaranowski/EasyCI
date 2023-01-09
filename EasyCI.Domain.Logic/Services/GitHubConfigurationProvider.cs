using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;
using EasyCI.Domain.Logic.Helpers;

namespace EasyCI.Domain.Logic.Services
{
    public class GitHubConfigurationProvider : IGitHubConfigurationProvider
    {
        public GitHubConfiguration Get()
        {
            string owner = Environment.GetEnvironmentVariable("EasyCI_Owner").AssertValue();
            string repositoryName = Environment.GetEnvironmentVariable("EasyCI_RepositoryName").AssertValue();
            string branchName = Environment.GetEnvironmentVariable("EasyCI_BranchName").AssertValue();
            string accessToken = Environment.GetEnvironmentVariable("EasyCI_AccessToken").AssertValue();

            return new GitHubConfiguration(owner, repositoryName, branchName, accessToken);
        }
    }
}
