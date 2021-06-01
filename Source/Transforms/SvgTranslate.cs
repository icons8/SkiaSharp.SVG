using SkiaSharp;
using System.Globalization;

namespace Svg.Transforms
{
    public sealed class SvgTranslate : SvgTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override SKMatrix SKMatrix
        {
            get
            {
                var SKMatrix = new SKMatrix();
                SKMatrix.Translate(X, Y);
                return SKMatrix;
            }
        }

        public override string WriteToString()
        {
            return $"translate({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgTranslate(float x, float y)
        {
            X = x;
            Y = y;
        }

        public SvgTranslate(float x)
            : this(x, 0f)
        {
        }

        public override object Clone()
        {
            return new SvgTranslate(X, Y);
        }
    }
}
