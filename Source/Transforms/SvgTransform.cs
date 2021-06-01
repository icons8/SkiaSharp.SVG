using System;
using SkiaSharp;

namespace Svg.Transforms
{
    public abstract class SvgTransform : ICloneable
    {
        public abstract SKMatrix SKMatrix { get; }
        public abstract string WriteToString();

        public abstract object Clone();

        #region Equals implementation
        public override bool Equals(object obj)
        {
            var other = obj as SvgTransform;
            if (other == null)
                return false;

            return SKMatrix.Equals(other.Matrix);
        }

        public override int GetHashCode()
        {
            return SKMatrix.GetHashCode();
        }

        public static bool operator ==(SvgTransform lhs, SvgTransform rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgTransform lhs, SvgTransform rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        public override string ToString()
        {
            return WriteToString();
        }
    }
}
