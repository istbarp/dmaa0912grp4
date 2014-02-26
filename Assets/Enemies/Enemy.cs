using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	private double Attack = 1.00;
	private int Level = 1;
	private float LevelMultiplier = 1.1f;

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
		Move ();
	}

	void Move() {
		// Update Position
	}

    void Fire() {
        if (Target != null) {
            Turret.
        } else {

        }
    }
}

