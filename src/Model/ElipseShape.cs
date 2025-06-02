using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw.src.Model
{
    [Serializable]
    public class ElipseShape : Shape
    {
        public ElipseShape(Rectangle rect) : base(rect)
        {
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            GraphicsState state = grfx.Save();

            grfx.MultiplyTransform(this.TransformMatrix);

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, FillColor)))
            {
                grfx.FillEllipse(brush, Rectangle);
            }

            using (Pen borderPen = new Pen(BorderColor, BorderWidth))
            {
                grfx.DrawEllipse(borderPen, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            }

            grfx.Restore(state);
        }

        public override bool Contains(PointF point)
        {
            PointF[] pts = new PointF[] { point };

            Matrix inverse = TransformMatrix.Clone();
            inverse.Invert();
            inverse.TransformPoints(pts);

            PointF localPoint = pts[0];

            float a = Width / 2f;   // Полу-голяма ос
            float b = Height / 2f;  // Полу-малка ос
            float centerX = Location.X + a;
            float centerY = Location.Y + b;

            // if point in elipse
            float normalizedX = (localPoint.X - centerX) / a;
            float normalizedY = (localPoint.Y - centerY) / b;

            return (normalizedX * normalizedX + normalizedY * normalizedY) <= 1;
        }

    }
}
