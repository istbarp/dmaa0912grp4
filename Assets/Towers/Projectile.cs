using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private double projectileSpeed = 5.0f;
	private double projectileRange = 1.5f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//transform.Translate(Vector3.forward * projectileSpeed);
		double distance = projectileSpeed * Time.deltaTime;

		if(distance >= projectileRange)
		{
			Destroy(gameObject);
		}
	}

    public void AddMesh(Mesh myMesh){
        this.gameObject.AddComponent<MeshFilter>().mesh = myMesh;
    }
}
