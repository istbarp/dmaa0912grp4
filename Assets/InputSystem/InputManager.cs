using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputManager
{
    public bool UseJoystick { get; set; }
    private const float deadzone = 0.4f;
    public List<InputHandler> InputKeys;

    /// <summary>
    /// Controlekeys/Macrokeys/keybinding keys
    /// </summary>
    public SortedDictionary<KeyCode, bool> Controlekeys = new SortedDictionary<KeyCode, bool> { { KeyCode.LeftControl, false },
        { KeyCode.RightControl, false }, { KeyCode.LeftAlt, false }, { KeyCode.RightAlt, false }, { KeyCode.AltGr, false },
        { KeyCode.LeftShift, false }, { KeyCode.RightShift, false } };

    /// <summary>
    /// Tags used to check key types
    /// </summary>
    public List<string> Tags = new List<string>() { "Movement", "Movement", "Movement", "Movement",
        "Camera Control", "Camera Control", "Camera Control", };

    /// <summary>
    /// Commands used to check key presses
    /// </summary>
    public List<string> Commands = new List<string>() { "Move Forward", "Move Backward", "Strafe Left", "Strafe Right", 
        "Select/Command", "Deselect/Quick Move", "Rotate", };
    
    /// <summary>
    /// Controls, please reffer to below to see how is set it up!!!
    /// </summary>
    public List<string> Controls = new List<string>() { "W; UpArrow; None", "S; DownArrow; None", "A; LeftArrow; None", "D; RightArrow; None",
        KeyCode.Mouse0.ToString() + "; " + KeyCode.None.ToString() + "; " + KeyCode.None.ToString(),
        KeyCode.Mouse1.ToString() + "; " + KeyCode.None.ToString() + "; " + KeyCode.None.ToString(),
        KeyCode.Mouse2.ToString() + "; " + KeyCode.None.ToString() + "; " + KeyCode.None.ToString(), };

    /// <summary>
    /// Instantiate a new Inputmanager
    /// </summary>
    public InputManager()
    {
        InputKeys = new List<InputHandler>();
        foreach (string s in Commands)
        {
            if (!PlayerPrefs.HasKey(s))
            {
                CreateConfig();
            }
        }
        LoadConfig();
        UseJoystick = true;
    }

    /// <summary>
    /// Create the configs and keybindings
    /// </summary>
    void CreateConfig()
    {
        PlayerPrefs.DeleteAll();
        // Command = Key1; Key2; Joystick Button/Key
        if (Commands.Count != Controls.Count || Commands.Count != Tags.Count)
        {
            Debug.Log("OPS!!! You obviously forgot to setup the two lists \"Commands\" and \"Controls\" correctly in Inputmanager.cs, ensure both lists have the same numbers of entries!!!");
            return;
        }
        for (int i = 0; i < Commands.Count; i++)
        {
            PlayerPrefs.SetString(Commands[i], Controls[i]);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the config and keybindings
    /// </summary>
    void LoadConfig()
    {
        InputKeys = new List<InputHandler>();
        try
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                if (!PlayerPrefs.HasKey(Commands[i]))
                {
                    CreateConfig();
                }
                else
                {
                    string[] inputs = PlayerPrefs.GetString(Commands[i]).Trim().Split(';');
                    List<string> keys1 = new List<string>();
                    List<string> keys2 = new List<string>();
                    string keys3 = "";
                    keys1.AddRange(inputs[0].Split('+'));
                    keys2.AddRange(inputs[1].Split('+'));
                    keys3 = inputs[2];
                    InputKeys.Add(new InputHandler(Commands[i], keys1.ToArray(), keys2.ToArray(), keys3, Tags[i]));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error in InputConfig.ini, Generating a new one..." + ex.Message);
        }
        if (!PlayerPrefs.HasKey("JoystickMouseX"))
        {
            PlayerPrefs.SetString("JoystickMouseX", StaticValues.MouseAxisX);
        }
        StaticValues.MouseAxisX = PlayerPrefs.GetString("JoystickMouseX");
        if (!PlayerPrefs.HasKey("JoystickMouseY"))
        {
            PlayerPrefs.SetString("JoystickMouseY", StaticValues.MouseAxisY);
        }
        StaticValues.MouseAxisY = PlayerPrefs.GetString("JoystickMouseY");
    }

    /// <summary>
    /// Saves the config file
    /// </summary>
    public void SaveConfig()
    {
        for (int i = 0; i < Commands.Count; i++)
        {
            PlayerPrefs.SetString(Commands[i], InputKeys[i].ToString());
            Debug.Log(InputKeys[i].ToString());
        }
        PlayerPrefs.SetString("JoystickMouseX", StaticValues.MouseAxisX);
        PlayerPrefs.SetString("JoystickMouseY", StaticValues.MouseAxisY);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Apply a new input/key to a command
    /// </summary>
    /// <param name="_command">The command that should be changed</param>
    /// <param name="kc">the new keycode</param>
    /// <param name="kst">where to store the input</param>
    private void ChangeKeys(string _command, KeyCode kc, KeyStoreType kst)
    {
        InputHandler ih = (InputHandler)InputKeys.Find(i => i.Command == _command);
        if (kst == KeyStoreType.Key1)
        {
            ih.Key1.Clear();
            for (int i = 0; i < Controlekeys.Count; i++)
            {
                if (Controlekeys.Values.ElementAt(i) == true)
                {
                    ih.Key1.Add(Controlekeys.Keys.ElementAt(i));
                }
            }
            ih.Key1.Add(kc);
        }
        else if (kst == KeyStoreType.Key2)
        {
            ih.Key2.Clear();
            for(int i = 0; i < Controlekeys.Count; i++)
            {
                if (Controlekeys.Values.ElementAt(i) == true)
                {
                    ih.Key2.Add(Controlekeys.Keys.ElementAt(i));
                }
            }
            ih.Key2.Add(kc);
        }
        PlayerPrefs.SetString(_command, ih.ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Apply a new input/key to a command
    /// </summary>
    /// <param name="_command">The command that should be changed</param>
    /// <param name="kc">the new keycode as a string</param>
    private void ChangeJoystick(string _command, string kc)
    {
        InputHandler ih = (InputHandler)InputKeys.Find(i => i.Command == _command);
        ih.Joystick = kc;
        PlayerPrefs.SetString(_command, ih.ToString());
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Checks wether controle/macro keys is pressed, SHOULD BE CHECKED ONCE EVERY FRAME!!!
    /// </summary>
    public void CheckControleKeys()
    {
        int j = Controlekeys.Count;
        for (int i = 0; i < j; i++)
        {
            if (Input.GetKeyDown(Controlekeys.Keys.ElementAt(i)))
            {
                Controlekeys[Controlekeys.Keys.ElementAt(i)] = true;
            }
            if (Input.GetKeyUp(Controlekeys.Keys.ElementAt(i)))
            {
                Controlekeys[Controlekeys.Keys.ElementAt(i)] = false;
            }
        }
    }

    /// <summary>
    /// Check wether a key assigned to a command is pressed.
    /// </summary>
    /// <param name="command">The command to check</param>
    /// <param name="stage">what stage the key shold be in</param>
    /// <returns>Returns true if the key(s) for the command is pressed, otherwise false</returns>
    public bool CheckInput(string command, KeyStages stage)
    {
        InputHandler inputChecker = InputKeys.Find(f => f.Command == command);
        if (UseJoystick)
        {
            if (JoystickChecker(inputChecker.Joystick, stage))
            {
                return true;
            }
        }
        if (stage == KeyStages.Press)
        {
            if (ListCheckerPress(inputChecker.Key1))
            {
                return true;
            }
            else if (ListCheckerPress(inputChecker.Key2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (stage == KeyStages.Down)
        {
            if (ListCheckerDown(inputChecker.Key1))
            {
                return true;
            }
            else if (ListCheckerDown(inputChecker.Key2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (stage == KeyStages.Up)
        {
            if (ListCheckerUp(inputChecker.Key1))
            {
                return true;
            }
            else if (ListCheckerUp(inputChecker.Key2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// Check the keys in a list to see if they're pressed.
    /// </summary>
    /// <param name="list">list of keycodes</param>
    /// <returns>true if everything is pressed</returns>
    private bool ListCheckerPress(List<KeyCode> list)
    {
        bool checkval = false;
        foreach (KeyCode k in list)
        {
            if (Controlekeys.ContainsKey(k))
            {
                if (Controlekeys[k])
                {
                    checkval = true;
                    continue;
                }
            }
            if (Input.GetKey(k))
            {
                checkval = true;
            }
            else
            {
                return false;
            }
        }
        return checkval;
    }

    /// <summary>
    /// Check the keys in a list to see if they're pressed.
    /// </summary>
    /// <param name="list">list of keycodes</param>
    /// <returns>true if everything is pressed</returns>
    private bool ListCheckerUp(List<KeyCode> list)
    {
        bool checkval = false;
        foreach (KeyCode k in list)
        {
            if (Controlekeys.ContainsKey(k))
            {
                if (Controlekeys[k])
                {
                    checkval = true;
                    continue;
                }
            }
            if (Input.GetKeyUp(k))
            {
                checkval = true;
            }
            else
            {
                return false;
            }
        }
        return checkval;
    }

    /// <summary>
    /// Check the keys in a list to see if they're pressed.
    /// </summary>
    /// <param name="list">list of keycodes</param>
    /// <returns>true if everything is pressed</returns>
    private bool ListCheckerDown(List<KeyCode> list)
    {
        bool checkval = false;
        foreach (KeyCode k in list)
        {
            if (Controlekeys.ContainsKey(k))
            {
                if (Controlekeys[k])
                {
                    checkval = true;
                    continue;
                }
            }
            if (Input.GetKeyDown(k))
            {
                checkval = true;
            }
            else
            {
                return false;
            }
        }
        return checkval;
    }

    private bool MouseChecker(string key, KeyStages ks)
    {
        return true;
    }

    /// <summary>
    /// checks the joystick for input
    /// </summary>
    /// <param name="key">key to check</param>
    /// <param name="ks">the stage</param>
    /// <returns>return trues if the key is pressed</returns>
    private bool JoystickChecker(string key, KeyStages ks)
    {
        if (key.Contains("Axis"))
        {
            for (int i = 0; i < 5; i++)
            {
                if (key == StaticValues.MouseAxisX || key == StaticValues.MouseAxisY)
                {
                    continue;
                }

                if (key == "Axis" + i + "X+" && deadzone < Input.GetAxis("Axis" + i + "X"))
                {
                    return true;
                }
                else if (key == "Axis" + i + "Y+" && deadzone < Input.GetAxis("Axis" + i + "Y"))
                {
                    return true;
                }
                else if (key == "Axis" + i + "X-" && -deadzone > Input.GetAxis("Axis" + i + "X"))
                {
                    return true;
                }
                else if (key == "Axis" + i + "Y-" && -deadzone > Input.GetAxis("Axis" + i + "Y"))
                {
                    return true;
                }
            }
        }
        else if (key.ToLower().Contains("joystick"))
        {
            if (ks == KeyStages.Press)
            {
                return Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key, true));
            }
            else if (ks == KeyStages.Down)
            {
                return Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), key, true));
            }
            else if (ks == KeyStages.Up)
            {
                return Input.GetKeyUp((KeyCode)Enum.Parse(typeof(KeyCode), key, true));
            }
        }
        return false;
    }

    /// <summary>
    /// make preperations to Change the the input for a command.
    /// </summary>
    /// <param name="_command">The command that should be changed</param>
    /// <param name="kst">Where to store the new input</param>
    /// <returns></returns>
    public bool ChangeCurrentKeyInput(string _command, KeyStoreType kst)
    {
        if (Event.current.isMouse)
        {
            ChangeKeys(_command, (KeyCode)Enum.Parse(typeof(KeyCode), "mouse" + Event.current.button, true), kst);
            return true;
        }
        else if (Event.current.keyCode == KeyCode.Escape)
        {
            ChangeKeys(_command, KeyCode.None, kst);
            return true;
        }
        else if (!Controlekeys.Keys.Contains(Event.current.keyCode) && kst != KeyStoreType.Joystick && Event.current.keyCode != KeyCode.None)
        {
            ChangeKeys(_command, Event.current.keyCode, kst);
            return true;
        }
        else if (UseJoystick)
        {
            for (int i = 0; i < 20; i++)
            {
                if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "JoystickButton" + i, true)))
                {
                    ChangeJoystick(_command, "JoystickButton" + i);
                    return true;
                }
            }
            for(int i = 0; i < 5; i++)
            {
                if (deadzone < Input.GetAxis("Axis" + i + "X"))
                {
                    ChangeJoystick(_command, "Axis" + i + "X+");
                    return true;
                }
                else if (deadzone < Input.GetAxis("Axis" + i + "Y"))
                {
                    ChangeJoystick(_command, "Axis" + i + "Y+");
                    return true;
                }
                else if (-deadzone > Input.GetAxis("Axis" + i + "X"))
                {
                    ChangeJoystick(_command, "Axis" + i + "X-");
                    return true;
                }
                else if (-deadzone > Input.GetAxis("Axis" + i + "Y"))
                {
                    ChangeJoystick(_command, "Axis" + i + "Y-");
                    return true;
                }
            }
        }
        return false;
    }
}