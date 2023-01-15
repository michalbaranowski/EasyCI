using Docker.DotNet;
using Docker.DotNet.Models;
using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;
using EasyCI.Domain.Logic.Helpers;

namespace EasyCI.Domain.Logic.Services.CommandsRunner
{
    public class DockerCommandsRunner : CommandsRunnerBase, IDockerCommandsRunner
    {
        protected override AvailableApps CurrentApp => AvailableApps.Docker;

        private readonly IEnvironmentConfigProvider _configProvider;

        public DockerCommandsRunner(IAppPathResolver appPathResolver, IEnvironmentConfigProvider configProvider)
            : base(appPathResolver)
        {
            _configProvider = configProvider;
        }

        public void Build()
        {
            var config = _configProvider.Get();
            var command = $"build --no-cache {config.ExactPath} -t {config.ImageName}:latest";

            RunAsProcess(command);
        }

        public void Run()
        {
            var config = _configProvider.Get();
            var command = $"run -d -p {config.HttpPort}:80 {config.HttpsPort}:443 ";

            if (config.EnvironmentVariables.Any())
            {
                foreach (var envVariable in config.EnvironmentVariables)
                {
                    command += "-e ";

                    var dockerVariableName = envVariable.Substring(0, envVariable.IndexOf('='));
                    var envVariableName = envVariable.Substring(envVariable.IndexOf('=') + 1, envVariable.Length - dockerVariableName.Length - 1)
                        .Replace("\"", String.Empty);

                    var envVariableValue = Environment.GetEnvironmentVariable(envVariableName);
                    command += $"{dockerVariableName}=\"{envVariableValue}\"";

                    command += " ";
                }
            }

            command += config.ImageName;
            RunAsProcess(command);
        }

        public void StopContainers(List<string> containerIds)
        {
            if (containerIds.Any() == false)
            {
                return;
            }

            var command = $"stop {string.Join(" ", containerIds)}";
            RunAsProcess(command);
        }

        public void RemoveContainers(List<string> containerIds)
        {
            if (containerIds.Any() == false)
            {
                return;
            }

            var command = $"rm {string.Join(" ", containerIds)}";
            RunAsProcess(command);
        }

        public void RemoveImages(List<string> imageIds)
        {
            if (imageIds.Any() == false)
            {
                return;
            }

            var command = $"rmi {string.Join(" ", imageIds)}";
            RunAsProcess(command);
        }

        public Dictionary<string, string> GetDockerContainerIdsWithImageIds()
        {
            var config = _configProvider.Get();
            var client = new DockerClientConfiguration(GetDockerUriByCurrentOs()).CreateClient();

            var containers = client.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    All = true
                }).Result;

            return containers
                .Where(c => c.Image == config.ImageName)
                .ToDictionary(x => x.ID, x => x.ImageID);
                
        }

        private Uri GetDockerUriByCurrentOs()
        {
            var url = CurrentOsHelper.IsWindows() ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock";
            return new Uri(url);
        }
    }
}
