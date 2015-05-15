using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LibGit2Sharp.Tests.TestHelpers
{
    public static class Constants
    {
        public static readonly bool IsRunningOnUnix = IsUnixPlatform();
        public static readonly string TemporaryReposPath = BuildPath();
        public const string UnknownSha = "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef";
        public static readonly Identity Identity = new Identity("A. U. Thor", "thor@valhalla.asgard.com");
        public static readonly Signature Signature = new Signature(Identity, new DateTimeOffset(2011, 06, 16, 10, 58, 27, TimeSpan.FromHours(2)));

        private static bool IsUnixPlatform()
        {
            // see http://mono-project.com/FAQ%3a_Technical#Mono_Platforms
            var p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }

        // Populate these to turn on live credential tests:  set the
        // PrivateRepoUrl to the URL of a repository that requires
        // authentication. Define PrivateRepoCredentials to return an instance of
        // UsernamePasswordCredentials (for HTTP Basic authentication) or
        // DefaultCredentials (for NTLM/Negotiate authentication).
        //
        // For example:
        // public const string PrivateRepoUrl = "https://github.com/username/PrivateRepo";
        // ... return new UsernamePasswordCredentials { Username = "username", Password = "swordfish" };
        //
        // Or:
        // public const string PrivateRepoUrl = "https://tfs.contoso.com/tfs/DefaultCollection/project/_git/project";
        // ... return new DefaultCredentials();

        public const string PrivateRepoUrl = "";

        public static Credentials PrivateRepoCredentials(string url, string usernameFromUrl,
                                                         SupportedCredentialTypes types)
        {
            return null;
        }

        public static string BuildPath()
        {
            string tempPath = null;

            if (IsRunningOnUnix)
            {
                // We're running on Mono/*nix. Let's unwrap the path
                tempPath = UnwrapUnixTempPath();
                Trace.TraceInformation("Running on Unix, tempPath: '{0}'", tempPath);
            }
            else
            {
                const string LibGit2TestPath = "LibGit2TestPath";

                // We're running on .Net/Windows
                if (Environment.GetEnvironmentVariables().Contains(LibGit2TestPath))
                {
                    Trace.TraceInformation("{0} environment variable detected", LibGit2TestPath);
                    tempPath = Environment.GetEnvironmentVariables()[LibGit2TestPath] as String;
                }

                if (String.IsNullOrWhiteSpace(tempPath) || !Directory.Exists(tempPath))
                {
                    Trace.TraceInformation("Using default test path value");
                    tempPath = Path.GetTempPath();
                }
            }

            string testWorkingDirectory = Path.Combine(tempPath, "LibGit2Sharp-TestRepos");
            Trace.TraceInformation("Test working directory set to '{0}'", testWorkingDirectory);
            return testWorkingDirectory;
        }

        private static string UnwrapUnixTempPath()
        {
            var assembly = Assembly.Load("Mono.Posix");
            var type = assembly.GetType("Mono.Unix.UnixPath");

            return (string)type.InvokeMember("GetCompleteRealPath",
                BindingFlags.Static | BindingFlags.FlattenHierarchy |
                BindingFlags.InvokeMethod | BindingFlags.Public,
                null, type, new object[] { Path.GetTempPath() });
        }
    }
}
