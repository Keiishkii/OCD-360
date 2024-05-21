using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialEnvironmentContentManager
{
    [SerializeField] private List<TutorialEnvironmentContentContainer> _tutorialEnviromentContentContainers = new List<TutorialEnvironmentContentContainer>();
    private Dictionary<string, (EnvironmentDataScriptableObject, float)> _tutorialContentDictionary;

    public (EnvironmentDataScriptableObject, float) Content(string key)
    {
        if (_tutorialContentDictionary == null) GenerateDictionary();
        if (_tutorialContentDictionary.TryGetValue(key, out (EnvironmentDataScriptableObject, float) content)) return content;
        
        Debug.Log($"Dictionary did not container the requested key: {key}");
        return (null, 0);
    }

    private void GenerateDictionary()
    {
        _tutorialContentDictionary = new Dictionary<string, (EnvironmentDataScriptableObject, float)>();
        foreach (TutorialEnvironmentContentContainer container in _tutorialEnviromentContentContainers) _tutorialContentDictionary.Add(container.key, (container.environment, container.axisRotation));
    }
}

[Serializable]
public class TutorialEnvironmentContentContainer
{
    public string key;
    public EnvironmentDataScriptableObject environment;
    public float axisRotation;
}
