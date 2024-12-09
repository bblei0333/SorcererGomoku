using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GomokuControl : MonoBehaviour
{
    private const int TILE_COUNT_X = 15;
    private const int TILE_COUNT_Y = 15;
    public GameObject offblack; // Black piece prefab
    public GameObject offwhite; // White piece prefab
    public GameObject offbomb;  // Bomb piece prefab
    public int[,] grinfo = new int[15, 15]; // Grid information storing piece states
    private GameObject[,] tiles; // Array to store tile objects
    private Camera currentCamera; // Camera reference for raycasting
    private Vector2Int currentHover; // Current tile under mouse cursor
    private Vector3 bounds; // Grid bounds for positioning tiles
    [SerializeField] private Material tileMaterial; // Material for normal tiles
    [SerializeField] private Material hoverMaterial; // Material for hovered tiles
    [SerializeField] private Material BombHover; // Material for hovered tiles
    [SerializeField] private float tileSize = 0.05f; // Size of each tile
    [SerializeField] private float yOffset = 0.2f; // Y offset for tile height
    [SerializeField] private Vector3 boardCenter = new Vector3(-1.333f, 0, -1.333f); // Board center position

    private int clientID {get;}
    void Start(){
        // Debugging and initialization of components
        Debug.Log("Client ID: " + clientID);
    }

    private void Awake(){
        // Set up the grid on game start
        GenerateAlltiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);
    }

    private Vector3 GetTileCenter(int x, int y){
        // Calculate and return the center position of a tile
        return new Vector3(x * tileSize, 0.55f, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }

    public void SyncGrid(){
        // Sync the grid by destroying old pieces and instantiating new ones based on byte data
        GameObject[] killList = GameObject.FindGameObjectsWithTag("PiecesToKill");
        foreach (GameObject obj in killList){
            Destroy(obj); // Destroy previous pieces
        }

        // Instantiate pieces based on the synced grid data (bytes)
        for (int x = 0; x < 225; x++){
            int xcord = x % 15;
            int ycord = x / 15;
            byte state = GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[x];

            // Instantiate corresponding pieces based on the byte value
            if (state == 1){
                Instantiate(offblack, GetTileCenter(xcord, ycord), Quaternion.identity);
            } else if (state == 2){
                Instantiate(offwhite, GetTileCenter(xcord, ycord), Quaternion.identity);
            } else if (state == 3){
                GameObject thingToDie = Instantiate(offbomb, GetTileCenter(xcord, ycord), Quaternion.identity);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord , ycord, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord -1 , ycord, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord +1, ycord, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord , ycord + 1, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord , ycord - 1, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord - 1, ycord - 1, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord + 1, ycord - 1, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord - 1, ycord + 1, 0);
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord + 1, ycord + 1, 0);
                SyncGrid();
            }
        }
    }

    private void Update(){
        // Ensure the camera reference is set up
        if (!currentCamera){
            currentCamera = Camera.main;
            return;
        }

        // Handle raycasting and tile hover logic
        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover"))){
            Vector2Int hitPosition = LookUpTileIndex(info.transform.gameObject);

            // Handle tile hover
            if (currentHover == -Vector2Int.one){
                currentHover = hitPosition;
                if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID != 2){
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = hoverMaterial;}
                else{
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = BombHover;
                    
                }
            }
            else{
                // Revert the old hover tile and apply hover to the new tile
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                tiles[currentHover.x, currentHover.y].GetComponent<MeshRenderer>().material = tileMaterial;
                currentHover = hitPosition;
                if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID != 2){
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = hoverMaterial;}
                else{
                tiles[hitPosition.x, hitPosition.y].transform.localScale = new Vector3(3f, 1f, 3f);
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = BombHover;
                }
            }
        }
        else{
            // If no tile is hovered, reset hover state
            if (currentHover != -Vector2Int.one){
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                tiles[currentHover.x, currentHover.y].GetComponent<MeshRenderer>().material = tileMaterial;
                currentHover = -Vector2Int.one;
            }
        }

        // Handle piece placement logic when left-click is pressed
        if (Input.GetMouseButtonDown(0)){
            Debug.Log("Turn: " + GameObject.Find("Normy").GetComponent<IntSync>().gaga);

            if(GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(currentHover.x , currentHover.y) && 
               GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga){

                // Handle piece placement based on the next piece ID (black, white, bomb)
                int pieceID = GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID;
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, pieceID + 1);

                Debug.Log("Sending piece placement at: " + currentHover);
                BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
                GameObject.Find("Normy").GetComponent<IntSync>().Turn();
                GameObject.Find("Normy").GetComponent<PlaySync>().Play();
                SyncGrid(); // Sync the grid after placing a piece
            }
            CheckForWin(1); // Check for a win condition for player 1 (black)
            CheckForWin(2);
        }

        // Right-click to check if a tile is filled or empty
        if (Input.GetMouseButtonDown(1)){
            if(GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(currentHover.x , currentHover.y)){
                Debug.Log("Empty at: " + currentHover);
            }
            else{
                Debug.Log("Filled at: " + currentHover);
            }
        }
    }

    private void GenerateAlltiles(float tileSize, int tileCountX, int tileCountY){
        // Generate all tiles for the grid
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + new Vector3(0.3575f, 0, 0.3575f);
        tiles = new GameObject[tileCountX, tileCountY];

        for (int x = 0; x < tileCountX; x++){
            for (int y = 0; y < tileCountY; y++){
                tiles[x, y] = GenerateSingleTile(tileSize, x, y); // Generate each individual tile
            }
        }
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y){
        // Create a single tile object at the given position
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y + 1) * tileSize) - bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, yOffset, y * tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

        int[] tris = new int[]{0, 1, 2, 1, 3, 2};
        mesh.vertices = vertices;
        mesh.triangles = tris;

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject; // Return the created tile
    }

    private Vector2Int LookUpTileIndex(GameObject hitInfo){
        // Look up the tile index based on the hit game object
        for (int x = 0; x < TILE_COUNT_X; x++){
            for (int y = 0; y < TILE_COUNT_Y; y++){
                if (tiles[x, y] == hitInfo){
                    return new Vector2Int(x, y); // Return the tile position
                }
            }
        }
        return -Vector2Int.one; // Return invalid position if not found
    }

    
    private int coordToInt(int x, int y){
        return y*15+x;
    }

    public void CheckForWin(int player){
        // Check for a winning condition for the given player (1 = player 1, 2 = player 2)
        /*
        for (int row = 0; row < 15; row++){
            for (int col = 0; col < 15; col++){
                if (grinfo[row, col] == player){
                    // Check horizontal, vertical, and diagonal lines for 5 consecutive pieces
                    if (col < 11 && grinfo[row, col + 1] == player && grinfo[row, col + 2] == player &&
                        grinfo[row, col + 3] == player && grinfo[row, col + 4] == player){
                        Debug.Log(player + " wins!");
                    }
                    if (row < 11 && grinfo[row + 1, col] == player && grinfo[row + 2, col] == player &&
                        grinfo[row + 3, col] == player && grinfo[row + 4, col] == player){
                        Debug.Log(player + " wins!");
                    }
                    if (row < 11 && col < 11 && grinfo[row + 1, col + 1] == player && grinfo[row + 2, col + 2] == player &&
                        grinfo[row + 3, col + 3] == player && grinfo[row + 4, col + 4] == player){
                        Debug.Log(player + " wins!");
                    }
                    if (row < 11 && col > 3 && grinfo[row + 1, col - 1] == player && grinfo[row + 2, col - 2] == player &&
                        grinfo[row + 3, col - 3] == player && grinfo[row + 4, col - 4] == player){
                        Debug.Log(player + " wins!");
                    }
                }
            }
        }
    }
    */
        for (int spaceInt = 0; spaceInt < 225; spaceInt++)
        {
            if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[spaceInt] == (byte)player){
                int xCoord = spaceInt % 15;
                int yCoord = spaceInt / 15;
                if (yCoord < 11 && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord, yCoord+1)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord, yCoord+2)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord, yCoord+3)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord, yCoord+4)] == (byte)player){
                    Debug.Log(player + "wins");
                }
                if (xCoord < 11 && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+1, yCoord)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+2, yCoord)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+3, yCoord)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+4, yCoord)] == (byte)player){
                    Debug.Log(player + "wins");
                }
                if (xCoord < 11 && yCoord < 11 && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+1, yCoord+1)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+2, yCoord+2)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+3, yCoord+3)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+4, yCoord+4)] == (byte)player){
                    Debug.Log(player + "wins");
                }
                if (xCoord < 11 && yCoord > 3 && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+1, yCoord-1)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+2, yCoord-2)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+3, yCoord-3)] == (byte)player && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xCoord+4, yCoord-4)] == (byte)player){
                    Debug.Log(player + "wins");
                }
                
            }
        }
    }
}
