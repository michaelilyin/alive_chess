namespace AliveChessLibrary.Mathematic.GeometryUtils
{
    public class Wall2D
    {
        private Vector2D m_vA, m_vB, m_vN;
        public Vector2D Normal
        {
            get { return m_vN; }
            set { m_vN = value; }
        }
        public Vector2D To
        {
            get { return m_vB; }
            set { m_vB = value; CalculateNormal(); }
        }
        public Vector2D From
        {
            get { return m_vA; }
            set { m_vA = value; CalculateNormal(); }
        }
        public void CalculateNormal()
        {
            Vector2D temp = Vector2D.Vec2DNormalize(To - From);
            Normal.X = -temp.Y;
            Normal.Y = temp.X;
        }
        public Vector2D Center() { return (From + To) / 2.0; }
        public Wall2D() { }
        public Wall2D(Vector2D A, Vector2D B)
        {
            this.m_vA = A;
            this.m_vB = B;
            CalculateNormal();
        }
        public Wall2D(Vector2D A, Vector2D B, Vector2D N)
        {
            this.m_vA = A;
            this.m_vB = B;
            this.m_vN = N;
        }        
    }
}
