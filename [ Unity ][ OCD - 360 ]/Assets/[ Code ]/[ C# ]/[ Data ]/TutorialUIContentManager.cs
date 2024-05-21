using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TutorialUIContentManager
{
    [SerializeField] private List<TutorialUIContentContainer> _tutorialContentContainers = new List<TutorialUIContentContainer>();
    private Dictionary<string, TutorialContentScriptableObject> _tutorialContentDictionary;

    public TutorialContentScriptableObject Content(string key)
    {
        if (_tutorialContentDictionary == null) GenerateDictionary();
        if (_tutorialContentDictionary.TryGetValue(key, out TutorialContentScriptableObject content)) return content;
        
        Debug.Log($"Dictionary did not container the requested key: {key}");
        return null;
    }

    private void GenerateDictionary()
    {
        _tutorialContentDictionary = new Dictionary<string, TutorialContentScriptableObject>();
        foreach (TutorialUIContentContainer container in _tutorialContentContainers) _tutorialContentDictionary.Add(container.key, container.contentData);
    }
}

[Serializable]
public class TutorialUIContentContainer
{
    public string key;
    public TutorialContentScriptableObject contentData;
}
