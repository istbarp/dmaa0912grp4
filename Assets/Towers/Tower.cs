using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    private bool isactive = false;
    private bool isplaced = false;

    private int Level = 1;
    private float LevelDamageMultiplier = 1.1f;
    private float LevelHealthMultiplier = 1.1f;
    private float LevelReloadTimeMultiplier = 0.9f;

    private float BaseDamage = 5;
    private float CurrentDamage { get { return (BaseDamage * Mathf.Pow(LevelDamageMultiplier,Level)); } }

    private float BaseReloadTime = 1;
    private float ReloadTimer = 0;
    private float CurrentReloadTime { get { return (BaseReloadTime * Mathf.Pow(LevelReloadTimeMultiplier,Level)); } }

	private GameObject Target;
    public List<GameObject> Reachable = new List<GameObject>();
	private Projectile Projectile;

	private Quaternion rotation;
	private double nextMoveTime;
	private double nextFireTime;

    public float Cost = 100;
	public float turnSpeed = 30f;

    private float BaseCost = 100;
    private float UpgradeCost { get { return (BaseCost * Mathf.Pow(1.25f, Level)); } }

	
	// Update is called once per frame
	void Update () 
	{
        if (!isplaced)
        {
            MoveTower();
            return;
        }

        if (Reachable.Count != 0)
        {
            Fire();
        }

        ReloadTimer += Time.deltaTime;

        //if (Target != null)
        //{
        //    if (Time.time >= nextMoveTime)
        //    {
        //        CalculateAim(Target.gameObject.transform.position);
        //    }

        //    if (Time.time >= 0.75f * Time.deltaTime)
        //    {
        //        Fire();
        //    }
        //} 
        //else
        //{
        //    FindTarget();
        //}
	}

    /// <summary>
    /// Moves the tower along the grid and check if the possition is valid for the towers to be placed at
    /// </summary>
    private void MoveTower()
    {
        Vector2 vec = Camera.main.ScreenToWorldPlane(Input.mousePosition);

        if (vec.x < 0)
        {
            vec.x = 0;
        }
        if (vec.x > 31)
        {
            vec.x = 31;
        }
        if (vec.y < 0)
        {
            vec.y = 0;
        }
        if (vec.y > 31)
        {
            vec.y = 31;
        }

        this.transform.position = new Vector3(Mathf.RoundToInt(vec.x) + 0.5f, this.transform.position.y, Mathf.RoundToInt(vec.y) + 0.5f);

        if (GameObject.Find("Grid").GetComponent<Terrain>().terrainData.GetAlphamaps(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), 1, 1)[0, 0, 1] == 1)
        {
            this.renderer.material.color = Color.white;
			if (Input.GetKey(KeyCode.Mouse0) )
            {
                isplaced = true;
                StaticValues.Money -= BaseCost;
            }
        }
        else
        {
            this.renderer.material.color = Color.red;
        }
    }

//	private void CalculateAim()
//	{
//		if (Reachable [0] != null)
//		{
//			transform.LookAt (Reachable [0].transform.position);
//			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y - 45, 0);
//		}
//	}

    //private void FindTarget()
    //{
    //    if (Target == null)
    //    {
    //        Enemy[] enemies = gameObject.GetComponents<Enemy>();
    //        foreach(Enemy e in enemies)
    //        {
    //            Vector3 pos = e.gameObject.transform.position;
    //            if(isInRange(this.gameObject.transform.position,pos,50))
    //            {
    //                //this.Target = e;
    //                break;
    //            }
    //        }
    //    }
    //}

    //private bool isInRange(Vector3 source, Vector3 target, float range)
    //{
    //    float x = Mathf.Sqrt(Mathf.Pow(source.x - target.x,2));
    //    float y = Mathf.Sqrt(Mathf.Pow(source.y - target.y,2));
    //    float z = Mathf.Sqrt(Mathf.Pow(source.z - target.z,2));

    //    return ( range > ( Mathf.Sqrt( Mathf.Pow(z,2) + Mathf.Pow( (Mathf.Sqrt( Mathf.Pow(x,2) + Mathf.Pow(y,2))), 2) ) ) );
    //}

	private void Fire()
	{
        if (BaseReloadTime < ReloadTimer)
        {
            if (Reachable[0].gameObject != null)
            {
                Reachable[0].GetComponent<Enemy>().CurrentHealth -= CurrentDamage;
                ReloadTimer = 0;
                if (Reachable[0].GetComponent<Enemy>().CurrentHealth <= 0)
                {
                    Reachable.RemoveAt(0);
                }
            }
            else
            {
                Reachable.RemoveAt(0);
            }
        }
	}

    public void Upgrade()
    {
        if (StaticValues.Money >= this.UpgradeCost)
        {
            StaticValues.Money -= this.UpgradeCost;
            this.Level += 1;
        }
    }
    //public void TakeDamage(float damage) {
    //    this.TakenDamage += damage;
    //}

    void OnTriggerEnter(Collider Other)
    {
        Reachable.Add(Other.gameObject);
    }

    void OnTriggerExit(Collider Other)
    {
        Reachable.Remove(Other.gameObject);
    }
}