using System;
using System.Drawing;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Mathematic.GeometryUtils
{
    public enum VectorDirection {Clockwise = 1, Anticlockwise = -1};
    public class Vector2D : Position
    {
        public new double X
        { get; set; }
        public new double Y
        { get; set; }
        public Vector2D(int x,int y)
        {
            this.X=x;
            this.Y=y;
        }

        public Vector2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2D()
        {
            this.Zero();
        }

        public Vector2D(Vector2D rhs)
        {
            this.X=rhs.X;
            this.Y=rhs.Y;
        }

        public void Zero(){X=0; Y=0;}
  
        public bool IsZero() {return (X*X +Y*Y) < Double.Epsilon;}
  
        public static bool operator==(Vector2D lhs, Vector2D rhs)
        {
            if(!object.ReferenceEquals(lhs,rhs))
            {
                if (!object.Equals(lhs, null) && !object.Equals(rhs, null))
                {
                    return lhs.X.Equals(rhs.X) && lhs.Y.Equals(rhs.Y);
                }
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if(obj is Vector2D)
            {
                Vector2D vec2d = (obj as Vector2D);
                return vec2d.X.Equals(this.X) && vec2d.Y.Equals(this.Y);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator !=(Vector2D lhs, Vector2D rhs)
        {
            if (!object.ReferenceEquals(lhs, rhs))
            {                
                if (!object.Equals(lhs,null) && !object.Equals(rhs, null))
                {
                    return (lhs.X != rhs.X) || (lhs.Y != rhs.Y);
                }
                return true;
            }
            return false;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }


        //------------------------- LengthSq -------------------------------------
        //
        //  returns the squared length of a 2D vector
        //------------------------------------------------------------------------
        public double LengthSq()
        {
            return (X * X + Y * Y);
        }


        //------------------------- Vec2DDot -------------------------------------
        //
        //  calculates the dot product
        //------------------------------------------------------------------------
        public double Dot(Vector2D v2)
        {
            return X*v2.X + Y*v2.Y;
        }

        //------------------------ Sign ------------------------------------------
        //
        //  returns positive if v2 is clockwise of this vector,
        //  minus if anticlockwise (Y axis pointing down, X axis to right)
        //------------------------------------------------------------------------


        public VectorDirection Sign(Vector2D v2)
        {
            if (Y*v2.X > X*v2.Y)
            { 
              return VectorDirection.Anticlockwise;
            }
            else 
            {
              return VectorDirection.Clockwise;
            }
        }

        //------------------------------ Perp ------------------------------------
        //
        //  Returns a vector perpendicular to this vector
        //------------------------------------------------------------------------
        public Vector2D Perp()
        {
            return new Vector2D(-Y, X);
        }

        //------------------------------ Distance --------------------------------
        //
        //  calculates the euclidean distance between two vectors
        //------------------------------------------------------------------------
        public double Distance(Vector2D v2)
        {
            double ySeparation = v2.Y - Y;
            double xSeparation = v2.X - X;
            return Math.Sqrt(ySeparation*ySeparation + xSeparation*xSeparation);
        }


        //------------------------------ DistanceSq ------------------------------
        //
        //  calculates the euclidean distance squared between two vectors 
        //------------------------------------------------------------------------
        public double DistanceSq(Vector2D v2)
        {
            double ySeparation = v2.Y - Y;
            double xSeparation = v2.X - X;

            return ySeparation*ySeparation + xSeparation*xSeparation;
        }

        //----------------------------- Truncate ---------------------------------
        //
        //  truncates a vector so that its length does not exceed max
        //------------------------------------------------------------------------
        public void Truncate(double max)
        {
            if (this.Length() > max)
            {
              this.Normalize();
              Vector2D vec2d = this;
              vec2d = vec2d * max;
            } 
        }

        //--------------------------- Reflect ------------------------------------
        //
        //  given a normalized vector this method reflects the vector it
        //  is operating upon. (like the path of a ball bouncing off a wall)
        //------------------------------------------------------------------------
        public void Reflect(Vector2D norm)
        {
            Vector2D vec2d = this;
            vec2d = this + 2.0 * this.Dot(norm) * norm.GetReverse();
        }

        //----------------------- GetReverse ----------------------------------------
        //
        //  returns the vector that is the reverse of this vector
        //------------------------------------------------------------------------
        public Vector2D GetReverse()
        {
            return new Vector2D(-this.X, -this.Y);
        }


        //------------------------- Normalize ------------------------------------
        //
        //  normalizes a 2D Vector
        //------------------------------------------------------------------------
        public void Normalize()
        { 
            double vector_length = this.Length();

            if (vector_length > Double.Epsilon)
            {
              this.X = this.X / vector_length;
              this.Y = this.Y / vector_length;
            }
        }


        //------------------------------------------------------------------------non member functions

        public static Vector2D Vec2DNormalize(Vector2D v)
        {
            Vector2D vec = v;

            double vector_length = vec.Length();

            if (vector_length > Double.Epsilon)
            {
                vec.X = vec.X / vector_length;
                vec.Y = vec.Y / vector_length;
            }

            return vec;
        }


        public static double Vec2DDistance(Vector2D v1, Vector2D v2)
        {
            double ySeparation = v2.Y - v1.Y;
            double xSeparation = v2.X - v1.X;
            return Math.Sqrt(ySeparation*ySeparation + xSeparation*xSeparation);
        }

        public static double Vec2DDistanceSq(Vector2D v1, Vector2D v2)
        {
            double ySeparation = v2.Y - v1.Y;
            double xSeparation = v2.X - v1.X;
            return ySeparation*ySeparation + xSeparation*xSeparation;
        }

        public static double Vec2DLength(Vector2D v)
        {
            return Math.Sqrt(v.X*v.X + v.Y*v.Y);
        }

        public static double Vec2DLengthSq(Vector2D v)
        {
            return (v.X*v.X + v.Y*v.Y);
        }


        public static Vector2D POINTtoVector(Point p)
        {
          return new Vector2D(p.X, p.Y);
        }



        public static PointF VectorToPOINT(Vector2D v)
        {
            return new PointF((float)v.X,(float)v.Y);
        }

        //------------------------------------------------------------------------operator overloads
        public static Vector2D operator*(Vector2D lhs, double rhs)
        {
          Vector2D result=new Vector2D(lhs);
          result.X *= rhs;
          result.Y *= rhs;
          return result;
        }

        public static Vector2D operator*(double lhs, Vector2D rhs)
        {
          Vector2D result=new Vector2D(rhs);
          result.X *= lhs;
          result.Y *= lhs;
          return result;
        }

        //overload the - operator
        public static Vector2D operator-(Vector2D lhs, Vector2D rhs)
        {
          Vector2D result=new Vector2D(lhs);
          result.X -= rhs.X;
          result.Y -= rhs.Y;
          
          return result;
        }

        //overload the + operator
        public static Vector2D operator+(Vector2D lhs, Vector2D rhs)
        {
            Vector2D result = new Vector2D(lhs);
            result.X += rhs.X;
            result.Y += rhs.Y;

            return result;
        }

        //overload the / operator
        public static Vector2D operator/(Vector2D lhs, double val)
        {
            Vector2D result=new Vector2D(lhs);
            result.X /= val;
            result.Y /= val;

            return result;
        }

/////////////////////////////////////////////////////////////////////////////////


////treats a window as a toroid
//inline void WrapAround(Vector2D &pos, int MaxX, int MaxY)
//{
//  if (pos.x > MaxX) {pos.x = 0.0;}

//  if (pos.x < 0)    {pos.x = (double)MaxX;}

//  if (pos.y < 0)    {pos.y = (double)MaxY;}

//  if (pos.y > MaxY) {pos.y = 0.0;}
//}

////returns true if the point p is not inside the region defined by top_left
////and bot_rgt
//inline bool NotInsideRegion(Vector2D p,
//                            Vector2D top_left,
//                            Vector2D bot_rgt)
//{
//  return (p.x < top_left.x) || (p.x > bot_rgt.x) || 
//         (p.y < top_left.y) || (p.y > bot_rgt.y);
//}

//inline bool InsideRegion(Vector2D p,
//                         Vector2D top_left,
//                         Vector2D bot_rgt)
//{
//  return !((p.x < top_left.x) || (p.x > bot_rgt.x) || 
//         (p.y < top_left.y) || (p.y > bot_rgt.y));
//}

//inline bool InsideRegion(Vector2D p, int left, int top, int right, int bottom)
//{
//  return !( (p.x < left) || (p.x > right) || (p.y < top) || (p.y > bottom) );
//}

////------------------ isSecondInFOVOfFirst -------------------------------------
////
////  returns true if the Target position is in the field of view of the entity
////  positioned at posFirst facing in facingFirst
////-----------------------------------------------------------------------------
//inline bool isSecondInFOVOfFirst(Vector2D posFirst,
//                                 Vector2D facingFirst,
//                                 Vector2D posSecond,
//                                 double    fov)
//{
//  Vector2D toTarget = Vec2DNormalize(posSecond - posFirst);

//  return facingFirst.Dot(toTarget) >= cos(fov/2.0);
//}
     }
}
