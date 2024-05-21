using MINELab;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DataDisplay : EditorWindow
{
    #region [ Unserialised Fields ]
    private AnalysisTool _analysisTool;
    private VisualTreeAsset _UXMLDocument;
    private VisualElement _rootVisualElement;
    private GraphicalDisplay _graphicalDisplay;
    private MinMaxSlider _timeSpanMinMaxSlider;
    #endregion
    
    
    
    [MenuItem ("[ MINE Lab ]/Data Display")]
    public static void  ShowWindow () => GetWindow<DataDisplay>("Data Display");

    private void CreateGUI()
    {
        _analysisTool = AnalysisTool.Instance;
            
        QueryElements();
        RegisterCallbacks();
    }

    private void OnDestroy() => UnregisterCallbacks();
    private void QueryElements()
    {
        _UXMLDocument = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/[ Editor ]/[ UXML ]/GraphDisplay.uxml");
        _rootVisualElement = _UXMLDocument.Instantiate();

        _graphicalDisplay = _rootVisualElement.Q<GraphicalDisplay>("GraphicalDisplay");
        _timeSpanMinMaxSlider = _rootVisualElement.Q<MinMaxSlider>("TimeSpanController");
        
        rootVisualElement.Add(_rootVisualElement);
    }

    private void RegisterCallbacks()
    {
        _timeSpanMinMaxSlider.RegisterValueChangedCallback(OnTimeSpanValueChanged);
    }

    private void UnregisterCallbacks()
    {
        _timeSpanMinMaxSlider.UnregisterValueChangedCallback(OnTimeSpanValueChanged);
    }

    private void OnTimeSpanValueChanged(ChangeEvent<UnityEngine.Vector2> callback)
    {
        _graphicalDisplay.XMin = Mathf.RoundToInt(callback.newValue.x);
        _graphicalDisplay.XMax = Mathf.RoundToInt(callback.newValue.y);
    }
}
