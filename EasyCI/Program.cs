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
bool shouldStop = false;
int exceptionsCounter = 0;

do
{
    try
    {
        var lastCommitSha = repoStatusProvider.GetLastCommitSha();
        if (prevCommitSha != lastCommitSha)
        {
            prevCommitSha = lastCommitSha;

            gitRunner.Pull();

            var dictionary = dockerRunner.GetDockerContainerIdsWithImageIds();
            var containerIdList = dictionary.Select(n => n.Key).ToList();
            var imageIdList = dictionary.Select(n => n.Value).Distinct().ToList();

            dockerRunner.Build();
            dockerRunner.StopContainers(containerIdList);
            dockerRunner.RemoveContainers(containerIdList);
            dockerRunner.RemoveImages(imageIdList);
            dockerRunner.Run();
        }
        else
        {
            var now = DateTime.UtcNow;
            Console.WriteLine($"{now}: No new commits!");
        }

        Thread.Sleep(TimeSpan.FromSeconds(10));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());

        exceptionsCounter++;
        if (exceptionsCounter == 10)
        {
            break;
        }

        Thread.Sleep(TimeSpan.FromMinutes(1));
    }
} while (shouldStop == false);