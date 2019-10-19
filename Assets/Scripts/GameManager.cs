using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    //Span to continue living
    public float lowerLivingRemaining;
    public float HigherLivingRemaining;

    //Resurrect if at the correct value
    public float lowerDeadWillBecomeAlive;
    public float higherDeadWillBecomeAlive;

    //Color mutation
    [Range(0f, 1f)]
    public float fillPercentage;

    public int gameareaWidth;
    public int gameareaHeight;
    public int gameareaDepth;

    public float secondsPerTick = 2f;
    public float timeTickStart = 0f;
    public int tick = 0;

    public GameObject cellPrefab;

    public Cell[,,] cellObjects;

    void Start() {
        if (!instance)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        cellObjects = new Cell[gameareaWidth, gameareaHeight, gameareaDepth];
        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    GameObject cellGameObject = Instantiate(cellPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                    cellObjects[x, y, z] = cellGameObject.GetComponent<Cell>();
                    cellObjects[x, y, z].alive = Random.Range(0, 1f) <= fillPercentage;
                    Debug.Log(cellObjects[x, y, z].alive);
                }
            }
        }
    }
    
    void Update() {
        float deltaTicktimer = Time.time - timeTickStart;
        UpdateCellVisuals(deltaTicktimer / secondsPerTick);
        if (deltaTicktimer > secondsPerTick) {
            UpdateCellStates();
            tick++;
            timeTickStart = Time.time;
            deltaTicktimer = 0f;
        }
    }

    void UpdateCellStates() {
        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    cellObjects[x, y, z].EarlyUpdateState(this);
                }
            }
        }

        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    cellObjects[x, y, z].LateUpdateState(this);
                }
            }
        }
    }

    void UpdateCellVisuals(float progress) {
        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    cellObjects[x, y, z].UpdateVisual(this, tick, progress);
                }
            }
        }
    }
}

