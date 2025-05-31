using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Draw
{
	/// <summary>
	/// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
	/// </summary>
	public abstract class Shape
	{
		#region Constructors
		
		public Shape()
		{
		}
		
		public Shape(RectangleF rect)
		{
			rectangle = rect;
		}
		
		public Shape(Shape shape)
		{
			this.Height = shape.Height;
			this.Width = shape.Width;
			this.Location = shape.Location;
			this.rectangle = shape.rectangle;
			
			this.FillColor =  shape.FillColor;
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Обхващащ правоъгълник на елемента.
		/// </summary>
		private RectangleF rectangle;		
		public virtual RectangleF Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}
		
		/// <summary>
		/// Широчина на елемента.
		/// </summary>
		public virtual float Width {
			get { return Rectangle.Width; }
			set { rectangle.Width = value; }
		}
		
		/// <summary>
		/// Височина на елемента.
		/// </summary>
		public virtual float Height {
			get { return Rectangle.Height; }
			set { rectangle.Height = value; }
		}
		
		/// <summary>
		/// Горен ляв ъгъл на елемента.
		/// </summary>
		public virtual PointF Location {
			get { return Rectangle.Location; }
			set { rectangle.Location = value; }
		}

        public virtual PointF GetCenter()
        {
            return new PointF(
                Rectangle.X + Rectangle.Width / 2f,
                Rectangle.Y + Rectangle.Height / 2f
            );
        }

        /// <summary>
        /// Цвят на елемента.
        /// </summary>
        private Color fillColor;
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}

        /// <summary>
        /// Цвят на контура на елемента.
        /// </summary>
        private Color borderColor = Color.Black;
        public virtual Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        /// <summary>
        /// Ширина на рамката.
        /// </summary>
        private float borderWidth = 1f;
        public virtual float BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; }
        }

        /// <summary>
        /// Въртене на примитив.
        /// </summary>
        private Matrix transformMatrix = new Matrix();
        public virtual Matrix TransformMatrix
        {
            get { return transformMatrix; }
            set { transformMatrix = value; }
        }
        #endregion


        /// <summary>
        /// Проверка дали точка point принадлежи на елемента.
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns>Връща true, ако точката принадлежи на елемента и
        /// false, ако не пренадлежи</returns>
        public virtual bool Contains(PointF point)
		{
            PointF[] pts = new PointF[] { point };

            Matrix inverse = TransformMatrix.Clone();
            inverse.Invert();
            inverse.TransformPoints(pts);

            return Rectangle.Contains(pts[0]);
        }
		
		/// <summary>
		/// Визуализира елемента.
		/// </summary>
		/// <param name="grfx">Къде да бъде визуализиран елемента.</param>
		public virtual void DrawSelf(Graphics grfx)
		{
			// shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth);
		}

        public RectangleF GetTransformedBounds()
        {
            PointF[] corners = new PointF[]
            {
        new PointF(Rectangle.Left, Rectangle.Top),
        new PointF(Rectangle.Right, Rectangle.Top),
        new PointF(Rectangle.Right, Rectangle.Bottom),
        new PointF(Rectangle.Left, Rectangle.Bottom)
            };

            TransformMatrix.TransformPoints(corners);

            float minX = corners.Min(p => p.X);
            float maxX = corners.Max(p => p.X);
            float minY = corners.Min(p => p.Y);
            float maxY = corners.Max(p => p.Y);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public virtual void Translate(float dx, float dy)
        {
            TransformMatrix.Translate(dx, dy, MatrixOrder.Append);
            Rectangle = new RectangleF(Rectangle.X + dx, Rectangle.Y + dy, Rectangle.Width, Rectangle.Height);
        }
        public virtual void Rotate(float angle, PointF center)
        {
            TransformMatrix.RotateAt(angle, center, MatrixOrder.Append);
        }

        public virtual void Scale(float scaleX, float scaleY, PointF center)
        {
            TransformMatrix.Translate(-center.X, -center.Y, MatrixOrder.Append);
            TransformMatrix.Scale(scaleX, scaleY, MatrixOrder.Append);
            TransformMatrix.Translate(center.X, center.Y, MatrixOrder.Append);
        }

        public virtual void Rotate(float angle)
        {
            Rotate(angle, GetCenter());
        }

        public virtual void Scale(float scaleX, float scaleY)
        {
            Scale(scaleX, scaleY, GetCenter());
        }
    }
}
