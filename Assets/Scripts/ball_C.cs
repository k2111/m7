using UnityEngine;
using System.Collections;

public class ball_C : MonoBehaviour {
	public Vector3 myDestination;	
	public int myGridX, myGridY;	
	public bool isMoving;	
	private Color myColor;	
	private bool isSelected;	
	private game myController;	
	private GameObject label;	
	

	void Start () {
		myController = GameObject.Find ("Main Camera").GetComponent<game>();
		label = new GameObject();
		}

	void Update() {
		Camera gameCam = GameObject.Find ("Main Camera").GetComponent<Camera>();
		label.transform.position = gameCam.WorldToViewportPoint(this.transform.position);
	}

	public void setColor(Color newColor) {
		myColor = newColor;
		this.GetComponent<Renderer>().material.color = myColor;

	}

	public Color getColor() {
		return myColor;
	}
	

	public void OnMouseDown() {
		if (!isSelected) {
			isSelected = true;
		}
		else {
			isSelected = false;
			isMoving = false;
		}
			myController.selected(this.gameObject, isSelected);
	}
		
	public bool isArrived ()
	{
		bool returnVal = false;
		if (this.transform.position == myDestination)
			returnVal = true;
		return returnVal;
	}
}