using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public double projectileSpeed = 5.0f;
	public double projectileRange = 1.5f;

    private Enemy Target;

    public Projectile(Enemy Target) {
        this.Target = Target;
    }

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
