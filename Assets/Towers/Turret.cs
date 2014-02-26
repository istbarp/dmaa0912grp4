using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    private int Level = 1;
    private float LevelDamageMultiplier = 1.1f;
    private float LevelHealthMultiplier = 1.1f;
    private float LevelReloadTimeMultiplier = 0.9f;

    private float TakenDamage = 0;
    private float BaseHealth = 100;
    private float MaxHealth { get { return (BaseHealth * Mathf.Pow(LevelHealthMultiplier,Level)); } }
    private float CurrentHealth { get { if(MaxHealth < TakenDamage){return 0;}else{return (MaxHealth - TakenDamage);} } }

    private float BaseDamage = 5;
    private float CurrentDamage { get { return (BaseDamage * Mathf.Pow(LevelDamageMultiplier,Level)); } }


    private float BaseReloadTime = 2;
    private float CurrentReloadTime { get { return (BaseReloadTime * Mathf.Pow(LevelReloadTimeMultiplier,Level)); } }

	private Enemy Target;
	private Projectile Projectile;

	private Quaternion rotation;
	private double nextMoveTime;
	private double nextFireTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Target != null) 
		{
			if(Time.time >= nextMoveTime)
			{
				//CalculateAim(Target.position);
			}

			if(Time.time >= 0.75f * Time.deltaTime)
			{
				Fire ();
			}
		}
	}

	private void CalculateAim(Vector3 targetPosition)
	{
		Vector3 aimPoint = new Vector3 (targetPosition.x, targetPosition.y, targetPosition.z);
		rotation = Quaternion.LookRotation (aimPoint);
	}

	private void Fire()
	{
		Projectile clone = (Projectile)Instantiate(Projectile, transform.position, rotation);
        // Finish this
        //clone.AddMesh(mesh?);
        clone.rigidbody.AddForce(clone.transform.forward * 1f);
	}

    public void TakeDamage(float damage) {
        this.TakenDamage += damage;
    }
}
