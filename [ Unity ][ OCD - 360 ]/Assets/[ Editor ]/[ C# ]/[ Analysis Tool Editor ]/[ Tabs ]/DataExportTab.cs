using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MINELab
{
    public partial class AnalysisTool : EditorWindow
    {
        public class DataExportTab
        {
            #region [ Unserialised Fields ]
            private AnalysisTool _analysisTool;
            private bool _tabEnabled;
            public bool TabEnabled
            {
                get => _tabEnabled;
                set
                {
                    _tabEnabled = value;
                    _dataExportTabVisualElement.style.display = (value) ? DisplayStyle.Flex : DisplayStyle.None;
                    
                    if (value)
                    {
                        if (!_dataExportTabButton.ClassListContains("selected")) _dataExportTabButton.ToggleInClassList("selected");
                        if (_dataExportTabButton.ClassListContains("unselected")) _dataExportTabButton.ToggleInClassList("unselected");
                    }
                    else
                    {
                        if (_dataExportTabButton.ClassListContains("selected")) _dataExportTabButton.ToggleInClassList("selected");
                        if (!_dataExportTabButton.ClassListContains("unselected")) _dataExportTabButton.ToggleInClassList("unselected");
                    }
                }
            }
            #endregion
            
            #region [ UXML Elemnts ]
            private VisualElement _dataExportTabVisualElement;
        
            private Button _dataExportTabButton;
            #endregion
            
            
            
            public void Instantiate(AnalysisTool analysisTool) => _analysisTool = analysisTool;
            
            public void QueryElements(VisualElement root)
            {
                _dataExportTabVisualElement = root.Q<VisualElement>("DataExportTab");
                _dataExportTabButton = root.Q<Button>("DataExportTabButton");
            }
        
            public void RegisterCallbacks()
            {
                _dataExportTabButton.clickable.clicked += OnDataExportTabButtonPressed;
            }

            public void UnregisterCallbacks()
            {
                _dataExportTabButton.clickable.clicked -= OnDataExportTabButtonPressed;
            }
            
            
            
            private void OnDataExportTabButtonPressed() => _analysisTool.ActiveTab = TabSelection.DataExport;
        }
    }    
}