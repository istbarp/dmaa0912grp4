using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Static values the have no real place to be.
/// </summary>
public static class StaticValues
{
    public static string MouseAxisX = "None";
    public static string MouseAxisY = "None";
    public static int invertX = 1;



    public static int invertY = 1;
    public static float deadZone = 0.25f;
    public static float AxisxSpeed = 100;
    public static float AxisySpeed = 100;

    /// <summary>
    /// makes a list into a single string which is used for gui e.g.
    /// </summary>
    /// <param name="l">A list</param>
    /// <returns>Returns a string with everything from the list</returns>
    public static string GetListContentAsString(this List<KeyCode> l)
    {
        string str = "";
        for (int i = 0; i < l.Count; i++)
        {
            str += l[i].ToString();
            if (i + 1 != l.Count)
            {
                str += " + ";
            }
        }
        return str;
    }

    /// <summary>
    /// Calculates where the a point is at the grid
    /// </summary>
    /// <param name="cam">The camera that should be calculated from</param>
    /// <param name="screenPos">A possition on the screen</param>
    /// <returns>a vector2 reletive to a plane at position a global y=0</returns>
    public static Vector2 ScreenToWorldPlane(this Camera cam, Vector2 screenPos)
    {
        // Create a ray going into the scene starting 
        // from the screen position provided 
        Ray ray = cam.ScreenPointToRay(screenPos);

        // ray didn't hit any solid object, so return the 
        // intersection point between the ray and 
        // the Y=0 plane (horizontal plane)
        float t = -ray.origin.y / ray.direction.y;
        return new Vector2(ray.GetPoint(t).x, ray.GetPoint(t).z);
    }
}