using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
	
	// Use this for initialization
	void OnGUI()
	{
		int height = Screen.height / 2;
		int width = Screen.width / 2;
		
		// Make a background box
		GUI.Box(new Rect(width, height, 100, 90), "Loader Menu");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(width + 10, height + 30, 80, 20), "Level 1")) {
			Application.LoadLevel(1);
		}
	}
}
