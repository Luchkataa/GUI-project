using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw.src.Model
{
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
            float a = Width / 2;   // Полу-голяма ос
            float b = Height / 2;  // Полу-малка ос
            float centerX = Location.X + a;
            float centerY = Location.Y + b;

            float normalizedX = (point.X - centerX) / a;
            float normalizedY = (point.Y - centerY) / b;

            return (normalizedX * normalizedX + normalizedY * normalizedY) <= 1;
        }

    }
}
