using UnityEngine;
using System.Collections;

public class RotatorScript : MonoBehaviour 
{
	
	public float speedx;
	public float speedy;
	public float speedz;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		this.transform.Rotate(speedx,speedy,speedz);
		
	}
}

