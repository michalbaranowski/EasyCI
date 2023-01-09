namespace EasyCI.Domain.Contracts.Models
{
    public class GitHubConfiguration
    {
        public GitHubConfiguration(
            string owner,
            string repositoryName,
            string branchName,
            string accessToken)
        {
            Owner = owner;
            RepositoryName = repositoryName;
            BranchName = branchName;
            AccessToken = accessToken;
        }

        public string Owner { get; }
        public string RepositoryName { get; }
        public string BranchName { get; }
        public string AccessToken { get; }
    }
}
