using System.Collections.Generic;

namespace AliveChessLibrary.Mathematic.GeometryUtils
{
    public static class Transformations
    {
        //--------------------------- WorldTransform -----------------------------
        //
        //  given a std::vector of 2D vectors, a position, orientation and scale,
        //  this function transforms the 2D vectors into the object's world space
        //------------------------------------------------------------------------
        public static IList<Vector2D> WorldTransform(IList<Vector2D> points,
                                                    Vector2D pos,
                                                    Vector2D forward,
                                                    Vector2D side,
                                                    Vector2D scale)
        {
            //copy the original vertices into the buffer about to be transformed
            IList<Vector2D> TranVector2Ds = points;
            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            //scale
            if ((scale.X != 1.0) || (scale.Y != 1.0))
            {
                matTransform.Scale(scale.X, scale.Y);
            }
            //rotate
            matTransform.Rotate(forward, side);
            //and translate
            matTransform.Translate(pos.X, pos.Y);
            //now transform the object's vertices
            matTransform.TransformVector2Ds(TranVector2Ds);
            return TranVector2Ds;
        }

        //--------------------------- WorldTransform -----------------------------
        //
        //  given a std::vector of 2D vectors, a position and  orientation
        //  this function transforms the 2D vectors into the object's world space
        //------------------------------------------------------------------------
        public static IList<Vector2D> WorldTransform(IList<Vector2D> points,
                                         Vector2D pos,
                                         Vector2D forward,
                                         Vector2D side)
        {
            //copy the original vertices into the buffer about to be transformed
            IList<Vector2D> TranVector2Ds = points;
            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            //rotate
            matTransform.Rotate(forward, side);
            //and translate
            matTransform.Translate(pos.X, pos.Y);
            //now transform the object's vertices
            matTransform.TransformVector2Ds(TranVector2Ds);
            return TranVector2Ds;
        }

        //--------------------- PointToWorldSpace --------------------------------
        //
        //  Transforms a point from the agent's local space into world space
        //------------------------------------------------------------------------
        public static Vector2D PointToWorldSpace(Vector2D point,
                                            Vector2D AgentHeading,
                                            Vector2D AgentSide,
                                            Vector2D AgentPosition)
        {
            //make a copy of the point
            Vector2D TransPoint = point;
            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            //rotate
            matTransform.Rotate(AgentHeading, AgentSide);
            //and translate
            matTransform.Translate(AgentPosition.X, AgentPosition.Y);
            //now transform the vertices
            matTransform.TransformVector2Ds(TransPoint);
            return TransPoint;
        }

        //--------------------- VectorToWorldSpace --------------------------------
        //
        //  Transforms a vector from the agent's local space into world space
        //------------------------------------------------------------------------
        public static Vector2D VectorToWorldSpace(Vector2D vec,
                                             Vector2D AgentHeading,
                                             Vector2D AgentSide)
        {
            //make a copy of the point
            Vector2D TransVec = vec;
            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            //rotate
            matTransform.Rotate(AgentHeading, AgentSide);
            //now transform the vertices
            matTransform.TransformVector2Ds(TransVec);
            return TransVec;
        }


        //--------------------- PointToLocalSpace --------------------------------
        //
        //------------------------------------------------------------------------
        public static Vector2D PointToLocalSpace(Vector2D point,
                                     Vector2D AgentHeading,
                                     Vector2D AgentSide,
                                      Vector2D AgentPosition)
        {

            //make a copy of the point
            Vector2D TransPoint = point;
            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            double Tx = -AgentPosition.Dot(AgentHeading);
            double Ty = -AgentPosition.Dot(AgentSide);
            //create the transformation matrix
            matTransform.TransMatrix.M11 = AgentHeading.X; matTransform.TransMatrix.M12 = AgentSide.X;
            matTransform.TransMatrix.M21 = AgentHeading.Y; matTransform.TransMatrix.M22 = AgentSide.Y;
            matTransform.TransMatrix.M31 = Tx; matTransform.TransMatrix.M32 = Ty;
            //now transform the vertices
            matTransform.TransformVector2Ds(TransPoint);
            return TransPoint;
        }

        //--------------------- VectorToLocalSpace --------------------------------
        //
        //------------------------------------------------------------------------
        public static Vector2D VectorToLocalSpace(Vector2D vec,
                                     Vector2D AgentHeading,
                                     Vector2D AgentSide)
        {

            //make a copy of the point
            Vector2D TransPoint = vec;
            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            //create the transformation matrix
            matTransform.TransMatrix.M11 = AgentHeading.X; matTransform.TransMatrix.M12 = AgentSide.X;
            matTransform.TransMatrix.M21 = AgentHeading.Y; matTransform.TransMatrix.M22 = AgentSide.Y;
            //now transform the vertices
            matTransform.TransformVector2Ds(TransPoint);
            return TransPoint;
        }

        //-------------------------- Vec2DRotateAroundOrigin --------------------------
        //
        //  rotates a vector ang rads around the origin
        //-----------------------------------------------------------------------------
        public static void Vec2DRotateAroundOrigin(Vector2D v, double ang)
        {
            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            //rotate
            matTransform.Rotate(ang);
            //now transform the object's vertices
            matTransform.TransformVector2Ds(v);
        }

        //------------------------ CreateWhiskers ------------------------------------
        //
        //  given an origin, a facing direction, a 'field of view' describing the 
        //  limit of the outer whiskers, a whisker length and the number of whiskers
        //  this method returns a vector containing the end positions of a series
        //  of whiskers radiating away from the origin and with equal distance between
        //  them. (like the spokes of a wheel clipped to a specific segment size)
        //----------------------------------------------------------------------------
        public static IList<Vector2D> CreateWhiskers(int NumWhiskers,
                                                    double WhiskerLength,
                                                    double fov,
                                                    Vector2D facing,
                                                    Vector2D origin)
        {
            //this is the magnitude of the angle separating each whisker
            double SectorSize = fov / (double)(NumWhiskers - 1);

            IList<Vector2D> whiskers = new List<Vector2D>();
            Vector2D temp = new Vector2D(0, 0);
            double angle = -fov * 0.5;
            for (int w = 0; w < NumWhiskers; ++w)
            {
                //create the whisker extending outwards at this angle
                temp = facing;
                Vec2DRotateAroundOrigin(temp, angle);
                whiskers.Add(origin + WhiskerLength * temp);
                angle += SectorSize;
            }
            return whiskers;
        }
    }
}
