using System;
using System.Drawing;

namespace Draw.src.Model
{
    public class TrapezoidShape : Shape
    {
        public TrapezoidShape(Rectangle rect) : base(rect)
        {
        }

        public override void DrawSelf(Graphics grfx)
        {
            float topWidth = Width * 0.5f;
            float bottomWidth = Width;

            var points = new PointF[]
            {
                new PointF(Location.X + (Width - topWidth) / 2, Location.Y), //gore lqvo
                new PointF(Location.X + (Width + topWidth) / 2, Location.Y), // gore dqsno
                new PointF(Location.X + (Width + bottomWidth) / 2, Location.Y + Height), //dolu lqvo
                new PointF(Location.X + (Width - bottomWidth) / 2, Location.Y + Height) // dolu dqsno
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
    }
}
