using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    public class StarShape : Shape
    {
        public StarShape(RectangleF rect) : base(rect)
        {
        }

        public override bool Contains(PointF point)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddPolygon(CalculateStarPoints(this.Rectangle));
                return path.IsVisible(point);
            }
        }

        public override void DrawSelf(Graphics grfx)
        {
            using (SolidBrush brush = new SolidBrush(this.FillColor))
            using (Pen pen = new Pen(Color.Black, 2))
            {
                PointF[] starPoints = CalculateStarPoints(Rectangle);

                grfx.FillPolygon(brush, starPoints);

                grfx.DrawPolygon(pen, starPoints);
            }
        }

        private PointF[] CalculateStarPoints(RectangleF bounds)
        {
            float cx = bounds.X + bounds.Width / 2; //centur
            float cy = bounds.Y + bounds.Height / 2; //centur
            float radiusOuter = bounds.Width / 2; //vunshen krug
            float radiusInner = radiusOuter / 2.5f; //vutreshen krug
            int points = 5;

            PointF[] starPoints = new PointF[10];
            double angle = -Math.PI / 2; //vurha na zvezdata da e vertikalen

            for (int i = 0; i < 10; i++)
            {
                float r = (i % 2 == 0) ? radiusOuter : radiusInner; //chetni - outer ; nechetni - inner
                starPoints[i] = new PointF(
                    cx + (float)(Math.Cos(angle) * r),
                    cy + (float)(Math.Sin(angle) * r)
                );
                angle += Math.PI / points; //+36 na vsqka iteraciq na loop-a, na 10tata spira
            }
            return starPoints;
        }
    }
}
