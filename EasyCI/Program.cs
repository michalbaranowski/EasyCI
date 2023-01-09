using EasyCI.Domain.Logic.Services;

var configProvider = new GitHubConfigurationProvider();
var clientProvider = new GitHubClientProvider(configProvider);
var repoStatusProvider = new GitHubRepositoryStatusProvider(clientProvider, configProvider);

var envConfigProvider = new EnvironmentConfigProvider();
var dockerRunner = new DockerCommandsRunner(envConfigProvider);

string prevCommitSha = String.Empty;

do
{
    var lastCommitSha = repoStatusProvider.GetLastCommitSha();
    if (prevCommitSha != lastCommitSha)
    {
        prevCommitSha = lastCommitSha;

        dockerRunner.Build();
        dockerRunner.Stop();
        dockerRunner.RemoveContainers();
        dockerRunner.Run();
    } 
    else
    {
        Console.WriteLine("No new commits!");
    }

    Thread.Sleep(10000);
} while (true);