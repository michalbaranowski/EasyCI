using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;
using EasyCI.Domain.Logic.Helpers;

namespace EasyCI.Domain.Logic.Services
{
    public class EnvironmentConfigProvider : IEnvironmentConfigProvider
    {
        public EnvironmentConfiguration Get()
        {
            string path = Environment.GetEnvironmentVariable("EasyCI.ProjectPath").AssertValue();
            string name = Environment.GetEnvironmentVariable("EasyCI.ProjectName").AssertValue();
            string port = Environment.GetEnvironmentVariable("EasyCI.Port").AssertValue();
            string envVariablesStr = Environment.GetEnvironmentVariable("EasyCI.EnvironmentVariables").AssertValue();

            var envVariablesList = envVariablesStr.Split(",").ToList();

            return new EnvironmentConfiguration(path, name, port, envVariablesList);
        }
    }
}
