using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MINELab
{
    [Serializable]
    public partial class AnalysisTool : EditorWindow
    {
        public class DataImportTab
        {
            #region [ Serialised Fields ]
            public List<AnalysisDataContainer> analysisDataContainers = new List<AnalysisDataContainer>();
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
                
                VisualElement MakeItem()
                {
                    analysisDataContainers.Add(new AnalysisDataContainer());
                    return _participantImportDataListElement.Instantiate();
                }

                
                
                void BindItem(VisualElement element, int i)
                {
                    ObjectField experimentDataObjectField = element.Q<ObjectField>("ExperimentDataObjectField");
                    experimentDataObjectField.value = analysisDataContainers[i].experimentData;
                    ObjectField emteqJSONObjectField = element.Q<ObjectField>("EmteqJSONObjectField");
                    emteqJSONObjectField.value = analysisDataContainers[i].emteqJSON;
                    ObjectField emteqDabObjectField = element.Q<ObjectField>("EmteqDabObjectField");
                    emteqDabObjectField.value = analysisDataContainers[i].emteqDab;
                    ObjectField emteqEyeDataObjectField = element.Q<ObjectField>("EmteqEyeDataObjectField");
                    emteqEyeDataObjectField.value = analysisDataContainers[i].emteqEyeData;
                }

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