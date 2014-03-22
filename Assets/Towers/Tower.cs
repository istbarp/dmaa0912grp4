using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    private bool isactive = false;
    private bool isplaced = false;

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

    private float BaseCost = 100;
    private float UpgradeCost { get { return (BaseCost * Mathf.Pow(1.25f, Level)); } }

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!isplaced)
        {
            MoveTower();
        }

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
                StaticValues.PlayerMoney -= BaseCost;
            }
			//&& turretCost[isplaced] <= myCash
			//myCash -= turretCost[isplaced];
        }
        else
        {
            this.renderer.material.color = Color.red;
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

    public void Upgrade() {
        if (StaticValues.PlayerMoney >= this.UpgradeCost)
        {
            StaticValues.PlayerMoney -= this.UpgradeCost;
            this.Level += 1;
        }
    }
    //public void TakeDamage(float damage) {
    //    this.TakenDamage += damage;
    //}
}
