using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WP7_Client.PresentationLayer.XNAScreenManager
{
    public enum Texture{
        Blank,
        Grass,
        Tree,
        Stone,
        Gold,
        Castle,
        King,
        Wood,
        Selection
    }

    public class Textures
    {
        private static readonly Dictionary<Texture,Texture2D> _textures = new Dictionary<Texture, Texture2D>(); 
       
        public static void Initiaize(ContentManager mgr)
        {
            _textures.Add(Texture.Blank, mgr.Load<Texture2D>("blank"));
            _textures.Add(Texture.Gold, mgr.Load<Texture2D>("gold"));
            _textures.Add(Texture.Stone, mgr.Load<Texture2D>("stone"));
            _textures.Add(Texture.Castle, mgr.Load<Texture2D>("castle"));
            _textures.Add(Texture.King, mgr.Load<Texture2D>("king"));
            _textures.Add(Texture.Grass, mgr.Load<Texture2D>("grass"));
            _textures.Add(Texture.Tree, mgr.Load<Texture2D>("tree"));
            _textures.Add(Texture.Wood, mgr.Load<Texture2D>("wood"));
            _textures.Add(Texture.Selection, mgr.Load<Texture2D>("selection"));
        }
            
        public static Texture2D Get(Texture type)
        {
            return _textures[type];
        }
    }
}
