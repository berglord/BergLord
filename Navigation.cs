using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BotProject
{
    class Navigation
    {
        public Vector2 TestVector = new Vector2(43.97f, 69.57f);

        public static Navigation Instance;
        private Vector2 _up = new Vector2(0, 1);

        public Vector2 Forward;
        public Vector2 Heading;
        public double HeadingAngle;
        public double TargetAngle;
        public Vector2 TargetClickVector;

        public Navigation()
        {
            Instance = this;
        }

        public double CaclulateDelta()
        {
            Heading = Vector2.Normalize(TestVector - Character.Instance.PlayerPosition);
            Heading.Y = Heading.Y * -1;

            Forward = DegreeToVector2((float)Character.Instance.PlayerDirection);
            Forward = Vector2.Normalize(Forward);

            HeadingAngle = GetDegrees(Heading.X, Heading.Y);
            TargetAngle = GetTargetDegrees(Heading.X, Heading.Y, Forward);
            TargetClickVector = DegreeToVector2((float)TargetAngle);
            
            Console.WriteLine($"HeadingAngle - {HeadingAngle} |  Heading - {Heading} | Foward - {Forward} | TargetAngle - {TargetAngle} \n" +
                $" | Click Vector {TargetClickVector.ToString()}");

            return HeadingAngle;                   
        }

        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2((float)Math.Sin(radian),(float)Math.Cos(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2((float)degree * (float)(Math.PI / 180));
        }

        static double GetDegrees(double tx, double ty)
        {
            double ox = 0.0;
            double oy = 0.0;
            var vector2 = new NavPoint(tx - ox, ty - oy);
            var vector1 = new NavPoint(0.0, 1.0); // 12 o'clock == 0°, assuming that y goes from bottom to top

            double angleInRadians = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);
            var result = (360 - (angleInRadians * (180 / Math.PI)));
            if (result > 359) result = result - 360;
            var r = result;
            r =Character.Instance.PlayerDirection - result;
           //Console.WriteLine("Rotation to Match : " + r);
            return result;
        }

        static double GetTargetDegrees(float tx, float ty, Vector2 forward)
        {
            float ox = 0.0f;
            float oy = 0.0f;
            var vector2 = new Vector2(tx - ox, ty - oy);
            //var vector1 = new NavPoint(0.0, 1.0); // 12 o'clock == 0°, assuming that y goes from bottom to top

            double angleInRadians = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(forward.Y, forward.X);
            var result = (360 - (angleInRadians * (180 / Math.PI)));
            if (result > 359) result = result - 360;

            return result;
        }
    }

    public struct NavPoint
    {
        public double X;
        public double Y;

        public NavPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}

