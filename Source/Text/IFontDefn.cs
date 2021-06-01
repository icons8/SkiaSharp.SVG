using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SkiaSharp;

namespace Svg
{
    public interface IFontDefn : IDisposable
    {
        float Size { get; }
        float SizeInPoints { get; }
        void AddStringToPath(ISvgRenderer renderer, SKPath path, string text, PointF location);
        float Ascent(ISvgRenderer renderer);
        IList<RectangleF> MeasureCharacters(ISvgRenderer renderer, string text);
        SizeF MeasureString(ISvgRenderer renderer, string text);
    }
}
