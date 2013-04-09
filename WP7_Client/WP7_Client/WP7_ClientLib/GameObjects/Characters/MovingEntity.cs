using System;
using AliveChessLibrary.Mathematic.GeometryUtils;

namespace AliveChessLibrary.GameObjects.Characters
{
    public abstract class MovingEntity
    {
        private readonly double _mDMass;
        private double _mDMaxForce;
        private double _mDMaxSpeed;
        private Vector2D _mPosition;
        //a normalized vector pointing in the direction the entity is heading. 
        private Vector2D _mVHeading;
        //a vector perpendicular to the heading vector
        private Vector2D _mVSide;
        private Vector2D _mVVelocity;

        protected MovingEntity()
        {
            _mDMaxSpeed = 1;
            _mDMass = 1;
            _mDMaxForce = 1;
            _mVHeading = new Vector2D(1, 0);
            _mVVelocity = new Vector2D(1, 1);
            _mPosition = new Vector2D();
        }

        protected MovingEntity(Vector2D position,
                            Vector2D velocity,
                            double maxSpeed,
                            Vector2D heading,
                            double mass,
                            double maxForce)
        {
            _mPosition = position;
            _mVVelocity = velocity;
            _mDMaxSpeed = maxSpeed;
            _mVHeading = heading;
            _mDMass = mass;
            _mDMaxForce = maxForce;
        }

        public virtual Vector2D Position
        {
            get { return _mPosition; }
            set { _mPosition = value; }
        }

        public virtual Vector2D Velocity
        {
            get { return _mVVelocity; }
            set { _mVVelocity = value; }
        }

        public virtual Vector2D Heading
        {
            get { return _mVHeading; }
            set { _mVHeading = value; }
        }

        public virtual Vector2D Side
        {
            get
            {
                if (_mVSide == null && _mVHeading != null)
                    _mVSide = _mVHeading.Perp();
                return _mVSide;
            }
            set { _mVSide = value; }
        }

        public virtual double Mass
        {
            get { return _mDMass; }
        }

        public virtual double MaxSpeed
        {
            get { return _mDMaxSpeed; }
            set { _mDMaxSpeed = value; }
        }

        public virtual double MaxForce
        {
            get { return _mDMaxForce; }
            set { _mDMaxForce = value; }
        }

        public double Speed
        {
            get { return _mVVelocity.Length(); }
        }

        public double SpeedSq
        {
            get { return _mVVelocity.LengthSq(); }
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
            var rotationMatrix = new Matrix2D();

            //notice how the direction of rotation has to be determined when creating
            //the rotation matrix
            rotationMatrix.Rotate(angle*(int) Heading.Sign(toTarget));
            rotationMatrix.TransformVector2Ds(_mVHeading);
            rotationMatrix.TransformVector2Ds(_mVVelocity);

            //finally recreate m_vSide
            _mVSide = _mVHeading.Perp();
            return false;
        }

        public bool IsSpeedMaxedOut()
        {
            return _mDMaxSpeed*_mDMaxSpeed >= _mVVelocity.LengthSq();
        }
    }
}