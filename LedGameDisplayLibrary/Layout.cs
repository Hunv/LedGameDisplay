using System;
using System.Collections.Generic;
using System.Text;

namespace LedGameDisplayLibrary
{
    public class Layout
    {
        /// <summary>
        /// The Name of the Layout
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Width of the LED Panel this Layout is for
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The Height of the LED Panel this Layout is for
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The name of the CharacterSet that will used for this Layout
        /// </summary>
        public string CharacterSet { get; set; }

        /// <summary>
        /// The Areas this Layout contains
        /// </summary>
        public List<Area> AreaList { get; set; }

        /// <summary>
        /// The Characters, that part of the CharactersSet
        /// </summary>
        public List<Character> CharacterList { get; set; }

        /// <summary>
        /// If content of an Area is larger than the Area, should the content be cut?
        /// </summary>
        public bool HardAreaBorders { get; set; }

        /// <summary>
        /// The Brightness this Layout should use
        /// </summary>
        public byte Brightness { get; set; }

    }
}
