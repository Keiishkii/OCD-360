using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Experiment Block", menuName = "[ MINE Lab ]/Experiment Block")]
public class Block : ScriptableObject
{
    public List<Trial> trials = new ();
    public List<EnvironmentDataScriptableObject> _beachEnvironments = new ();
}