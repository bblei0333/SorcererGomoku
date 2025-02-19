using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class GomokuControl : MonoBehaviour
{
   private const int TILE_COUNT_X = 15;
   private const int TILE_COUNT_Y = 15;
   private int frameCounter;
   public bool blackwin1done,blackwin2done,blackwin3done,whitewin1done,whitewin2done,whitewin3done, disabledplay, gamertime;
   public GameObject offblack, offwhite, offbomb, stone, share, doubleAgent, sniper, bombhover, bomby, bt, wt, bw, ww, GameMat, mystery, physicsW, physicsB;
   public int[,] grinfo = new int[15, 15]; // Grid information storing piece states
   private GameObject[,] tiles; // Array to store tile objects
   private Camera currentCamera; // Camera reference for raycasting
   private Vector2Int currentHover; // Current tile under mouse cursor
   private Vector3 bounds; // Grid bounds for positioning tiles
   public Color endColor = Color.red;
   [SerializeField] private Material tileMaterial; // Material for normal tiles
   [SerializeField] private Material hoverMaterial; // Material for hovered tiles
   [SerializeField] private float tileSize = 0.05f; // Size of each tile
   [SerializeField] private float yOffset = 0.2f; // Y offset for tile height
   [SerializeField] private Vector3 boardCenter = new Vector3(-1.333f, 0, -1.333f); // Board center position
    public float radius = 5.0F;
    public float power = 10.0F;


   Random rnd = new Random();
       private int clientID {get;}
   void Start(){
       // Debugging and initialization of components
       MeshRenderer renderer = GameMat.GetComponent<MeshRenderer>();
       Debug.Log("Client ID: " + clientID);
   }

   IEnumerator BlackWinScreen(){
    disabledplay = true;
    GameObject yayeey = Instantiate(bw, new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(90, 0, 0)));
    GameObject.Find("GomokuBoard").GetComponent<PiecePool>().realStart();
    yield return new WaitForSeconds(4f);
    Destroy(yayeey);
    disabledplay = false;
   }
   IEnumerator WhiteWinScreen(){
    disabledplay = true;
    GameObject yayeey = Instantiate(ww, new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(90, 0, 0)));
    GameObject.Find("GomokuBoard").GetComponent<PiecePool>().realStart();
    yield return new WaitForSeconds(4f);
    Destroy(yayeey);
    disabledplay = false;
   }


   private void Awake(){
       // Set up the grid on game start
       GenerateAlltiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);
   }


   private Vector3 GetTileCenter(int x, int y){
       // Calculate and return the center position of a tile
       return new Vector3(x * tileSize, 0.55f, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
   }

   public void SyncWin(){
    if(GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 1 && !blackwin1done){
        Instantiate(bt, new Vector3(-2.87f, 0.29f, -5.13f), Quaternion.Euler(new Vector3(-90, 0, 0)));
        blackwin1done = true;
        Debug.Log("1 Black win");
        StartCoroutine(BlackWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 2 && !blackwin2done){
        Instantiate(bt, new Vector3(-1.42f, 0.29f, -5.13f), Quaternion.Euler(new Vector3(-90, 0, 0)));
        blackwin2done = true;
        StartCoroutine(BlackWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 3 && !blackwin3done){
        Instantiate(bt, new Vector3(-.008f, 0.29f, -5.13f), Quaternion.Euler(new Vector3(-90, 0, 0)));
        blackwin3done = true;
        StartCoroutine(BlackWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 1 && !whitewin1done){
        Instantiate(wt, new Vector3(2.87f, 0.29f, -5.13f), Quaternion.Euler(new Vector3(-90, 0, 0)));
        whitewin1done = true;
        StartCoroutine(WhiteWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 2 && !whitewin2done){
        Instantiate(wt, new Vector3(1.42f, 0.29f, -5.13f), Quaternion.Euler(new Vector3(-90, 0, 0)));
        whitewin2done = true;
        StartCoroutine(WhiteWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 3 && !whitewin3done){
        Instantiate(wt, new Vector3(-.008f, 0.29f, -5.13f), Quaternion.Euler(new Vector3(-90, 0, 0)));
        whitewin3done = true;
        StartCoroutine(WhiteWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 2 && GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 2 && !gamertime){
        Debug.Log("muhmuhmuh");
        gamertime = true;
       if(GameMat.GetComponent<Renderer>() != null){
        GameMat.GetComponent<Renderer>().material.color = endColor;
       }
     }
   }
   
   public void ClearBoard(){
    for (int x = 0; x < 15; x++){
        for(int y = 0; y < 15; y++){
            byte state = GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[(y * 15 + x)];
            if(state != 4){
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(x, y, 0);
            }
        }
    }
    SyncGrid();
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
               for (int b = -1; b < 2; b++)
               {
                for (int y = -1; y < 2; y++){
                    if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(xcord + b, ycord + y)] != (byte)4){
                    GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(xcord + b, ycord + y, 0);
                    }
                }
               }
               SyncGrid();
              
           } else if (state == 4){
               Instantiate(stone, GetTileCenter(xcord, ycord), Quaternion.identity);
           } else if (state == 5){
               Instantiate(share, GetTileCenter(xcord, ycord), Quaternion.identity);
           } else if (state == 6){
               //Instantiate(doubleAgent, GetTileCenter(xcord, ycord), Quaternion.identity);
           } else if (state == 7){
               //Instantiate(sniper, GetTileCenter(xcord, ycord), Quaternion.identity);
           } else if (state == 8){
            //Mystery
           } else if (state == 20){
                Instantiate(physicsW, GetTileCenter(xcord, ycord), Quaternion.identity);
           }
           SyncWin();
       }
   }
   

   private void Update(){
       // Ensure the camera reference is set up
       frameCounter++;

        // Check if the counter is divisible by 30
        if (frameCounter >= 30)
        {
            // Trigger your action here
            SyncGrid();
            SyncWin();

            // Reset the frame counter to 0 after the action is triggered
            frameCounter = 0;
        }
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
               if(bomby == null){
               bomby = Instantiate(bombhover, GetTileCenter(hitPosition.x, hitPosition.y), Quaternion.identity);
               }
                  
               }
           }
           else{
               // Revert the old hover tile and apply hover to the new tile
               Destroy(bomby);
               tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
               tiles[currentHover.x, currentHover.y].GetComponent<MeshRenderer>().material = tileMaterial;
               currentHover = hitPosition;
               if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID != 2){
               tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
               tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = hoverMaterial;}
               else{
               tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
               bomby = Instantiate(bombhover, GetTileCenter(hitPosition.x, hitPosition.y), Quaternion.identity);
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
       if (Input.GetKeyDown(KeyCode.Space)){
           GameObject.Find("GomokuBoard").GetComponent<PiecePool>().doHold();
       }
       // Handle piece placement logic when left-click is pressed
       if (Input.GetMouseButtonDown(0)){
           int bombTriggered = 0;
           Debug.Log("Turn: " + GameObject.Find("Normy").GetComponent<IntSync>().gaga);
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 2 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
               // If next piece is bomb and it is clients turn
               int pieceID = GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID;
               GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, pieceID + 1);
               for (int b = -1; b < 2; b++)
               {
                for (int y = -1; y < 2; y++){
                    if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] != (byte)4 && (GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] == (byte)1)){
                        Instantiate(physicsB, GetTileCenter(currentHover.x + b, currentHover.y + y), Quaternion.identity);
                    }
                    if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] != (byte)4 && (GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] == (byte)2)){
                        Instantiate(physicsW, GetTileCenter(currentHover.x + b, currentHover.y + y), Quaternion.identity);
                    }

                }
               }
            
               
            Vector3 explosionPos = GetTileCenter(currentHover.x, currentHover.y );
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null){
                    rb.AddExplosionForce(power, explosionPos, radius, 0.5F);
                }
            }
            //GameObject.Find("GomokuBoard").GetComponent<ExampleClass>().Boomer(GetTileCenter(currentHover.x, currentHover.y));

             
               bombTriggered = 1;
              
           }
           //if next piece is a stone and it is clients turn
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 3 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
               int stonesPlaced = 0;
               while(stonesPlaced != 2){
                   int rndSpot = rnd.Next(0, 225);
                   int rndXCoord = rndSpot % 15;
                   int rndYCoord = rndSpot / 15;
                   if(!GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(rndXCoord, rndYCoord) && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[rndSpot] != (byte)4){
                       GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(rndXCoord, rndYCoord, 4);
                       stonesPlaced++;
                   }
               }
           }
           int agentTriggered = 0;
           int petrifyTriggered = 0;
           //if next piece is a doubleAgent and it is the clients turn
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 5 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
               //checks if clicked space == black or share and client == white
               Debug.Log("Double clicked");
               if((GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)1 || GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)5) && GameObject.Find("Normy").GetComponent<Spawner>().ID == 1){
                   GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 2);
                   agentTriggered = 1;
                   Debug.Log("Double placed");
                  
               }
               //checks if clicked space == white or share and client == black
               else if((GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)2 || GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)5) && GameObject.Find("Normy").GetComponent<Spawner>().ID == 0){
                   GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 1);
                   agentTriggered = 1;
                   Debug.Log("Double placed");
               }
              
           }
           //if next piece is a petrify and it is the clients turn
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 6 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
               if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)1 || GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)5 || GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)2){
                   GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 4);
                   petrifyTriggered = 1;
               }
              


           }
           //if clicked space is empty and it is clients turn
           if(GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(currentHover.x , currentHover.y) &&
              GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){


               // Handle piece placement based on the next piece ID (black, white, bomb)
               int pieceID = GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID;
               if(pieceID != 3 && pieceID != 5 && pieceID != 6 && pieceID != 7){
                   GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, pieceID + 1);
               }

               if(pieceID == 7){
                    int randMystery = rnd.Next(0,4);
                    if(randMystery == 0){
                        GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 1);
                    }
                    else if(randMystery == 1){
                        GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 2);
                    }
                    else if(randMystery == 2){
                        GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 4);
                    }
                    else{
                        GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 5);
                    }
            

               }

               if(pieceID != 5 && pieceID != 6){
                   Debug.Log("Sending piece placement at: " + currentHover);
                   BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
                   GameObject.Find("Normy").GetComponent<IntSync>().Turn();
                   GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               }
      
               SyncGrid(); // Sync the grid after placing a piece
           }
           if(bombTriggered == 1){
               GameObject.Find("Normy").GetComponent<IntSync>().Turn();
               GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               Debug.Log("Sending piece placement at: " + currentHover);
               BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
           }
           if(agentTriggered == 1){
               GameObject.Find("Normy").GetComponent<IntSync>().Turn();
               GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               Debug.Log("Sending piece placement at: " + currentHover);
               BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
           }
           if(petrifyTriggered == 1){
               GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               GameObject.Find("Normy").GetComponent<IntSync>().Turn();
              
               Debug.Log("Sending piece placement at: " + currentHover);
               BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
           }
           SyncGrid();
           CheckForWin(1); // Check for a win condition for player 1 (black) on client side
           CheckForWin(2); // Check for a win condition for player 2 (white) on client side
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


  
    public void CheckForWin(int player)
   {
       var byteSync = GameObject.Find("Normy").GetComponent<ByteSync>();
       var bytes = byteSync._model.bytes;


       // Helper function to check for a win in a specific direction
       bool CheckDirection(int x, int y, int dx, int dy)
       {
           for (int i = 1; i <= 4; i++)
           {
               int nx = x + i * dx;
               int ny = y + i * dy;
               if (nx < 0 || nx >= 15 || ny < 0 || ny >= 15 ||
                   (bytes[coordToInt(nx, ny)] != (byte)player && bytes[coordToInt(nx, ny)] != (byte)5))
               {
                   return false;
               }
           }
           return true;
       }


       for (int spaceInt = 0; spaceInt < 225; spaceInt++)
       {
           if (bytes[spaceInt] == (byte)player || bytes[spaceInt] == (byte)5)
           {
               int xCoord = spaceInt % 15;
               int yCoord = spaceInt / 15;


               // Check in all directions: vertical, horizontal, diagonal-down, diagonal-up
               if ((yCoord < 11 && CheckDirection(xCoord, yCoord, 0, 1)) ||       // Vertical
                   (xCoord < 11 && CheckDirection(xCoord, yCoord, 1, 0)) ||       // Horizontal
                   (xCoord < 11 && yCoord < 11 && CheckDirection(xCoord, yCoord, 1, 1)) || // Diagonal-down
                   (xCoord < 11 && yCoord > 3 && CheckDirection(xCoord, yCoord, 1, -1)))   // Diagonal-up
               {
                   if(player == 1){
                        GameObject.Find("Normy").GetComponent<IntSync>().BlackWin();    
                   }
                   if(player == 2){

                        GameObject.Find("Normy").GetComponent<IntSync>().WhiteWin();
                   }
                   Debug.Log(player + " wins");
                   SyncGrid();
                   ClearBoard();
                   return; // Exit early after finding a win
               }
           }
       }
   }
}