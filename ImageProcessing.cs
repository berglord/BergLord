using AForge.Imaging;
using AForge.Imaging.Filters;
using BotProject.Properties;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
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
                    //  var namePlates = FindNamePlates(buffer, buffer2);
                    var namePlatePoint = FindAllImages(buffer, buffer2);
                    //invoke an action against the main thread to draw the buffer to the background image of the main form.
                    DisplayForm.Instance.Invoke(new Action(() =>
                        {
                            DisplayForm.Instance.NamePlatePoints = namePlatePoint;
                            // if (namePlates != null) DisplayForm.Instance.NamePlates = namePlates;
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

            Character.Instance.PlayerDirection = result[2];
            Character.Instance.PlayerPosition = new System.Numerics.Vector2((float)result[0], (float)result[1]);

          //  Console.WriteLine(Character.Instance.PlayerDirection);
          //  Console.WriteLine(Character.Instance.PlayerPosition);
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

        public List<Point> FindAllImages(Bitmap imageBase, Bitmap imageToMatch)
        {
            Image<Bgr, byte> source = new Image<Bgr, byte>(imageBase);
            Image<Bgr, byte> findImage = new Image<Bgr, byte>(imageToMatch);

            var list = new List<Point>();

            // It looks like you just need filenames here...
            // Simple parallel foreach suggested by HouseCat (in 2.):

            Image<Gray, float> result = source.MatchTemplate(findImage,
               Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            // By using C# 7.0, we can do inline out declarations here:
            //result.MinMax(
            //        out double[] minValues,
            //        out double[] maxValues,
            //        out Point[] minLocations,
            //        out Point[] maxLocations);

            //if(maxValues[0] > 0.80f)
            //{
            //    list.Add(maxLocations[0]);
            //    return list;
            //}
            for (int x = 0; x < result.Data.GetLength(1); x++)
            {
                for (int y = 0; y < result.Data.GetLength(0); y++)
                {
                    bool canAdd = true;

                    double score = result.Data[y, x, 0];
                    if (score > 0.70)
                    {
                        var point = new Point(x, y);
                        foreach (var p in list)
                        {
                            if (GetDistance(p, point) < 100) canAdd = false;
                        }
                         if (canAdd) list.Add(point);
                    }
                }
            }
            return list;
        }
        private static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }

        public List<Rectangle> Test2(Bitmap imageBase, Bitmap imageToMatch)
        {
            var list = new List<Rectangle>();
            Image<Bgr, byte> source = new Image<Bgr, byte>(imageBase);
            Image<Bgr, byte> findImage = new Image<Bgr, byte>(imageToMatch);
            using (Image<Bgr, byte> imgSrc = source)
            {
                //SubImage= template img, imgsrc = source image
                using (Image<Gray, float> result = imgSrc.MatchTemplate(findImage, TemplateMatchingType.SqdiffNormed))
                {

                    double[] minValues, maxValues;
                    Point[] minLocations, maxLocations;
                    result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                    if (maxValues[0] > 0.80f)
                    {
                        Rectangle match = new Rectangle(maxLocations[0], imageToMatch.Size);
                        //imgSrc.Draw(match, new Bgr(Color.Blue), -1);
                        list.Add(match); // adding to rectangle list 
                    }
                }
            }
            Console.Write("List Count - " + list.Count);
            return list;
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
