using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MINELab
{
    [Serializable]
    public partial class AnalysisTool : EditorWindow
    {
        [Serializable]
        public class DataImportTab
        {
            #region [ Serialised Fields ]
            [SerializeField] private List<AnalysisDataContainer> analysisDataContainers = new List<AnalysisDataContainer>();
            #endregion
            
            #region [ Unserialised Fields ]
            private AnalysisTool _analysisTool;
            private bool _tabEnabled;
            public bool TabEnabled
            {
                get => _tabEnabled;
                set
                {
                    _tabEnabled = value;
                    _dataImportTabVisualElement.style.display = (value) ? DisplayStyle.Flex : DisplayStyle.None;

                    if (value)
                    {
                        if (!_dataImportTabButton.ClassListContains("selected")) _dataImportTabButton.ToggleInClassList("selected");
                        if (_dataImportTabButton.ClassListContains("unselected")) _dataImportTabButton.ToggleInClassList("unselected");
                    }
                    else
                    {
                        if (_dataImportTabButton.ClassListContains("selected")) _dataImportTabButton.ToggleInClassList("selected");
                        if (!_dataImportTabButton.ClassListContains("unselected")) _dataImportTabButton.ToggleInClassList("unselected");
                    }
                }
            }
            #endregion
            
            #region [ UXML Elemnts ]
            private VisualTreeAsset _participantImportDataListElement;
            private VisualElement _dataImportTabVisualElement;
            private ListView _participantImportListView;
            private Button _dataImportTabButton;
            private Button _processDataButton;
            #endregion



            public void Instantiate(AnalysisTool analysisTool)
            {
                _analysisTool = analysisTool;
            }

            public void QueryElements(VisualElement root)
            {
                _dataImportTabVisualElement = root.Q<VisualElement>("DataImportTab");
                _dataImportTabButton = root.Q<Button>("DataImportTabButton");
                _processDataButton = root.Q<Button>("ProcessDataButton");
                _participantImportListView = root.Q<ListView>("ParticipantImportListView");
                
                SetupParticipantDataListView();
            }
            
            public void RegisterCallbacks()
            {
                _dataImportTabButton.clickable.clicked += OnDataInputTabButtonPressed;
            }
            
            public void UnregisterCallbacks()
            {
                _dataImportTabButton.clickable.clicked -= OnDataInputTabButtonPressed;
            }
            
            
            
            private void OnDataInputTabButtonPressed() => _analysisTool.ActiveTab = TabSelection.DataImport;
            
            private void SetupParticipantDataListView()
            {
                _participantImportDataListElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/[ Editor ]/[ UXML ]/DataImportParticipantListElement.uxml");
                
                VisualElement MakeItem() => _participantImportDataListElement.CloneTree();
                
                void BindItem(VisualElement element, int i)
                {
                    if (element.Q<ObjectField>("ExperimentDataObjectField") is { } experimentDataObjectField && analysisDataContainers?[i].experimentData is { } experimentData)
                        experimentDataObjectField.value = experimentData;
                    
                    if (element.Q<ObjectField>("EmteqJSONObjectField") is { } emteqJSONObjectField && analysisDataContainers?[i].experimentData is { } emteqJSON)
                        emteqJSONObjectField.value = emteqJSON;
                    
                    if (element.Q<ObjectField>("EmteqDabObjectField") is { } emteqDabObjectField && analysisDataContainers?[i].experimentData is { } emteqDab)
                        emteqDabObjectField.value = emteqDab;
                    
                    if (element.Q<ObjectField>("EmteqEyeDataObjectField") is { } field && analysisDataContainers?[i].experimentData is { } emteqEyeData)
                        field.value = emteqEyeData;
                }

                _participantImportListView.itemsAdded += (index) =>
                {
                    Debug.Log($"Added: {index.ToArray()[0]}"); 
                };
                
                _participantImportListView.itemsRemoved += (index) =>
                {
                    Debug.Log($"Removed: {index.ToArray()[0]}"); 
                }; 
                
                _participantImportListView.makeItem = MakeItem;
                _participantImportListView.bindItem = BindItem;
                _participantImportListView.itemsSource = analysisDataContainers;
                _participantImportListView.selectionType = SelectionType.Multiple;
                _participantImportListView.fixedItemHeight = 80;
                _participantImportListView.style.flexGrow = 1f;
            }
        }
    }    
}

[Serializable]
public class AnalysisDataContainer
{
    public TextAsset experimentData; 
    public TextAsset emteqJSON;
    public TextAsset emteqDab;
    public TextAsset emteqEyeData;
}