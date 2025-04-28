using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Draw.src.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
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
		private Shape selection;
		public Shape Selection {
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
			if (selection != null) {
				selection.Location = new PointF(selection.Location.X + p.X - lastLocation.X, selection.Location.Y + p.Y - lastLocation.Y);
				lastLocation = p;
			}
		}
        /// <summary>
        /// Променя цвета на контура на селектираната форма.
        /// </summary>
        /// <param name="color">Новият цвят на контура.</param>
        public void SetBorderColor(Color color)
        {
            if (Selection != null)
            {
                Selection.BorderColor = color;
            }
        }


        /// <summary>
        /// Завъртане на примитива.
        /// </summary>
        public void Rotate(float angle)
        {
            if (Selection != null)
            {
                Matrix rotateMatrix = Selection.TransformMatrix.Clone();

                PointF center = new PointF(
                    Selection.Rectangle.Left + Selection.Rectangle.Width / 2,
                    Selection.Rectangle.Top + Selection.Rectangle.Height / 2
                );

                rotateMatrix.RotateAt(angle, center);

                Selection.TransformMatrix = rotateMatrix;
            }
        }

    }
}
