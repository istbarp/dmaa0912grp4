using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cash;

public class Enemy : MonoBehaviour
{
	public int cashValue = 50;
    public List<Transform> WayPoints = new List<Transform>();
    public float Offset = 0.001f;
    private int CurrentWaypoint;

    public float Armor = 0;
    public float Speed = 1;
    public float CurrentHealth = 10;
    public float BaseHealth = 10;
    public int Value = 1;
    public int Damage = 1;

    //Rotation Stuff
    private Quaternion torot;
    private Quaternion frrot;
    private float sprot = 0;

    void Start()
    {
        CurrentHealth = BaseHealth;
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
        if (CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
			Cash.cash += cashValue;
        }
	}

    /// <summary>
    /// Moves the enemy towards a waypoint 
    /// </summary>
	void Move()
    {
        if (Quaternion.Angle(this.transform.rotation, torot) > Offset)
        {
            this.transform.rotation = Quaternion.Slerp(frrot, torot, Time.deltaTime + sprot * Speed);
            sprot += Time.deltaTime;
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, WayPoints[CurrentWaypoint].position, Time.deltaTime * Speed);
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
                frrot = this.transform.rotation;
                torot = Quaternion.LookRotation(WayPoints[CurrentWaypoint].position - transform.position);
            }
        }
	}

    public void TakeDamage(float damage)
    {
        //this.TakenDamage += damage;
    }
}

