using SkiaSharp;
using System.Globalization;

namespace Svg.Transforms
{
    public sealed class SvgRotate : SvgTransform
    {
        public float Angle { get; set; }

        public float CenterX { get; set; }

        public float CenterY { get; set; }

        public override SKMatrix SKMatrix
        {
            get
            {
                var SKMatrix = new SKMatrix();
                SKMatrix.Translate(CenterX, CenterY);
                SKMatrix.Rotate(Angle);
                SKMatrix.Translate(-CenterX, -CenterY);
                return SKMatrix;
            }
        }

        public override string WriteToString()
        {
            return $"rotate({Angle.ToSvgString()}, {CenterX.ToSvgString()}, {CenterY.ToSvgString()})";
        }

        public SvgRotate(float angle)
        {
            Angle = angle;
        }

        public SvgRotate(float angle, float centerX, float centerY)
            : this(angle)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        public override object Clone()
        {
            return new SvgRotate(Angle, CenterX, CenterY);
        }
    }
}
