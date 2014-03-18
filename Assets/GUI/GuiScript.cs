using UnityEngine;
using System.Collections;

public class GuiScript : MonoBehaviour {

	void OnGUI () {

		Cash cash = new Cash ();

		//Tower box
		GUI.Box (new Rect (Screen.width - 100,10,100,300), "Towers");

		if(GUI.Button(new Rect(Screen.width - 90,40,80,20),"Tower 1"));
		{
			//load tower 1
		}
		if(GUI.Button(new Rect(Screen.width - 90,80,80,20),"Tower 2"));
		{
			//load tower 2
		}
		if(GUI.Button(new Rect(Screen.width - 90,120,80,20),"Tower 3"));
		{
			//load tower 3
		}
		if(GUI.Button(new Rect(Screen.width - 90,160,80,20),"Tower 4"));
		{
			//load tower 4
		}

		//Box in the bottom right corner
		GUI.Box (new Rect (Screen.width - 130, Screen.height - 50, 130, 50), "Money");

		//set string money to correct number
		//money = class.currentmoney();

		GUI.Label (new Rect (Screen.width - 120, Screen.height - 25, 120, 50), cash.getCash()); //money);

	}
	
}
