namespace AliveChessLibrary.GameObjects.Abstract
{
    public enum UpdateType
    {
        KingMove          = 0, // король переместился
        KingAppear        = 1, // король появился
        KingDisappear     = 2, // король исчез 
        ResourceDisappear = 3  // ресурс исчез
    }
}
