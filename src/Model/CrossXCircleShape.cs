using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CrossXCircleShape : Shape
    {
        public CrossXCircleShape() : base() { }

        public CrossXCircleShape(RectangleF rect) : base(rect) { }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            GraphicsState state = grfx.Save();
            grfx.MultiplyTransform(this.TransformMatrix);

            float size = Math.Min(Rectangle.Width, Rectangle.Height);
            RectangleF circleRect = new RectangleF(
                Rectangle.X + (Rectangle.Width - size) / 2,
                Rectangle.Y + (Rectangle.Height - size) / 2,
                size,
                size
            );

            PointF center = new PointF(circleRect.X + size / 2, circleRect.Y + size / 2);
            float radius = size / 2;

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, FillColor)))
            {
                grfx.FillEllipse(brush, circleRect);
            }

            using (Pen pen = new Pen(BorderColor, BorderWidth))
            {
                grfx.DrawEllipse(pen, circleRect);

                for (int i = 0; i < 2; i++)
                {
                    double angle1 = (45 + i * 90) * Math.PI / 180;
                    double angle2 = (225 + i * 90) * Math.PI / 180;

                    PointF p1 = new PointF(
                        center.X + radius * (float)Math.Cos(angle1),
                        center.Y + radius * (float)Math.Sin(angle1));

                    PointF p2 = new PointF(
                        center.X + radius * (float)Math.Cos(angle2),
                        center.Y + radius * (float)Math.Sin(angle2));

                    grfx.DrawLine(pen, p1, p2);
                }
            }

            grfx.Restore(state);
        }

        public override bool Contains(PointF point)
        {
            float size = Math.Min(Rectangle.Width, Rectangle.Height);
            RectangleF circleRect = new RectangleF(
                Rectangle.X + (Rectangle.Width - size) / 2,
                Rectangle.Y + (Rectangle.Height - size) / 2,
                size,
                size
            );

            Matrix inverse = TransformMatrix.Clone();
            inverse.Invert();
            PointF[] pts = { point };
            inverse.TransformPoints(pts);
            point = pts[0];

            float centerX = circleRect.X + size / 2;
            float centerY = circleRect.Y + size / 2;
            float radius = size / 2;

            float dx = point.X - centerX;
            float dy = point.Y - centerY;

            return dx * dx + dy * dy <= radius * radius;
        }
    }
}