using System;
using AliveChessLibrary.Mathematic.GeometryUtils;

namespace AliveChessLibrary.GameObjects.Characters
{
    public abstract class MovingEntity
    {
        private Vector2D m_position;
        private Vector2D m_vVelocity;
        //a normalized vector pointing in the direction the entity is heading. 
        private Vector2D m_vHeading;
        //a vector perpendicular to the heading vector
        private Vector2D m_vSide;
        private double m_dMass;
        //the maximum speed this entity may travel at.
        private double m_dMaxSpeed;
        //the maximum force this entity can produce to power itself 
        //(think rockets and thrust)
        private double m_dMaxForce;

        private King _target;

        public MovingEntity()
        {
            this.m_dMaxSpeed = 1;
            this.m_dMass = 1;
            this.m_dMaxForce = 1;
            this.m_vHeading = new Vector2D(1, 0);
            this.m_vVelocity = new Vector2D(1, 1);
            this.m_position = new Vector2D();
        }

        public MovingEntity(Vector2D position,
            Vector2D velocity,
            double max_speed,
            Vector2D heading,
            double mass,
            double max_force)
        {
            this.m_position = position;
            this.m_vVelocity = velocity;
            this.m_dMaxSpeed = max_speed;
            this.m_vHeading = heading;
            this.m_dMass = mass;
            this.m_dMaxForce = max_force;
        }

        private double Clamp(double arg, double minVal, double maxVal)
        {
            if (arg < minVal)
            {
                arg = minVal;
            }

            if (arg > maxVal)
            {
                arg = maxVal;
            }
            return arg;
        }
        //--------------------------- RotateHeadingToFacePosition ---------------------
        //
        //  given a target position, this method rotates the entity's heading and
        //  side vectors by an amount not greater than m_dMaxTurnRate until it
        //  directly faces the target.
        //
        //  returns true when the heading is facing in the desired direction
        //-----------------------------------------------------------------------------
        public bool RotateHeadingToFacePosition(Vector2D target)
        {
            Vector2D toTarget = Vector2D.Vec2DNormalize(target - Position);

            double dot = Heading.Dot(toTarget);

            //some compilers lose acurracy so the value is clamped to ensure it
            //remains valid for the acos
            dot = Clamp(dot, -1, 1);

            //first determine the angle between the heading vector and the target
            double angle = Math.Acos(dot);

            //return true if the player is facing the target
            if (angle < 0.00001) return true;

            ////clamp the amount to turn to the max turn rate
            //if (angle > m_dMaxTurnRate) angle = m_dMaxTurnRate;

            //The next few lines use a rotation matrix to rotate the player's heading
            //vector accordingly
            Matrix2D RotationMatrix = new Matrix2D();

            //notice how the direction of rotation has to be determined when creating
            //the rotation matrix
            RotationMatrix.Rotate(angle * (int)Heading.Sign(toTarget));
            RotationMatrix.TransformVector2Ds(m_vHeading);
            RotationMatrix.TransformVector2Ds(m_vVelocity);

            //finally recreate m_vSide
            m_vSide = m_vHeading.Perp();
            return false;
        }

        public virtual Vector2D Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
            }
        }

        public virtual Vector2D Velocity
        {
            get { return m_vVelocity; }
            set { m_vVelocity = value; }
        }

        public virtual Vector2D Heading
        {
            get { return m_vHeading; }
            set { m_vHeading = value; }
        }

        public virtual Vector2D Side
        {
            get
            {
                if (m_vSide == null && m_vHeading != null)
                    m_vSide = m_vHeading.Perp();
                return m_vSide;
            }
            set { m_vSide = value; }
        }

        public virtual double Mass
        {
            get { return m_dMass; }
        }

        public virtual double MaxSpeed
        {
            get { return m_dMaxSpeed; }
            set { m_dMaxSpeed = value; }
        }

        public virtual double MaxForce
        {
            get { return m_dMaxForce; }
            set { m_dMaxForce = value; }
        }

        public bool IsSpeedMaxedOut() { return m_dMaxSpeed * m_dMaxSpeed >= m_vVelocity.LengthSq(); }
        public double Speed { get { return m_vVelocity.Length(); } }
        public double SpeedSq { get { return m_vVelocity.LengthSq(); } }
    }
}
