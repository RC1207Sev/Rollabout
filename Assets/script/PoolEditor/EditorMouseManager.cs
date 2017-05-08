using UnityEngine;
using System.Collections;

public class EditorMouseManager : MonoBehaviour {

	public GameObject ghostPrefab;

	public GameObject ghostPrefabProjection;

	private Vector3 defaultPosition, lastPosition;

	public GameObject GUI2Mask;

	public GameObject GridGO;

	public GameObject MainCamera;

	FileHandler mapSaver;

    MultiStyleSelector ghostPrefabPicker;

    GameObject tempRandGO;

	Ray ray;
	RaycastHit hit;
	int layerMask;

	RectTransform objectRectTransform;
	float width;
	float height;
	float rightOuterBorder;
	float leftOuterBorder;
	float topOuterBorder;
	float bottomOuterBorder;

	GameObject newPiece;

	// Use this for initialization
	void Start () {

		// Bit shift the index of the layer (9) to get a bit mask
		layerMask = 1 << 9;
		
		// This would cast rays only against colliders in layer 8, so we just inverse the mask.
		layerMask = ~layerMask;

		defaultPosition = new Vector3(0,0,0);

		mapSaver = FindObjectOfType (typeof (FileHandler)) as FileHandler;
			//this.GetComponent<FileHandler>();

	}
	
	// Update is called once per frame
	void Update () {

		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		objectRectTransform = GUI2Mask.GetComponent<RectTransform> ();
		width = objectRectTransform.rect.width;
		height = objectRectTransform.rect.height;
		rightOuterBorder = (width * .5f);
		leftOuterBorder = (width * -.5f);
		topOuterBorder = (height * .5f);
		bottomOuterBorder = (height * -.5f);

		// The following line determines if the cursor is on the object
		if (!(Input.mousePosition.x <= (GUI2Mask.transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (GUI2Mask.transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (GUI2Mask.transform.position.y + topOuterBorder) && Input.mousePosition.y >= (GUI2Mask.transform.position.y + bottomOuterBorder)))
		{
			ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			
			if( Physics.Raycast( ray, out hit, 100) && (hit.transform.gameObject.name == "GridElement") && (ghostPrefab.name != "PieceDeleter"))
			{
				ghostPrefabProjection.transform.position = hit.transform.position;
				ghostPrefab.transform.position = hit.transform.position;
                ghostPrefab.transform.position = new Vector3(ghostPrefab.transform.position.x, ghostPrefab.transform.position.y - .03f, ghostPrefab.transform.position.z);
                lastPosition = hit.transform.position;

				// left mouse click: create piece
				if( Input.GetMouseButtonDown(0) && (ghostPrefab.name != "PieceDeleter")) 
				{
                    tempRandGO = ghostPrefabPicker.GetRandomPrefab();
                    newPiece = (GameObject) Instantiate(tempRandGO, hit.transform.position, ghostPrefab.transform.rotation);
                    newPiece.name = tempRandGO.name;
                    newPiece.SendMessage("Wakeup", SendMessageOptions.DontRequireReceiver);
                    newPiece.transform.position = new Vector3(newPiece.transform.position.x, newPiece.transform.position.y + 0.01f, newPiece.transform.position.z);
					//hit.transform.Translate(0,-1000,0);
					mapSaver.addPiece (newPiece.transform.position, newPiece.transform.rotation, ghostPrefab.name,"");
				}
				// Right Mouse Button: rotate element (ghost)
				if( Input.GetMouseButtonDown(1)) // left mouse clicked
				{
					ghostPrefab.transform.Rotate (0,0,90);
				}
				
			}else
			{
				// if we are deleting, shows only over placed pieces
				if (ghostPrefab.name == "PieceDeleter"){

					ray = Camera.main.ScreenPointToRay( Input.mousePosition );
					
					if( Physics.Raycast( ray, out hit, 100, 1 << 9))
					{

						ghostPrefabProjection.transform.position = hit.transform.position;
						ghostPrefab.transform.position = hit.transform.position;
                        ghostPrefab.transform.position = new Vector3(ghostPrefab.transform.position.x, ghostPrefab.transform.position.y + .03f, ghostPrefab.transform.position.z);
                        lastPosition = hit.transform.position;
						
						// left mouse click: create piece
						if( Input.GetMouseButtonDown(0)) 
						{
							newPiece = hit.transform.gameObject;
							mapSaver.RemovePiece (newPiece);
							Destroy(newPiece);
						}

					}else // if we are not on a grid piece and we are not deleting, reset the ghost position
					{
						ghostPrefab.transform.position = Vector3.zero;
						ghostPrefabProjection.transform.position = Vector3.zero;
						
					}
				}

			}
		}

	
	}

	public void ChangeGhost(MultiStyleSelector ghost)
	{
		ghostPrefab.transform.position = defaultPosition;
        Destroy(ghostPrefab);
        ghostPrefabPicker = ghost;
        tempRandGO = ghost.GetRandomPrefab();
        ghostPrefab = Instantiate(tempRandGO);//[Random.Range(0,ghost.Length-1)];
        ghostPrefab.name = tempRandGO.name;
    }



}
