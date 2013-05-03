// Static Model
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessLibrary.Statistics;
using BehaviorAILibrary.DecisionLayer;
using BehaviorAILibrary.DecisionLayer.NeuralNetwork;
using BehaviorAILibrary.MotionLayer;
using BehaviorAILibrary.PerseptionLayer;
using AliveChessLibrary.Interfaces;

namespace BehaviorAILibrary
{
    public class BotKing : King
    {
        private King _parent;
        //private IAnimat animat;
        private IPlayer animat;
        private Steering steering;
        private IBrain brain;
        private PoolingStation poolingStation;

        private Rectangle square;
        private bool obstacleFound = false;

        private TimeSpan _thinkPeriod = TimeSpan.Zero;
        private TimeSpan _maxThinkPeriod = TimeSpan.FromSeconds(1);

        private Statistic _statistic;
        private ICommunity _community;

        public BotKing(King parent)
        {
            this.Parent = parent;
        }

        public BotKing(King parent, NeuroTeacher teacher)
            : this(parent)
        {
            steering = new Steering(this);
            poolingStation = new PoolingStation(this);
            brain = new NeuroBrain(this, teacher, poolingStation);
            square = new Rectangle(0, 0, 30, 30);
        }

        public BotKing(IPlayer animat, King parent, NeuroTeacher teacher)
            : this(parent, teacher)
        {
            this.animat = animat;
        }

        private void UpdateVelocity()
        {
            Vector2D force = this.Steering.Calculate();

            if (Steering.SteeringForce.IsZero())
            {
                const double BrakingRate = 0.8;
                Velocity = Velocity * BrakingRate;
            }
            Vector2D accel = force / Mass;
            Velocity += accel;
            Velocity.Truncate(MaxSpeed);
        }

        private Point GetCoordinates()
        {
            return new Point(X + (int)Math.Round(Velocity.X), Y + (int)Math.Round(Velocity.Y));
        }

        public void UpdateMovement()
        {
            UpdateVelocity();
            Point coordinates = GetCoordinates();
            if (!Velocity.IsZero())
            {
                if (!Check(coordinates.X, coordinates.Y, 1))
                {
                    if (steering.WanderIsOn())
                    {
                        Heading = Vector2D.Vec2DNormalize(
                            Direction.ChooseRandomDirection(CheckPosition, X, Y, 1));
                    }
                    if (steering.FleeIsOn() || steering.SeekIsOn() || steering.PursuitIsOn()
                        || steering.EvadeIsOn())
                    {
                        ObstacleFound = true;
                    }

                    Side = Heading.Perp();
                }
                else
                {
                    Heading = Vector2D.Vec2DNormalize(Velocity);
                    Side = Heading.Perp();
                    DoStep(new Vector2D(coordinates.X, coordinates.Y));
                    poolingStation.Position = coordinates;
                }
            }
        }

        private bool Check(int x, int y, int costLimit)
        {
            if (steering.WanderIsOn())
                return CheckPosition(x, y, costLimit);
            else return Map.CheckPoint(x, y, costLimit);
        }

        private bool CheckPosition(int x, int y, int costLimit)
        {
            return Map.CheckPoint(x, y, costLimit) && CheckSquare(x, y);
        }

        private bool CheckSquare(int x, int y)
        {
            return x >= square.Left && x <= square.Right && y >= square.Top && y <= square.Bottom;
        }

        public void AttachNeuralNetwork(INeuralNetwork network)
        {
            this.brain.NN = network;
        }

        public void AssignCommunity(ICommunity community)
        {
            if (Parent.Player == null || Parent.Player.Community == null || Parent.Community == community)
                this._community = community;
            else throw new InvalidOperationException("Attempt to assign differ community then parent king has");
        }

        public King Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public Steering Steering
        {
            get { return steering; }
            set { steering = value; }
        }

        public bool ObstacleFound
        {
            get { return obstacleFound; }
            set { obstacleFound = value; }
        }

        #region Override Methods

        public override void Update()
        {
            if (_thinkPeriod > _maxThinkPeriod)
            {
                brain.Process();
                brain.Think();
                this._thinkPeriod = TimeSpan.Zero;
            }
            else _thinkPeriod += TimeSpan.FromMilliseconds(10);

            if (Time > TimeSpan.FromMilliseconds(300))
            {
                if (IsMove)
                {
                    base.Update();
                }
                else
                {
                    if (this.State == KingState.BigMap)
                    {
                        UpdateMovement();
                    }
                }

                Time = TimeSpan.Zero;
            }
            else Time += TimeSpan.FromMilliseconds(10);
        }

        public override void AddCastle(Castle castle)
        {
            Parent.AddCastle(castle);
        }

        public override void AddMine(Mine mine)
        {
            Parent.AddMine(mine);
        }

        public override void AddSteps(Queue<FPosition> steps)
        {
            Parent.AddSteps(steps);
        }

        //public override void AddView(MapPoint point)
        //{
        //    Parent.AddView(point);
        //}

        public override void AttachStartCastle(Castle castle)
        {
            Parent.AttachStartCastle(castle);
        }

        public override void ClearSteps()
        {
            Parent.ClearSteps();
        }

        public override void ComeInCastle(Castle castle)
        {
            Parent.ComeInCastle(castle);
        }

        public override bool Equals(int other)
        {
            return Parent.Equals(other);
        }

        public override Castle GetCastleById(int id)
        {
            return Parent.GetCastleById(id);
        }

        public override Mine GetMineById(int id)
        {
            return Parent.GetMineById(id);
        }

        public override bool HasCastle(MapPoint point)
        {
            return Parent.HasCastle(point);
        }

        public override bool HasMine(MapPoint point)
        {
            return Parent.HasMine(point);
        }

        public override bool InsideCastle(Castle castle)
        {
            return Parent.InsideCastle(castle);
        }

        public override void LeaveCastle()
        {
            Parent.LeaveCastle();
        }

        public override void MoveBy(float x, float y)
        {
            Parent.MoveBy(x, y);
        }

        public override void MoveBy(Position step)
        {
            Parent.MoveBy(step);
        }

        public override void OutOfGame()
        {
            Parent.OutOfGame();
        }

        public override void RemoveAllCastles()
        {
            Parent.RemoveAllCastles();
        }

        public override void RemoveAllMines()
        {
            Parent.RemoveAllMines();
        }

        public override void RemoveCastle(Castle castle)
        {
            Parent.RemoveCastle(castle);
        }

        public override void RemoveCastle(int id)
        {
            Parent.RemoveCastle(id);
        }

        public override void RemoveMine(int id)
        {
            Parent.RemoveMine(id);
        }

        public override void RemoveMine(Mine mine)
        {
            Parent.RemoveMine(mine);
        }

        public override Castle SearchCastle()
        {
            return Parent.SearchCastle();
        }

        #endregion

        #region Override Properties

        public override IPlayer Player
        {
            get { return this.animat; }
            set { this.animat = value; }
        }

        public override Statistic Statistic
        {
            get { return _statistic; }
        }

        public override int? AnimatId
        {
            get { return Parent.AnimatId; }
            set { Parent.AnimatId = value; }
        }

        public override EntitySet<Castle> Castles
        {
            get { return Parent.Castles; }
            set { Parent.Castles = value; }
        }

        public override Map Map
        {
            get { return Parent.Map; }
            set { Parent.Map = value; }
        }

        //public override MapPoint ViewOnMap
        //{
        //    get { return Parent.ViewOnMap; }
        //    set { Parent.ViewOnMap = value; }
        //}

        public override int X
        {
            get { return Parent.X; }
            set { Parent.Y = value; }
        }

        public override int Y
        {
            get { return Parent.Y; }
            set { Parent.Y = value; }
        }

        public override Castle CurrentCastle
        {
            get { return Parent.CurrentCastle; }
        }

        public override int Distance
        {
            get { return Parent.Distance; }
            set { Parent.Distance = value; }
        }

        public override int? EmpireId
        {
            get { return Parent.EmpireId; }
            set { Parent.EmpireId = value; }
        }

        public override int Experience
        {
            get { return Parent.Experience; }
            set { Parent.Experience = value; }
        }

        //public override GameData GameData
        //{
        //    get { return Parent.GameData; }
        //    set { Parent.GameData = value; }
        //}

        public override int Id
        {
            get { return Parent.Id; }
            set { Parent.Id = value; }
        }

        public override IInteraction Interaction
        {
            get { return Parent.Interaction; }
            set { Parent.Interaction = value; }
        }

        public override bool IsLeader
        {
            get { return Parent.IsLeader; }
        }

        public override bool IsMove
        {
            get { return Parent.IsMove; }
            set { Parent.IsMove = value; }
        }

        public override int? MapId
        {
            get { return Parent.MapId; }
            set { Parent.MapId = value; }
        }

        //public override int MapPointId
        //{
        //    get { return Parent.MapPointId; }
        //    set { Parent.MapPointId = value; }
        //}

        public override int MilitaryRank
        {
            get { return Parent.MilitaryRank; }
            set { Parent.MilitaryRank = value; }
        }

        public override EntitySet<Mine> Mines
        {
            get { return Parent.Mines; }
            set { Parent.Mines = value; }
        }

        public override string Name
        {
            get { return Parent.Name; }
            set { Parent.Name = value; }
        }

        public override int? PlayerId
        {
            get { return Parent.PlayerId; }
            set { Parent.PlayerId = value; }
        }

        public override int PrevX
        {
            get { return Parent.PrevX; }
            set { Parent.PrevX = value; }
        }

        public override int PrevY
        {
            get { return Parent.Y; }
            set { Parent.Y = value; }
        }

        /*public override EntitySet<Resource> Resources
        {
            get { return Parent.Resources; }
            set { Parent.Resources = value; }
        }*/

        public override ResourceStore ResourceStore
        {
            get
            {
                return Parent.ResourceStore;
            }
            set
            {
                Parent.ResourceStore = value;
            }
        }

        public override bool Sleep
        {
            get { return Parent.Sleep; }
            set { Parent.Sleep = value; }
        }

        public override Castle StartCastle
        {
            get { return Parent.StartCastle; }
        }

        public override KingState State
        {
            get { return Parent.State; }
            set { Parent.State = value; }
        }

        public override int StepCount
        {
            get { return Parent.StepCount; }
        }

        public override int? UnionId
        {
            get { return Parent.UnionId; }
            set { Parent.UnionId = value; }
        }

        public override EntitySet<Unit> Units
        {
            get { return Parent.Units; }
            set { Parent.Units = value; }
        }

        public override bool Updated
        {
            get { return Parent.Updated; }
            set { Parent.Updated = value; }
        }

        public override VisibleSpace VisibleSpace
        {
            get { return Parent.VisibleSpace; }
            set { Parent.VisibleSpace = value; }
        }

        public override ICommunity Community
        {
            get { return _community; }
        }

        public override IEvaluator Evaluator
        {
            get { return Parent.Evaluator; }
            set { Parent.Evaluator = value; }
        }

        public override Vector2D Heading
        {
            get { return Parent.Heading; }
            set { Parent.Heading = value; }
        }

        public override double Mass
        {
            get { return Parent.Mass; }
        }

        public override double MaxForce
        {
            get { return Parent.MaxForce; }
            set { Parent.MaxForce = value; }
        }

        public override double MaxSpeed
        {
            get { return Parent.MaxSpeed; }
            set { Parent.MaxSpeed = value; }
        }

        public override Vector2D Position
        {
            get { return Parent.Position; }
            set { Parent.Position = value; }
        }

        public override Vector2D Side
        {
            get { return Parent.Side; }
            set { Parent.Side = value; }
        }

        public override Vector2D Velocity
        {
            get { return Parent.Velocity; }
            set { Parent.Velocity = value; }
        }

        #endregion

    }// END CLASS DEFINITION BotKing

} // AliveChess.Logic.GameLogic.BotsLogic
