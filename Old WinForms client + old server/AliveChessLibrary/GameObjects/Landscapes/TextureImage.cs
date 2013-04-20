using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [Serializable, ProtoContract]
    public class TextureImage
    {
        private int textureImageId = 0;  // id картинки (число в имени файла, записаное до первого "_")
        private string name = "";         // имя картинки. например: 1_into_3_1
        private string address = "";
        private int textureId = 0;       // id текстуры, напримет для картинки 1_into_3_1 textureId=3 (в чём)
        private int borderTextureId = 0; // с чем граничит, т.е. для картинки 1_into_3_1 borderTextureId=1 (что)
        private int? directionId = null;     /* "что в чём" устанавливают textureId и borderTextureId, а
                                    directionId показывает, в каком именно направлении они граничат */
        private AliveChessLibrary.GameObjects.Abstract.PointTypes pointType = AliveChessLibrary.GameObjects.Abstract.PointTypes.None;

        #region Constructors

        public TextureImage()
        {
        }

        public TextureImage(int textureImageId, int textureId, int borderTextureId, string textureName, AliveChessLibrary.GameObjects.Abstract.PointTypes type, int? directionId)
        {
            this.textureImageId = textureImageId;
            this.textureId = textureId;
            this.borderTextureId = borderTextureId;
            this.pointType = type;
            this.directionId = directionId;


            // На заметку. При конкатенации строк используй StringBuilder, String.Format, String.Concat или String.Join. 
            //Иначе в памяти будут создаваться ненужные строки
            // Пример: StringBuilder sb = new StringBuilder(); sb.Append("str1").Append("str2").ToString();

            switch (this.pointType)
            {
                case AliveChessLibrary.GameObjects.Abstract.PointTypes.Landscape:
                    if (this.textureId == this.borderTextureId)
                    {
                        this.name = textureName;
                        this.address = "Textures/landscape_textures/" + this.textureImageId + "_" + this.name + ".jpg";
                    }
                    else
                    {
                        if (directionId >= 1 && directionId <= 4)
                        {
                            this.name = textureId + "_winth_" + borderTextureId;
                            this.address = "Textures/landscape_textures/" + this.textureImageId + "_" + textureName + directionId + ".jpg";
                        }
                        if (directionId >= 5 && directionId <= 8)
                        {
                            this.name = borderTextureId + "_into_" + textureId;
                            this.address = "Textures/landscape_textures/" + this.textureImageId + "_" + textureName + (directionId - 4) + ".jpg";
                        }
                    }
                    break;
                case AliveChessLibrary.GameObjects.Abstract.PointTypes.SingleObject:
                    this.name = textureName;
                    this.address = "Textures/SingleObjects/" + this.TextureImageId + "_" + this.name + ".png";
                    break;
                case AliveChessLibrary.GameObjects.Abstract.PointTypes.MultyObject:
                    this.name = textureName;
                    this.address = "Textures/MultyObject/" + this.TextureImageId + "_" + this.name + ".png";
                    break;
            }


        }

        #endregion

        #region Properties

        /*!--------------исправить! пометить вместо свойств поля-----------------------!*/

        [ProtoMember(1)]
        public int TextureImageId
        {
            get { return textureImageId; }
            set { textureImageId = value; }
        }

        [ProtoMember(2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [ProtoMember(3)]
        public int TextureId
        {
            get { return textureId; }
            set { textureId = value; }
        }

        [ProtoMember(4)]
        public int BorderTextureId
        {
            get { return borderTextureId; }
            set { borderTextureId = value; }
        }

        [ProtoMember(5)]
        public int? DirectionId
        {
            get { return directionId; }
            set { directionId = value; }
        }

        [ProtoMember(6)]
        public string Address
        {
            get { return address; }
        }

        #endregion

        public static string GetImage(int id, string nameTexture)
        {
            return "";
        }

    }
}