using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GSK_99
{
    public class Figure
    {
        public List<Vertex> Vertices { get; set; }
        public bool IsFunction { get; set; }
        public bool IsHaveTmo { get; set; }

        /// <summary>
        /// Нужен для проверки при перемещении
        /// </summary>
        public int Index { get; set; } = -1;

        private Graphics Graphics { get; }
        public Pen DrawPen { get; set; }
        private readonly int _width;
        private readonly int _height;
        
        // public override bool Equals(object obj)
        // {
        //     var figs = (List<Vertex>) obj;
        //     return Vertices
        //         .Any(vertex => figs
        //             .Any(ver => vertex.X == ver.X && vertex.Y == ver.Y));
        // }

        public bool Equals(Figure other) => 
            Equals(Vertices, other.Vertices) && IsFunction == other.IsFunction && IsHaveTmo == other.IsHaveTmo;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Vertices != null ? Vertices.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsFunction.GetHashCode();
                hashCode = (hashCode * 397) ^ IsHaveTmo.GetHashCode();
                return hashCode;
            }
        }

        public Figure(int width, int height, Graphics graphics, Pen drawPen)
        {
            Vertices = new List<Vertex>();
            _width = width;
            _height = height;
            Graphics = graphics;
            DrawPen = drawPen;
        }

        public Figure(int width, int height, List<Vertex> vertices, Graphics graphics, Pen drawPen)
        {
            _width = width;
            _height = height;
            Vertices = vertices;
            Graphics = graphics;
            DrawPen = drawPen;
        }

        public void AddVertex(float x, float y)
        {
            Vertices.Add(new Vertex(x, y));
            Graphics.DrawEllipse(new Pen(Color.Blue), x - 2, y - 2, 10, 10);
            if (Vertices.Count > 1)
                Graphics.DrawLine(DrawPen, Vertices[Vertices.Count - 2].ToPoint(),
                    Vertices[Vertices.Count - 1].ToPoint());
        }

        public Figure Cloning() => new Figure(_width, _height, Vertices.ToList(), Graphics, DrawPen)
            {IsFunction = IsFunction, IsHaveTmo = IsHaveTmo};

        #region Все для закрашивания

        // Алгоритм закрашивания внутри многоугольника
        public void Fill()
        {
            if (IsFunction)
            {
                PaintingLineInFigure();
                return;
            }

            var arr = SearchYMinAndMax();
            var min = arr[0];
            var max = arr[1];
            var xs = new List<float>();

            for (var y = (int) min; y < max; y++)
            {
                var k = 0;
                for (var i = 0; i < Vertices.Count - 1; i++)
                {
                    k = i < Vertices.Count ? i + 1 : 1;
                    xs = CheckIntersection(xs, i, k, y);
                }

                xs = CheckIntersection(xs, k, 0, y);
                xs.Sort();

                for (var i = 0; i + 1 < xs.Count; i += 2)
                    Graphics.DrawLine(DrawPen, new Point((int) xs[i], y), new Point((int) xs[i + 1], y));

                xs.Clear();
            }
        }

        public List<float>[] CalculationListXrAndXl(int y)
        {
            var k = 0;
            var xR = new List<float>();
            var xL = new List<float>();
            for (var i = 0; i < Vertices.Count - 1; i++)
            {
                k = i < Vertices.Count ? i + 1 : 1;
                if (Check(i, k, y))
                {
                    var x = -((y * (Vertices[i].X - Vertices[k].X))
                                - Vertices[i].X * Vertices[k].Y + Vertices[k].X * Vertices[i].Y)
                            / (Vertices[k].Y - Vertices[i].Y);
                    if (Vertices[k].Y - Vertices[i].Y > 0)
                        xR.Add(x);
                    else
                        xL.Add(x);
                }
            }

            if (Check(k, 0, y))
            {
                var x = -(y * (Vertices[k].X - Vertices[0].X)
                            - Vertices[k].X * Vertices[0].Y + Vertices[0].X * Vertices[k].Y)
                        / (Vertices[0].Y - Vertices[k].Y);
                if (Vertices[0].Y - Vertices[k].Y > 0)
                    xR.Add(x);
                else
                    xL.Add(x);
            }

            return new[] {xL, xR};
        }

        /// <summary>
        ///  Условие пересечения
        /// </summary>
        private bool Check(int i, int k, int y) =>
            (Vertices[i].Y < y && Vertices[k].Y >= y) || (Vertices[i].Y >= y && Vertices[k].Y < y);

        /// <summary>
        ///  Проверка пересичения прямой Y c отрезком
        /// </summary>
        private List<float> CheckIntersection(List<float> xs, int i, int k, int y)
        {
            if (Check(i, k, y))
            {
                var x = -((y * (Vertices[i].X - Vertices[k].X)) - Vertices[i].X * Vertices[k].Y +
                          Vertices[k].X * Vertices[i].Y)
                        / (Vertices[k].Y - Vertices[i].Y);
                xs.Add(x);
            }

            return xs;
        }

        private void PaintingLineInFigure()
        {
            for (var i = 0; i < Vertices.Count - 1; i++)
                Graphics.DrawLine(DrawPen, Vertices[i].ToPoint(), Vertices[i + 1].ToPoint());
        }

        #endregion

        // Поиск мин/макс X
        private float[] SearchXMinAndMax()
        {
            var min = Vertices[0].X;
            var max = 0.0f;
            foreach (var t in Vertices)
            {
                min = t.X < min ? t.X : min;
                max = t.X > max ? t.X : max;
            }

            return new[] {min, max};
        }

        // Поиск мин/макс Y
        public float[] SearchYMinAndMax()
        {
            if (Vertices.Count == 0)
                return new float[] {0, 0, 0};

            var min = Vertices[0].Y;
            var max = Vertices[0].Y;
            var j = 0;
            for (var i = 0; i < Vertices.Count; i++)
            {
                var item = Vertices[i];
                min = Vertices[i].Y < min ? Vertices[i].Y : min;

                if (item.Y > max)
                {
                    max = item.Y;
                    j = i;
                }
            }

            min = min < 0 ? 0 : min;
            max = max > _height ? _height : max;
            return new[] {min, max, j};
        }

        #region Геометрические преобразования

        public bool ThisFigure(int mx, int my)
        {
            var m = 0;
            for (var i = 0; i <= Vertices.Count - 1; i++)
            {
                var k = i < Vertices.Count - 1 ? i + 1 : 0;
                var pi = Vertices[i];
                var pk = Vertices[k];
                if ((pi.Y < my) & (pk.Y >= my) | (pi.Y >= my) & (pk.Y < my)
                    && (my - pi.Y) * (pk.X - pi.X) / (pk.Y - pi.Y) + pi.X < mx)
                    m++;
            }

            return m % 2 == 1;
        }

        public void Move(int dx, int dy)
        {
            var bufferDr = FillBeginGeometricTransformation();
            for (var i = 0; i < Vertices.Count; i++)
            {
                var buffer = new Vertex(Vertices[i].X + dx, Vertices[i].Y + dy);
                Vertices[i] = buffer;
            }

            DrawPen.Color = bufferDr;
            if (!IsHaveTmo) 
                Fill();
        }

        private Color FillBeginGeometricTransformation()
        {
            var bufferDr = DrawPen.Color;
            DrawPen.Color = Color.White;
            Fill();
            return bufferDr;
        }

        private void ToAndFromCenter(bool start, Vertex e)
        {
            if (start)
            {
                float[,] toCenter =
                {
                    {1, 0, 0},
                    {0, 1, 0},
                    {-e.X, -e.Y, 1}
                };
                for (var i = 0; i < Vertices.Count; i++)
                    Vertices[i] = Matrix_1x3_3x3(Vertices[i], toCenter);
            }
            else
            {
                float[,] fromCenter =
                {
                    {1, 0, 0},
                    {0, 1, 0},
                    {e.X, e.Y, 1}
                };
                for (var i = 0; i < Vertices.Count; i++)
                    Vertices[i] = Matrix_1x3_3x3(Vertices[i], fromCenter);
            }
        }

        private Vertex CenterFigure()
        {
            var e = new Vertex();
            var arrayY = SearchYMinAndMax();
            var arrayX = SearchXMinAndMax();
            e.X = (arrayX[0] + arrayX[1]) / 2;
            e.Y = (arrayY[0] + arrayY[1]) / 2;
            return e;
        }

        /// <summary>
        ///  Отражение
        /// </summary>
        public void Mirror()
        {
            var matrix = new float[,]
            {
                {-1, 0, 0},
                {0, -1, 0},
                {0, 0, 1}
            };
            var bufferDr = FillBeginGeometricTransformation();
            var e = CenterFigure();
            ToAndFromCenter(true, e);
            for (var i = 0; i < Vertices.Count; i++)
                Vertices[i] = Matrix_1x3_3x3(Vertices[i], matrix);

            ToAndFromCenter(false, e);
            DrawPen.Color = bufferDr;
            Fill();
        }

        public void Zoom(float zoom)
        {
            zoom = zoom <= 0 ? -0.1f : 0.1f;

            var sx = 1 + zoom;
            var sy = 1 + zoom;
            float[,] matrix =
            {
                {sx, 0, 0},
                {0, sy, 0},
                {0, 0, 1}
            };
            var bufferDr = FillBeginGeometricTransformation();
            var e = CenterFigure();
            ToAndFromCenter(true, e);

            for (var i = 0; i < Vertices.Count; i++)
                Vertices[i] = Matrix_1x3_3x3(Vertices[i], matrix);

            ToAndFromCenter(false, e);
            DrawPen.Color = bufferDr;
            Fill();
        }

        private int _updateAlpha;

        public void Rotation(int mouse, MouseEventArgs em, TextBox angleBox)
        {
            float alpha = 0;
            if (mouse > 0)
            {
                alpha += 0.0175f;
                _updateAlpha++;
            }
            else
            {
                alpha -= 0.0175f;
                _updateAlpha--;
            }

            var bufferDr = FillBeginGeometricTransformation();
            angleBox.Text = _updateAlpha.ToString();
            var e = new Vertex(em.X, em.Y);
            ToAndFromCenter(true, e);

            float[,] matrixRotation =
            {
                {(float) Math.Cos(alpha), (float) Math.Sin(alpha), 0.0f},
                {-(float) Math.Sin(alpha), (float) Math.Cos(alpha), 0.0f},
                {0.0f, 0.0f, 1.0f}
            };
            for (var i = 0; i < Vertices.Count; i++)
                Vertices[i] = Matrix_1x3_3x3(Vertices[i], matrixRotation);

            ToAndFromCenter(false, e);
            DrawPen.Color = bufferDr;
            Fill();
        }

        private static Vertex Matrix_1x3_3x3(Vertex point, float[,] matrix3X3) =>
            new Vertex
            (
                point.X * matrix3X3[0, 0] + point.Y * matrix3X3[1, 0] + point.Thirst * matrix3X3[2, 0],
                point.X * matrix3X3[0, 1] + point.Y * matrix3X3[1, 1] + point.Thirst * matrix3X3[2, 1],
                point.X * matrix3X3[0, 2] + point.Y * matrix3X3[1, 2] + point.Thirst * matrix3X3[2, 2]
            );

        #endregion
    }
}