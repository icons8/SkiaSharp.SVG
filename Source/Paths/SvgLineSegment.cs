using System.Drawing;
using SkiaSharp;

namespace Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(PointF start, PointF end)
            : base(start, end)
        {
        }

        public override void AddToPath(SKPath SKPath)
        {
            SKPath.AddLine(Start, End);
        }

        public override string ToString()
        {
            return "L" + End.ToSvgString();
        }
    }
}
