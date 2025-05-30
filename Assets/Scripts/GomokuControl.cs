using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
using Normal.Realtime;
using UnityEngine.EventSystems;
public class GomokuControl : MonoBehaviour
{
   private const int TILE_COUNT_X = 15;
   private const int TILE_COUNT_Y = 15;
   private int frameCounter, TempCoordx, TempCoordy, TempID, LastPlayerID;
   public bool blackwin1done,blackwin2done,blackwin3done,whitewin1done,whitewin2done,whitewin3done, disabledplay, gamertime, flipyet, placeyet, shitlin;
   public GameObject offblack, offwhite, offbomb, stone, share, doubleAgent, sniper, bombhover, bomby, bt, wt, bw, ww, GameMat, mystery, physicsW, physicsB, physicsS, BetterHelpUI, HelpMenu, LPPHighlight, ExplosionParticle, ExplosionLight, grey1, grey2, grey3, grey4, grey5, black1, black2, black3, white3, white4, white5, helper, fire1, fire2, fire3, fire4;
   public int[,] grinfo = new int[15, 15]; // Grid information storing piece states
   public int Delayer;
   private GameObject[,] tiles; // Array to store tile objects
   private Camera currentCamera; // Camera reference for raycasting
   private Vector2Int currentHover; // Current tile under mouse cursor
   private Vector3 bounds; // Grid bounds for positioning tiles
   public Color endColor = Color.black;
   [SerializeField] private Material tileMaterial; // Material for normal tiles
   [SerializeField] private Material hoverMaterial; // Material for hovered tiles
   [SerializeField] private float tileSize = 0.05f; // Size of each tile
   [SerializeField] private float yOffset = 0.2f; // Y offset for tile height
   [SerializeField] private Vector3 boardCenter = new Vector3(-1.333f, 0, -1.333f); // Board center position
    public float radius = 5.0F;
    public float power = 10.0F;

    public int LPPID;
    public int LPPx;
    public int LPPy;
    public bool OngoingAnimation = false;
    public bool OncomingAnimation = false;

    public bool Booming = false;
    public bool showHelp = false;
    public bool createdMenu = false;
    public CameraShake cameraShake;
    


   Random rnd = new Random();
       private int clientID {get;}
   void Start(){
       // Debugging and initialization of components
       MeshRenderer renderer = GameMat.GetComponent<MeshRenderer>();
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

   public IEnumerator DelayedEnding(int AID){
    yield return new WaitForSeconds(1f);
    if(AID == 2){
        AnimationOver(2);}
   }


   private void Awake(){
       // Set up the grid on game start
       GenerateAlltiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);
   }

   public void AnimationOver(int AID){
    Debug.Log("Play resuming");
    OngoingAnimation = false;
    OncomingAnimation = false;
    GameObject.Find("Normy").GetComponent<IntSync>().SetAnimation(0);
    disabledplay = false;
    if(AID == 1 && LastPlayerID == GameObject.Find("Normy").GetComponent<Spawner>().ID){
        GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(TempCoordx, TempCoordy, TempID);
        SyncGrid();
        CheckForWin(1);
        CheckForWin(2);
        SyncWin();
        GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, TempID, TempCoordx, TempCoordy);
    }
    if(AID == 3){
        GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(TempCoordx, TempCoordy, TempID);
        CheckForWin(1);
        CheckForWin(2);
        SyncGrid();
    }
   }


   private Vector3 GetTileCenter(int x, int y){
       // Calculate and return the center position of a tile
       return new Vector3(x * tileSize, 0.55f, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
   }
   private Vector3 GetTileCenterButDifferent(int x, int y){
       // Calculate and return the center position of a tile
       return new Vector3(x * tileSize, 2.55f, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
   }

   public void SyncWin(){
    if(GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 1 && !blackwin1done){
        grey1.SetActive(false);
        black1.SetActive(true);
        blackwin1done = true;
        Debug.Log("1 Black win");
        StartCoroutine(BlackWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 2 && !blackwin2done){
        grey2.SetActive(false);
        black2.SetActive(true);
        blackwin2done = true;
        StartCoroutine(BlackWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 3 && !blackwin3done){
        grey3.SetActive(false);
        black3.SetActive(true);
        blackwin3done = true;
        StartCoroutine(BlackWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 1 && !whitewin1done){
        grey5.SetActive(false);
        white5.SetActive(true);
        whitewin1done = true;
        StartCoroutine(WhiteWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 2 && !whitewin2done){
        grey4.SetActive(false);
        white4.SetActive(true);
        whitewin2done = true;
        StartCoroutine(WhiteWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 3 && !whitewin3done){
        grey3.SetActive(false);
        white3.SetActive(true);
        whitewin3done = true;
        StartCoroutine(WhiteWinScreen());
    }
    if(GameObject.Find("Normy").GetComponent<IntSync>().pwwin == 2 && GameObject.Find("Normy").GetComponent<IntSync>().pbwin == 2 && !gamertime){
        gamertime = true;
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
    if(!OngoingAnimation){
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
               //Debug.Log("Sync boom true");
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
                //Instantiate(physicsW, GetTileCenter(xcord, ycord), Quaternion.identity);
           }
           
            //CheckForWin(0);
        //CheckForWin(1);
           SyncWin();
       }
    }
   }
   

   private void Update(){
       // Ensure the camera reference is set up
       frameCounter++;
       
        if(showHelp && !createdMenu){
            Debug.Log("daflkj");
            helper.SetActive(true);
            createdMenu = true;
        }
            if(!showHelp){
            helper.SetActive(false);
            createdMenu = false;
        }

        if (GameMat.GetComponent<Renderer>() != null && gamertime && !shitlin)
        {
            GameMat.GetComponent<Renderer>().material.color = endColor;
            fire1.SetActive(true);
            fire2.SetActive(true);
            fire3.SetActive(true);
            fire4.SetActive(true);
            shitlin = true;
       }
        if (GameObject.Find("Normy").GetComponent<IntSync>().cameraShake == true)
        {
            StartCoroutine(cameraShake.Shake(.2f, .4f));
            if (GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga)
            {
                GameObject.Find("Normy").GetComponent<IntSync>().cameraToggle(false);
            }
        }
        cameraShake.posReset();
        if(GameObject.Find("Normy").GetComponent<IntSync>().LPPID != 0){
            /*
            LPPx = GameObject.Find("Normy").GetComponent<IntSync>().LPPx;
            LPPy = GameObject.Find("Normy").GetComponent<IntSync>().LPPy;
            Vector3 LPPHighlightPos = GetTileCenter(LPPx,LPPy);
            int inkRot = GameObject.Find("Normy").GetComponent<PlaySync>().inkRot;
            LPPHighlight.transform.position = LPPHighlightPos;

            LPPHighlight.transform.Rotate(0,inkRot,0);
            */
        }
       
        if(GameObject.Find("Normy").GetComponent<IntSync>().Animating != 0 && !OngoingAnimation){
            OncomingAnimation = true;
        }
        // Check if the counter is divisible by 30
        if (frameCounter >= 30)
        {

            // Trigger your action here
            if(OngoingAnimation){
                disabledplay = true;
            }
            SyncGrid();
            SyncWin();
            LastPlayerID = GameObject.Find("Normy").GetComponent<IntSync>().LPlayer;
            LPPID = GameObject.Find("Normy").GetComponent<IntSync>().LPPID;
            LPPx = GameObject.Find("Normy").GetComponent<IntSync>().LPPx;
            LPPy = GameObject.Find("Normy").GetComponent<IntSync>().LPPy;
            Vector3 thingfordumbidiotunity = GetTileCenter(LPPx,LPPy);
            if(OncomingAnimation && !OngoingAnimation && LPPID == 6 && !flipyet && GameObject.Find("Normy").GetComponent<Spawner>().ID != GameObject.Find("Normy").GetComponent<IntSync>().gaga && !GameObject.Find("Normy").GetComponent<AnimationController>().flipped){
                flipyet = true;
                Debug.Log(flipyet);
                OngoingAnimation = true;
                OncomingAnimation = false;
                GameObject.Find("Normy").GetComponent<AnimationController>().DoAFlip(GameObject.Find("Normy").GetComponent<IntSync>().f1, GameObject.Find("Normy").GetComponent<IntSync>().f2, thingfordumbidiotunity);
                GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
                    foreach (GameObject obj in pKillList){
                        Realtime.Destroy(obj);
                    }
                    Vector3 LPPHighlightPos = GetTileCenter(currentHover.x,currentHover.y);
                    int rndInkYRot = rnd.Next(0,360);
                    Quaternion InkRot = Quaternion.Euler(0, rndInkYRot, 0);
                    InkRot = InkRot.normalized;
                    Realtime.Instantiate("LPPHighlight", LPPHighlightPos, InkRot);
            }
            if(OncomingAnimation && !OngoingAnimation && LPPID == 9){
                OngoingAnimation = true;
                OncomingAnimation = false;
                if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().PID != LastPlayerID){
                    GameObject.Find("Normy").GetComponent<AnimationController>().Grabbed();
                }
                else{
                    StartCoroutine(DelayedEnding(2));
                }
            }
        
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
       if (Input.GetKeyDown(KeyCode.Space) && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga){
           GameObject.Find("GomokuBoard").GetComponent<PiecePool>().doHold();
       }
     
       if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            // Get the event system and create a PointerEventData object
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition // Set the position of the pointer to the current mouse position
            };

            // Create a list to store the raycast results
            var raycastResults = new System.Collections.Generic.List<RaycastResult>();

            // Perform the raycast to detect UI elements
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            // Loop through the results and check if any of the objects are BetterHelpUI
            foreach (var result in raycastResults)
            {
                if (result.gameObject == BetterHelpUI)
                {
                    Debug.Log("You clicked on BetterHelpUI!");
                    if(showHelp){
                        showHelp = false;
                    } else{
                        showHelp = true;
                        Debug.Log("showHelp true");
                    }
                    // Put your code here to handle the click event on BetterHelpUI
                    return; // Exit once we detect the click on BetterHelpUI
                }
            }
        }
       
    
       // Handle piece placement logic when left-click is pressed
       if (Input.GetMouseButtonDown(0) && !OngoingAnimation && !(currentHover.x > 14 || currentHover.x < 0 || currentHover.y < 0 || currentHover.y > 14)){
    
           int bombTriggered = 0;
           Debug.Log("Turn: " + GameObject.Find("Normy").GetComponent<IntSync>().gaga);
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 2 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
               // If next piece is bomb and it is clients turn
               int pieceID = GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID;
               //GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, pieceID + 1);
               GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 3, currentHover.x, currentHover.y);
               bombTriggered = 1;
               Quaternion rotation = Quaternion.Euler(15,15, 15);
               int gridSpace = -1;
                int[] gridIDs = new int[9];  

                //Debug.Log(xcord + ", " + ycord); 
                for (int b = -1; b < 2; b++)
                {
                    for (int y = -1; y < 2; y++){
                        gridSpace++;
                        if(currentHover.x + b < 15 && currentHover.x + b > -1 && currentHover.y + y < 15 && currentHover.y + y > -1){
                            if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] == (byte)1){
                                gridIDs[gridSpace] = 1;
                            }
                            else if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] == (byte)2){
                                gridIDs[gridSpace] = 2;
                            }
                            else if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] == (byte)5){
                                gridIDs[gridSpace] = 5;
                            }
                            else{
                                gridIDs[gridSpace] = 0;
                            }
                            if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x + b, currentHover.y + y)] != (byte)4){
                                GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x + b, currentHover.y + y, 0);
                            }
                        } else{
                            gridIDs[gridSpace] = 0;
                        }
                        
                    }
                }

               int gridInstantiateSpace = -1;
               for (int b = -1; b < 2; b++)
               {
                for (int y = -1; y < 2; y++){
                    
                        gridInstantiateSpace++;
                        int rndXrot = rnd.Next(0,30);
                        int rndYrot = rnd.Next(0,30);   
                        int rndZrot = rnd.Next(0,30);
                        rotation.x = rndXrot;
                        rotation.y = rndYrot;
                        rotation.z = rndZrot;
                        rotation = rotation.normalized;
                        Vector3 physicsPos = GetTileCenter(currentHover.x + b, currentHover.y + y);
                        physicsPos.y += 0.5f;
                        byte gridID = (byte) gridIDs[gridInstantiateSpace];
                        if(gridID == 1){
                            //Instantiate(physicsB, physicsPos, rotation);
                            GameObject PB = Realtime.Instantiate("PhysicsB", physicsPos, rotation);
                            PB.tag = "PhysicsToKill";
                        } else if(gridID == 2){
                            //Instantiate(physicsW, physicsPos, rotation);
                            GameObject PW = Realtime.Instantiate("PhysicsW", physicsPos, rotation);
                            PW.tag = "PhysicsToKill";
                        } else if(gridID == 5){
                            //Instantiate(physicsS, physicsPos, rotation);
                            GameObject PS = Realtime.Instantiate("PhysicsS", physicsPos, rotation);
                            PS.tag = "PhysicsToKill";
                        }
                    
                    }

                }
                Vector3 explosionPos = GetTileCenter(currentHover.x, currentHover.y );
                Vector3 explosionLightPos = explosionPos;
                explosionLightPos.y = 1.0f;
                
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null){
                        rb.AddExplosionForce(power, explosionPos, radius, 0.25F);
                    }
                }
                //StartCoroutine(cameraShake.Shake(.25f, .55f));
                GameObject.Find("Normy").GetComponent<IntSync>().cameraToggle(true);
                Debug.Log("Shake TRUE");
                
                Realtime.Instantiate("Explosion", explosionPos, rotation);
                
                
                Realtime.Instantiate("ExplosionLight", explosionLightPos, rotation);
                StartCoroutine(explosionKiller());
                StartCoroutine(exploLightKiller());
                StartCoroutine(physicsKiller());
                //LPPHighlight Destroy
                GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
                    foreach (GameObject obj in pKillList){
                        Realtime.Destroy(obj);
                    }
                

           
              
           }
           //if next piece is a grab and it is clients turn
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 8 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
                GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 9, currentHover.x, currentHover.y);
                GameObject.Find("Normy").GetComponent<IntSync>().SetAnimation(2);
                if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().PID == 1){
                    GameObject.Find("Normy").GetComponent<IntSync>().WhiteGrab(1);
                    
                }
                else{
                    GameObject.Find("Normy").GetComponent<IntSync>().BlackGrab(1);
                }
           }
           //STONE IS HERE!!
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 3 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
               int stonesPlaced = 0;
               GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 4, currentHover.x, currentHover.y);
               //LPPHightlight Destroy
               GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
                    foreach (GameObject obj in pKillList){
                        Realtime.Destroy(obj);
                    }
               while(stonesPlaced != 2){
                   int rndSpot = rnd.Next(0, 225);
                   int rndXCoord = rndSpot % 15;
                   int rndYCoord = rndSpot / 15;
                   if(!GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(rndXCoord, rndYCoord) && GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[rndSpot] != (byte)4){
                       GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(rndXCoord, rndYCoord, 4);
                       stonesPlaced++;
                       //LPPHighlight Place
                       Vector3 LPPHighlightPos = GetTileCenter(rndXCoord, rndYCoord);
                       int rndInkYRot = rnd.Next(0,360);
                       Quaternion InkRot = Quaternion.Euler(0, rndInkYRot, 0);
                       InkRot = InkRot.normalized;
                       Realtime.Instantiate("LPPHighlight", LPPHighlightPos, InkRot);
                   }
               }
           }
           int agentTriggered = 0;
           int petrifyTriggered = 0;
           //DOUBLE AGENT HERE!!
           //DOUBLE AGENT HERE!!
if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 5 && 
   GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && 
   !disabledplay)
{
    // Get the current state of the clicked tile FIRST
    byte currentState = GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)];
    
    // Block action if tile is empty
    if (currentState == 0)
    {
        Debug.Log("Double Agent cannot target empty tiles!");
        return; // Exit early, no action taken
    }

    // Proceed only if tile has a valid piece (black/white/share)
    GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 6, currentHover.x, currentHover.y);
    
    if ((currentState == 1 || currentState == 5) && GameObject.Find("Normy").GetComponent<Spawner>().ID == 1)
    {
        // White player flip logic
        TempCoordx = currentHover.x;
        TempCoordy = currentHover.y;
        TempID = 2;
        SyncGrid();
        agentTriggered = 1;
        
        if (currentState == 1) {
            GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x, currentHover.y, 0);
            GameObject.Find("Normy").GetComponent<IntSync>().SetFlip(2, 1);
        } else {
            GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x, currentHover.y, 0);
            GameObject.Find("Normy").GetComponent<IntSync>().SetFlip(3, 1);
        }
        GameObject.Find("Normy").GetComponent<IntSync>().SetAnimation(1);
    }
    else if ((currentState == 2 || currentState == 5) && GameObject.Find("Normy").GetComponent<Spawner>().ID == 0)
    {
        // Black player flip logic
        TempCoordx = currentHover.x;
        TempCoordy = currentHover.y;
        TempID = 1;
        SyncGrid();
        agentTriggered = 1;
        
        if (currentState == 2) {
            GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x, currentHover.y, 0);
            GameObject.Find("Normy").GetComponent<IntSync>().SetFlip(1, 2);
        } else {
            GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x, currentHover.y, 0);
            GameObject.Find("Normy").GetComponent<IntSync>().SetFlip(3, 2);
        }
        GameObject.Find("Normy").GetComponent<IntSync>().SetAnimation(1);
    }
    else
    {
       
        return; // Block invalid actions (e.g., flipping own color)
    }
}
           //if next piece is a petrify and it is the clients turn
           if(GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID == 6 && GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){
               if(GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)1 || GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)5 || GameObject.Find("Normy").GetComponent<ByteSync>()._model.bytes[coordToInt(currentHover.x, currentHover.y)] == (byte)2){
                   GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x , currentHover.y, 4);
                   petrifyTriggered = 1;
                   GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 7, currentHover.x, currentHover.y);
                   GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
                    foreach (GameObject obj in pKillList){
                        Realtime.Destroy(obj);
                    }
                    Vector3 LPPHighlightPos = GetTileCenter(currentHover.x,currentHover.y);
                    int rndInkYRot = rnd.Next(0,360);
                    Quaternion InkRot = Quaternion.Euler(0, rndInkYRot, 0);
                    InkRot = InkRot.normalized;
                    Realtime.Instantiate("LPPHighlight", LPPHighlightPos, InkRot);
                }
              


           }
           //if clicked space is empty and it is clients turn
           if(GameObject.Find("Normy").GetComponent<ByteSync>().checkEmpty(currentHover.x , currentHover.y) &&
              GameObject.Find("Normy").GetComponent<Spawner>().ID == GameObject.Find("Normy").GetComponent<IntSync>().gaga && !disabledplay){


               // BLACK WHITE SHARE HERE!!!
               int pieceID = GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID;
               if(pieceID == 0 || pieceID == 1 || pieceID == 4){ 
                    TempCoordx = currentHover.x;
                    TempCoordy = currentHover.y;
                    TempID = pieceID + 1;
                    GameObject.Find("Normy").GetComponent<AnimationController>().PPAP(pieceID, GetTileCenter(currentHover.x, currentHover.y));
                    GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
                    foreach (GameObject obj in pKillList){
                        Realtime.Destroy(obj);
                    }
                    Vector3 LPPHighlightPos = GetTileCenter(currentHover.x,currentHover.y);
                    int rndInkYRot = rnd.Next(0,360);
                    Quaternion InkRot = Quaternion.Euler(0, rndInkYRot, 0);
                    InkRot = InkRot.normalized;
                    Realtime.Instantiate("LPPHighlight", LPPHighlightPos, InkRot);
               }

               if(pieceID == 7){
                
                    int randMystery = rnd.Next(0,4);
                    if (randMystery == 0)
                    {
                        TempCoordx = currentHover.x;
                        TempCoordy = currentHover.y;
                        TempID = 1;
                        GameObject.Find("Normy").GetComponent<AnimationController>().PPAP(0, GetTileCenter(currentHover.x, currentHover.y));
                        GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 1, currentHover.x, currentHover.y);
                    }
                    else if (randMystery == 1)
                    {
                        TempCoordx = currentHover.x;
                        TempCoordy = currentHover.y;
                        TempID = 2;
                        GameObject.Find("Normy").GetComponent<AnimationController>().PPAP(1, GetTileCenter(currentHover.x, currentHover.y));
                        GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 2, currentHover.x, currentHover.y);
                    }
                    else if (randMystery == 2)
                    {
                        TempCoordx = currentHover.x;
                        TempCoordy = currentHover.y;
                        TempID = 5;
                        GameObject.Find("Normy").GetComponent<AnimationController>().PPAP(4, GetTileCenter(currentHover.x, currentHover.y));
                        GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 4, currentHover.x, currentHover.y);
                    }
                    else
                    {
                        GameObject.Find("Normy").GetComponent<ByteSync>().doPlace(currentHover.x, currentHover.y, 5);
                        GameObject.Find("Normy").GetComponent<IntSync>().SetLPP(GameObject.Find("Normy").GetComponent<Spawner>().ID, 5, currentHover.x, currentHover.y);
                        //CheckForWin(0);
                        //CheckForWin(1);
                    }
                    GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
                    foreach (GameObject obj in pKillList){
                        Realtime.Destroy(obj);
                    }
                    Vector3 LPPHighlightPos = GetTileCenter(currentHover.x,currentHover.y);
                    int rndInkYRot = rnd.Next(0,360);
                    Quaternion InkRot = Quaternion.Euler(0, rndInkYRot, 0);
                    InkRot = InkRot.normalized;
                    Realtime.Instantiate("LPPHighlight", LPPHighlightPos, InkRot);
            

               }

               if(pieceID != 5 && pieceID != 6 && pieceID != 2){
                   BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
                   
                   GameObject.Find("Normy").GetComponent<IntSync>().Turn();
                   GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               }
      
               SyncGrid(); // Sync the grid after placing a piece
           }
           if(bombTriggered == 1){
               GameObject.Find("Normy").GetComponent<IntSync>().Turn();
               GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
           }
           if(agentTriggered == 1){
               GameObject.Find("Normy").GetComponent<IntSync>().Turn();
               GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
           }
           if(petrifyTriggered == 1){
               GameObject.Find("Normy").GetComponent<PlaySync>().Play();
               GameObject.Find("Normy").GetComponent<IntSync>().Turn();
               BroadcastMessage("PiecePlaced"); // Notify that a piece was placed
           }
           
           SyncGrid();
           CheckForWin(1); // Check for a win condition for player 1 (black) on client side
           CheckForWin(2); // Check for a win condition for player 2 (white) on client side
           /*
           if(GameObject.Find("Normy").GetComponent<IntSync>().LPPID != 4 && GameObject.Find("Normy").GetComponent<IntSync>().LPPID != 7 && GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID != 6 && GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID != 2 && GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID != 6 && GameObject.Find("GomokuBoard").GetComponent<PiecePool>().nextPieceID != 3){
            GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
            foreach (GameObject obj in pKillList){
                RealtimeView view = obj.GetComponent<RealtimeView>();
                if(view != null && view.isOwnedLocallySelf){
                    Realtime.Destroy(obj); // Destroy previous pieces
                }
            }
            Vector3 LPPHighlightPos = GetTileCenter(currentHover.x,currentHover.y);
            int rndInkYRot = rnd.Next(0,360);
            Quaternion InkRot = Quaternion.Euler(0, rndInkYRot, 0);
            InkRot = InkRot.normalized;
            Realtime.Instantiate("LPPHighlight", LPPHighlightPos, InkRot);
           }
           */
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

   IEnumerator physicsKiller(){
    yield return new WaitForSeconds(1.5f);
    GameObject[] pKillList = GameObject.FindGameObjectsWithTag("PhysicsToKill");
       foreach (GameObject obj in pKillList){
           Realtime.Destroy(obj); // Destroy previous pieces
           
       }

   }

   IEnumerator explosionKiller(){
    yield return new WaitForSeconds(1.5f);
    GameObject[] eKillList = GameObject.FindGameObjectsWithTag("ExplosionKill");
       foreach (GameObject obj in eKillList){
           Realtime.Destroy(obj); // Destroy previous pieces
           
       }
   }

   IEnumerator exploLightKiller(){
    yield return new WaitForSeconds(0.075f);
    GameObject[]eLKillList = GameObject.FindGameObjectsWithTag("ExploLightKill");
        foreach(GameObject obj in eLKillList){
            Realtime.Destroy(obj);
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
                    GameObject[] pKillList = GameObject.FindGameObjectsWithTag("HighlightKill");
                    foreach (GameObject obj in pKillList){
                        Realtime.Destroy(obj);
                    }
                   SyncGrid();
                   ClearBoard();
                   return; // Exit early after finding a win
               }
           }
       }
   }
}