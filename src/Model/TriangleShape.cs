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

            var points = new PointF[]
            {
            new PointF(Location.X + Width / 2, Location.Y),
            new PointF(Location.X, Location.Y + Height),
            new PointF(Location.X + Width, Location.Y + Height)
            };

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, FillColor)))
            {
                grfx.FillPolygon(brush, points);
            }

            using (Pen borderPen = new Pen(BorderColor, BorderWidth))
            {
                grfx.DrawPolygon(borderPen, points);
            }
        }

        public override bool Contains(PointF point)
        {
            var points = new PointF[]
            {
                new PointF(Location.X + Width / 2, Location.Y),
                new PointF(Location.X, Location.Y + Height),
                new PointF(Location.X + Width, Location.Y + Height)
            };

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddPolygon(points);
                return path.IsVisible(point);
            }
        }
    }
}
