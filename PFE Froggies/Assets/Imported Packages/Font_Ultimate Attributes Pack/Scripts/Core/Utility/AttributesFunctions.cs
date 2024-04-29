using UnityEngine;
using System.Text.RegularExpressions;

namespace UltimateAttributesPack
{
    public static class AttributesFunctions
    {
        public static Color GetColorByName(string colorName, Color baseColor)
        {
            // Detect if color is valid
            Color returnedColor = baseColor;
            bool validColor = false;
            switch (colorName) // If color name is valid
            {
                case "black":
                    returnedColor = Color.black;
                    validColor = true;
                    break;
                case "blue":
                    returnedColor = Color.blue;
                    validColor = true;
                    break;
                case "cyan":
                    returnedColor = Color.cyan;
                    validColor = true;
                    break;
                case "green":
                    returnedColor = Color.green;
                    validColor = true;
                    break;
                case "grey":
                    returnedColor = Color.grey;
                    validColor = true;
                    break;
                case "magenta":
                    returnedColor = Color.magenta;
                    validColor = true;
                    break;
                case "red":
                    returnedColor = Color.red;
                    validColor = true;
                    break;
                case "white":
                    returnedColor = Color.white;
                    validColor = true;
                    break;
                case "yellow":
                    returnedColor = Color.yellow;
                    validColor = true;
                    break;
                case "orange":
                    returnedColor = new Color(1, 0.5f, 0, 1);
                    validColor = true;
                    break;
                case "pink":
                    returnedColor = new Color(0.96f, 0.19f, 0.72f, 1);
                    validColor = true;
                    break;
                case "purple":
                    returnedColor = new Color(0.74f, 0, 0.52f, 1);
                    validColor = true;
                    break;
                case "light green":
                    returnedColor = new Color(0.52f, 0.9f, 0.54f, 1);
                    validColor = true;
                    break;
                case "light blue":
                    returnedColor = new Color(0, 0.94f, 1, 1);
                    validColor = true;
                    break;
                case "brown":
                    returnedColor = new Color(0.35f, 0.24f, 0.19f, 1);
                    validColor = true;
                    break;
                case "dark blue":
                    returnedColor = new Color(0, 0.12f, 0.36f, 1);
                    validColor = true;
                    break;
            }
            if (!validColor) // If color name is not valided
            {
                Regex hexRegex = new Regex("^[a-fA-F0-9]+$");
                bool isHex = hexRegex.IsMatch(colorName);
                if (isHex) // If it's an Hex, set color
                {
                    ColorUtility.TryParseHtmlString("#" + colorName, out returnedColor);
                }
            }
            return returnedColor;
        }
    }
}