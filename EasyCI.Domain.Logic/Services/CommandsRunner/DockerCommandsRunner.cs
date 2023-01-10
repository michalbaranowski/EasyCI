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
            var command = $"build {config.ExactPath} -t {config.ImageName}";

            RunAsProcess(command);
        }

        public void Run()
        {
            var config = _configProvider.Get();
            var command = $"run -d -p {config.Port}:80 ";

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

        public void Stop()
        {
            var config = _configProvider.Get();
            var command = $"stop {string.Join(" ", GetDockerContainerIdsByImageName(config.ImageName))}";

            RunAsProcess(command);
        }

        public void RemoveContainers()
        {
            var config = _configProvider.Get();
            var command = $"rm {string.Join(" ", GetDockerContainerIdsByImageName(config.ImageName))}";

            RunAsProcess(command);
        }

        private List<string> GetDockerContainerIdsByImageName(string imageName)
        {
            var client = new DockerClientConfiguration(GetDockerUriByCurrentOs()).CreateClient();

            var containers = client.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    All = true
                }).Result;

            return containers
                .Where(c => c.Image == imageName)
                .Select(n => n.ID)
                .ToList();
        }

        private Uri GetDockerUriByCurrentOs()
        {
            var url = CurrentOsHelper.IsWindows() ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock";
            return new Uri(url);
        }
    }
}
