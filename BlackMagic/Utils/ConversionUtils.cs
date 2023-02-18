using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class ConversionUtils
    {
        public static float DegToRad(float deg)
        {
            return deg * MathF.PI / 180;
        }

        public static float RadToDeg(float rad)
        {
            return rad * 180 / MathF.PI;
        }

        //Converts string in format {R:___ G:___ B:___ A:___} to Color value
        public static Color RGBAToColor(string rgba) //TODO: Add error handling
        {
            //Setup local variables
            string rawR = "";
            string rawG = "";
            string rawB = "";
            string rawA = "";
            byte j = 0; // <= To keep track of which (rgba) is being modified

            //Loop through characters of string
            for (int i = 0; i < rgba.Length;)
            {
                //Skip any variables taht aren't numbers
                if (!byte.TryParse(rgba[i].ToString(), out _))
                {
                    i++;
                    continue;
                }

                //Locate and parse number segments into corresponding string variables
                while (i < rgba.Length && byte.TryParse(rgba[i].ToString(), out _))
                {
                    switch (j)
                    {
                        case 0:
                            rawR += rgba[i].ToString();
                            break;
                        case 1:
                            rawG += rgba[i].ToString();
                            break;
                        case 2:
                            rawB += rgba[i].ToString();
                            break;
                        case 3:
                            rawA += rgba[i].ToString();
                            break;
                    }
                    i++;
                }
                j++;
            }

            //Convert raw string values to integers
            byte r = byte.Parse(rawR);
            byte g = byte.Parse(rawG);
            byte b = byte.Parse(rawB);
            byte a = byte.Parse(rawA);

            //Create new color
            Color color = new Color(r, g, b, a);

            //Return value
            return color;
        }
    }
}
