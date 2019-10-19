using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Game Rule", menuName = "Game Rule")]
public class GameRule : ScriptableObject {
    /// <summary>
    /// The rules for a cell to continue living, indicated by the first 2 numbers in rule.
    /// In the original Game Of Life, 2333 is the rule, meaning cells with 2 or 3 neighbours remain.
    /// If outside this span the cell dies.
    /// </summary>
    public int lowerLivingRemaining;
    public int higherLivingRemaining;

    /// <summary>
    /// The rules for a cell to continue living, indicated by the last 2 numbers in rule.
    /// In the original Game Of Life, 2333 is the rule, meaning cells with 3 neighbours will become alive if dead.
    /// </summary>
    public int lowerDeadWillBecomeAlive;
    public int higherDeadWillBecomeAlive;
}
