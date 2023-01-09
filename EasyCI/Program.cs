using EasyCI.Domain.Logic.Services;

var configProvider = new GitHubConfigurationProvider();
var clientProvider = new GitHubClientProvider(configProvider);
var repoStatusProvider = new GitHubRepositoryStatusProvider(clientProvider, configProvider);

string prevCommitSha = String.Empty;

do
{
    var lastCommitSha = repoStatusProvider.GetLastCommitSha();
    if (string.IsNullOrEmpty(prevCommitSha) == false && prevCommitSha != lastCommitSha)
    {
        prevCommitSha = lastCommitSha;
        Console.WriteLine("New commit!");
    } 
    else
    {
        Console.WriteLine("No new commits!");
    }

    Thread.Sleep(10000);
} while (true);