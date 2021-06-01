using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Svg.FilterEffects
{
    internal sealed class RawSKBitmap : IDisposable
    {
        private SKBitmap _originSKBitmap;
        private SKBitmapData _SKBitmapData;
        private IntPtr _ptr;
        private int _bytes;
        private byte[] _argbValues;

        public RawSKBitmap(SKBitmap originSKBitmap)
        {
            _originSKBitmap = originSKBitmap;
            _SKBitmapData = _originSKBitmap.LockBits(new Rectangle(0, 0, _originSKBitmap.Width, _originSKBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _ptr = _SKBitmapData.Scan0;
            _bytes = this.Stride * _originSKBitmap.Height;
            _argbValues = new byte[_bytes];
            Marshal.Copy(_ptr, _argbValues, 0, _bytes);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _originSKBitmap.UnlockBits(_SKBitmapData);
        }

        #endregion

        public int Stride
        {
            get { return _SKBitmapData.Stride; }
        }

        public int Width
        {
            get { return _SKBitmapData.Width; }
        }

        public int Height
        {
            get { return _SKBitmapData.Height; }
        }

        public byte[] ArgbValues
        {
            get { return _argbValues; }
            set
            {
                _argbValues = value;
            }
        }

        public SKBitmap SKBitmap
        {
            get
            {
                Marshal.Copy(_argbValues, 0, _ptr, _bytes);
                return _originSKBitmap;
            }
        }

    }
}