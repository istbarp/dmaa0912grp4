using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public List<Transform> WayPoints = new List<Transform>();
    public float Offset = 0.001f;
    private int CurrentWaypoint;

    private int Level = 1;
    private float LevelDamageMultiplier = 1.1f;
    private float LevelHealthMultiplier = 1.1f;
    private float LevelReloadTimeMultiplier = 0.9f;

    public float speed = 1;
    private float TakenDamage = 0;
    private float BaseHealth = 100;
    private Quaternion torot;
    private float dprot = 0.05f;
    private float MaxHealth { get { return (BaseHealth * Mathf.Pow(LevelHealthMultiplier,Level)); } }
    private float CurrentHealth { get { if(MaxHealth < TakenDamage){return 0;}else{return (MaxHealth - TakenDamage);} } }
    
    //private float BaseDamage = 5;
    //private float CurrentDamage { get { return (BaseDamage * Mathf.Pow(LevelDamageMultiplier,Level)); } }
    
    //private float BaseReloadTime = 2;
    //private float CurrentReloadTime { get { return (BaseReloadTime * Mathf.Pow(LevelReloadTimeMultiplier,Level)); } }


	//private Turret Target;
	//private Object Base;

    void Start()
    {
        CurrentWaypoint = 0;
        if (Offset < 0.00001)
        {
            Offset = 0.00001f;
        }
        if (Offset > 1)
        {
            Offset = 1;
        }

        WayPoints = (GameObject.Find("WayPoints").transform.GetComponentsInChildren<Transform>()).ToList<Transform>();
        WayPoints.RemoveAt(0);
        this.transform.LookAt(WayPoints[0]);
        torot = this.transform.rotation;
    }

    void Update()
	{
        Move();
	}

    float sprot = 0;
    /// <summary>
    /// Moves the enemy towards a waypoint 
    /// </summary>
	void Move()
    {
        if (Quaternion.Angle(this.transform.rotation, torot) > Offset)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, torot, ((Time.deltaTime + sprot) * speed) * dprot);
            sprot += Time.deltaTime;
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, WayPoints[CurrentWaypoint].position, Time.deltaTime * speed);
            sprot = 0;
        }
        
        if (Vector3.Distance(this.transform.position, WayPoints[CurrentWaypoint].position) < Offset)
        {
            if (CurrentWaypoint == WayPoints.Count() - 1)
            {
                Destroy(this.gameObject);
                GameObject.Find("Base").GetComponent<Base>().Life--;
            }
            else
            {
                CurrentWaypoint++;
                torot = Quaternion.LookRotation(WayPoints[CurrentWaypoint].position - transform.position);
            }
        }
	}

    //private void Fire() {
    //    if (Target != null) {
    //        //Either inflict direct damage or Instantiate a projectile
    //        Target.TakeDamage(this.CurrentDamage);
    //    } else {
    //        //find Target
    //    }
    //}

    public void TakeDamage(float damage)
    {
        this.TakenDamage += damage;
    }
}

