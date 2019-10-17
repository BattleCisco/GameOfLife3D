using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    int x, y, z;
    public CellAttributes cellAttributes;

    public Color cellColor;
    public int gameTickBirth;
    public bool alive;
    public bool nextAlive;
    public bool falling;

    public AnimationCurve birthCurve;
    public AnimationCurve deathCurve;

    public void SetStartValues(int x, int y, int z, Color cellColor, bool alive) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.cellColor = cellAttributes && cellAttributes.permanentColor? cellAttributes.color : cellColor;
        this.alive = alive;
    }

    public bool HasGravity() {
        if (cellAttributes)
            return cellAttributes.gravity;
        return false;
    }

    public void UpdateCell(Cell[][][] cells, int tick, float deltaTickTimer) {
        if (GameManager.instance.thisTick == gameTickBirth && birthCurve.keys.Length > 0)
            transform.localScale = new Vector3(birthCurve.Evaluate(deltaTickTimer), birthCurve.Evaluate(deltaTickTimer), birthCurve.Evaluate(deltaTickTimer));
        else
            transform.localScale = Vector3.one;

        if (cellAttributes && cellAttributes.invulnerable)
            return;

    }

    public int CheckAliveNeighbours(int gameareaWidth, int gameareaHeight, int gameareaDepth) {
        int count = 0;
        for (int xd = 0; xd < gameareaWidth; xd++) {
            for (int yd = 0; yd < gameareaHeight; yd++) {
                for (int zd = 0; zd < gameareaDepth; zd++) {
                    if (!(xd == x && yd == y && zd == z)) {
                        if (GameManager.instance.cellObjects[xd + x, yd + y, zd + z] != null && GameManager.instance.cellObjects[xd + x, yd + y, zd + z].alive) {
                            count++;
                        }
                    }
                }
            }
        }
        return count;
    }
}
