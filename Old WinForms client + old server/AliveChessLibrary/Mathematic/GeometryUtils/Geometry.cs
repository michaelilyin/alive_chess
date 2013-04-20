namespace AliveChessLibrary.Mathematic.GeometryUtils
{
    public static class Geometry
    {
        public static bool LineIntersection2D(Vector2D A,
                               Vector2D B,
                               Vector2D C,
                               Vector2D D,
                               double dist,
                               Vector2D point)
        {

            double rTop = (A.Y - C.Y) * (D.X - C.X) - (A.X - C.X) * (D.Y - C.Y);
            double rBot = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);
            double sTop = (A.Y - C.Y) * (B.X - A.X) - (A.X - C.X) * (B.Y - A.Y);
            double sBot = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);
            if ((rBot == 0) || (sBot == 0))
            {
                //lines are parallel
                return false;
            }
            double r = rTop / rBot;
            double s = sTop / sBot;
            if ((r > 0) && (r < 1) && (s > 0) && (s < 1))
            {
                dist = Vector2D.Vec2DDistance(A, B) * r;
                point = A + r * (B - A);
                return true;
            }
            else
            {
                dist = 0;
                return false;
            }
        }
    }
}
