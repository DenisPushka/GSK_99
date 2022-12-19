
namespace GSK_99
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxMain = new System.Windows.Forms.PictureBox();
            this.PaintCubeSpline = new System.Windows.Forms.Button();
            this.SelectColorComboBox = new System.Windows.Forms.ComboBox();
            this.SelectFigureComboBox = new System.Windows.Forms.ComboBox();
            this.SelectTMOComboBox = new System.Windows.Forms.ComboBox();
            this.ButtonForTMO_ = new System.Windows.Forms.Button();
            this.GTComboBox = new System.Windows.Forms.ComboBox();
            this.ToPaint = new System.Windows.Forms.Button();
            this.ButtonClear_ = new System.Windows.Forms.Button();
            this.DeleteFigure_ = new System.Windows.Forms.Button();
            this.Angle = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxMain
            // 
            this.pictureBoxMain.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBoxMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxMain.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxMain.Location = new System.Drawing.Point(342, 0);
            this.pictureBoxMain.Name = "pictureBoxMain";
            this.pictureBoxMain.Size = new System.Drawing.Size(458, 440);
            this.pictureBoxMain.TabIndex = 0;
            this.pictureBoxMain.TabStop = false;
            this.pictureBoxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureMouseDown);
            this.pictureBoxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMouseMove);
            // 
            // PaintCubeSpline
            // 
            this.PaintCubeSpline.Location = new System.Drawing.Point(12, 195);
            this.PaintCubeSpline.Margin = new System.Windows.Forms.Padding(5);
            this.PaintCubeSpline.Name = "PaintCubeSpline";
            this.PaintCubeSpline.Size = new System.Drawing.Size(148, 60);
            this.PaintCubeSpline.TabIndex = 2;
            this.PaintCubeSpline.Text = "Нарисовать кубический сплайн";
            this.PaintCubeSpline.UseVisualStyleBackColor = true;
            this.PaintCubeSpline.Click += new System.EventHandler(this.ButtonCubeSpline);
            // 
            // SelectColorComboBox
            // 
            this.SelectColorComboBox.FormattingEnabled = true;
            this.SelectColorComboBox.Items.AddRange(new object[] {"Черный", "Красный", "Синий", "Зеленый"});
            this.SelectColorComboBox.Location = new System.Drawing.Point(12, 91);
            this.SelectColorComboBox.Name = "SelectColorComboBox";
            this.SelectColorComboBox.Size = new System.Drawing.Size(148, 21);
            this.SelectColorComboBox.TabIndex = 1;
            this.SelectColorComboBox.Text = "Цвет";
            this.SelectColorComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectColor);
            // 
            // SelectFigureComboBox
            // 
            this.SelectFigureComboBox.FormattingEnabled = true;
            this.SelectFigureComboBox.Items.AddRange(new object[] {"Фигура 4", "Стрелка 3"});
            this.SelectFigureComboBox.Location = new System.Drawing.Point(12, 142);
            this.SelectFigureComboBox.Name = "SelectFigureComboBox";
            this.SelectFigureComboBox.Size = new System.Drawing.Size(148, 21);
            this.SelectFigureComboBox.TabIndex = 0;
            this.SelectFigureComboBox.Text = "Выбор фигуры";
            this.SelectFigureComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectFigure);
            // 
            // SelectTMOComboBox
            // 
            this.SelectTMOComboBox.FormattingEnabled = true;
            this.SelectTMOComboBox.Items.AddRange(new object[] {"Объединение", "Пересечение", "Симметрическая разность", "Разность А/В", "Разность В/А"});
            this.SelectTMOComboBox.Location = new System.Drawing.Point(188, 91);
            this.SelectTMOComboBox.Name = "SelectTMOComboBox";
            this.SelectTMOComboBox.Size = new System.Drawing.Size(148, 21);
            this.SelectTMOComboBox.TabIndex = 3;
            this.SelectTMOComboBox.Text = "ТМО";
            // 
            // ButtonForTMO_
            // 
            this.ButtonForTMO_.Location = new System.Drawing.Point(188, 128);
            this.ButtonForTMO_.Name = "ButtonForTMO_";
            this.ButtonForTMO_.Size = new System.Drawing.Size(148, 47);
            this.ButtonForTMO_.TabIndex = 4;
            this.ButtonForTMO_.Text = "Применить выбранное ТМО\r\n";
            this.ButtonForTMO_.UseVisualStyleBackColor = true;
            // 
            // GTComboBox
            // 
            this.GTComboBox.FormattingEnabled = true;
            this.GTComboBox.Items.AddRange(new object[] {"Перемещение", "Разворот вокруг заданного центра на 30 градусов", "Пропорциональное масштабировние относительно центра фигуры", "Зеркальное отражение относительно центра фигуры"});
            this.GTComboBox.Location = new System.Drawing.Point(12, 281);
            this.GTComboBox.Name = "GTComboBox";
            this.GTComboBox.Size = new System.Drawing.Size(324, 21);
            this.GTComboBox.TabIndex = 5;
            this.GTComboBox.Text = "Геометрические преобразования";
            this.GTComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectGt);
            // 
            // ToPaint
            // 
            this.ToPaint.Location = new System.Drawing.Point(188, 207);
            this.ToPaint.Name = "ToPaint";
            this.ToPaint.Size = new System.Drawing.Size(148, 36);
            this.ToPaint.TabIndex = 6;
            this.ToPaint.Text = "Перейти в свободное рисование";
            this.ToPaint.UseVisualStyleBackColor = true;
            this.ToPaint.Click += new System.EventHandler(this.ToPaint_Click);
            // 
            // ButtonClear_
            // 
            this.ButtonClear_.Location = new System.Drawing.Point(36, 319);
            this.ButtonClear_.Name = "ButtonClear_";
            this.ButtonClear_.Size = new System.Drawing.Size(101, 40);
            this.ButtonClear_.TabIndex = 7;
            this.ButtonClear_.Text = "Очистка экрана";
            this.ButtonClear_.UseVisualStyleBackColor = true;
            this.ButtonClear_.Click += new System.EventHandler(this.ButtonClear__Click);
            // 
            // DeleteFigure_
            // 
            this.DeleteFigure_.Location = new System.Drawing.Point(205, 322);
            this.DeleteFigure_.Name = "DeleteFigure_";
            this.DeleteFigure_.Size = new System.Drawing.Size(101, 37);
            this.DeleteFigure_.TabIndex = 8;
            this.DeleteFigure_.Text = "Удаление фигуры";
            this.DeleteFigure_.UseVisualStyleBackColor = true;
            this.DeleteFigure_.Click += new System.EventHandler(this.DeleteFigure__Click);
            // 
            // Angle
            // 
            this.Angle.Location = new System.Drawing.Point(122, 383);
            this.Angle.Name = "Angle";
            this.Angle.Size = new System.Drawing.Size(100, 20);
            this.Angle.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(800, 440);
            this.Controls.Add(this.Angle);
            this.Controls.Add(this.DeleteFigure_);
            this.Controls.Add(this.ButtonClear_);
            this.Controls.Add(this.ToPaint);
            this.Controls.Add(this.GTComboBox);
            this.Controls.Add(this.ButtonForTMO_);
            this.Controls.Add(this.SelectTMOComboBox);
            this.Controls.Add(this.SelectFigureComboBox);
            this.Controls.Add(this.SelectColorComboBox);
            this.Controls.Add(this.PaintCubeSpline);
            this.Controls.Add(this.pictureBoxMain);
            this.Name = "Form1";
            this.Text = "Курсовая работа";
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox Angle;

        private System.Windows.Forms.Button DeleteFigure_;

        private System.Windows.Forms.Button ButtonClear_;

        private System.Windows.Forms.Button ToPaint;

        private System.Windows.Forms.ComboBox GTComboBox;

        private System.Windows.Forms.Button ButtonForTMO_;

        private System.Windows.Forms.ComboBox SelectTMOComboBox;

        private System.Windows.Forms.Button PaintCubeSpline;

        private System.Windows.Forms.ComboBox SelectColorComboBox;

        private System.Windows.Forms.ComboBox SelectFigureComboBox;

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMain;
    }
}

