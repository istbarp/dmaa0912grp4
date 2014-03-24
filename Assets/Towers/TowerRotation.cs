using UnityEngine;
using System.Collections;

public class TowerRotation : MonoBehaviour {


	//public Tower Reachable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CalculateAim();
	}

	private void CalculateAim()
	{
		if (transform.parent.GetComponent<Tower>().Reachable[0] != null || transform.parent.GetComponent<Tower>().Reachable.Count != 0)
		{
			transform.LookAt(transform.parent.GetComponent<Tower>().Reachable[0].transform.position);
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y - 180, 0);
		}
	}
}
