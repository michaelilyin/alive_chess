namespace AliveChessLibrary.Utility
{
    public delegate void LoadingHandler<T>(T sender);
    public delegate void DeferredLoadingHandler<T>(T sender);
    public delegate void DeferredTargetedLoadingHandler<T>(T sender, int? targetId);

    public struct Constants
    {
        public const int ImpassablePoint = 10;
    }
}
