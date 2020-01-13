using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging;
using BotProject.Properties;
using IronOcr;
using Tesseract;

namespace BotProject
{
    public partial class DisplayForm : Form
    {
        public static DisplayForm Instance;
        public bool IsRunning;
        public Bitmap OriginalBitMap;
        public Bitmap DebugBitMap;
        public TemplateMatch[] NamePlates = new TemplateMatch[0];
        public List<Point> NamePlatePoints = new List<Point>();
        private Bitmap _neutralHp;

        private ImageProcessing _imageProcessor;
        private WoWProcess _wowProcess;
        private Character _character;
        private Navigation _navigation;


        public DisplayForm()
        {
            Instance = this;
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            _neutralHp = new Bitmap(Resources.NeutralTest);
        }


        private void DisplayFormLoad(object sender, EventArgs e)
        {
            
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsRunning = false;
        }


        private void CaptureButtonClick(object sender, EventArgs e)
        {
            IsRunning = true;
           
            _wowProcess = new WoWProcess();

            // Start Capture

            _imageProcessor = new ImageProcessing();
            _character = new Character();
            _navigation = new Navigation();
            //Program.CaptureThread = new Thread(_imageProcessor.OnUpdate);
           // Program.CaptureThread.Start();
            UpdateLoop();

            //while (IsRunning)
            //{
            //    // convert image to grayscale
            //    Bitmap templateImage = new Bitmap(_neutralHp);
            //    Grayscale grayScale = new GrayscaleBT709();
            //    Bitmap grayTemplate = grayScale.Apply(templateImage);
            //    // create template matching algorithm's instance
            //    ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);

            //    // find all matchings with specified above similarity
            //    Bitmap sourceImage = new Bitmap(_originalBitMap);
            //    Bitmap graySource = grayScale.Apply(sourceImage);
            //    TemplateMatch[] matchings = tm.ProcessImage(graySource, grayTemplate);

            //    Graphics g = Graphics.FromImage(sourceImage);
            //    var rect = new Rect(685, 275, 110, 65);
            //    for (int i = 0; i < matchings.Length; i++)
            //    {
            //        if (matchings[i].Similarity > 0.90f)
            //        {
            //            if (matchings[i].Rectangle.Size.Width < 25) continue;
            //            int X = matchings[i].Rectangle.X;
            //            int Y = matchings[i].Rectangle.Y;

            //            g.DrawRectangle(new Pen(Color.Red, 3), X, Y, matchings[i].Rectangle.Width, matchings[i].Rectangle.Height);
            //            DebugImage.Image = sourceImage;

            //            g.DrawRectangle(new Pen(Color.Red, 3), rect.X1, rect.Y1, rect.Width,rect.Height);
            //        }

            //    }

            //    TesseractEngine engine = new Tesseract.TesseractEngine("./TessData", "eng", Tesseract.EngineMode.TesseractOnly);
            //    Page page =engine.Process(_originalBitMap,rect, Tesseract.PageSegMode.Auto);

            //    wait(200);
            //}
        }

        public void wait(int milliseconds) // It will wait number of miliseconds. 
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds <= milliseconds)
            {
                Application.DoEvents();
            }
        }


         async void UpdateLoop()
        {
            while (IsRunning)
            {
                try
                {
                    _imageProcessor.CaptureImage();
                    Graphics g = Graphics.FromImage(DebugBitMap);
                    var count = 0;
                    if(NamePlates != null) {
                        foreach (var match in NamePlates)
                        {
                            if (match.Similarity > 0.87)
                            {
                                count++;
                                // if (match.Rectangle.Size.Width < 25) continue;
                                int X = match.Rectangle.X;
                                int Y = match.Rectangle.Y;

                                g.DrawRectangle(new Pen(Color.Red, 3), X, Y, match.Rectangle.Width, match.Rectangle.Height);
                            }
                        }
                    }
                    foreach(var point in NamePlatePoints)
                    {
                        g.DrawRectangle(new Pen(Color.Red, 3), point.X, point.Y, 60, 10);
                    }
                    var middle = new Vector2((WoWProcess.Instance.Width / 2), (WoWProcess.Instance.Height / 2) + 50);
                    g.DrawLine(new Pen(Color.Cyan, 3), middle.X, middle.Y, middle.X + (Navigation.Instance.TargetClickVector.X * 75), middle.Y - (Navigation.Instance.TargetClickVector.Y * 75));
                    // g.DrawRectangle(new Pen(Color.Red, 3), middle.X + (Navigation.Instance.TargetClickVector.X * 250), middle.Y - (Navigation.Instance.TargetClickVector.Y * 100), 20, 20 );
                   // SolidBrush myBrush = new SolidBrush(Color.Cyan);
                   // g.FillRectangle(myBrush, middle.X, middle.Y , 5, 5);

                    var delta = Navigation.Instance.CaclulateDelta();
                   
                    SetDebugImage(DebugBitMap);
                    Console.WriteLine(Character.Instance.PlayerDirection + " Char Pos - : "+ Character.Instance.PlayerPosition);
                    await Task.Delay(50);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        } 

        public async void Wait(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }
        
        public void SetRawImage(Bitmap image)
        {
                RawImage.Image = image;
                OriginalBitMap = image;
        }

        public void SetDebugImage(Bitmap image)
        {
                DebugImage.Image = image;
                DebugBitMap = image;
        }

    }
}
