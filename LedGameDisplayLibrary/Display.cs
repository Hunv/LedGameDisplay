using LedGameDisplayLibrary.Effects;
using Newtonsoft.Json;
using rpi_ws281x;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace LedGameDisplayLibrary
{
    public static class Display
    {
        /// <summary>
        /// Returns the width of the LED Panel based on Layout Configuration
        /// </summary>
        public static int X { get { return LayoutConfig.Width; }  }

        /// <summary>
        /// Returns the height of the LED Panel based on Layout Configuration
        /// </summary>
        public static int Y { get { return LayoutConfig.Height; } }

        /// <summary>
        /// Is the first line data line feed on the left and the second line on the right and so on?
        /// </summary>
        public static bool HasAlternatingRows { get; set; } = true;

        /// <summary>
        /// Is the data feed line at the bottom of the panel?
        /// </summary>
        public static bool IsBottomToTop { get; set; } = true;

        /// <summary>
        /// Returns the amount of LEDs of the panel
        /// </summary>
        public static int LedCount { get { return X * Y; } }

        /// <summary>
        /// Contains the Frames per Second after running Calibrate()
        /// </summary>
        public static double FPS { get; private set; }

        /// <summary>
        /// The CharacterSet to use
        /// </summary>
        public static string CharacterSet { get; set; } = "Default";

        /// <summary>
        /// The Layout loaded from config file
        /// </summary>
        public static Layout LayoutConfig { get; set; }

        /// <summary>
        /// All available Characters
        /// </summary>
        public static List<CharacterSet> CharacterSets { get; set; }

        /// <summary>
        /// The Hardware Instance of the WS281X Library
        /// </summary>
        public static WS281x WS281X { get; set; }

        /// <summary>
        /// The Settings for the WS281X Controller
        /// </summary>
        private static Settings ControllerSettings {get;set;}

        /// <summary>
        /// The Controller for the WS281X Library
        /// </summary>
        private static Controller Controller { get; set; }

        public static void Calibrate()
        {
            if (LedCount == 0)
            {
                Console.WriteLine("Please set X and Y first");
                return;
            }

            //if all pixel changing
            TestColorWipe cfrw = new TestColorWipe();

            Console.WriteLine("Starting all pixel change benchmark...");

            var start = DateTime.Now;
            cfrw.Execute(); //Performing 256 changes
            var end = DateTime.Now;

            var diff2 = end.Subtract(start);
            FPS = 256 / diff2.TotalSeconds;

            Console.WriteLine("full pixel change FPS: {0} FPS", 256 / diff2.TotalSeconds);
            Console.WriteLine("Press enter to continue...");
            Console.Read();
        }

        public static void Initialize(string layoutName)
        {
            LoadLayout(layoutName);
            LoadCharacters();
         
            Console.WriteLine("Initializing Controller for {0} with {1} LEDs on PWM0, DMA Channel {2} and a Frequency of {3}Hz", layoutName, LedCount, LayoutConfig.DmaChannel, LayoutConfig.Frequency);
            
            //ControllerSettings = Settings.CreateDefaultSettings(); //800kHz and DMA Channel 10
            ControllerSettings = new Settings(LayoutConfig.Frequency, LayoutConfig.DmaChannel); //Using DMA Channel 10 limits the number of LEDs to 400 on a Raspberry Pi 3b. Don't know why
            Controller = ControllerSettings.AddController(ControllerType.PWM0, LedCount, StripType.WS2812_STRIP, LayoutConfig.Brightness, false);
            WS281X = new WS281x(ControllerSettings);

            SetAll(Color.Black);
        }

        public static void LoadLayout(string layoutName)
        {
            var layoutFilePath = string.Format("Config/Layouts/{0}.layout", layoutName);

            if (!File.Exists(layoutFilePath))
            {
                Console.WriteLine("Layout file not exists. Please create file {0}", layoutFilePath);
                return;
            }

            StreamReader sR = new StreamReader(layoutFilePath, Encoding.Default);
            var layout = sR.ReadToEnd();
            sR.Close();
                       
            LayoutConfig = JsonConvert.DeserializeObject<Layout>(layout);
        }

        public static void LoadCharacters()
        {
            var characterPath = string.Format("Config/Characters/{0}", LayoutConfig.CharacterSet);
            if (!Directory.Exists(characterPath))
            {
                Console.WriteLine("CharacterSet not found");
                return;
            }

            CharacterSets = new List<CharacterSet>();

            //Get each resolution:
            foreach (var aResolutionFolder in Directory.GetDirectories(characterPath))
            {
                var charSet = new CharacterSet();
                var directoryName = aResolutionFolder.Substring(aResolutionFolder.Replace('\\', '/').LastIndexOf('/') + 1);
                charSet.Width = Convert.ToInt32(directoryName.Split('x')[0]);
                charSet.Height = Convert.ToInt32(directoryName.Split('x')[1]);
                charSet.Name = LayoutConfig.CharacterSet;
                charSet.Characters = new List<Character>();

                foreach (var charDef in LayoutConfig.CharacterList)
                {
                    StreamReader sR = new StreamReader(aResolutionFolder + "/" + charDef.File, Encoding.Default);
                    var charFile = sR.ReadToEnd().Replace("\r","").Split('\n');
                    sR.Close();

                    var charDefinition = new Character(charFile[0].Length, charFile.Length);
                    charDefinition.Name = charDef.Name;
                    charDefinition.Char = charDef.Char;
                    charDefinition.File = charDef.File;

                    var lineCount = 0;
                    foreach(var charContentLine in charFile)
                    {
                        if (lineCount > charSet.Height) //Ignore lines, that will exceed the charater size
                            continue;

                        var charContent = charContentLine.ToCharArray();

                        //Get pixels, maximum the number of the width
                        for (var pxNum = 0; pxNum < charSet.Width && pxNum < charContent.Length; pxNum++)
                        {
                            if (charContent[pxNum] == ' ')
                                continue;

                            var baseBrightness = (byte)(((int.Parse(charContent[pxNum].ToString(), System.Globalization.NumberStyles.HexNumber) + 1) * 16) - 1);
                            charDefinition.Pixels[pxNum, lineCount] = Color.FromArgb(baseBrightness, 255, 255, 255);
                        }

                        lineCount++;
                    }

                    //Add the character to the character Set
                    charSet.Characters.Add(charDefinition);
                }

                //add the character Set to the total list
                CharacterSets.Add(charSet);
            }
        }

        public static void SetLed(int LedNumber, Color color)
        {
            Controller.SetLED(LedNumber, color);
        }
        public static void SetAll(Color color)
        {
            Controller.SetAll(color);
        }

        public static void Render()
        {
            WS281X.Render();
        }

        public static void ShowString(string text, string areaName = null, string characterSet = null)
        {
            var area = areaName == null ? new Area() {Width = X, Height = Y} : LayoutConfig.AreaList.Single(x => x.Name == areaName);

            var matchingCharSets = CharacterSets.Where(x => x.Name == (characterSet ?? CharacterSet));
            matchingCharSets = matchingCharSets.Where(x => x.Height <= area.Height).OrderByDescending(x => x.Height).ThenByDescending(x => x.Width);
            var charSet = matchingCharSets.First();

            Console.WriteLine("Writing string {0} in area {1} using charSet {2}", text, area.Name, charSet.Name + "(" + charSet.Width + "x" + charSet.Height + ")");

            //Flush Area
            for (int x = area.PositionX; x < area.PositionX + area.Width; x++)
            {
                for (int y = area.PositionY; y < area.PositionY + area.Height; y++)
                {
                    Display.SetLed(Display.GetLedNumber(x, y), Color.Black);
                }
            }

            //Set the new text
            if (area.Align == "left")
            {
                var posX = area.PositionX;
                var posY = area.PositionY;

                foreach (var aChar in text.ToCharArray())
                {
                    var charObj = charSet.Characters.SingleOrDefault(x => x.Char == aChar);

                    for (int x = 0; x < charObj.Width; x++)
                    {
                        for (int y = 0; y < charObj.Height; y++)
                        {
                            //If the Area Borders are hard and content should be cut off, don't show pixels out of area.
                            if (LayoutConfig.HardAreaBorders && (posX + x > area.Width + area.PositionX || posY + y > area.Height + area.PositionY))
                                continue;

                            var ledNum = GetLedNumber(posX + x, posY + y);
                            //Console.WriteLine("Setting X={0}/{1} and Y={2}/{3} with LED Number {4}", area.PositionX, x, area.PositionY, y, ledNum);

                            SetLed(ledNum, charObj.Pixels[x, y]);
                        }
                    }

                    posX += charObj.Width + 1;
                }
            }
            else if (area.Align == "center")
            {
                //Get the width of the string
                var textWidth = 0;
                foreach (var aChar in text.ToCharArray())
                {
                    textWidth += charSet.Characters.SingleOrDefault(x => x.Char == aChar).Width;
                    textWidth++;
                }
                textWidth--; //Substract the tailing character space

                //Move the current write-position to the centered position
                var posX = area.Width / 2 - textWidth / 2;
                var posY = area.PositionY;

                //Set the new text centered
                foreach (var aChar in text.ToCharArray())
                {
                    var charObj = charSet.Characters.SingleOrDefault(x => x.Char == aChar);

                    for (int x = 0; x < charObj.Width; x++)
                    {
                        for (int y = 0; y < charObj.Height; y++)
                        {
                            //If the Area Borders are hard and content should be cut off, don't show pixels out of area.
                            if (LayoutConfig.HardAreaBorders && (posX + x > area.Width + area.PositionX || posY + y > area.Height + area.PositionY || posX < 0 || posY < 0))
                                continue;

                            var ledNum = GetLedNumber(posX + x, posY + y);
                            //Console.WriteLine("Setting X={0}/{1} and Y={2}/{3} with LED Number {4}", area.PositionX, x, area.PositionY, y, ledNum);

                            SetLed(ledNum, charObj.Pixels[x, y]);
                        }
                    }

                    posX += charObj.Width + 1;
                }
            }
            else if (area.Align == "right")
            {
                var posX = area.PositionX + area.Width;
                var posY = area.PositionY;

                //Set the new text right. Take the last character first
                foreach (var aChar in text.ToCharArray().Reverse())
                {
                    var charObj = charSet.Characters.SingleOrDefault(x => x.Char == aChar);

                    for (int x = charObj.Width; x >= 0; x--)
                    {
                        for (int y = charObj.Height; y >= 0; y--)
                        {
                            //If the Area Borders are hard and content should be cut off, don't show pixels out of area.
                            if (LayoutConfig.HardAreaBorders && (posX + x > area.Width + area.PositionX || posY + y > area.Height + area.PositionY || posX < 0 || posY < 0))
                                continue;

                            var ledNum = GetLedNumber(posX - x, posY - y);
                            //Console.WriteLine("Setting X={0}/{1} and Y={2}/{3} with LED Number {4}", area.PositionX, x, area.PositionY, y, ledNum);

                            SetLed(ledNum, charObj.Pixels[x, y]);
                        }
                    }

                    posX -= charObj.Width - 1;
                }
            }
            Render();
        }

        public static void ShowChar(Character charObj, string areaName)
        {
            var area = LayoutConfig.AreaList.Single(x => x.Name == areaName);

            for (int x = 0; x < charObj.Width; x++)
            {
                for (int y = 0; y < charObj.Height; y++)
                {
                    //If the Area Borders are hard and content should be cut off, don't show pixels out of area.
                    if (LayoutConfig.HardAreaBorders && (x > area.Width || y > area.Height))
                        continue;

                    var ledNum = GetLedNumber(area.PositionX + x, area.PositionY + y);
                    //Console.WriteLine("Setting X={0}/{1} and Y={2}/{3} with LED Number {4}", area.PositionX, x, area.PositionY, y, ledNum);

                    SetLed(ledNum, charObj.Pixels[x, y]);
                }
            }
            Render();
        }

        public static void ShowChar(char character, string areaName)
        {
            Console.WriteLine("showing character {0} in area {1}", character, areaName);

            //Height = 10 for testing. Needs to be advanced to support multiple sizes
            var charObj = CharacterSets
                .Single(x => x.Name == CharacterSet && x.Height == 10).Characters
                .DefaultIfEmpty(new Character(6, 10) {Name="Unknown"})
                .SingleOrDefault(x => x.Char == character);
            ShowChar(charObj, areaName);
        }

        public static int GetLedNumber(int matrixX, int matrixY)
        {
            //Console.WriteLine("Get LED Number of X={0} Y={1}", matrixX, matrixY);

            //contains the modified X-Coordinated in case of alternating Rows
            var calculatedX = matrixX;
            var calculatedY = matrixY;

            //If the panel has alternating rows and it is an even row, inverse the LED count.
            if (Display.HasAlternatingRows && matrixY % 2 != 0)
            {
                calculatedX = Display.X - 1 - matrixX;
                //Console.WriteLine("Alternating Row. X is now {0}", calculatedX);
            }

            //If the LED #1 is at the bottom, switch Y axis
            if (Display.IsBottomToTop)
            {
                calculatedY = Display.Y - 1 - matrixY;
                calculatedX = Display.X - 1 - calculatedX;
                //Console.WriteLine("Switched Y={0} to Y={1}", matrixY, calculatedY);
            }

            //Console.WriteLine("LED Number is {0}", calculatedX + X * matrixY);
            return calculatedX + Display.X * calculatedY;
        }
    }
}
