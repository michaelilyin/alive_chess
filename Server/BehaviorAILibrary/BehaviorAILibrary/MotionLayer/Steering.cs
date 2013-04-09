using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Mathematic.GeometryUtils;

namespace BehaviorAILibrary.MotionLayer
{
    public enum SummingMethod { WeightedAverage, Prioritized, Dithered };
    public enum Deceleration { Slow = 3, Normal = 2, Fast = 1 };
    [Flags]
    public enum BehaviorType
    {
        None = 0x00000,
        Seek = 0x00002,
        Flee = 0x00004,
        Arrive = 0x00008,
        Wander = 0x00010,
        Separation = 0x00040,
        WallAvoidance = 0x0080,
        Pursuit = 0x00200,
        Evade = 0x00400
    }

    /// <summary>
    /// алгоритмы движения в пространстве
    /// </summary>
    public class Steering
    {
        public const double PANIC_DISTANCE_SQUARED = 100.0 * 100.0;

        public Steering(BotKing king)
        {
            this.king = king;
            this.steeringForce = new Vector2D(this.King.X, this.King.Y);

            double theta = rnd.NextDouble() * 2 * Math.PI;

            //create a vector to a target position on the wander circle
            WanderTarget = new Vector2D(wanderRadius * Math.Cos(theta),
                                        wanderRadius * Math.Sin(theta));
        }

        private BotKing king;

        private Random rnd = new Random(DateTime.Now.Millisecond);

        //pointer to the world data
        //Raven_Game* m_pWorld;

        private Vector2D steeringForce;
        public Vector2D SteeringForce
        {
            get { return steeringForce; }
            set { steeringForce = value; }
        }

        //these can be used to keep track of friends, pursuers, or prey
        private MovingEntity targetAgent1;
        public MovingEntity TargetAgent1
        {
            get { return targetAgent1; }
            set { targetAgent1 = value; }
        }

        private MovingEntity targetAgent2;
        public MovingEntity TargetAgent2
        {
            get { return targetAgent2; }
            set { targetAgent2 = value; }
        }

        //the current Target
        private Vector2D target;
        public Vector2D Target
        {
            get { return target; }
            set { target = value; }
        }

        //a vertex buffer to contain the Feelers rqd for wall avoidance  
        private IList<Vector2D> feelers = new List<Vector2D>();
        public IList<Vector2D> Feelers
        {
            get { return feelers; }
            set { feelers = value; }
        }

        //the length of the 'feeler/s' used in wall detection
        private double wallDetectionFeelerLength = 25.0;

        //the current position on the wander circle the agent is
        //attempting to steer towards
        private Vector2D wanderTarget;
        public Vector2D WanderTarget
        {
            get { return wanderTarget; }
            set { wanderTarget = value; }
        }

        //explained above
        private double wanderJitter = 40.0;
        private double wanderRadius = 1.2;
        private double wanderDistance = 2.0;


        //multipliers. These can be adjusted to effect strength of the  
        //appropriate behavior.
        private double weightSeparation = 10.0;
        public double WeightSeparation
        {
            get { return weightSeparation; }
            set { weightSeparation = value; }
        }
        private double weightWander = 1.0;
        private double weightWallAvoidance = 10.0;
        private double weightSeek = 0.5;
        private double weightArrive = 1.0;

        //how far the agent can 'see'
        private double viewDistance = 8;

        //binary flags to indicate whether or not a behavior should be active
        private BehaviorType flags = BehaviorType.None;

        //default
        private Deceleration m_Deceleration = Deceleration.Normal;

        //is cell space partitioning to be used or not?
        private bool cellSpaceOn = false;

        public bool CellSpaceOn
        {
            get { return cellSpaceOn; }
            set { cellSpaceOn = value; }
        }

        //what type of method is used to sum any active behavior
        private SummingMethod m_SummingMethod = SummingMethod.Prioritized;

        public SummingMethod SummingMethod
        {
            get { return m_SummingMethod; }
            set { m_SummingMethod = value; }
        }

        public BotKing King
        {
            get { return king; }
        }

        //this function tests if a specific bit of m_iFlags is set
        private bool On(BehaviorType bt) { return (flags & bt) == bt; }

        private double RandomClamped()
        {
            return (rnd.NextDouble() - rnd.NextDouble());
        }

        //Steering(Raven_Game* world, Raven_Bot* agent);

        public Vector2D Calculate()
        {
            //reset the steering force
            SteeringForce.Zero();

            //tag neighbors if any of the following 3 group behaviors are switched on
            //if (On(separation))
            //{
            //  m_pWorld->TagRaven_BotsWithinViewRange(m_pRaven_Bot, m_dViewDistance);
            //}

            SteeringForce = CalculatePrioritized();

            return SteeringForce;
        }

        //calculates the component of the steering force that is parallel
        //with the Raven_Bot heading
        public double SideComponent()
        {
            return King.Side.Dot(SteeringForce);
        }
        public double ForwardComponent()
        {
            return King.Heading.Dot(SteeringForce);
        }


        private bool AccumulateForce(Vector2D RunningTot, Vector2D ForceToAdd)
        {
            //calculate how much steering force the vehicle has used so far
            double MagnitudeSoFar = RunningTot.Length();
            //calculate how much steering force remains to be used by this vehicle
            double MagnitudeRemaining = King.MaxForce - MagnitudeSoFar;
            //return false if there is no more force left to use
            if (MagnitudeRemaining <= 0.0) return false;
            //calculate the magnitude of the force we want to add
            double MagnitudeToAdd = ForceToAdd.Length();
            //if the magnitude of the sum of ForceToAdd and the running total
            //does not exceed the maximum force available to this vehicle, just
            //add together. Otherwise add as much of the ForceToAdd vector is
            //possible without going over the max.
            if (MagnitudeToAdd < MagnitudeRemaining)
            {
                // Edited by Kirichenko Igor. The reference passed by value
                RunningTot.X += ForceToAdd.X;
                RunningTot.Y += ForceToAdd.Y;

                //RunningTot += ForceToAdd;
            }
            else
            {
                MagnitudeToAdd = MagnitudeRemaining;
                //add it to the steering force
                RunningTot += (Vector2D.Vec2DNormalize(ForceToAdd) * MagnitudeToAdd);
            }
            return true;
        }

        private Vector2D CalculatePrioritized()
        {
            Vector2D force;

            if (On(BehaviorType.WallAvoidance))
            {
                force = WallAvoidance(new List<Wall2D>()) *
                        weightWallAvoidance;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            //these next three can be combined for flocking behavior (wander is
            //also a good behavior to add into this mix)
            if (On(BehaviorType.Separation))
            {
                force = Separation(King.Map.Kings) * weightSeparation;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            if (On(BehaviorType.Seek))
            {
                force = Seek(target) * weightSeek;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            if (On(BehaviorType.Arrive))
            {
                force = Arrive(target, m_Deceleration) * weightArrive;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            if (On(BehaviorType.Wander))
            {
                force = Wander() * weightWander;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            if (On(BehaviorType.Flee))
            {
                force = Flee(target) * weightWander;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            if (On(BehaviorType.Evade))
            {
                force = Evade() * weightWander;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            if (On(BehaviorType.Pursuit))
            {
                force = Pursuit() * weightWander;

                if (!AccumulateForce(SteeringForce, force)) return SteeringForce;
            }
            return SteeringForce;
        }

        /////////////////////////////////////////////////////////////////////////////// START OF BEHAVIORS

        //------------------------------- Seek -----------------------------------
        //
        //  Given a target, this behavior returns a steering force which will
        //  direct the agent towards the target
        //------------------------------------------------------------------------
        private Vector2D Seek(Vector2D target)
        {
            Vector2D DesiredVelocity = Vector2D.Vec2DNormalize(target - King.Position)
                                      * King.MaxSpeed;
            return (DesiredVelocity - King.Velocity);
        }

        private Vector2D Flee(Vector2D TargetPos)
        {
            //only flee if the target is within 'panic distance'. Work in distance
            //squared space.
            if (Vector2D.Vec2DDistanceSq(this.King.Position, target) > PANIC_DISTANCE_SQUARED)
            {
                return new Vector2D();
            }
            Vector2D DesiredVelocity = Vector2D.Vec2DNormalize(this.King.Position - TargetPos)
                                       * this.King.MaxSpeed;
            return (DesiredVelocity - this.King.Velocity);
        }

        private Vector2D Pursuit()
        {
            MovingEntity evader = this.targetAgent2;
            //if the evader is ahead and facing the agent then we can just seek
            //for the evader's current position.
            Vector2D ToEvader = evader.Position - this.King.Position;
            double RelativeHeading = this.King.Heading.Dot(evader.Heading);
            if ((ToEvader.Dot(this.King.Heading) > 0) &&
                (RelativeHeading < -0.95))  //acos(0.95)=18 degs
            {
                return Seek(evader.Position);
            }
            //Not considered ahead so we predict where the evader will be.
            //the look-ahead time is proportional to the distance between the evader
            //and the pursuer; and is inversely proportional to the sum of the
            //agents' velocities
            double LookAheadTime = ToEvader.Length() /
                                  (this.King.MaxSpeed + evader.Speed);
            //now seek to the predicted future position of the evader
            return Seek(evader.Position + evader.Velocity * LookAheadTime);
        }

        private Vector2D Evade()
        {
            MovingEntity pursuer = this.targetAgent2;
            /* Not necessary to include the check for facing direction this time */
            Vector2D ToPursuer = pursuer.Position - this.King.Position;
            //the look-ahead time is proportional to the distance between the pursuer
            //and the evader; and is inversely proportional to the sum of the
            //agents' velocities
            double LookAheadTime = ToPursuer.Length() /
                                   (this.King.MaxSpeed + pursuer.Speed);
            //now flee away from predicted future position of the pursuer
            return Flee(pursuer.Position + pursuer.Velocity * LookAheadTime);
        }

        //--------------------------- Arrive -------------------------------------
        //
        //  This behavior is similar to seek but it attempts to arrive at the
        //  target with a zero velocity
        //------------------------------------------------------------------------
        private Vector2D Arrive(Vector2D target, Deceleration deceleration)
        {
            Vector2D ToTarget = target - King.Position;
            //calculate the distance to the target
            double dist = ToTarget.Length();
            if (dist > 0)
            {
                //because Deceleration is enumerated as an int, this value is required
                //to provide fine tweaking of the deceleration..
                const double DecelerationTweaker = 0.3;
                //calculate the speed required to reach the target given the desired
                //deceleration
                double speed = dist / ((double)deceleration * DecelerationTweaker);
                //make sure the velocity does not exceed the max
                speed = Math.Min(speed, King.MaxSpeed);
                //from here proceed just like Seek except we don't need to normalize 
                //the ToTarget vector because we have already gone to the trouble
                //of calculating its length: dist. 
                Vector2D DesiredVelocity = ToTarget * speed / dist;
                return (DesiredVelocity - King.Velocity);
            }
            return new Vector2D(0, 0);
        }

        //--------------------------- Wander -------------------------------------
        //
        //  This behavior makes the agent wander about randomly
        //------------------------------------------------------------------------
        private Vector2D Wander()
        {
            //first, add a small random vector to the target's position
            WanderTarget += new Vector2D(RandomClamped() * wanderJitter,
                                         RandomClamped() * wanderJitter);
            //reproject this new vector back on to a unit circle
            WanderTarget.Normalize();
            //increase the length of the vector to the same as the radius
            //of the wander circle
            WanderTarget *= wanderRadius;
            //move the target into a position WanderDist in front of the agent
            Vector2D target = wanderTarget + new Vector2D(wanderDistance, 0);
            //project the target into world space
            Vector2D Target = Transformations.PointToWorldSpace(target,
                                                 King.Heading,
                                                 King.Side,
                                                 King.Position);
            //and steer towards it
            return Target - King.Position;
        }

        //--------------------------- WallAvoidance --------------------------------
        //
        //  This returns a steering force that will keep the agent away from any
        //  walls it may encounter
        //------------------------------------------------------------------------
        private Vector2D WallAvoidance(IList<Wall2D> walls)
        {
            //the feelers are contained in a feelers
            CreateFeelers();
            double DistToThisIP = 0.0;
            double DistToClosestIP = Double.MaxValue;
            //this will hold an index into the vector of walls
            int ClosestWall = -1;
            Vector2D SteeringForce = new Vector2D(),
                      point = new Vector2D(),         //used for storing temporary info
                      ClosestPoint = new Vector2D();  //holds the closest intersection point
            //examine each feeler in turn
            for (int flr = 0; flr < feelers.Count; ++flr)
            {
                //run through each wall checking for any intersection points
                for (int w = 0; w < walls.Count; ++w)
                {
                    if (Geometry.LineIntersection2D(King.Position,
                                           feelers[flr],
                                           walls[w].From,
                                           walls[w].To,
                                           DistToThisIP,
                                           point))
                    {
                        //is this the closest found so far? If so keep a record
                        if (DistToThisIP < DistToClosestIP)
                        {
                            DistToClosestIP = DistToThisIP;
                            ClosestWall = w;
                            ClosestPoint = point;
                        }
                    }
                }//next wall
                //if an intersection point has been detected, calculate a force  
                //that will direct the agent away
                if (ClosestWall >= 0)
                {
                    //calculate by what distance the projected position of the agent
                    //will overshoot the wall
                    Vector2D OverShoot = feelers[flr] - ClosestPoint;
                    //create a force in the direction of the wall normal, with a 
                    //magnitude of the overshoot
                    SteeringForce = walls[ClosestWall].Normal * OverShoot.Length();
                }
            }//next feeler
            return SteeringForce;
        }

        //------------------------------- CreateFeelers --------------------------
        //
        //  Creates the antenna utilized by WallAvoidance
        //------------------------------------------------------------------------
        public void CreateFeelers()
        {
            //feeler pointing straight in front
            feelers[0] = King.Position + wallDetectionFeelerLength *
                           King.Heading * King.Speed;

            //feeler to left
            Vector2D temp = King.Heading;
            Transformations.Vec2DRotateAroundOrigin(temp, Math.PI / 2 * 3.5);
            feelers[1] = King.Position + wallDetectionFeelerLength / 2.0 * temp;

            //feeler to right
            temp = King.Heading;
            Transformations.Vec2DRotateAroundOrigin(temp, Math.PI);
            feelers[2] = King.Position + wallDetectionFeelerLength / 2.0 * temp;
        }

        //---------------------------- Separation --------------------------------
        //
        // this calculates a force repelling from the other neighbors
        //------------------------------------------------------------------------
        private Vector2D Separation(IList<King> neighbors)
        {
            //iterate through all the neighbors and calculate the vector from the
            Vector2D steeringForce = new Vector2D(0, 0);
            foreach (King king in neighbors)
            {
                //make sure this agent isn't included in the calculations and that
                //the agent being examined is close enough. ***also make sure it doesn't
                //include the evade target ***
                if (!king.Equals(this.King) && !king.Equals(targetAgent1))
                {
                    Vector2D ToAgent = this.King.Position - new Vector2D(king.X, king.Y);

                    //scale the force inversely proportional to the agents distance  
                    //from its neighbor.
                    steeringForce += Vector2D.Vec2DNormalize(ToAgent) / ToAgent.Length();
                }
            }
            return steeringForce;
        }

        //Vector2D FollowPath()
        //{
        //    //move to next target if close enough to current target (working in
        //    //distance squared space)
        //    if (Vector2D.Vec2DDistanceSq(m_pPath->CurrentWaypoint(), king.Position) <
        //                       m_WaypointSeekDistSq)
        //    {
        //        m_pPath->SetNextWaypoint();
        //    }

        //    if (!m_pPath->Finished())
        //    {
        //        return Seek(m_pPath->CurrentWaypoint());
        //    }

        //    else
        //    {
        //        return Arrive(m_pPath->CurrentWaypoint(), normal);
        //    }
        //}

        public void SeekOn() { flags |= BehaviorType.Seek; }
        public void FleeOn() { flags |= BehaviorType.Flee; }
        public void ArriveOn() { flags |= BehaviorType.Arrive; }
        public void WanderOn() { flags |= BehaviorType.Wander; }
        public void SeparationOn() { flags |= BehaviorType.Separation; }
        public void WallAvoidanceOn() { flags |= BehaviorType.WallAvoidance; }
        public void PursuitOn() { flags |= BehaviorType.Pursuit; }
        public void EvadeOn() { flags |= BehaviorType.Evade; }

        public void SeekOff() { if (On(BehaviorType.Seek))   flags ^= BehaviorType.Seek; }
        public void FleeOff() { if (On(BehaviorType.Flee))   flags ^= BehaviorType.Flee; }
        public void ArriveOff() { if (On(BehaviorType.Arrive)) flags ^= BehaviorType.Arrive; }
        public void WanderOff() { if (On(BehaviorType.Wander)) flags ^= BehaviorType.Wander; }
        public void SeparationOff() { if (On(BehaviorType.Separation)) flags ^= BehaviorType.Separation; }
        public void WallAvoidanceOff() { if (On(BehaviorType.WallAvoidance)) flags ^= BehaviorType.WallAvoidance; }
        public void PursuitOff() { if (On(BehaviorType.Pursuit)) flags ^= BehaviorType.Pursuit; }
        public void EvadeOff() { if (On(BehaviorType.Evade)) flags ^= BehaviorType.Evade; }

        public bool SeekIsOn() { return On(BehaviorType.Seek); }
        public bool FleeIsOn() { return On(BehaviorType.Flee); }
        public bool ArriveIsOn() { return On(BehaviorType.Arrive); }
        public bool WanderIsOn() { return On(BehaviorType.Wander); }
        public bool SeparationIsOn() { return On(BehaviorType.Separation); }
        public bool WallAvoidanceIsOn() { return On(BehaviorType.WallAvoidance); }
        public bool PursuitIsOn() { return On(BehaviorType.Pursuit); }
        public bool EvadeIsOn() { return On(BehaviorType.Evade); }


        double WanderJitter() { return wanderJitter; }
        double WanderDistance() { return wanderDistance; }
        double WanderRadius() { return wanderRadius; }

        public void SwitchOffBehavior() { flags = BehaviorType.None; }
        public void SwitchOnBehavior(BehaviorType behaviorType) { flags = behaviorType; }
    }
}

