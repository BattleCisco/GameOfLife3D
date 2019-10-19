using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the cells
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameRule gameRule;

    public int LowerLivingRemaining { get { return gameRule.lowerLivingRemaining; } }
    public int HigherLivingRemaining { get { return gameRule.higherLivingRemaining; } }

    /// <summary>
    /// The rules for a cell to continue living, indicated by the last 2 numbers in rule.
    /// In the original Game Of Life, 2333 is the rule, meaning cells with 3 neighbours will become alive if dead.
    /// </summary>
    public int LowerDeadWillBecomeAlive { get { return gameRule.lowerDeadWillBecomeAlive; } }
    public int HigherDeadWillBecomeAlive { get { return gameRule.higherDeadWillBecomeAlive; } }

    /// <summary>
    /// How much of the game area should be filled on start
    /// </summary>
    [Range(-0.00001f, 1f)]
    public float fillPercentage;

    /// <summary>
    /// Gamearea dimensions, in the order of X, Y, Z.
    /// </summary>
    public int gameareaWidth;
    public int gameareaHeight;
    public int gameareaDepth;

    /// <summary>
    /// How often are they calculated
    /// </summary>
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

        //Snaps and sets up the pre-existing blocks into the array.
        for (int i=0; i < transform.childCount; i++){
            Transform childTransform = transform.GetChild(i);
            if (!childTransform.GetComponent<Cell>())
                continue;

            Vector3 position = childTransform.position;
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);
            int z = Mathf.RoundToInt(position.z);

            cellObjects[x, y, z] = childTransform.GetComponent<Cell>();
            cellObjects[x, y, z].alive = true;
            cellObjects[x, y, z].nextAlive = true;
        }

        //Sets up the rest of the blocks, setting them to alive or dead based on the result from the random.
        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    if (cellObjects[x, y, z])
                        continue;

                    GameObject cellGameObject = Instantiate(cellPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                    cellObjects[x, y, z] = cellGameObject.GetComponent<Cell>();
                    cellObjects[x, y, z].alive = Random.Range(0f, 1f) <= fillPercentage;
                    cellObjects[x, y, z].transform.localScale = Vector3.zero;
                    //Set to 0 to either be dead or play birth animation.
                }
            }
        }
    }
    
    /// <summary>
    /// Calculates when to make a states update
    /// Always updates the visual
    /// </summary>
    void Update() {
        float deltaTicktimer = Time.time - timeTickStart;
        UpdateCellVisuals(deltaTicktimer / secondsPerTick);
        if (deltaTicktimer > secondsPerTick) {
            UpdateCellStates();
            tick++;
            timeTickStart = Time.time;
        }
    }

    /// <summary>
    /// Updates animation on the cells
    /// </summary>
    /// <param name="progress">From 0 to 1, progress through the tick.</param>
    void UpdateCellVisuals(float progress) {
        for (int x = 0; x < gameareaWidth; x++) {
            for (int y = 0; y < gameareaHeight; y++) {
                for (int z = 0; z < gameareaDepth; z++) {
                    cellObjects[x, y, z].UpdateVisual(this, tick, progress);
                }
            }
        }
    }

    /// <summary>
    /// Updates the states on the cells, early then late.
    /// </summary>
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
}

