using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;
using EasyCI.Domain.Logic.Helpers;

namespace EasyCI.Domain.Logic.Services
{
    public class GitHubConfigurationProvider : IGitHubConfigurationProvider
    {
        public GitHubConfiguration Get()
        {
            string owner = Environment.GetEnvironmentVariable("EasyCI.Owner").AssertValue();
            string repositoryName = Environment.GetEnvironmentVariable("EasyCI.RepositoryName").AssertValue();
            string branchName = Environment.GetEnvironmentVariable("EasyCI.BranchName").AssertValue();
            string accessToken = Environment.GetEnvironmentVariable("EasyCI.AccessToken").AssertValue();

            return new GitHubConfiguration(owner, repositoryName, branchName, accessToken);
        }
    }
}
