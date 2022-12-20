using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GSK_99
{
    public partial class Form1 : Form
    {
        private readonly Graphics _graphics;
        private readonly Pen _drawPen = new Pen(Color.Black, 1);
        private Figure _figure;

        /// <summary>
        /// Множество для ТМО
        /// </summary>
        private int[] _setQ = new int[2];

        /// <summary>
        ///  Список фигур
        /// </summary>
        private readonly List<Figure> _figures = new List<Figure>();

        /// <summary>
        ///  Буффер
        /// </summary>
        private readonly Bitmap _bitmap;

        /// <summary>
        ///  Выбор гемометрической операции
        /// </summary>
        private int _operation;

        #region For move figure

        private bool _checkFigure;
        private Point _pictureBoxMousePosition;
        private Figure _figureForMove;

        #endregion

        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBoxMain.Width, pictureBoxMain.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _figure = new Figure(pictureBoxMain.Width, pictureBoxMain.Height, _graphics, _drawPen);
            _operation = -1;
            MouseWheel += GeometricTransformation;
        }

        // Обработчик события "нажатие мыши"
        private void PictureMouseDown(object sender, MouseEventArgs e)
        {
            _pictureBoxMousePosition = e.Location;
            switch (_operation)
            {
                // Удаление
                case -3:
                    if (SearchSelectFigure(e))
                    {
                        _figureForMove.DrawPen.Color = Color.White;
                        _figureForMove.Fill();
                        if (_figures.Any(figure => figure == _figureForMove))
                        {
                            _figures.Remove(_figure);
                            pictureBoxMain.Image = _bitmap;
                            return;
                        }

                        _figure = null;
                    }
                    else
                        _checkFigure = false;

                    break;
                // Рисование
                // Рисование примитива
                case -2:
                    // Создание фигуры4
                    if (SelectFigureComboBox.SelectedIndex == 0)
                    {
                        if (_figure.Vertices.Count < 1)
                        {
                            _figure.AddVertex(e.X, e.Y);
                            break;
                        }

                        if (_figure.Vertices.Count == 1) _figure.AddVertex(e.X, e.Y);
                        if (_figure.Vertices.Count == 2) CreateFg4();
                    }
                    // Создание стрелки3
                    else
                        CreateStr3(e);

                    _figure.DrawPen = _drawPen;
                    _figure.Fill();
                    _figures.Add(_figure.Cloning());
                    _figure = new Figure(pictureBoxMain.Width, pictureBoxMain.Height, _graphics,
                        new Pen(_drawPen.Color));
                    break;
                // Добавление точки
                case -1 when e.Button == MouseButtons.Left:
                {
                    _figure.AddVertex(e.X, e.Y);
                    break;
                }
                // Создание фигуры
                case -1 when e.Button == MouseButtons.Right:
                {
                    _figure.DrawPen = _drawPen;
                    _figure.Fill();
                    _figures.Add(_figure.Cloning());
                    _figure = new Figure(pictureBoxMain.Width, pictureBoxMain.Height, _graphics,
                        new Pen(_drawPen.Color));
                    break;
                }
                // Перемещение
                case 0:
                    if (SearchSelectFigure(e))
                    {
                        _graphics.DrawEllipse(new Pen(Color.Blue), e.X - 2, e.Y - 2, 10, 10);
                        _checkFigure = true;
                    }
                    else
                        _checkFigure = false;

                    break;
            }

            pictureBoxMain.Image = _bitmap;
        }

        // Обработчие события "Двигать мышь"
        private void PictureBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _operation == 0 & _checkFigure)
            {
                // Провека пересения с другими фигурами
                var figuresB = new List<Figure>();
                figuresB = SearchIntersection(figuresB, _figureForMove);

                _figureForMove.Move(e.X - _pictureBoxMousePosition.X, e.Y - _pictureBoxMousePosition.Y);

                if (_figureForMove.IsHaveTmo)
                {
                    _figures[_figures.Count - 1]
                        .Move(e.X - _pictureBoxMousePosition.X, e.Y - _pictureBoxMousePosition.Y);

                    // PaintTmo(_figureForMove, _figures[_figures.Count - 1]);
                    Tmo(_figureForMove, _figures[_figures.Count - 1]);

                    if (figuresB.Contains(_figures[_figures.Count - 1]))
                        figuresB.Remove(_figures[_figures.Count - 1]);
                }

                foreach (var figure in figuresB)
                    figure.Fill();

                pictureBoxMain.Image = _bitmap;
                _pictureBoxMousePosition = e.Location;
            }
        }

        private void GeometricTransformation(object sender, MouseEventArgs e)
        {
            var figureBuff = _figures[_figures.Count - 1];
            if (figureBuff.IsHaveTmo)
            {
                OperationGeometric(figureBuff, e);
                OperationGeometric(_figures[_figures.Count - 2], e);
                PaintTmo(figureBuff, _figures[_figures.Count - 2]);
            }
            else
                OperationGeometric(figureBuff, e);
        }

        private void PaintTmo(Figure figureBuff, Figure figure2)
        {
            var colorBuff1 = figure2.DrawPen.Color;
            var colorBuff2 = figureBuff.DrawPen.Color;
            figure2.DrawPen.Color = Color.White;
            figureBuff.DrawPen.Color = Color.White;
            figure2.Fill();
            figureBuff.Fill();
            figure2.DrawPen.Color = colorBuff1;
            figureBuff.DrawPen.Color = colorBuff2;
            Tmo(figureBuff, _figures[_figures.Count - 2]);
            pictureBoxMain.Image = _bitmap;
        }

        private void OperationGeometric(Figure figureBuff, MouseEventArgs e)
        {
            // Провека пересения с другими фигурами
            var figuresB = new List<Figure>();
            figuresB = SearchIntersection(figuresB, figureBuff);
            switch (_operation)
            {
                // Вращение
                case 1:
                    figureBuff.Rotation(e.Delta, e, Angle);
                    break;
                // Масштабирование
                case 2:
                    figureBuff.Zoom(e.Delta);
                    break;
                // Отражение
                case 3:
                    figureBuff.Mirror();
                    break;
            }

            foreach (var figure in figuresB)
                figure.Fill();
            pictureBoxMain.Image = _bitmap;
        }

        /// <summary>
        /// Поиск пересечений
        /// </summary>
        /// <param name="figuresB">Список фигур пересечений</param>
        /// <param name="figure">Движемая фигура</param>
        private List<Figure> SearchIntersection(List<Figure> figuresB, Figure figure)
        {
            foreach (var t in _figures)
                if (AddVertexFromSecondFigure(figure, t)
                    || AddVertexFromSecondFigure(t, figure))
                {
                    if (figure.Equals(t)) continue;
                    figuresB.Add(t);
                }

            figure.Index = -1;
            return figuresB;
        }

        /// <summary>
        /// Провека пересения фигугры1 с фигурой 2 по вхождению вершин фигуры1 в фигуру 2
        /// </summary>
        /// <param name="figureOne">Фигура 1</param>
        /// <param name="figureSecond">Фигура 2</param>
        private static bool AddVertexFromSecondFigure(Figure figureOne, Figure figureSecond) => figureOne.Vertices
            .Any(vertex => figureSecond
                .ThisFigure((int) vertex.X, (int) vertex.Y));

        private bool SearchSelectFigure(MouseEventArgs e)
        {
            for (var index = 0; index < _figures.Count; index++)
                if (_figures[index].ThisFigure(e.X, e.Y))
                {
                    _figureForMove = _figures[index];
                    _figureForMove.Index = index;
                    return true;
                }

            return false;
        }

        private void SelectFigure(object sender, EventArgs e) => _operation = -2;

        private void SelectGt(object sender, EventArgs e) => _operation = GTComboBox.SelectedIndex;

        private void ToPaint_Click(object sender, EventArgs e) => _operation = -1;

        private void SelectColor(object sender, EventArgs e)
        {
            switch (SelectColorComboBox.SelectedIndex)
            {
                case 0:
                    _drawPen.Color = Color.Black;
                    break;
                case 1:
                    _drawPen.Color = Color.Red;
                    break;
                case 2:
                    _drawPen.Color = Color.Blue;
                    break;
                case 3:
                    _drawPen.Color = Color.Green;
                    break;
            }
        }

        private void ButtonCubeSpline(object sender, EventArgs e)
        {
            if (_figure.Vertices.Count >= 4)
            {
                // Создание куб сплайна
                CreateCubeSpline();
                var figuresB = new List<Figure>();
                figuresB = SearchIntersection(figuresB, _figure);

                // добавление его в список фигур
                _figure.IsFunction = true;
                _figure.Fill();
                _figures.Add(_figure.Cloning());
                _figure = new Figure(pictureBoxMain.Width, pictureBoxMain.Height, _graphics, _drawPen);

                // Отрисовка фигур, которые пересекаются с функцуией
                foreach (var figure in figuresB)
                    figure.Fill();
                pictureBoxMain.Image = _bitmap;
            }
        }

        #region Построение фигур и функций

        private void CreateCubeSpline()
        {
            var color = _figure.DrawPen.Color;
            _figure.DrawPen.Color = Color.White;
            _figure.Fill();
            _figure.DrawPen.Color = color;
            var function = new List<Vertex>();
            var l = new PointF[4];
            var pv1 = _figure.Vertices[0].ToPoint();
            var pv2 = _figure.Vertices[0].ToPoint();

            const double dt = 0.04;
            double t = 0;
            Point pred = _figure.Vertices[0].ToPoint(), pt = _figure.Vertices[0].ToPoint();

            pv1.X = (int) (4 * (_figure.Vertices[1].X - _figure.Vertices[0].X));
            pv1.Y = (int) (4 * (_figure.Vertices[1].Y - _figure.Vertices[0].Y));
            pv2.X = (int) (4 * (_figure.Vertices[3].X - _figure.Vertices[2].X));
            pv2.Y = (int) (4 * (_figure.Vertices[3].Y - _figure.Vertices[2].Y));

            l[0].X = 2 * _figure.Vertices[0].X - 2 * _figure.Vertices[2].X + pv1.X + pv2.X; // Ax
            l[0].Y = 2 * _figure.Vertices[0].Y - 2 * _figure.Vertices[2].Y + pv1.Y + pv2.Y; // Ay
            l[1].X = -3 * _figure.Vertices[0].X + 3 * _figure.Vertices[2].X - 2 * pv1.X - pv2.X; // Bx
            l[1].Y = -3 * _figure.Vertices[0].Y + 3 * _figure.Vertices[2].Y - 2 * pv1.Y - pv2.Y; // By
            l[2].X = pv1.X; // Cx
            l[2].Y = pv1.Y; // Cy
            l[3].X = _figure.Vertices[0].X; // Dx 
            l[3].Y = _figure.Vertices[0].Y; // Dy 
            function.Add(new Vertex(pred.X, pred.Y));
            while (t < 1 + dt / 2)
            {
                var xt = ((l[0].X * t + l[1].X) * t + l[2].X) * t + l[3].X;
                var yt = ((l[0].Y * t + l[1].Y) * t + l[2].Y) * t + l[3].Y;

                pt.X = (int) Math.Round(xt);
                pt.Y = (int) Math.Round(yt);

                _graphics.DrawLine(_drawPen, pred, pt);
                pred = pt;
                function.Add(new Vertex(pred.X, pred.Y));
                t += dt;
            }

            _figure.Vertices = function;
        }

        private void CreateStr3(MouseEventArgs e)
        {
            var str3 = new List<Vertex>
            {
                new Vertex(e.X - 75, e.Y - 35),
                new Vertex(e.X + 50, e.Y - 35),
                new Vertex(e.X + 75, e.Y),

                new Vertex(e.X + 50, e.Y + 35),
                new Vertex(e.X - 75, e.Y + 35),
                new Vertex(e.X - 50, e.Y)
            };
            _figure.Vertices = str3;
        }

        private void CreateFg4()
        {
            _graphics.DrawLine(new Pen(Color.White), _figure.Vertices[0].ToPoint(), _figure.Vertices[1].ToPoint());
            var a = _figure.Vertices[0];
            var b = _figure.Vertices[1];
            var width = (b.X + a.X) / 2;
            var height = (b.Y + a.Y) / 2;

            var fg4 = new List<Vertex>
            {
                new Vertex(a.X + width / 4, a.Y),
                new Vertex(a.X, b.Y),
                new Vertex(a.X + width / 4, b.Y),
                new Vertex(width, b.Y + 2 * height / 3),
                new Vertex(b.X - width / 4, b.Y),
                new Vertex(b.X, b.Y),
                new Vertex(b.X - width / 4, a.Y)
            };
            _figure.Vertices = fg4;
        }

        #endregion

        private void ButtonClear__Click(object sender, EventArgs e)
        {
            _figures.Clear();
            _figure = new Figure(pictureBoxMain.Width, pictureBoxMain.Height, _graphics, new Pen(_drawPen.Color));
            pictureBoxMain.Image = _bitmap;
            _graphics.Clear(Color.White);
            _operation = -1;
        }

        private void DeleteFigure__Click(object sender, EventArgs e) => _operation = -3;

        #region ТМО

        // Алгоритм теоретико-множественных операций
        private void Tmo(Figure figure1, Figure figure2)
        {
            var arr = figure1.SearchYMinAndMax();
            var arr2 = figure2.SearchYMinAndMax();
            figure1.IsHaveTmo = true;
            figure2.IsHaveTmo = true;
            var minY = arr[0] < arr2[0] ? arr[0] : arr2[0];
            var maxY = arr[1] > arr2[1] ? arr[1] : arr2[1];
            for (var y = (int) minY; y < maxY; y++)
            {
                var a = figure1.CalculationListXrAndXl(y);
                var xAl = a[0];
                var xAr = a[1];
                var b = figure2.CalculationListXrAndXl(y);
                var xBl = b[0];
                var xBr = b[1];
                if (xAl.Count == 0 && xBl.Count == 0)
                    continue;

                #region Заполнение массива arrayM

                var arrayM = new M[xAl.Count * 2 + xBl.Count * 2];
                for (var i = 0; i < xAl.Count; i++)
                    arrayM[i] = new M(xAl[i], 2);

                var nM = xAl.Count;
                for (var i = 0; i < xAr.Count; i++)
                    arrayM[nM + i] = new M(xAr[i], -2);

                nM += xAr.Count;
                for (var i = 0; i < xBl.Count; i++)
                    arrayM[nM + i] = new M(xBl[i], 1);

                nM += xBl.Count;
                for (var i = 0; i < xBr.Count; i++)
                    arrayM[nM + i] = new M(xBr[i], -1);
                nM += xBr.Count;

                #endregion

                // Сортировка
                SortArrayM(arrayM);

                var q = 0;
                var xrl = new List<int>();
                var xrr = new List<int>();
                // Особый случай для правой границы сегмента
                if (arrayM[0].X >= 0 && arrayM[0].Dq < 0)
                {
                    xrl.Add(0);
                    q = -arrayM[1].Dq;
                }

                for (var i = 0; i < nM; i++)
                {
                    var x = arrayM[i].X;
                    var qNew = q + arrayM[i].Dq;
                    if (!IncludeQInSetQ(q) && IncludeQInSetQ(qNew))
                        xrl.Add((int) x);
                    else if (IncludeQInSetQ(q) && !IncludeQInSetQ(qNew))
                        xrr.Add((int) x);

                    q = qNew;
                }

                // Если не найдена правая граница последнего сегмента
                if (IncludeQInSetQ(q))
                    xrr.Add(pictureBoxMain.Height);

                for (var i = 0; i < xrr.Count; i++)
                    _graphics.DrawLine(_drawPen, new Point(xrr[i], y), new Point(xrl[i], y));
            }
        }

        // Проверка вхождения Q в множество setQ
        private bool IncludeQInSetQ(int q) => _setQ[0] <= q && q <= _setQ[1];

        /// <summary>
        ///  Сортировка по Х
        /// </summary>
        private static void SortArrayM(IList<M> arrayM)
        {
            for (var write = 0; write < arrayM.Count; write++)
            for (var sort = 0; sort < arrayM.Count - 1; sort++)
                if (arrayM[sort].X > arrayM[sort + 1].X)
                {
                    var buff = new M(arrayM[sort + 1].X, arrayM[sort + 1].Dq);
                    arrayM[sort + 1] = arrayM[sort];
                    arrayM[sort] = buff;
                }
        }

        private void ButtonForTMO__Click(object sender, EventArgs e)
        {
            if (_figures.Count > 1)
            {
                var fig1 = _figures[_figures.Count - 2];
                var fig2 = _figures[_figures.Count - 1];
                var colorBuff1 = fig1.DrawPen.Color;
                var colorBuff2 = fig2.DrawPen.Color;
                fig1.DrawPen.Color = Color.White;
                fig2.DrawPen.Color = Color.White;
                fig1.Fill();
                fig2.Fill();
                fig1.DrawPen.Color = colorBuff1;
                fig2.DrawPen.Color = colorBuff2;
                Tmo(fig1, fig2);
            }

            pictureBoxMain.Image = _bitmap;
        }

        // Выбор ТМО
        private void SelectTMOComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SelectTMOComboBox.SelectedIndex)
            {
                case 0:
                    _setQ = new[] {1, 3}; // Объединение
                    break;
                case 1:
                    _setQ = new[] {3, 3}; // Пересечение
                    break;
                case 2:
                    _setQ = new[] {1, 2}; // Симметричная разность
                    break;
                case 3:
                    _setQ = new[] {2, 2}; // Разность А/В
                    break;
                case 4:
                    _setQ = new[] {1, 1}; // Разность В/А
                    break;
            }
        }

        #endregion
    }
}