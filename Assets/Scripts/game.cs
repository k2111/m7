using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class game : MonoBehaviour {
	private int numRows = 10;  
	private int numCols = 10;  
	private GameObject[,] pieces;	
	private Color[] pieceColors;		
    private GameObject[] selectedPieces;	
	private int numSelected;	
	private bool weSwitched;
	private ArrayList matchedPieces;
    private int score = 0;
	public Text scoreText;


	void Start () {
		pieces = new GameObject[numRows, numCols];
		pieceColors = new Color[5] {Color.red, Color.blue, Color.yellow, Color.green, new Color(0.75f, 0.35f, 0.0f)};  //0.75f, 0.35f, 0.15f
		selectedPieces = new GameObject[2];
		matchedPieces = new ArrayList();

		for (int i = 0; i < numRows; i=i+1) {
			for (int j = 0; j < numCols; j=j+1) {
				pieces[i,j] = Instantiate(Resources.Load ("Ball", typeof(GameObject))) as GameObject;
				pieces[i,j].transform.position = new Vector3(i*1.2f-3, j*-1.2f+3, 0);
				pieces[i, j].GetComponent<ball_C>().myGridX = i;
				pieces[i, j].GetComponent<ball_C>().myGridY = j;
			}
		}
		while (isMatching ()) 
			swapColors ();
		matchedPieces.Clear (); 
		weSwitched = false; 
	}

	void Update () {
		if (numSelected == 2) {
			if (!weSwitched) {
				switchPieces();
			}
			if (selectedPieces[0].GetComponent<ball_C>().isArrived () &&
			       selectedPieces[1].GetComponent<ball_C>().isArrived ()) {
				if (isMatching ()){
					weSwitched = true;
				}
				if (matchedPieces.Count < 3) {
					if (!weSwitched) {
						returnPieces();
					}
					selectedPieces[1].GetComponent<ball_C>().OnMouseDown ();
					selectedPieces[0].GetComponent<ball_C>().OnMouseDown ();
					matchedPieces.Clear();
					weSwitched = false;
				}
				else {
					
					swapColors();
					matchedPieces.Clear(); 
				}
			}
		}
	}
	

	private bool isMatching() {
		bool returnVal = false;
		for(int i = 0; i < numRows; i++ ) {
			for (int j = 0; j < numCols -2; j++) {
				
				Color col1 = pieces[i,j].GetComponent<ball_C>().getColor ();
				Color col2 = pieces[i,j+1].GetComponent<ball_C>().getColor ();
				Color col3 = pieces[i,j+2].GetComponent<ball_C>().getColor ();

				if (col1 == col2 && col1 == col3) {
					returnVal = true;
					matchedPieces.Add (pieces[i,j]);
					matchedPieces.Add (pieces[i,j+1]);
					matchedPieces.Add (pieces[i,j+2]);
					int offset = j+3;
					j = j+2;
					while (offset < numCols) {
						col2 = pieces[i,offset].GetComponent<ball_C>().getColor ();
						if (col1 == col2) {
							matchedPieces.Add (pieces[i, offset]);
							j = offset;
							offset++;	
						}
						else {
							break;
						}
					}
				}
			}
		}

		for(int i = 0; i < numRows; i++ ) {
			for (int j = 0; j < numCols -2; j++) {
				//grab the colors of first three pieces
				Color col1 = pieces[j,i].GetComponent<ball_C>().getColor ();
				Color col2 = pieces[j+1,i].GetComponent<ball_C>().getColor ();
				Color col3 = pieces[j+2,i].GetComponent<ball_C>().getColor ();

				if (col1 == col2 && col1 == col3) {

					returnVal = true;
					matchedPieces.Add (pieces[j,i]);
					matchedPieces.Add (pieces[j+1,i]);
					matchedPieces.Add (pieces[j+2,i]);
					int offset = j+3;  
					j = j+2; 
					while (offset < numCols) {
						col2 = pieces[offset,i].GetComponent<ball_C>().getColor ();
						if (col1 == col2) {
							matchedPieces.Add (pieces[offset,i]);
							j = offset;
							offset++;	
						}
						else {
							break;
						}
					}
				}
			}
		}

		return returnVal;
	}
	

	void swapColors() {

		GameObject[] tempPieces = new GameObject[matchedPieces.Count];
		matchedPieces.CopyTo (tempPieces);
		if (matchedPieces.Count == 3) {
			score = score + 100;
		}
		if (matchedPieces.Count == 4) {
			score = score + 300;
		}
		if (matchedPieces.Count == 5) {
			score = score + 1000;
		}
		if (matchedPieces.Count == 6) {
			score = score + 1500;
		}
		if (matchedPieces.Count == 7) {
			score = score + 2000;
		}
		if (matchedPieces.Count == 8) {
			score = score + 2500;
		}
		if (matchedPieces.Count == 9) {
			score = score + 3000;
		}
		if (matchedPieces.Count == 10) {
			score = score + 3500;
		}
		scoreText.text = "Score: " + score;
		print (matchedPieces.Count + " balls in line!   " + "Score is " + score);
		for (int i = 0; i < matchedPieces.Count; i++) {
		}
		for (int i = 0; i < matchedPieces.Count; i++) {
			tempPieces [i].GetComponent<ball_C> ().setColor (pieceColors [Random.Range (0, 5)]);

		}
	}

	void switchPieces() {
		if (!selectedPieces[0].GetComponent<ball_C>().isMoving &&
		    !selectedPieces[1].GetComponent<ball_C>().isMoving) 
		{
			selectedPieces[0].GetComponent<ball_C>().myDestination = selectedPieces[1].transform.position;
			selectedPieces[1].GetComponent<ball_C>().myDestination = selectedPieces[0].transform.position;

			selectedPieces[0].GetComponent<ball_C>().isMoving = true;
			selectedPieces[1].GetComponent<ball_C>().isMoving = true;
		}
			
		selectedPieces [0].transform.position = Vector3.MoveTowards (selectedPieces [0].transform.position, selectedPieces [0].GetComponent<ball_C> ().myDestination, 1f * Time.deltaTime);
		selectedPieces [1].transform.position = Vector3.MoveTowards (selectedPieces [1].transform.position, selectedPieces [1].GetComponent<ball_C> ().myDestination, 1f * Time.deltaTime);

		if (selectedPieces[0].GetComponent<ball_C>().isArrived () &&
		       selectedPieces[1].GetComponent<ball_C>().isArrived ()) 
		{   
			int i1 = selectedPieces [0].GetComponent<ball_C> ().myGridX;
			int j1 = selectedPieces [0].GetComponent<ball_C> ().myGridY;
			int i2 = selectedPieces [1].GetComponent<ball_C> ().myGridX;
			int j2 = selectedPieces [1].GetComponent<ball_C> ().myGridY;
			pieces [i1, j1] = selectedPieces [1];
			pieces [i2, j2] = selectedPieces [0];
			selectedPieces [0].GetComponent<ball_C> ().myGridX = i2;
			selectedPieces [0].GetComponent<ball_C> ().myGridY = j2;
			selectedPieces [1].GetComponent<ball_C> ().myGridX = i1;
			selectedPieces [1].GetComponent<ball_C> ().myGridY = j1;
		}
	}

	void returnPieces() {
		Color color0 = selectedPieces[0].GetComponent<ball_C>().getColor ();
		Color color1 = selectedPieces[1].GetComponent<ball_C>().getColor ();

		selectedPieces[0].GetComponent<ball_C>().setColor(color1);
		selectedPieces[1].GetComponent<ball_C>().setColor(color0);
	}

	public void selected(GameObject piece, bool addPiece) {
		if (addPiece) {
			selectedPieces[numSelected] = piece;
			numSelected++;
		}
		else {
			numSelected--;
			selectedPieces[numSelected] = null;
		}
	}
}