using System;
using System.Drawing;

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

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, FillColor)))
            {
                grfx.FillEllipse(brush, Rectangle);
            }

            using (Pen borderPen = new Pen(BorderColor, BorderWidth))
            {
                grfx.DrawEllipse(borderPen, Rectangle);
            }
        }

    }
}
