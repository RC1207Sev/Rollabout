using UnityEngine;
using System.Collections;

public class Map1 : MonoBehaviour {

	public GameObject ball;
	public GameObject[] sideElementsPrefs;
	public GameObject[] obstacleElementsPrefs;
	GameObject[,] totalMap;

	int max_i = 10;
	int max_j = 10;
	int obstacle_rarity = 10;

	// Use this for initialization
	void Start () {

		int tempvalue;

		totalMap = new GameObject[max_i,max_j];

		for (int i=0; i<max_i; i++)
			for (int j=0; j<max_j; j++) 
				{
					tempvalue = Random.Range (0, sideElementsPrefs.Length);
					totalMap[i,j] = Instantiate (sideElementsPrefs[tempvalue], new Vector3(i,0,j), Quaternion.identity) as GameObject;
					Debug.Log ("Object created in " + i + "-" + j + " prefab number " + tempvalue);
					if ( Random.Range (0, obstacle_rarity) == 0)
					{
						tempvalue = Random.Range (0, obstacleElementsPrefs.Length);
						Instantiate (obstacleElementsPrefs[tempvalue], new Vector3(i,0,j), Quaternion.identity);
						Debug.Log ("Obstacle created in " + i + "-" + j + " prefab number " + tempvalue);
					}
					
				}

		Destroy (totalMap [Random.Range (1, max_i-1), Random.Range (1, max_j-1)]); //create hole

		Instantiate (ball, new Vector3((max_i/2),5,(max_j/2)), Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
