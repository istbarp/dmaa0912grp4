using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.eulerAngles = new Vector3(90 - Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y - 180, 0);
        IHealthBar hb = (IHealthBar)this.transform.parent.GetComponent(typeof(IHealthBar));
        this.renderer.material.SetFloat("_Health", hb.CurrentHealth / hb.BaseHealth);
	}
}
