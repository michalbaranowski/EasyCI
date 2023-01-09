using Docker.DotNet;
using Docker.DotNet.Models;
using EasyCI.Domain.Contracts.Services;
using System.Diagnostics;

namespace EasyCI.Domain.Logic.Services
{
    public class DockerCommandsRunner : IDockerCommandsRunner
    {
        private readonly IEnvironmentConfigProvider _configProvider;

        public DockerCommandsRunner(IEnvironmentConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public void Build()
        {
            var config = _configProvider.Get();
            var command = $"build {config.ProjectPath} -t {config.ImageName}";

            RunAsProcess(command);
        }

        public void Run()
        {
            var config = _configProvider.Get();
            var command = $"run -p {config.Port}:80 ";

            if (config.EnvironmentVariables.Any())
            {
                foreach (var envVariable in config.EnvironmentVariables)
                {
                    command += "-e ";

                    var dockerVariableName = envVariable.Substring(0, envVariable.IndexOf('='));
                    var envVariableName = envVariable.Substring(envVariable.IndexOf('=') + 1, envVariable.Length - dockerVariableName.Length - 1)
                        .Replace("\"", String.Empty);

                    var envVariableValue = Environment.GetEnvironmentVariable(envVariableName);

                    command += IsWindowsCurrentOs() ? $"{dockerVariableName}=\"{envVariableValue}\"" : $"{dockerVariableName}=\"{envVariableValue}\"";
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

        private void RunAsProcess(string command)
        {
            string dockerPath = IsWindowsCurrentOs() ? @"C:\Program Files\Docker\Docker\Resources\bin\docker.exe" : "/usr/bin/docker";
            var config = _configProvider.Get();

            using (var process = new Process())
            {
                process.StartInfo.FileName = dockerPath;
                process.StartInfo.Arguments = command;
                
                process.Start();
                process.WaitForExit();
            }
        }

        private bool IsWindowsCurrentOs()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT;
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
            var url = IsWindowsCurrentOs() ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock";
            return new Uri(url);
        }
    }
}
