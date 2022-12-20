using System.Drawing;

namespace GSK_99
{
    public struct Vertex
    {
        public float X;
        public float Y;
        public readonly float Thirst;

        public Vertex(float x = 0.0f, float y = 0.0f, float thirst = 1.0f) {
            X = x;
            Y = y;
            Thirst = thirst;
        }

        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
            Thirst = 1.0f;
        }
        
        public Point ToPoint() => new Point((int) X, (int) Y);
    }
}