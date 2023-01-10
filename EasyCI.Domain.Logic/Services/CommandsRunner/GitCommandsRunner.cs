using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;

namespace EasyCI.Domain.Logic.Services.CommandsRunner
{
    public class GitCommandsRunner : CommandsRunnerBase, IGitCommandsRunner
    {
        protected override AvailableApps CurrentApp => AvailableApps.Git;
        private readonly IEnvironmentConfigProvider _configProvider;

        public GitCommandsRunner(IAppPathResolver appPathResolver, IEnvironmentConfigProvider configProvider)
            : base(appPathResolver)
        {
            _configProvider = configProvider;
        }

        public void Pull()
        {
            var repoPath = _configProvider.Get().ExactPath;
            var command = $"-C {repoPath} pull";

            RunAsProcess(command);
        }
    }
}
