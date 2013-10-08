using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [Serializable, ProtoContract]
    public class Texture
    {
        // помечай атрибутами протобуфера поля иначе клиент на яве будет негодовать
        [ProtoMember(1)]
        private int id;
        [ProtoMember(2)]
        private string name;  // имя картинки НАПРИМЕР: "desertgrass"
        [ProtoMember(3)]
        private int wayCost;

        public Texture()
        {
        }

        public Texture(int id, string name, int wayCost)
        {
            this.id = id;
            this.name = name;
            this.wayCost = wayCost;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int WayCost
        {
            get { return wayCost; }
            set { wayCost = value; }
        }
    }
}