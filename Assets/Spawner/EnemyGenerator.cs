using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour
{
    public int Seed = 0;
    public int WaveAmount = 1;
    public int CurrentWave = 0;
    public int WaveSize = 1;

    public GameObject Enemy;

    private System.Random rand;
    private int ChanceofBossWave = 0;
    private int ChanceofSpecial = 0;

    private float spawntimer = 0;
    private bool spawnactive = false;

	// Use this for initialization
	void Start ()
    {
	    rand = new System.Random(Seed);
        WaveAmount = rand.Next(5, 50);
        spawntimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (spawnactive)
        {
            spawntimer += Time.deltaTime;
            if (spawntimer > 1)
            {
                GameObject.Instantiate(Enemy, this.transform.position, this.transform.rotation);
                WaveSize--;
                spawntimer = 0;
            }
        }
        if (WaveSize == 0)
        {
            spawnactive = false;
            spawntimer = 0;
        }
	}

    void NormalWave()
    {
        WaveSize = rand.Next(6, 30);
        Enemy e = Enemy.GetComponent<Enemy>();
        e.BaseHealth = 10 + (CurrentWave * 1.05f);
        e.Armor = 0;
        e.Speed = rand.Next(1, 2);
        e.Value = (int)(5 + (CurrentWave * 1.10f));
    }

    void SpecialWave()
    {
        WaveSize = rand.Next(3, 20);
        int a = rand.Next(1, 3);
        Enemy e = Enemy.GetComponent<Enemy>();
        if (a == 1)
        {
            e.Armor = rand.Next(2, 5);
        }
        else if (a == 2)
        {
            e.BaseHealth = rand.Next(100, 250) + (CurrentWave * 1.05f);
        }
        else if (a == 3)
        {
            e.Speed = rand.Next(10, 100) / 10;
        }
        e.Value = (int)(e.BaseHealth / 100 + e.Armor * 5 + e.Speed * 1.10f + CurrentWave * 1.075);
    }

    void BossWave()
    {
        WaveSize = rand.Next(1, 3);
        MonsterAbility a = (MonsterAbility)rand.Next(1, 7);
        Enemy e = Enemy.GetComponent<Enemy>();
        e.BaseHealth = 10 + (CurrentWave * 1.10f);
        if ((a & MonsterAbility.Armor) == MonsterAbility.Armor)
        {
            e.Armor = rand.Next(1, 10);
        }
        if ((a & MonsterAbility.Health) == MonsterAbility.Health)
        {
            if (e.Armor > 5)
            {
                e.Armor -= 4;
            }
            e.BaseHealth += (rand.Next(10, 100) * CurrentWave)  / WaveSize;
        }
        if ((a & MonsterAbility.Speed) == MonsterAbility.Speed)
        {
            if (e.BaseHealth * WaveSize > 1000)
            {
                e.Speed = (rand.Next(5, 10) / 10);
            }
            else
            {
                e.Speed = (rand.Next(5, 50) / 10);
            }
        }
        e.Value = (int)(e.BaseHealth / 100 + e.Armor * 10 + e.Speed * 1.10f + CurrentWave * 1.10);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(5, 5, 150, 32), "Force Wave") && !spawnactive)
        {
            int i = rand.Next(0, 100);
            if (WaveAmount == CurrentWave || i < ChanceofBossWave)
            {
                Debug.Log("Boss Wave Forced!");
                BossWave();
                ChanceofBossWave = 0;
            }
            else if (i < ChanceofSpecial)
            {
                Debug.Log("Special Wave Forced!");
                SpecialWave();
                ChanceofSpecial = 0;
            }
            else
            {
                Debug.Log("Wave Forced!");
                NormalWave();
                ChanceofBossWave += 2;
                ChanceofSpecial += 5;
            }
            spawntimer = 0;
            spawnactive = true;
            CurrentWave++;
        }
    }
}

enum MonsterAbility
{
    Health = 1, Armor = 2, Speed = 4,
}