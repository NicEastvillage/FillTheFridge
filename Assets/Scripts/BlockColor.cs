using UnityEngine;
using System.Collections;

public static class BlockColor {

	public static Color FromString(string color)
    {
        switch(color)
        {
            case "red": return Color.red;
            case "blue": return Color.blue;
            case "green": return Color.green;
            case "yellow": return Color.yellow;
            case "purple": return Color.magenta;
            case "magenta": return Color.magenta;
            case "cyan": return Color.cyan;
            case "white": return Color.white;
            case "black": return Color.black;
            case "grey": return Color.grey;
            case "orange": return new Color(1, 0.4f, 0);
            default: return Color.white;
        }
    }
}
