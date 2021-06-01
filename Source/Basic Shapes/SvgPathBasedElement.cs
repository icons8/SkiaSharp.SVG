using System.Drawing;
using SkiaSharp;

namespace Svg
{
    /// <summary>
    /// Represents an element that is using a SKPath as rendering base.
    /// </summary>
    public abstract partial class SvgPathBasedElement : SvgVisualElement
    {
        public override RectangleF Bounds
        {
            get
            {
                var path = Path(null);
                if (path == null)
                    return new RectangleF();
                if (Transforms == null || Transforms.Count == 0)
                    return path.GetBounds();

                using (path = (SKPath)path.Clone())
                using (var SKMatrix = Transforms.GetMatrix())
                {
                    path.Transform(matrix);
                    return path.GetBounds();
                }
            }
        }
    }
}
