using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw.src.Model
{
    [Serializable]
    public class GroupShape : Shape
    {
        public List<Shape> SubShapes { get; set; } = new List<Shape>();

        public GroupShape()
        {
        }

        public override bool Contains(PointF point)
        {
            PointF[] pts = new PointF[] { point };

            Matrix inverse = TransformMatrix.Clone();
            inverse.Invert();
            inverse.TransformPoints(pts);

            foreach (var shape in SubShapes)
            {
                if (shape.Contains(pts[0]))
                    return true;
            }

            return false;
        }

        public override void DrawSelf(Graphics grfx)
        {
            GraphicsState state = grfx.Save();

            grfx.MultiplyTransform(this.TransformMatrix);

            foreach (var shape in SubShapes)
            {
                shape.DrawSelf(grfx);
            }

            grfx.Restore(state);
        }

        public override PointF GetCenter()
        {
            if (SubShapes.Count == 0) return base.GetCenter();

            float sumX = 0, sumY = 0;
            foreach (var s in SubShapes)
            {
                PointF c = s.GetCenter();
                sumX += c.X;
                sumY += c.Y;
            }

            return new PointF(sumX / SubShapes.Count, sumY / SubShapes.Count);
        }

        public override void Translate(float dx, float dy)
        {
            foreach (var shape in SubShapes)
                shape.Translate(dx, dy);
        }

        public override void Rotate(float angle)
        {
            PointF center = GetCenter();

            this.TransformMatrix.RotateAt(angle, center, MatrixOrder.Append);

            foreach (var shape in SubShapes)
                shape.Rotate(angle, center);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            PointF center = GetCenter();

            this.TransformMatrix.Translate(-center.X, -center.Y, MatrixOrder.Append);
            this.TransformMatrix.Scale(scaleX, scaleY, MatrixOrder.Append);
            this.TransformMatrix.Translate(center.X, center.Y, MatrixOrder.Append);

            foreach (var shape in SubShapes)
                shape.Scale(scaleX, scaleY, center);
        }

        public override Color BorderColor
        {
            get => base.BorderColor;
            set
            {
                base.BorderColor = value;
                foreach (var shape in SubShapes)
                {
                    shape.BorderColor = value;
                }
            }
        }

        public override Color FillColor
        {
            get => base.FillColor;
            set
            {
                base.FillColor = value;
                foreach (var shape in SubShapes)
                {
                    shape.FillColor = value;
                }
            }
        }

        public override float BorderWidth
        {
            get => base.BorderWidth;
            set
            {
                base.BorderWidth = value;
                foreach (var shape in SubShapes)
                {
                    shape.BorderWidth = value;
                }
            }
        }
    }
}
