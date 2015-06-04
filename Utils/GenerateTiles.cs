using UnityEngine;
using System.Collections;

public class GenerateTiles : MonoBehaviour {	
	
	public int board_size_x_;
	public int board_size_z_;
	public Transform tile_prefab_;
	
	// Use this for initialization
	void Start () {

		GameObject board = new GameObject();
		board.name = "Board";


		int startX = (board_size_x_ / 2) * -1;
		int endX = startX * -1;
		int startY = (board_size_z_ / 2) * -1;
		int endY = startY * -1;


		for ( int x = startX; x < endX; x++ ) {
			for ( int z = startY; z < endY; z++ ) {
				Transform tile = (Transform)Instantiate(tile_prefab_,new Vector2(x,z),Quaternion.identity);
				tile.name = "Tile" + x + z;
				tile.parent = board.transform;
			}
		}
	}
}