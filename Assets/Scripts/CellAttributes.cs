using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attribute", menuName = "Attribute")]
public class CellAttributes : ScriptableObject {
    public bool permanentColor;
    public Color color;
    public bool invulnerable;
}
