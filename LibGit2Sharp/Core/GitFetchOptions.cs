using System.Runtime.InteropServices;

namespace LibGit2Sharp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    internal class GitFetchOptions
    {
        public int Version = 1;
        public GitRemoteCallbacks RemoteCallbacks;
        public FetchPruneStrategy prune;
        public bool update_fetchhead = true;
        public TagFetchMode download_tags;

        public GitFetchOptions()
        {
            download_tags = TagFetchMode.Auto;
        }
    }
}
