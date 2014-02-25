using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
	public GameObject[] WayPoint;
	public double Offset = 1;
	private int CurrentWaypoint;

	// Use this for initialization
	void Start ()
	{
        CurrentWaypoint = 0;
        if (Offset < 0.01)
        {
            Offset = 0.01;
        }
        if (Offset > 10)
        {
            Offset = 10;
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.position = Vector3.MoveTowards(this.transform.position, WayPoint[CurrentWaypoint].transform.position, Time.deltaTime);
        this.transform.LookAt(WayPoint[CurrentWaypoint].transform.position);
		if (Vector3.Distance(this.transform.position, WayPoint[CurrentWaypoint].transform.position) < Offset)
		{
			if (CurrentWaypoint == WayPoint.Length - 1)
			{
				Destroy(this.gameObject);
				//TODO: Decrease lifes left by one - Need script
			}
			else
			{
				CurrentWaypoint++;
			}
		}
	}
}