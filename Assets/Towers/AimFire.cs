using UnityEngine;
using System.Collections;

public class AimFire : MonoBehaviour {

	public GameObject myProjectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = .25f;
	public float errorAmount = .001f;
	public Transform myTarget;
	public GameObject muzzleEffect;
	public Transform[] muzzlePositions;
	public Transform turretTurn;

	private float nextFireTime;
	private float nextMoveTime;
	private Quaternion desiredRotation;
	private float aimError;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (myTarget) {
			if(Time.time >= nextMoveTime) {
				CalculateAimPosition(myTarget.position);
				turretTurn.rotation = Quaternion.Lerp(turretTurn.rotation, desiredRotation, Time.deltaTime*turnSpeed);
			}

			if(Time.time >= nextFireTime) {
				FireProjectile();
			}
		}
	}

	void OnTriggerEnter(Collider something) {
		if (something.gameObject.tag == "Enemy") {
			//nextFireTime = Time.time+(reloadTime*.5);
			myTarget = something.gameObject.transform;
		}
	}

	void OnTriggerExit(Collider something) {
		if (something.gameObject.transform == myTarget) {
			myTarget = null;
		}
	}

	void CalculateAimPosition(Vector3 targetPos) {
		Vector3 aimPoint = new Vector3(targetPos.x + aimError, targetPos.y + aimError, targetPos.z + aimError);
		desiredRotation = Quaternion.LookRotation(aimPoint);
	}

	void CalculateAimError() {
		aimError = Random.Range (-errorAmount, errorAmount);
	}

	void FireProjectile() {
		//audio.Play();
		nextFireTime = Time.time+reloadTime;
		nextMoveTime = Time.time+firePauseTime;
		CalculateAimError();

//		foreach(theMuzzlePos in muzzlePositions) {
//			Instantiate(myProjectile, theMuzzlePos.position, theMuzzlePos.rotation);
//			Instantiate(muzzleEffect, theMuzzlePos.position, theMuzzlePos.rotation);
//		}
	}
}
