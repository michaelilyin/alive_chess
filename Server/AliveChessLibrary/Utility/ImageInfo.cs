namespace AliveChessLibrary.Utility
{
    public struct ImageInfo
    {
        private int? _imageId;
        private string _imageName;
        private int? _width;
        private int? _height;
        private int? _textureId;
        private int? _borderTextureId;
        private int? _direction;

        public int? ImageId
        {
            get { return _imageId; }
            set { _imageId = value; }
        }

        public string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }

        public int? TextureId
        {
            get { return _textureId; }
            set { _textureId = value; }
        }

        public int? BorderTextureId
        {
            get { return _borderTextureId; }
            set { _borderTextureId = value; }
        }

        public int? Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public int? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int? Height
        {
            get { return _height; }
            set { _height = value; }
        }
    }
}
