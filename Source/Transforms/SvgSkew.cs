﻿using System;
using SkiaSharp;
using System.Globalization;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified skew vector to this Matrix.
    /// </summary>
    public sealed class SvgSkew : SvgTransform
    {
        public float AngleX { get; set; }

        public float AngleY { get; set; }

        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Shear((float)Math.Tan(AngleX / 180f * Math.PI), (float)Math.Tan(AngleY / 180f * Math.PI));
                return matrix;
            }
        }

        public override string WriteToString()
        {
            if (AngleY == 0f)
                return $"skewX({AngleX.ToSvgString()})";
            return $"skewY({AngleY.ToSvgString()})";
        }

        public SvgSkew(float x, float y)
        {
            AngleX = x;
            AngleY = y;
        }

        public override object Clone()
        {
            return new SvgSkew(AngleX, AngleY);
        }
    }
}
