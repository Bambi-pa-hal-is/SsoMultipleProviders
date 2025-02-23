using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Helpers
{
    public class EnvironmentVariableHelper
    {
        public static string ReadEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Process) ??
                Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User) ??
                Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine) ??
                throw new ArgumentException(variable + " cannot be found");
        }
    }
}
