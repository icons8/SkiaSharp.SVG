using System.Drawing;
using SkiaSharp;

namespace Svg.Pathing
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(PointF moveTo)
            : base(moveTo, moveTo)
        {
        }

        public override void AddToPath(SKPath SKPath)
        {
            SKPath.StartFigure();
        }

        public override string ToString()
        {
            return "M" + Start.ToSvgString();
        }
    }
}
