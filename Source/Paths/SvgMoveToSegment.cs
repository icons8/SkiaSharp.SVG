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

        public override void AddToPath(GraphicsPath graphicsPath)
        {
            graphicsPath.StartFigure();
        }

        public override string ToString()
        {
            return "M" + Start.ToSvgString();
        }
    }
}
