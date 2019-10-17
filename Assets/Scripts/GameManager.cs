using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    //Span to continue living
    public float lowerEndLivingRemaining;
    public float HigherEndLivingRemaining;
    
    //Resurrect if at the correct value
    public float lowerEndDeadWillBecomeAlive;
    public float higherEndDeadWillBecomeAlive;

    //Color mutation
    public float chanceOfMutatingColor;
    public float fillPercentage;

    public int gameareaWidth;
    public int gameareaHeight;
    public int gameareaDepth;

    public float secondsPerTick=2f;
    public float tickStartClock=0f;
    public int thisTick = 0;

    public GameObject cell;
    Simulation simulation;

    public GameObject staticGameobject;
    public GameObject movingGameobject;

    public Cell[,,] cellObjects;

    void Start() {
        staticGameobject = new GameObject();
        staticGameobject.transform.SetParent(transform);

        movingGameobject = new GameObject();
        movingGameobject.transform.SetParent(transform);

        if (!instance)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        cellObjects = new Cell[gameareaWidth, gameareaHeight, gameareaDepth];
    }

    
    void Update() {
        float deltaTicktimer = Time.time - tickStartClock;
        if (deltaTicktimer > secondsPerTick) {
            UpdateCellStates();
            thisTick++;
            tickStartClock = Time.time;
            deltaTicktimer = 0f;
        }
    }

    void UpdateCellStates() {
        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    int neighbours = cellObjects[x, y, z].CheckAliveNeighbours(gameareaWidth, gameareaHeight, gameareaDepth);
                    if (cellObjects[x, y, z] == null) {
                        if(neighbours >= lowerEndDeadWillBecomeAlive)
                    }
                    if(neighbours >= lowerEndLivingRemaining)
                    
                    if (cellObjects[x, y, z] != null && cellObjects[x, y, z].HasGravity()) {
                        cellObjects[x, y, z].falling = true;
                        cellObjects[x, y, z].transform.SetParent(movingGameobject.transform);
                    }
                    else {
                        cellObjects[x, y, z].transform.SetParent(staticGameobject.transform);
                    }
                }
            }
        }

        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    if (cellObjects[x, y, z] != null && cellObjects[x, y, z].HasGravity()) {

                    }
                }
            }
        }
    }
    
        

    void setCellRendering(Cell[] newCells)
    {
        for (int i = 0; i < newCells.Length; i++)
        {
            if (newCells[i].alive)
            {
                MeshRenderer meshRenderer = cellObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.enabled = true;
                Material coloredMaterial = new Material(meshRenderer.material);
                coloredMaterial.color = newCells[i].cellColor;
                meshRenderer.material = coloredMaterial;
            }
            else
            {
                cellObjects[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}

