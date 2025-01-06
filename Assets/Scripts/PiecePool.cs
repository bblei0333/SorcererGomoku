using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PiecePool : MonoBehaviour
{
    public GameObject bp; // Black piece prefab
    public Canvas canvas;
    public GameObject wp; // White piece prefab
    public GameObject bomb; // Bomb piece prefab
    public GameObject stone;
    public GameObject share;
    public GameObject doubleAgent;
    public GameObject sniper;
    private GameObject slot1; // Slot for the first piece
    private GameObject slot2; // Slot for the second piece
    private GameObject slot3; // Slot for the third piece
    private GameObject swap1;
    private GameObject swap2;
    private GameObject hold;
    public int nextPieceID; // ID of the next piece to place
    private int pcount = 0; // Piece count (how many pieces have been placed)
    private int funnynum; // The index of the current piece being displayed
    public int PID; // Player ID (used for determining which piece to use)
    private int bing; // A flag to ensure `realStart` is only called once
    public int[] pool = new int[40]; // Pool that determines which pieces are available
    Random rnd = new Random(); // Random number generator

    // Called at the start of the game or when setting up the pool
    private void realStart()
    {
        // Randomly assign powerups to 8 positions in the pool
        for (int x = 0; x < 20;) //change number to add more pieces to pool
        {
            int r = rnd.Next(0, 40); // Randomly choose an index
            if (pool[r] == 0) // If the slot is empty
            {
                int randPowerUp = rnd.Next(0,5);
                if(randPowerUp == 0){
                    pool[r] = 3; // Assign a bomb
                }
                if(randPowerUp == 1){
                    if(r > 3){
                        pool[r] = 4; // Assign a stone
                        Debug.Log("Stone added");
                    }
                }
                if(randPowerUp == 2){
                    pool[r] = 5; // Assign a share
                    Debug.Log("Share added");
                }
                if(randPowerUp == 3){
                    pool[r] = 6; // Assign a doubleAgent
                    Debug.Log("DoubleAgent added");
                }
                if(randPowerUp == 4){
                    pool[r] = 7; // Assign a sniper
                    Debug.Log("Sniper added");
                }
                x++; // Increment the count of powerups
            }
        }
        // Optionally log pool values for debugging
        // for (int x = 0; x < 40; x++) { Debug.Log(pool[x]); }

        slotView(0); // Display the first set of pieces
        setID(); // Set the ID of the next piece
    }
    public void doHold(){
        if(!hold){
            hold = Instantiate(setSlot(pcount), canvas.transform);
            RectTransform rectTransform3 = hold.GetComponent<RectTransform>();
            rectTransform3.anchoredPosition = new Vector2(-630, 400);
            PiecePlaced();
        }
        else{
            swap1 = hold;
            swap2 = slot1;
            Destroy(hold);
            Destroy(slot1);
            slot1 = Instantiate(swap1, canvas.transform);
            RectTransform rectTransform1 = slot1.GetComponent<RectTransform>();
            rectTransform1.anchoredPosition = new Vector2(630,400);
            hold = Instantiate(swap2, canvas.transform);
            RectTransform rectTransform2 = hold.GetComponent<RectTransform>();
            rectTransform2.anchoredPosition = new Vector2(-630,400);
            // Determine the next piece ID based on the current piece's type
            if (slot1.tag == "MenuBlack") nextPieceID = 0; // Black piece
            if (slot1.tag == "MenuWhite") nextPieceID = 1; // White piece
            if (slot1.tag == "MenuBomb") nextPieceID = 2; // White piece
            Debug.Log("NID = " + nextPieceID);
            Debug.Log(slot1);
            Debug.Log(slot1.tag);
        }
    }

    // Sets the slot for a given piece index, returns a prefab based on piece type
    public GameObject setSlot(int num)
    {
        if (pool[num] == 0) // If the slot is empty, return the appropriate piece (based on player ID)
        {
            return PID == 1 ? wp : bp; // If player 1, return white piece; otherwise, black piece
        }
        else
        {
            if(pool[num] == 3){
                return bomb;
            }
            if(pool[num] == 4){
                //Debug.Log("Returning stone");
                return stone;
            }
            if(pool[num] == 5){
                //Debug.Log("Returning share");
                return share;
            }
            if(pool[num] == 6){
                //Debug.Log("Returning doubleAgent");
                return doubleAgent;
            }
            else{
                //Debug.Log("Returning sniper");
                return sniper;
            }
            
        }
    }

    // Called when a piece is placed, increments piece count and updates the slots
    void PiecePlaced()
    {
        pcount++; // Increment the piece count
        slotView(pcount); // Update the slots with the new piece
        setID(); // Update the next piece ID based on the current piece
    }

    // Updates the visible slots based on the current piece count
    public void slotView(int num)
    {
        // If the slots are already instantiated, destroy them before creating new ones
        if (slot1) 
        {
            Destroy(slot1);
            Destroy(slot2);
            Destroy(slot3);
        }

        // Instantiate the next 3 slots (based on the current piece count)
        for (int x = num; x < num + 3; x++)
        {
            Vector2 position = new Vector2(630, (400 - (85 * (x - num)))); // Set position for each slot
            if (x - num == 0)
            {
                slot1 = Instantiate(setSlot(x), canvas.transform);
                RectTransform rectTransform = slot1.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = position;
                funnynum = x; // Save the current piece index for future reference
            }
            else if (x - num == 1)
            {
                slot2 = Instantiate(setSlot(x), canvas.transform);
                RectTransform rectTransform = slot2.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = position;
            }
            else if (x - num == 2)
            {
                slot3 = Instantiate(setSlot(x), canvas.transform);
                RectTransform rectTransform = slot3.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = position;
            }
        }
    }

    // Set the ID of the next piece to place based on the current piece type
    public void setID()
    {
        // Determine the next piece ID based on the current piece's type
        if (setSlot(funnynum) == bp) nextPieceID = 0; // Black piece
        if (setSlot(funnynum) == wp) nextPieceID = 1; // White piece
        if (setSlot(funnynum) == bomb) nextPieceID = 2; // Bomb piece
        if (setSlot(funnynum) == stone) nextPieceID = 3; //Stone piece
        if (setSlot(funnynum) == share) nextPieceID = 4; //Share piece
        if (setSlot(funnynum) == doubleAgent) nextPieceID = 5; //DoubleAgent piece
        if (setSlot(funnynum) == sniper) nextPieceID = 6; //Sniper piece
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the pool setup is done only once after the client connects
        if (GameObject.Find("Normy").GetComponent<Spawner>().ding != 0 && bing == 0)
        {
            bing++; // Flag that the setup is done
            realStart(); // Set up the pool and slots
        }
    }
}
