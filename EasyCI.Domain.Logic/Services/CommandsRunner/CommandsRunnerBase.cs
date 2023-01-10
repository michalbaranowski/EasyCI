using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;
using System.Diagnostics;

namespace EasyCI.Domain.Logic.Services.CommandsRunner
{
    public abstract class CommandsRunnerBase
    {
        private readonly IAppPathResolver _appPathResolver;

        protected abstract AvailableApps CurrentApp { get; }

        public CommandsRunnerBase(IAppPathResolver appPathResolver)
        {
            _appPathResolver = appPathResolver;
        }

        protected void RunAsProcess(string command)
        {
            string currentAppPath = _appPathResolver.ResolveAppPath(CurrentApp);

            using (var process = new Process())
            {
                process.StartInfo.FileName = currentAppPath;
                process.StartInfo.Arguments = command;

                process.Start();
                process.WaitForExit();
            }
        }
    }
}
