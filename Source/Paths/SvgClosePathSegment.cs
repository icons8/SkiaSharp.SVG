using SkiaSharp;

namespace Svg.Pathing
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public override void AddToPath(SKPath SKPath)
        {
            var pathData = SKPath.PathData;

            if (pathData.Points.Length > 0)
            {
                // Important for custom line caps. Force the path the close with an explicit line, not just an implicit close of the figure.
                var last = pathData.Points.Length - 1;
                if (!pathData.Points[0].Equals(pathData.Points[last]))
                {
                    var i = last;
                    while (i > 0 && pathData.Types[i] > 0) --i;
                    SKPath.AddLine(pathData.Points[last], pathData.Points[i]);
                }

                SKPath.CloseFigure();
            }
        }

        public override string ToString()
        {
            return "z";
        }
    }
}
