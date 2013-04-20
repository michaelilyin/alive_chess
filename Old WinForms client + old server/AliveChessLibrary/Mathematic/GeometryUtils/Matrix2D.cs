using System;
using System.Collections.Generic;

namespace AliveChessLibrary.Mathematic.GeometryUtils
{
    public class Matrix2D
    {
        public class Matrix 
        {
            double _11 = 0, _12 = 0, _13 = 0;
            double _21 = 0, _22 = 0, _23 = 0;
            double _31 = 0, _32 = 0, _33 = 0;
            public double M13
            {
                get { return _13; }
                set { _13 = value; }
            }

            public double M12
            {
                get { return _12; }
                set { _12 = value; }
            }

            public double M11
            {
                get { return _11; }
                set { _11 = value; }
            }            

            public double M23
            {
                get { return _23; }
                set { _23 = value; }
            }

            public double M22
            {
                get { return _22; }
                set { _22 = value; }
            }

            public double M21
            {
                get { return _21; }
                set { _21 = value; }
            }            

            public double M33
            {
                get { return _33; }
                set { _33 = value; }
            }

            public double M32
            {
                get { return _32; }
                set { _32 = value; }
            }

            public double M31
            {
                get { return _31; }
                set { _31 = value; }
            }
        }
        private Matrix mat=new Matrix();
        public Matrix2D()
        {
            Identity();
        }
        public Matrix TransMatrix
        {
            get { return mat; }
            set { mat = value; }
        }
        public void TransformVector2Ds(IList<Vector2D> vPoints)
        {
            foreach(Vector2D vec2d in vPoints)
            {
                double tempX = (mat.M11 * vec2d.X) + (mat.M21 * vec2d.Y) + (mat.M31);
                double tempY = (mat.M12 * vec2d.X) + (mat.M22 * vec2d.Y) + (mat.M32);            
                vec2d.X = (int)tempX;
                vec2d.Y = (int)tempY;
            }
        }
        //applies a 2D transformation matrix to a single Vector2D
        public void TransformVector2Ds(Vector2D vPoint)
        {

          double tempX =(TransMatrix.M11*vPoint.X) + (TransMatrix.M21*vPoint.Y) + (TransMatrix.M31);

          double tempY = (TransMatrix.M12*vPoint.X) + (TransMatrix.M22*vPoint.Y) + (TransMatrix.M32);
          
          vPoint.X = (int)tempX;

          vPoint.Y = (int)tempY;
        }
        //multiply two matrices together
        private void MatrixMultiply(Matrix mIn)
        {
            Matrix mat_temp=new Matrix();
            
            //first row
            mat_temp.M11 = (TransMatrix.M11 * mIn.M11) + (TransMatrix.M12 * mIn.M21) + (TransMatrix.M13 * mIn.M31);
            mat_temp.M12 = (TransMatrix.M11 * mIn.M12) + (TransMatrix.M12 * mIn.M22) + (TransMatrix.M13 * mIn.M32);
            mat_temp.M13 = (TransMatrix.M11 * mIn.M13) + (TransMatrix.M12 * mIn.M23) + (TransMatrix.M13 * mIn.M33);

            //second
            mat_temp.M21 = (TransMatrix.M21 * mIn.M11) + (TransMatrix.M22 * mIn.M21) + (TransMatrix.M23 * mIn.M31);
            mat_temp.M22 = (TransMatrix.M21 * mIn.M12) + (TransMatrix.M22 * mIn.M22) + (TransMatrix.M23 * mIn.M32);
            mat_temp.M23 = (TransMatrix.M21 * mIn.M13) + (TransMatrix.M22 * mIn.M23) + (TransMatrix.M23 * mIn.M33);

            //third
            mat_temp.M31 = (TransMatrix.M31 * mIn.M11) + (TransMatrix.M32 * mIn.M21) + (TransMatrix.M33 * mIn.M31);
            mat_temp.M32 = (TransMatrix.M31 * mIn.M12) + (TransMatrix.M32 * mIn.M22) + (TransMatrix.M33 * mIn.M32);
            mat_temp.M33 = (TransMatrix.M31 * mIn.M13) + (TransMatrix.M32 * mIn.M23) + (TransMatrix.M33 * mIn.M33);

            TransMatrix = mat_temp;
        }

        //create an identity matrix
        public void Identity()
        {
            mat.M11 = 1; mat.M12 = 0; mat.M13 = 0;
            mat.M21 = 0; mat.M22 = 1; mat.M23 = 0;
            mat.M31 = 0; mat.M32 = 0; mat.M33 = 1;
        }

        //create a transformation matrix
        public void Translate(double x, double y)
        {
            Matrix mat_temp=new Matrix();          
            mat_temp.M11 = 1; mat_temp.M12 = 0; mat_temp.M13 = 0;          
            mat_temp.M21 = 0; mat_temp.M22 = 1; mat_temp.M23 = 0;          
            mat_temp.M31 = x; mat_temp.M32 = y;    mat_temp.M33 = 1;          
            //and multiply
            MatrixMultiply(mat_temp);
        }

        //create a scale matrix
        public void Scale(double xScale, double yScale)
        {
            Matrix mat_temp=new Matrix();          
            mat_temp.M11 = xScale; mat_temp.M12 = 0; mat_temp.M13 = 0;          
            mat_temp.M21 = 0; mat_temp.M22 = yScale; mat_temp.M23 = 0;          
            mat_temp.M31 = 0; mat_temp.M32 = 0; mat_temp.M33 = 1;          
            //and multiply
            MatrixMultiply(mat_temp);
        }

        //create a rotation matrix
        public void Rotate(double rot)
        {
            Matrix mat_temp=new Matrix();
            double Sin = Math.Sin(rot);
            double Cos = Math.Cos(rot);          
            mat_temp.M11 = Cos;  mat_temp.M12 = Sin; mat_temp.M13 = 0;          
            mat_temp.M21 = -Sin; mat_temp.M22 = Cos; mat_temp.M23 = 0;          
            mat_temp.M31 = 0; mat_temp.M32 = 0;mat_temp.M33 = 1;          
            //and multiply
            MatrixMultiply(mat_temp);
        }

        //create a rotation matrix from a 2D vector
        public void Rotate(Vector2D fwd, Vector2D side)
        {
            Matrix mat_temp=new Matrix();          
            mat_temp.M11 = fwd.X;  mat_temp.M12 = fwd.Y; mat_temp.M13 = 0;          
            mat_temp.M21 = side.X; mat_temp.M22 = side.Y; mat_temp.M23 = 0;          
            mat_temp.M31 = 0; mat_temp.M32 = 0;mat_temp.M33 = 1;          
            //and multiply
            MatrixMultiply(mat_temp);
        }
    }
}
