namespace LibGit2Sharp
{
    public enum FetchPruneStrategy
    {
        /// <summary>
        /// Use the setting from the configuration
        /// </summary>
        Fallback = 0,  // Default?

        /// <summary>
        /// Force pruning on
        /// </summary>
        Prune,

        /// <summary>
        /// Force pruning off
        /// </summary>
        NoPrune,
    }
}
