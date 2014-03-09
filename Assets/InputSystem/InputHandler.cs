using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Class for storing command and assosiated keys.
/// </summary>
public class InputHandler
{
    public string Command { get; private set; }
    public List<KeyCode> Key1 { get; private set; }
    public List<KeyCode> Key2 { get; private set; }
    public string Joystick { get; set; }
    public string TagCategory { get; set; }
    
    /// <summary>
    /// Instantiate a new input handler
    /// </summary>
    /// <param name="_command">A command</param>
    /// <param name="_controlKeys1">all keycodes as strings for primary key</param>
    /// <param name="_controlKeys2">all keycodes as strings for secondary key</param>
    /// <param name="_joystick">the joystick key</param>
    public InputHandler(string _command, string[] _controlKeys1, string[] _controlKeys2, string _joystick, string _tagcategory)
    {
        Command = _command;
        Key1 = KeyParser(_controlKeys1);
        Key2 = KeyParser(_controlKeys2);
        Joystick = _joystick;
        TagCategory = _tagcategory;
    }

    /// <summary>
    /// Parses the stored strings to a correct keycode
    /// </summary>
    /// <param name="keys">Array of keys</param>
    /// <returns>A list with the real keycodes</returns>
    private List<KeyCode> KeyParser(string[] keys)
    {
        List<KeyCode> listkeys = new List<KeyCode>();
        foreach (string s in keys)
        {
            listkeys.Add((KeyCode)Enum.Parse(typeof(KeyCode), s, true));
        }
        return listkeys;
    }

    /// <summary>
    /// Makes this command into a string for storing
    /// </summary>
    /// <returns>Returns a string you and save and read another time</returns>
    public override string ToString()
    {
        string s = "";
        int c = 0;
        foreach (KeyCode k in Key1)
        {
            if (c > 0)
            {
                s += " + ";
            }
            s += k.ToString();
            c++;
        }
        s += "; ";
        c = 0;
        foreach (KeyCode k in Key2)
        {
            if (c > 0)
            {
                s += " + ";
            }
            s += k.ToString();
            c++;
        }
        s += ";";
        c = 0;

        s += Joystick;

        return s;
    }
}