using UnityEngine;
using System.Collections;

public class Cash : MonoBehaviour 
{

	public static float myCash = 200;
	public static int[] turretCost = new int[200];


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public string getCash()
	{
		return myCash.ToString();
	}
}
