using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Experiment Block", menuName = "[ MINE Lab ]/Experiment Block")]
public class BlockDataScriptableObject : ScriptableObject
{
    public List<Trial> trials = new ();
}