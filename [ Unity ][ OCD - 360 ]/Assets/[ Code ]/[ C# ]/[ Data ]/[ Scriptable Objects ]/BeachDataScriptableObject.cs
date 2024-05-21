using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Experiment Beaches", menuName = "[ MINE Lab ]/Experiment Beaches")]
public class BeachDataScriptableObject : ScriptableObject
{
    public List<EnvironmentDataScriptableObject> environments = new ();
}