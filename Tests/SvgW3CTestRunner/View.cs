﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Svg;
using System.Diagnostics;


namespace SvgW3CTestRunner
{
    public partial class View : Form
    {
        //DIRECTORY SEPARATOR: The value of this field is a slash ("/") on UNIX and on Mac OSX, and a backslash ("\") on the Windows operating systems.
        static private string sprt = Path.DirectorySeparatorChar.ToString();

        //Data folders
        private string _svgBasePath = @".." + sprt + ".." + sprt + ".." + sprt + ".." + sprt + "W3CTestSuite" + sprt + "svg" + sprt;
        private string _pngBasePath = @".." + sprt + ".." + sprt + ".." + sprt + ".." + sprt + "W3CTestSuite" + sprt + "png" + sprt;

        public View()
        {
            InitializeComponent();
            // ignore tests pertaining to javascript or xml reading
            var passingtestsTxt = _svgBasePath + @".." + sprt + "PassingTests.txt";
            var passes = File.ReadAllLines(passingtestsTxt).ToDictionary((f) => f, (f) => true);
            var files = (from f in
                         (from g in Directory.GetFiles(_svgBasePath)
                          select Path.GetFileName(g))
                         where !f.StartsWith("animate-") && !f.StartsWith("conform-viewer") &&
                         !f.Contains("-dom-") && !f.StartsWith("linking-") && !f.StartsWith("interact-") &&
                         !f.StartsWith("script-") && f.EndsWith(".svg")
                         && File.Exists(_pngBasePath + f.Substring(0, f.Length - 3) + "png")
                         orderby f
                         select (object)f);

            var other = files.Where(f => ((string)f).StartsWith("__"));
            lstFilesOtherPassing.Items.AddRange(other.Where(f => passes.ContainsKey((string)f)).ToArray());
            lstFilesOtherFailing.Items.AddRange(other.Where(f => !passes.ContainsKey((string)f)).ToArray());
            files = files.Where(f => !((string)f).StartsWith("__"));
            lstW3CFilesPassing.Items.AddRange(files.Where(f => passes.ContainsKey((string)f)).ToArray());
            lstW3CFilesFailing.Items.AddRange(files.Where(f => !passes.ContainsKey((string)f)).ToArray());
        }



        private void boxConsoleLog_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {   //click event

                var contextMenu = new System.Windows.Forms.ContextMenuStrip();
                var menuItem = new ToolStripMenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.Items.Add(menuItem);

                boxConsoleLog.ContextMenuStrip = contextMenu;
            }
        }


        void CopyAction(object sender, EventArgs e)
        {
            if (boxConsoleLog.SelectedText != null && boxConsoleLog.SelectedText != "")
            {
                //Clipboard.SetText(boxConsoleLog.SelectedText.Replace("\n", "\r\n"));

                boxConsoleLog.Copy();
            }
        }


        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //render svg
            var lstFiles = sender as ListBox;
            var fileName = lstFiles.SelectedItem.ToString();
            if (fileName.StartsWith("#")) return;

            //display png
            var png = Image.FromFile(_pngBasePath + Path.GetFileNameWithoutExtension(fileName) + ".png");
            picPng.Image = png;

            var doc = new SvgDocument();
            try
            {
                Debug.Print(fileName);
                doc = SvgDocument.Open(_svgBasePath + fileName);
                if (fileName.StartsWith("__"))
                {
                    picSvg.Image = doc.Draw();
                }
                else
                {
                    var img = new SKBitmap(480, 360);
                    doc.Draw(img);
                    picSvg.Image = img;
                }

                this.boxConsoleLog.AppendText("\n\nWC3 TEST " + fileName + "\n");
                this.boxDescription.Text = GetDescription(doc);

            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG RENDERING ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSvg.Image = null;
            }

            //save load
            try
            {
                using (var memStream = new MemoryStream())
                {
                    doc.Write(memStream);
                    memStream.Position = 0;
                    var baseUri = doc.BaseUri;
                    doc = SvgDocument.Open<SvgDocument>(memStream);
                    doc.BaseUri = baseUri;

                    if (fileName.StartsWith("__"))
                    {
                        picSaveLoad.Image = doc.Draw();
                    }
                    else
                    {
                        var img = new SKBitmap(480, 360);
                        doc.Draw(img);
                        picSaveLoad.Image = img;
                    }
                }
            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG SERIALIZATION ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSaveLoad.Image = null;
            }

            //compare svg to png
            try
            {
                picSVGPNG.Image = PixelDiff((SKBitmap)picPng.Image, (SKBitmap)picSvg.Image);
            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG TO PNG COMPARISON ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSVGPNG.Image = null;
            }
        }

        private void fileTabBox_TabIndexChanged(object sender, EventArgs e)
        {
            picSvg.Image = null;
            picPng.Image = null;
            picSaveLoad.Image = null;
            picSVGPNG.Image = null;
        }

        private SvgElement GetChildWithDescription(SvgElement element, string description)
        {
            var docElements = element.Children.Where(child => child is NonSvgElement && (child as NonSvgElement).Name == description);
            return docElements.Count() > 0 ? docElements.First() : null;
        }

        private string GetDescription(SvgDocument document)
        {
            string description = string.Empty;
            var testCaseElement = GetChildWithDescription(document, "SVGTestCase");
            if (testCaseElement != null)
            {
                var descriptionElement = GetChildWithDescription(testCaseElement, "testDescription");
                if (descriptionElement != null)
                {
                    var regex = new Regex("\r\n *");
                    var descriptionLines = new List<string>();
                    foreach (var child in descriptionElement.Children)
                    {
                        if (child.Content != null)
                            descriptionLines.Add(regex.Replace(child.Content, " "));
                    }
                    return string.Join("\n", descriptionLines.ToArray());
                }
            }
            return description;
        }

        unsafe SKBitmap PixelDiff(SKBitmap a, SKBitmap b)
        {
            SKBitmap output = new SKBitmap(a.Width, a.Height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(Point.Empty, a.Size);
            using (var aData = a.LockBitsDisposable(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
            using (var bData = b.LockBitsDisposable(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
            using (var outputData = output.LockBitsDisposable(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb))
            {
                byte* aPtr = (byte*)aData.Scan0;
                byte* bPtr = (byte*)bData.Scan0;
                byte* outputPtr = (byte*)outputData.Scan0;
                int len = aData.Stride * aData.Height;
                for (int i = 0; i < len; i++)
                {
                    // For alpha use the average of both images (otherwise pixels with the same alpha won't be visible)
                    if ((i + 1) % 4 == 0)
                        *outputPtr = (byte)((*aPtr + *bPtr) / 2);
                    else
                        *outputPtr = (byte)~(*aPtr ^ *bPtr);

                    outputPtr++;
                    aPtr++;
                    bPtr++;
                }
            }
            return output;
        }
    }

    static class SKBitmapExtensions
    {
        public static DisposableImageData LockBitsDisposable(this SKBitmap SKBitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
        {
            return new DisposableImageData(SKBitmap, rect, flags, format);
        }

        public class DisposableImageData : IDisposable
        {
            private readonly SKBitmap _SKBitmap;
            private readonly SKBitmapData _data;

            internal DisposableImageData(SKBitmap SKBitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
            {
                _SKBitmap = SKBitmap;
                _data = SKBitmap.LockBits(rect, flags, format);
            }

            public void Dispose()
            {
                _SKBitmap.UnlockBits(_data);
            }

            public IntPtr Scan0
            {
                get { return _data.Scan0; }
            }

            public int Stride
            {
                get { return _data.Stride; }
            }

            public int Width
            {
                get { return _data.Width; }
            }

            public int Height
            {
                get { return _data.Height; }
            }

            public PixelFormat PixelFormat
            {
                get { return _data.PixelFormat; }
            }

            public int Reserved
            {
                get { return _data.Reserved; }
            }
        }
    }
}
