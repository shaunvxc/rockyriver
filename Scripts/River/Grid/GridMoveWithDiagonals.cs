using UnityEngine;
using System.Collections;

public class GridMoveWithDiagonals : MonoBehaviour
{

	public float moveSpeed = 5F;
	private float gridSize;
	
	private enum Orientation {
		Horizontal,
		Vertical
	};
	
	private Orientation gridOrientation = Orientation.Vertical;
	private bool allowDiagonals = true;
	private bool correctDiagonalSpeed = true;
	private Vector2 input;
	private bool isMoving = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	private float factor;
	
	public bool isEnemy;
	
	private float xAxisMovement = 0F;
	private float yAxisMovement = 0F;
	
	private float maxXBoundary;
	private Vector2 currentWaypoint;
	private bool halt = false;

	private float yBound;
	private bool movingDown = false;

	private Transform _transform;

	void Awake() {
		_transform = transform;
	}

	void Start() { 
		
		gridSize = PanchoRiver.Instance.TileSize * .5F;
		
	}
	
	public void Update() {
		
		if (!isMoving) {
			
						input = new Vector2 (xAxisMovement, yAxisMovement);
			
						if (!allowDiagonals) {
				
								if (Mathf.Abs (input.x) > Mathf.Abs (input.y)) {  
										input.y = 0;  //  then he is moving left or right
								} else {
										input.x = 0;  // then he is moving up or down
								}
						}
					
						if (input != Vector2.zero) {
								//StartCoroutine (move (transform));  // if the character is making a movement
								StartCoroutine("move", _transform);
						}
				}
	} 
	
	// draw the grid tomorrow night to help visualize it
	
	public IEnumerator move(Transform transform) {
		isMoving = true;
		startPosition = transform.position;
		
		startPosition.z = 0F;
		t = 0;
		
		
		if(gridOrientation == Orientation.Horizontal) {
			endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
			                          startPosition.y, startPosition.z + System.Math.Sign(input.y) * gridSize);
		} else {
			endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
			                          startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);
		}
		
		if(allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0) {
			factor = 0.7071f;                            // to allow for the diagonal movements
		} else { 
			factor = 1f;
		}
		
		
		endPosition.z = 0F;
		while (t < 1F) {
			
			t += Time.deltaTime * (moveSpeed/gridSize) * factor;

			transform.position = Vector2.Lerp(startPosition, endPosition, t);
			
			yield return null;
		}

		isMoving = false;
		yield return 0;
	}
	
	
	
	public void setWaypoint(Vector2 waypoint) {
		this.currentWaypoint = waypoint;
	}
	
	public void haltAllMovement() { 
		halt = true;
		yAxisMovement = 0F;
		haltXAxisMovement ();
		//haltYAxisMovement ();
	}

	public void	paddleLeftAndUp() { 
		xAxisMovement = -1F;
		yAxisMovement = .1F; // or .5?
	}

	public void paddleRightAndUp() { 
		xAxisMovement = 1F;
		yAxisMovement = .1F; // or .5?
	}

	public void paddleLeft() {
		xAxisMovement = -1F;
	}

	public void paddleRight() { 
		xAxisMovement = 1F;
	}
	
	public void moveLeft()  {
		xAxisMovement = -1F;
		maxXBoundary = transform.position.x - gridSize;
	}
	
	public void moveRight() { 
		xAxisMovement = 1F;
		maxXBoundary = transform.position.x + gridSize;
	}
	
	public void haltXAxisMovement() { 
		xAxisMovement = 0F;
	}
	
	public void moveDown() { 
		yAxisMovement = -1F;
		xAxisMovement = 0F;
	}

	public void moveDown(float yPos) {
		yAxisMovement = -1F;
		xAxisMovement = 0F;
		movingDown = true;
		yBound = yPos;
	}
	
	public void moveUp ()  {
		yAxisMovement = 1F;
	}
	
	public void haltYAxisMovement() { 
		StopCoroutine ("move");
		isMoving = false;
		yAxisMovement = 0F;
		halt = true;
	}
	

}

