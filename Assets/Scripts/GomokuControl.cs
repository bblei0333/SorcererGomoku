using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GomokuControl : MonoBehaviour
{
    private const int TILE_COUNT_X = 15;
    private const int TILE_COUNT_Y = 15;

    public GameObject bp;
    public GameObject wp;
    public int[,] grinfo = new int[15,15];
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;
    [SerializeField] private Material tileMaterial;
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private float tileSize = 0.05f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = new Vector3(-1.333f, 0, -1.333f);
    void PiecePlaced(){
        //teehee
    }

    private int clientID {get;}

    void Start(){
        Debug.Log(clientID);
    }

    private void Awake(){
        GenerateAlltiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);
    }
    private Vector3 GetTileCenter(int x, int y){
        return new Vector3(x * tileSize, 0.55f, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }
    public void SyncGrid(){
        for(int x = 0; x<225; x++){
            int xcord = x % 15;
            int ycord = x / 15;
            if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[x] == (byte)1){
                GameObject.Find("Normy").GetComponent<Spawner>().doSpawn("BpNorm", GetTileCenter(xcord, ycord), Quaternion.identity);
                Debug.Log("black");
            }
            if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[x] == (byte)2){
                GameObject.Find("Normy").GetComponent<Spawner>().doSpawn("WpNorm", GetTileCenter(xcord, ycord), Quaternion.identity);
                Debug.Log("white");
            }
            if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[x] == (byte)3){
                GameObject.Find("Normy").GetComponent<Spawner>().doSpawn("BoNorm", GetTileCenter(xcord, ycord), Quaternion.identity);
                Debug.Log("bomb");
            }
        }
    }
    private void Update(){
        if(!currentCamera){
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile" , "Hover"))){
            Vector2Int hitPosition = LookUpTileIndex(info.transform.gameObject);

            if(currentHover == -Vector2Int.one){
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = hoverMaterial;
            }
            if(currentHover != -Vector2Int.one){
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                tiles[currentHover.x, currentHover.y].GetComponent<MeshRenderer>().material = tileMaterial;
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = hoverMaterial;
            }
        }
        else{
            if(currentHover != -Vector2Int.one){
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                tiles[currentHover.x, currentHover.y].GetComponent<MeshRenderer>().material = tileMaterial;
                currentHover = -Vector2Int.one;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(GameObject.Find("Normy").GetComponent<Spawner>().ID);
            Debug.Log("Turn: " + GameObject.Find("Normy").GetComponent<IntSync>().gaga);
            if(GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(currentHover.x , currentHover.y) && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga){
                //GameObject.Find("Normy").GetComponent<Spawner>().doSpawn(1, GetTileCenter(currentHover.x, currentHover.y), Quaternion.identity);
                //grinfo[currentHover.x , currentHover.y] = 1;
                if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 0){
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 1);
                }
                if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 1){
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 2);
                }
                if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 2){
                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 3);
                }
                Debug.Log("Sending it");
                Debug.Log("X: " + currentHover.x + "Y: " + currentHover.y);
                BroadcastMessage("PiecePlaced");
                SyncGrid();
            }
            //Debug.Log(currentHover + " " + grinfo[currentHover.x , currentHover.y]);
            CheckForWin(1);
        }
        if(Input.GetMouseButtonDown(1)){
            if(GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(currentHover.x , currentHover.y)){
                Debug.Log("Empty");
                Debug.Log(currentHover);
            }
            else{
                Debug.Log("Filled");
            }
        }
    }
    private void GenerateAlltiles(float tileSize, int tileCountX, int tileCountY){

        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + new Vector3(0.3575f, 0, 0.3575f);

        tiles = new GameObject[tileCountX,tileCountY];
        for( int x = 0; x<tileCountX; x++){
            for(int y = 0; y<tileCountY; y++){
                tiles[x,y] = GenerateSingleTile(tileSize, x, y);
            }
        }

    }
    private GameObject GenerateSingleTile(float tileSize, int x, int y){
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds; 
        vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize)- bounds; 
        vertices[2] = new Vector3((x+1) * tileSize, yOffset, y * tileSize)- bounds; 
        vertices[3] = new Vector3((x+1) * tileSize, yOffset, (y+1) * tileSize)- bounds; 

        int[] tris = new int[]{0,1,2,1,3,2};
        mesh.vertices = vertices;
        mesh.triangles = tris;

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }
    private Vector2Int LookUpTileIndex(GameObject hitInfo){
        for (int x = 0; x < TILE_COUNT_X; x++){
            for (int y = 0; y< TILE_COUNT_Y; y++){
                if(tiles[x,y] == hitInfo){
                    return new Vector2Int(x,y);
                }
            }
        }
        return -Vector2Int.one;
    }
    private void CheckForWin(int player){
        for(int row = 0; row < 15; row++){
            for(int col = 0; col < 15; col++){
                if(grinfo[row,col] == player){
                if(col < 11){
                    if(grinfo[row, col + 1] == player && grinfo[row, col + 2] == player && grinfo[row, col + 3] == player && grinfo[row, col + 4] == player){
                        Debug.Log(player + "win");
                    }
                }
                if(row < 11){
                    if(grinfo[row+ 1, col ] == player && grinfo[row + 2, col] == player && grinfo[row + 3, col] == player && grinfo[row + 4, col] == player){
                        Debug.Log(player + "win");
                    }
                }
                if(row < 11 && col < 11){
                    if(grinfo[row + 1, col + 1] == player && grinfo[row + 2, col + 2] == player && grinfo[row + 3, col + 3] == player && grinfo[row + 4, col + 4] == player){
                        Debug.Log(player + "win");
                    }
                }
                if(row < 11 && col > 3){
                    if(grinfo[row + 1, col - 1] == player && grinfo[row + 2, col - 2] == player && grinfo[row + 3, col - 3] == player && grinfo[row + 4, col - 4] == player){
                        Debug.Log(player + "win");
                    }
                }
            }
            }
        }
    }
}
