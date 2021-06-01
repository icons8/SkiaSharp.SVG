using System;
using System.Drawing;
using SkiaSharp;

namespace Svg
{
    public interface ISvgRenderer : IDisposable
    {
        float DpiY { get; }
        void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit);
        void DrawImageUnscaled(Image image, Point location);
        void DrawPath(Pen pen, SKPath path);
        void FillPath(Brush brush, SKPath path);
        ISvgBoundable GetBoundable();
        Region GetClip();
        ISvgBoundable PopBoundable();
        void RotateTransform(float fAngle, SKMatrixOrder order = SKMatrixOrder.Append);
        void ScaleTransform(float sx, float sy, SKMatrixOrder order = SKMatrixOrder.Append);
        void SetBoundable(ISvgBoundable boundable);
        void SetClip(Region region, CombineMode combineMode = CombineMode.Replace);
        SmoothingMode SmoothingMode { get; set; }
        SKMatrix Transform { get; set; }
        void TranslateTransform(float dx, float dy, SKMatrixOrder order = SKMatrixOrder.Append);
        void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit, float opacity);
    }
}
