using EasyCI.Domain.Logic.Services;
using EasyCI.Domain.Logic.Services.CommandsRunner;

var configProvider = new GitHubConfigurationProvider();
var clientProvider = new GitHubClientProvider(configProvider);
var repoStatusProvider = new GitHubRepositoryStatusProvider(clientProvider, configProvider);

var appPathResolver = new AppPathResolver();
var envConfigProvider = new EnvironmentConfigProvider();

var gitRunner = new GitCommandsRunner(appPathResolver, envConfigProvider);
var dockerRunner = new DockerCommandsRunner(appPathResolver, envConfigProvider);

string prevCommitSha = string.Empty;

do
{
    var lastCommitSha = repoStatusProvider.GetLastCommitSha();
    if (prevCommitSha != lastCommitSha)
    {
        prevCommitSha = lastCommitSha;

        gitRunner.Pull();
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