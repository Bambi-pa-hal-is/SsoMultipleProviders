using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Helpers
{
    public static class ConfigurationHelpers
    {
        public static void LoadEnvironmentFile(string filePath = "./.env")
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var index = line.IndexOf('=');

                if (index == -1)
                    continue; // Skip lines without '='

                var key = line.Substring(0, index).Trim();
                var value = line.Substring(index + 1).Trim();

                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }
        }
    }
}
