using System;
using System.Linq;
using System.Windows;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WP7_Client.LogicLayer;
using WP7_Client.NetLayer;
using WP7_Client.PresentationLayer.XNAScreenManager;
using Color = Microsoft.Xna.Framework.Color;
using Texture = WP7_Client.PresentationLayer.XNAScreenManager.Texture;

namespace WP7_Client.PresentationLayer.ConcreteScreens
{
    public class BigMapScreen : GameScreen
    {
        private float offsetX;
        private float offsetY;
        private const int Width = 80;
        private const int Height = 80;
        private readonly int _mapHeight;
        private readonly int _mapWidth;
        private King _king;
        private Castle _castle;
        private int MapPixelsWidth
        {
            get { return _mapWidth * Width; }
        }
        private int MapPixelsHeight
        {
            get { return _mapHeight * Height; }
        }

        private King FocusedOnKing
        {
            set
            {
                _king = value;
                if (value == null) return;
                offsetX = CountFocusOffsetX(_king);
                offsetY = CountFocusOffsetY(_king);
            }
            get { return _king; }
        }

        private Castle FocusedOnCastle
        {
            set
            {
                _castle = value;
                if (value == null) return;
                offsetX = CountFocusOffsetX(_castle);
                offsetY = CountFocusOffsetY(_castle);
            }
            get { return _castle; }
        }

        public GameWorld World { get; private set; }

        public BigMapScreen()
        {
            World = ((App) Application.Current).World;
            _mapHeight = World.Map.SizeY;
            _mapWidth = World.Map.SizeX;
            EnabledGestures = GestureType.FreeDrag | GestureType.Tap | GestureType.DoubleTap | GestureType.Pinch;
        }

        public void Initialize()
        {
            FocusedOnKing = World.Map.Kings.FirstOrDefault();
        }

        public override void Draw()
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            for (var i = (int) (Math.IEEERemainder(offsetX, Width) - Width); i < ScreenWidth; i += Width)
            {
                for (var j = (int) (Math.IEEERemainder(offsetY, Height) - Height); j < ScreenHeigth; j += Height)
                {
                    spriteBatch.Draw(Textures.Get(Texture.Grass), new Vector2((i), (j)), Color.White);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (var obj in World.Map.SingleObjects)
            {
                Texture2D texture;
                switch (obj.SingleObjectType)
                {
                    case SingleObjectType.Tree:
                        texture = Textures.Get(Texture.Tree);
                        break;
                    case SingleObjectType.Obstacle:
                        texture = Textures.Get(Texture.Stone);
                        break;
                    default:
                        texture = Textures.Get(Texture.Blank);
                        break;
                }
                if (OutOfScreenBorder(obj)) continue;
                spriteBatch.Draw(texture, new Vector2(FindScreenX(obj), FindScreenY(obj)), Color.White);
            }
            foreach (var obj in World.Map.Resources)
            {
                Texture2D texture;
                switch (obj.ResourceType)
                {
                    case ResourceTypes.Gold:
                        texture = Textures.Get(Texture.Gold);
                        break;
                    case ResourceTypes.Wood:
                        texture = Textures.Get(Texture.Wood);
                        break;
                    default:
                        texture = Textures.Get(Texture.Blank);
                        break;
                }
                if (OutOfScreenBorder(obj)) continue;
                spriteBatch.Draw(texture, new Vector2(FindScreenX(obj), FindScreenY(obj)), Color.White);
            }
            foreach (var king in World.Map.Kings.Where(king => !OutOfScreenBorder(king)))
            {
                spriteBatch.Draw(Textures.Get(Texture.King), new Vector2(FindScreenX(king), FindScreenY(king)),
                                 Color.White);
            }
            foreach (var castle in World.Map.Castles.Where(castle => !OutOfScreenBorder(castle)))
            {
                spriteBatch.Draw(Textures.Get(Texture.Castle), new Vector2(FindScreenX(castle), FindScreenY(castle)),
                                 Color.White);
            }
            if (FocusedOnKing != null)
            {
                spriteBatch.Draw(Textures.Get(Texture.Selection),
                                 new Vector2(FindScreenX(FocusedOnKing) - 10, FindScreenY(FocusedOnKing) - 10),
                                 Color.White);
            }
            if (FocusedOnCastle != null)
            {
                spriteBatch.Draw(Textures.Get(Texture.Selection),
                                 new Vector2(FindScreenX(FocusedOnCastle) - 10, FindScreenY(FocusedOnCastle) - 10),
                                 Color.White);
            }
            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            foreach (var gesture in input.Gestures)
            {
                switch (gesture.GestureType)
                {
                    case GestureType.FreeDrag:
                        {
                            if (offsetX - gesture.Delta.X <= (MapPixelsWidth) - ScreenWidth && offsetX - gesture.Delta.X >= 0)
                            {
                                offsetX -= gesture.Delta.X;
                            }
                            if (offsetY - gesture.Delta.Y <= (MapPixelsHeight) - ScreenHeigth && offsetY - gesture.Delta.Y >= 0)
                            {
                                offsetY -= gesture.Delta.Y;
                            }
                            break;
                        }
                    case GestureType.Tap:
                        {
                            var X = FindMapX(gesture.Position.X);
                            var Y = FindMapY(gesture.Position.Y);
                            if (FocusedOnKing != null)
                            {
                                if (FocusedOnKing.X != X && FocusedOnKing.Y != Y)
                                {
                                    var point = World.Map.GetObject(X, Y);
                                    if (point != null)
                                    {
                                        switch (point.PointType)
                                        {
                                            case PointTypes.Castle:
                                                {
                                                    RequestManager.SendComeInCaste((Castle) point.Owner);
                                                    break;
                                                }
                                            default:
                                                {
                                                    break;
                                                }
                                        }
                                    }
                                    RequestManager.SendMoveKing(X, Y);
                                }
                                else
                                {
                                    FocusedOnKing = null;
                                }
                            }
                            else
                            {
                                var king = World.Map.Kings.FirstOrDefault(iking => iking.X == X && iking.Y == Y);
                                FocusedOnKing = king;
                                if (FocusedOnKing == null)
                                {
                                    var castle =
                                        World.Map.Castles.FirstOrDefault(icastle => icastle.X == X && icastle.Y == Y);
                                    FocusedOnCastle = castle;
                                }
                            }

                            break;
                        }
                    case GestureType.DoubleTap:
                        {
                            break;
                        }
                }
            }
        }

        public void MoveKing(MoveKingResponse response)
        {
            if (response.Steps == null) return;
            foreach (var pos in response.Steps)
            {
                DoMove(pos);
            }
        }

        public void DoMove(Position pos)
        {
            FocusedOnKing.X = pos.X;
            FocusedOnKing.Y = pos.Y;
            FocusedOnKing = World.Map.Kings.FirstOrDefault(king => king.X == pos.X && king.Y == pos.Y);
        }

        public void KingCollectsResource(bool fromMine, Resource resource)
        {
            var tmp = FocusedOnKing.Resources.FirstOrDefault(res => res.ResourceType == resource.ResourceType);
            if (tmp != null)
            {
                tmp.CountResource += resource.CountResource;
            }
            else
            {
                FocusedOnKing.Resources.Add(resource);
            }
            World.Map.RemoveResource(
                World.Map.Resources.FirstOrDefault(res => res.X == FocusedOnKing.X && res.Y == FocusedOnKing.Y));
        }

        public void HandleGetObjects(GetObjectsResponse response)
        {
            if (response.Kings != null)
            {
                foreach (var king in response.Kings)
                {
                    if (!World.Map.Kings.Any(iking => iking.X == king.X && iking.Y == king.Y)) continue;
                    if (!World.Map.Kings.Any(iking => iking.X == king.PrevX && iking.Y == king.PrevY)) continue;
                    World.Map.RemoveKing(
                        World.Map.Kings.First(iking => iking.X == king.PrevX && iking.Y == king.PrevY));
                    World.Map.AddKing(king);
                }
            }
            if (response.Resources == null) return;
            World.Map.Resources.Clear();
            foreach (var res in response.Resources)
            {
                World.Map.AddResource(res);
            }
        }

        public void HandleGetGameState(GetGameStateResponse response)
        {
            World.Map.AddKing(response.King);
            World.Player.AddKing(response.King);
            Initialize();
        }

        #region Calculations

        private int CountFocusOffsetX(IPosition<int> obj)
        {
            var tmp = obj.X*Width - ScreenWidth/2 + Width/2;
            return tmp >= 0 ? tmp <= MapPixelsWidth - ScreenWidth ? tmp : MapPixelsWidth - ScreenWidth : 0;
        }

        private int CountFocusOffsetY(IPosition<int> obj)
        {
            var tmp = obj.Y*Height - ScreenHeigth/2 + Height/2;
            return tmp >= 0 ? tmp <= MapPixelsHeight - ScreenHeigth ? tmp : MapPixelsHeight - ScreenHeigth : 0;
        }

        private int FindScreenX(IPosition<int> obj)
        {
            return (int) (obj.X*Width - offsetX);
        }

        private int FindScreenY(IPosition<int> obj)
        {
            return (int) (obj.Y*Height - offsetY);
        }

        private bool OutOfScreenBorder(IPosition<int> obj)
        {
            return obj.X*Width < offsetX - Math.IEEERemainder(offsetX, Width) - Width || obj.X*Width > offsetX + ScreenWidth ||
                   obj.Y*Height < offsetY - Math.IEEERemainder(offsetY, Height) - Height || obj.Y*Height > offsetY + ScreenHeigth;
        }

        private int FindMapX(float X)
        {
            return (int) (offsetX + X)/Width;
        }

        private int FindMapY(float Y)
        {
            return (int) (offsetY + Y)/Height;
        }

#endregion 
    }
}
