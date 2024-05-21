using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialRatingInputManager
{
    [SerializeField] private List<TutorialRatingContainer> _tutorialRatingContainers = new List<TutorialRatingContainer>();
    private Dictionary<string, Vector2> _tutorialContentDictionary;
    
    public float offsetLeniency = 0;

    public Vector2 Content(string key)
    {
        if (_tutorialContentDictionary == null) GenerateDictionary();
        if (_tutorialContentDictionary.TryGetValue(key, out Vector2 content)) return content;
        
        Debug.Log($"Dictionary did not container the requested key: {key}");
        return Vector2.zero;
    }

    private void GenerateDictionary()
    {
        _tutorialContentDictionary = new Dictionary<string, Vector2>();
        foreach (TutorialRatingContainer container in _tutorialRatingContainers) _tutorialContentDictionary.Add(container.key, container.rating);
    }
}

[Serializable]
public class TutorialRatingContainer
{
    public string key;
    public Vector2 rating;
}
