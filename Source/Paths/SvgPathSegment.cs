using System.Drawing;
using SkiaSharp;

namespace Svg.Pathing
{
    public abstract class SvgPathSegment
    {
        public PointF Start { get; set; }
        public PointF End { get; set; }

        protected SvgPathSegment()
        {
        }

        protected SvgPathSegment(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public abstract void AddToPath(SKPath SKPath);

        public SvgPathSegment Clone()
        {
            return MemberwiseClone() as SvgPathSegment;
        }
    }
}
