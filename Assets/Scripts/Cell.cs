using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A cell within the simulation, works together with a GameManager
/// </summary>
public class Cell : MonoBehaviour {

    /// <summary>
    /// Using coordinates as array indexes because we don't need more than one simulation
    /// </summary>
    int X { get { return (int)transform.position.x; } }
    int Y { get { return (int)transform.position.y; } }
    int Z { get { return (int)transform.position.z; } }

    /// <summary>
    /// Overrides the rules for a block with a scriptableObject to allow more flexibility
    /// </summary>
    public CellAttributes cellAttributes;

    /// <summary>
    /// Keep track of birth tick to do birth animation instead of death;
    /// </summary>
    public int birthTick;
    public int deathTick;
    public bool alive;
    public bool nextAlive;

    public AnimationCurve birthCurve;
    public AnimationCurve deathCurve;

    void Start() {
        nextAlive = false;
        birthTick = 0;
        deathTick = -1;
    }

    /// <summary>
    /// Updates the visual of the cell, animations etc.
    /// </summary>
    /// <param name="gameManager">The active GameManager</param>
    /// <param name="tick">The current game tick</param>
    /// <param name="progress">Between 0 and 1, depending on how far it is the animation</param>
    public void UpdateVisual(GameManager gameManager, int tick, float progress) {
        if (gameManager.tick == birthTick && alive)
            transform.localScale = new Vector3(birthCurve.Evaluate(progress), birthCurve.Evaluate(progress), birthCurve.Evaluate(progress));
        else if(gameManager.tick == deathTick)
            transform.localScale = new Vector3(deathCurve.Evaluate(progress), deathCurve.Evaluate(progress), deathCurve.Evaluate(progress));
    }

    /// <summary>
    /// Precalculations to check if it's alive
    /// </summary>
    /// <param name="gameManager">The active GameManager</param>
    public void EarlyUpdateState(GameManager gameManager) {
        if (cellAttributes != null && cellAttributes.invulnerable)
            return;

        int neighbours = CheckAliveNeighbours(gameManager);
        Debug.Log(neighbours);

        if (alive) {
            nextAlive = false;
            if (neighbours >= gameManager.LowerLivingRemaining && neighbours <= gameManager.HigherLivingRemaining) {
                nextAlive = true;
            }
            if (!nextAlive)
                deathTick = gameManager.tick + 1;
        }
        else {
            nextAlive = false;
            if (neighbours >= gameManager.LowerDeadWillBecomeAlive && neighbours <= gameManager.HigherDeadWillBecomeAlive) {
                nextAlive = true;
                birthTick = gameManager.tick;
            }
            if (nextAlive)
                birthTick = gameManager.tick + 1;
        }

    }

    /// <summary>
    /// Moves the nextAlive to alive. Order of execution could have changed the outcome otherwise.
    /// </summary>
    /// <param name="gameManager"></param>
    public void LateUpdateState(GameManager gameManager) {
        alive = nextAlive;
    }

    /// <summary>
    /// Counts the neighbouring cells.
    /// </summary>
    /// <param name="gameManager">The active GameManager</param>
    /// <returns>Number of alive cells</returns>
    public int CheckAliveNeighbours(GameManager gameManager) {
        int count = 0;
        for (int xd = -1; xd <= 1; xd++)
            if (xd + X >= 0 && xd + X < gameManager.gameareaWidth)

                for (int yd = -1; yd <= 1; yd++)
                    if (yd + Y >= 0 && yd + Y < gameManager.gameareaHeight)

                        for (int zd = -1; zd <= 1; zd++)
                            if (zd + Z >= 0 && zd + Z < gameManager.gameareaDepth)

                                if (!(xd == 0 && yd == 0 && zd == 0))
                                    if (gameManager.cellObjects[xd + X, yd + Y, zd + Z] != null && 
                                        gameManager.cellObjects[xd + X, yd + Y, zd + Z].alive) {
                                        count++;
                                    }
        return count;
    }
}
