using System;
using System.Drawing;
using System.Linq;
using LedGameDisplayLibrary;
using LedGameDisplayLibrary.Effects;

namespace LedGameDisplayTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Console.WriteLine("Enter the number of pixels on the horizontal axis (width) [60]");
            //string width = Console.ReadLine();
            //int x = string.IsNullOrWhiteSpace(width) ? 60 : Convert.ToInt32(width);

            //Console.WriteLine("Enter the number of pixels on the vertical axis (height) [10]");
            //string height = Console.ReadLine();
            //int y = string.IsNullOrWhiteSpace(height) ? 10 : Convert.ToInt32(height);

            //Display.X = x;
            //Display.Y = y;

            var input = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("What do you want to test:");
                Console.WriteLine("Press CTRL + C to abort to current test.");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Color wipe animation");
                Console.WriteLine("2 - Rainbow color animation");
                Console.WriteLine("3 - fading left to right animation");
                Console.WriteLine("4 - Calibration Tasks");
                Console.WriteLine("5 - Chartest");
                Console.WriteLine("6 - Areatest");

                Console.Write("What is your choice: ");
                input = int.Parse(Console.ReadLine());

                Display.Initialize("96x60");

                if (input != 4)
                {
                    var effect = GetEffect(input);
                    if (effect != null)
                    {
                        effect.Execute();
                    }
                }
                else if (input == 4)
                {
                    Display.Calibrate();
                }

            } while (input != 0);
        }

        private static IEffect GetEffect(int input)
        {
            IEffect result = null;

            switch(input)
            {
                case 1:
                    result = new TestPixelWipe();
                    Console.WriteLine("Select the Color (RGB Format)");
                    var hex = Console.ReadLine();
                    ((TestPixelWipe)result).Color = Color.FromArgb(Convert.ToByte(hex.Substring(0, 2), 16), Convert.ToByte(hex.Substring(2, 2), 16), Convert.ToByte(hex.Substring(4, 2), 16));
                    break;
                case 2:
                    Console.WriteLine("Not implemented");
                    Console.ReadLine();
                    break;
                case 3:
                    result = new IdleBar();
                    break;
                case 5:
                    result = new TestChar();
                    break;
                case 6:
                    result = new TestArea();
                    break;
            }

            return result;
        }
    }
}
