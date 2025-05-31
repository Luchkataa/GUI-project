using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
    public class TriangleShape : Shape
    {
        public TriangleShape(Rectangle rect) : base(rect)
        {
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            GraphicsState state = grfx.Save();

            grfx.MultiplyTransform(this.TransformMatrix);

            PointF top = new PointF(Rectangle.Left + Rectangle.Width / 2, Rectangle.Top);
            PointF left = new PointF(Rectangle.Left, Rectangle.Bottom);
            PointF right = new PointF(Rectangle.Right, Rectangle.Bottom);

            PointF[] points = { top, left, right };

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

            var points = new PointF[]
            {
                new PointF(Location.X + Width / 2, Location.Y),          // Top
                new PointF(Location.X, Location.Y + Height),             // Left
                new PointF(Location.X + Width, Location.Y + Height)      // Right
            };

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddPolygon(points);
                return path.IsVisible(localPoint);
            }
        }

    }
}
