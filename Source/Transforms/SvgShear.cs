using SkiaSharp;
using System.Globalization;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified shear vector to this SKMatrix.
    /// </summary>
    public sealed class SvgShear : SvgTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override SKMatrix SKMatrix
        {
            get
            {
                var SKMatrix = new SKMatrix();
                SKMatrix.Shear(X, Y);
                return SKMatrix;
            }
        }

        public override string WriteToString()
        {
            return $"shear({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgShear(float x)
            : this(x, x)
        {
        }

        public SvgShear(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override object Clone()
        {
            return new SvgShear(X, Y);
        }
    }
}
