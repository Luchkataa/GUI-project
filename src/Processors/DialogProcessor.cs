using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Draw.src.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Draw
{
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}

        #endregion

        #region Properties

        /// <summary>
        /// Избран елемент.
        /// </summary>
        private List<Shape> selection = new List<Shape>();
        public List<Shape> Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        /// <summary>
        /// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
        /// </summary>
        private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		
		#endregion
		
		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.White;

			ShapeList.Add(rect);
        }

        public void AddRandomStar()
        {
			Random rnd = new Random();
            float x = rnd.Next(100, 1000);
            float y = rnd.Next(100, 600);
			float size = 100;

            StarShape star = new StarShape(new RectangleF(x, y, size, size))
            {
                FillColor = Color.LightYellow
            };

            ShapeList.Add(star);
        }

        public void AddRandomElipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            ElipseShape ellipse = new ElipseShape(new Rectangle(x, y, 100, 200))
            {
                FillColor = Color.Green,
                BorderColor = Color.Black
            };

            ShapeList.Add(ellipse);
        }

        public void AddRandomTriangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TriangleShape triangle = new TriangleShape(new Rectangle(x, y, 100, 200))
            {
                FillColor = Color.LightBlue,
                BorderColor = Color.Black
            };

            ShapeList.Add(triangle);
        }

        public void AddRandomTrapezoid()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TrapezoidShape trapezoid = new TrapezoidShape(new Rectangle(x, y, 100, 200))
            {
                FillColor = Color.Brown,
                BorderColor = Color.Black
            };

            ShapeList.Add(trapezoid);
        }

        public void AddRandomCircle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            CrossXCircleShape circle = new CrossXCircleShape(new RectangleF(x, y, 100, 200))
            {
                FillColor = Color.Pink,
                BorderColor = Color.Black
            };

            ShapeList.Add(circle);
        }

        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
        public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
						
					return ShapeList[i];
				}	
			}
			return null;
		}

        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        public void TranslateTo(PointF p)
        {
            float dx = p.X - lastLocation.X;
            float dy = p.Y - lastLocation.Y;

            foreach (var shape in Selection)
            {
                shape.Translate(dx, dy);
            }

            lastLocation = p;
        }

        /// <summary>
        /// Променя цвета на контура на селектираната форма.
        /// </summary>
        /// <param name="color">Новият цвят на контура.</param>
        public void SetBorderColor(Color color)
        {
            foreach (var shape in Selection)
            {
                shape.BorderColor = color;
            }
        }


        /// <summary>
        /// Завъртане на примитива.
        /// </summary>
        public void Rotate(float angle)
        {
            foreach (var shape in Selection)
            {
                Matrix rotateMatrix = shape.TransformMatrix.Clone();

                PointF center = new PointF(
                    shape.Rectangle.Left + shape.Rectangle.Width / 2,
                    shape.Rectangle.Top + shape.Rectangle.Height / 2
                );

                rotateMatrix.RotateAt(angle, center);
                shape.TransformMatrix = rotateMatrix;
            }
        }

        public void ScaleAt(float scaleX, float scaleY, PointF center)
        {
            foreach (var shape in Selection)
            {
                Matrix transform = shape.TransformMatrix.Clone();
                transform.Translate(-center.X, -center.Y, MatrixOrder.Append);
                transform.Scale(scaleX, scaleY, MatrixOrder.Append);
                transform.Translate(center.X, center.Y, MatrixOrder.Append);
                shape.TransformMatrix = transform;
            }
        }

        public override void ReDraw(object sender, PaintEventArgs e)
        {
            base.ReDraw(sender, e);

            using (Pen selectionPen = new Pen(Color.DarkBlue))
            {
                selectionPen.DashStyle = DashStyle.Dash;

                foreach (var shape in Selection)
                {
                    RectangleF bounds = shape.GetTransformedBounds();
                    e.Graphics.DrawRectangle(selectionPen, Rectangle.Round(bounds));
                }
            }
        }
        public void GroupSelectedShapes()
        {
            if (Selection.Count <= 1) return;

            GroupShape group = new GroupShape();

            foreach (var shape in Selection)
            {
                group.SubShapes.Add(shape);
            }

            foreach (var shape in group.SubShapes)
            {
                ShapeList.Remove(shape);
            }

            ShapeList.Add(group);
            Selection.Clear();
            Selection.Add(group);
        }

        public void UngroupSelectedShape()
        {
            if (Selection.Count != 1 || !(Selection[0] is GroupShape group))
                return;

            ShapeList.Remove(group);

            foreach (var shape in group.SubShapes)
            {
                ShapeList.Add(shape);
            }

            Selection.Clear();
            Selection.AddRange(group.SubShapes);
        }

        public void DeleteSelected()
        {
            foreach (var shape in Selection.ToList())
            {
                if (ShapeList.Contains(shape))
                {
                    ShapeList.Remove(shape);
                }
            }

            Selection.Clear();
        }

    }
}
