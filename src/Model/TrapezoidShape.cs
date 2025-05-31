using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw.src.Model
{
    public class TrapezoidShape : Shape
    {
        public TrapezoidShape(Rectangle rect) : base(rect)
        {
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            GraphicsState state = grfx.Save();

            grfx.MultiplyTransform(this.TransformMatrix);

            PointF topLeft = new PointF(Rectangle.Left + Rectangle.Width / 4, Rectangle.Top);
            PointF topRight = new PointF(Rectangle.Right - Rectangle.Width / 4, Rectangle.Top);
            PointF bottomRight = new PointF(Rectangle.Right, Rectangle.Bottom);
            PointF bottomLeft = new PointF(Rectangle.Left, Rectangle.Bottom);

            PointF[] points = { topLeft, topRight, bottomRight, bottomLeft };

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, FillColor)))
            {
                grfx.FillPolygon(brush, points);
            }

            using (Pen borderPen = new Pen(BorderColor, BorderWidth))
            {
                grfx.DrawPolygon(borderPen, points);
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

            float topWidth = Width * 0.5f;
            float bottomWidth = Width;

            var points = new PointF[]
            {
                new PointF(Location.X + (Width - topWidth) / 2, Location.Y),
                new PointF(Location.X + (Width + topWidth) / 2, Location.Y),
                new PointF(Location.X + (Width + bottomWidth) / 2, Location.Y + Height),
                new PointF(Location.X + (Width - bottomWidth) / 2, Location.Y + Height)
            };

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddPolygon(points);
                return path.IsVisible(localPoint);
            }
        }


    }
}
