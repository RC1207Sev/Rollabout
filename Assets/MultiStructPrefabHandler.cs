using UnityEngine;
using System.Collections;
using System;

public class MultiStructPrefabHandler : MonoBehaviour {

    public string style;

    public GameObject[] prefabs;
    public string percentagesDistribution;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject GetRandomPrefab()
    {
        int temp = (int)UnityEngine.Random.Range(1, 100);
        int tempperc, totalperc = 0;
        string[] percentages = percentagesDistribution.Split(';');

        for (int i=0; i< percentages.Length; i++)
        {
            tempperc = Int32.Parse(percentages[i]);
            if ((temp > totalperc) && (temp < totalperc + tempperc))
            {
                return prefabs[i];
            }
            totalperc += tempperc;

        }

        return prefabs[0];

    }


}
