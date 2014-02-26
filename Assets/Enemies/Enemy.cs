using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
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


	private Turret Target;
	private Object Base;

	public Vector3 Position;



	void Start ()
	{

	}

	void Update ()
	{

	}

	void UpdateLate() {
		Move();
        Fire();
	}

	void Move() {
		// Update Position
	}

    private void Fire() {
        if (Target != null) {
            //Either inflict direct damage or Instantiate a projectile
            Target.TakeDamage(this.CurrentDamage);
        } else {
            //find Target
        }
    }

    public void TakeDamage(float damage) {
        this.TakenDamage += damage;
    }
}

