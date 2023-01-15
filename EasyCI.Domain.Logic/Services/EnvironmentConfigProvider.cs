using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;
using EasyCI.Domain.Logic.Helpers;

namespace EasyCI.Domain.Logic.Services
{
    public class EnvironmentConfigProvider : IEnvironmentConfigProvider
    {
        public EnvironmentConfiguration Get()
        {
            string path = Environment.GetEnvironmentVariable("EasyCI_ProjectPath").AssertValue();
            string name = Environment.GetEnvironmentVariable("EasyCI_ProjectName").AssertValue();
            string httpPort = Environment.GetEnvironmentVariable("EasyCI_HttpPort").AssertValue();
            string httpsPort = Environment.GetEnvironmentVariable("EasyCI_HttpsPort").AssertValue();
            string envVariablesStr = Environment.GetEnvironmentVariable("EasyCI_EnvironmentVariables").AssertValue();

            var envVariablesList = envVariablesStr.Split(",").ToList();

            return new EnvironmentConfiguration(path, name, httpPort, httpsPort, envVariablesList);
        }
    }
}
