using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [ProtoContract]
    //[Table(Name = "dbo.height_3d")]
    public class Height3D
    {
        private int _id;
        [ProtoMember(1)]
        private int _x;
        [ProtoMember(2)]
        private int _y;
        [ProtoMember(3)]
        private int _height;

        //[Column(Name = "height_id", Storage = "_id", CanBeNull = false, DbType = Constants.DB_INT,
        //   IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //[Column(Name = "height_x", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_x")]
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        //[Column(Name = "height_y", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_y")]
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        //[Column(Name = "height_z", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_height")]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
    }
}
