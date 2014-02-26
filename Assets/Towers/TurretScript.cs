using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {


	public double reloadtime = 1f;

	private Transform target = null;
	private Quaternion rotation;
	private double nextMoveTime;
	private double nextFireTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (target != null) 
		{
			if(Time.time >= nextMoveTime)
			{
				CalculateAim(target.position);
			}

			if(Time.time >= 0.75f * Time.deltaTime)
			{
				fire ();
			}
		}
	}

	private void CalculateAim(Vector3 targetPosition)
	{
		Vector3 aimPoint = new Vector3 (targetPosition.x, targetPosition.y, targetPosition.z);
		rotation = Quaternion.LookRotation (aimPoint);
	}

	private void fire()
	{
		Instantiate (ProjectileScript, transform.position, rotation);
	}
}
