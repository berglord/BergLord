using AForge.Imaging;
using AForge.Imaging.Filters;
using BotProject.Properties;
using IronOcr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;

namespace BotProject
{
    class ImageProcessing
    {
        public static ImageProcessing Instance;
        public Bitmap CapturedBitmap;
        public Bitmap DebugBitmap;
        private TesseractEngine _ocrEngine;
        private Rect _ocrRect = new Rect(640, 300, 160, 70);
        private Bitmap _neutralHp;
        private Bitmap _neutralHpGrayScale;
        private Grayscale _grayScale;
        private ExhaustiveTemplateMatching tm;

        private Bitmap _bmp;
        private Bitmap _debugBmp;
        public ImageProcessing()
        {
            Instance = this;
            _bmp = new Bitmap(WoWProcess.Instance.Width, WoWProcess.Instance.Height, PixelFormat.Format32bppArgb);
            tm = new ExhaustiveTemplateMatching(0.80f);
            _debugBmp = new Bitmap(_bmp);

            _neutralHp = new Bitmap(Resources.NeutralTest);
            _grayScale = new GrayscaleBT709();
            _neutralHpGrayScale = _grayScale.Apply(_neutralHp);
        }

        public void CaptureImage()
        {
            WoWProcess.Instance.GetProcessRect();
            Graphics graphics = Graphics.FromImage(_bmp);
            graphics.CopyFromScreen(WoWProcess.ProcessRectangle.left, WoWProcess.ProcessRectangle.top, 0, 0, new Size(WoWProcess.Instance.Width, WoWProcess.Instance.Height), CopyPixelOperation.SourceCopy);

            CapturedBitmap = _bmp;
            DebugBitmap = (Bitmap)_bmp.Clone();


            // Debug BitMap
            using (Graphics gr = Graphics.FromImage(DebugBitmap))
            {
                var pen = new Pen(Color.Green);
                gr.DrawRectangle(pen, new Rectangle(_ocrRect.X1, _ocrRect.Y1, _ocrRect.Width, _ocrRect.Height));
                var font = new Font(FontFamily.GenericMonospace, 12f, FontStyle.Bold);
                gr.DrawString("Co Ordinate \n Box", font, new SolidBrush(Color.Black), _ocrRect.X1, _ocrRect.Y1);
            }
            ProcessPlayerPosition();

            Bitmap buffer;
            buffer = new Bitmap(CapturedBitmap);
            Bitmap buffer2;
            buffer2 = new Bitmap(_neutralHp);

            //start an async task

            if (_namePlateCheckCount < 1)
            {
                _namePlateCheckCount++;
            }
            if (_namePlateCheckCount >= 1)
            {
                Task.Factory.StartNew(() =>
                {
                    var namePlates = FindNamePlates(buffer, buffer2);
                //invoke an action against the main thread to draw the buffer to the background image of the main form.
                DisplayForm.Instance.Invoke(new Action(() =>
                    {
                        if (namePlates != null) DisplayForm.Instance.NamePlates = namePlates;
                        _namePlateCheckCount++;
                    }));
                });
            }
            DisplayForm.Instance.SetRawImage(CapturedBitmap);
            DisplayForm.Instance.SetDebugImage(DebugBitmap);
            Thread.Sleep(50);
        }

        public void ProcessPlayerPosition()
        {
            if (_ocrEngine == null) _ocrEngine = new Tesseract.TesseractEngine("./TessData", "eng", Tesseract.EngineMode.Default);
            Page page = _ocrEngine.Process(_bmp, _ocrRect, Tesseract.PageSegMode.Auto);
            var trimmed = page.GetText().Trim();
            page.Dispose();
            string replaceWith = ", ";
            string replaceWhiteSpace = "";
            string removedBreaks = trimmed.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith).Replace(" ", replaceWhiteSpace).Replace(",", " ");
            var result = removedBreaks.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Convert.ToDouble).ToList();
            //  Console.WriteLine(result[0]);
            //  Console.WriteLine(result[1]);
            // Console.WriteLine(result[2]);
        }

        private int _namePlateCheckCount = 0;
        public TemplateMatch[] FindNamePlates(Bitmap image, Bitmap image2)
        {
            
            // find all matchings with specified above similarity
            Bitmap sourceImage = new Bitmap(image);
            Bitmap graySource = _grayScale.Apply(sourceImage);
            Bitmap graySource2 = _grayScale.Apply(image2);
            TemplateMatch[] matchings = tm.ProcessImage(graySource, graySource2);
            _namePlateCheckCount = 0;

            return matchings;
            //Graphics g = Graphics.FromImage(DebugBitmap);
            //Console.WriteLine(matchings.Length);
            //var rect = new Rect(685, 275, 110, 65);
            //for (int i = 0; i < matchings.Length / 10; i++)
            //{
            //    if (matchings[i].Similarity > 0.90f)
            //    {
            //        if (matchings[i].Rectangle.Size.Width < 25) continue;
            //        int X = matchings[i].Rectangle.X;
            //        int Y = matchings[i].Rectangle.Y;

            //        g.DrawRectangle(new Pen(Color.Red, 3), X, Y, matchings[i].Rectangle.Width, matchings[i].Rectangle.Height);

            //       // g.DrawRectangle(new Pen(Color.Red, 3), rect.X1, rect.Y1, rect.Width, rect.Height);
            //        DisplayForm.Instance.SetDebugImage(DebugBitmap);

            //    }
            //}
        }

        public Color GetColorAt(int x, int y)
        {
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(CapturedBitmap))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return CapturedBitmap.GetPixel(0, 0);
        }

        public void OnUpdate()
        {
            while (DisplayForm.Instance.IsRunning)
            {
                CaptureImage();
                Thread.Sleep(20);
            }
        }


    }
}
