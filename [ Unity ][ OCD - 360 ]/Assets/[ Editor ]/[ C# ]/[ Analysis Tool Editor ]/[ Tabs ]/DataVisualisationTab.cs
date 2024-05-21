using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MINELab
{
    public partial class AnalysisTool : EditorWindow
    {
        public class DataVisualisationTab
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
                    _dataVisualisationTabVisualElement.style.display = (value) ? DisplayStyle.Flex : DisplayStyle.None;

                    if (value)
                    {
                        if (!_dataVisualisationTabButton.ClassListContains("selected")) _dataVisualisationTabButton.ToggleInClassList("selected");
                        if (_dataVisualisationTabButton.ClassListContains("unselected")) _dataVisualisationTabButton.ToggleInClassList("unselected");
                    }
                    else
                    {
                        if (_dataVisualisationTabButton.ClassListContains("selected")) _dataVisualisationTabButton.ToggleInClassList("selected");
                        if (!_dataVisualisationTabButton.ClassListContains("unselected")) _dataVisualisationTabButton.ToggleInClassList("unselected");
                    }
                }
            }
            #endregion
            
            #region [ UXML Elemnts ]
            private VisualElement _dataVisualisationTabVisualElement;
            private Button _dataVisualisationTabButton;
            private Button _viewYDisplacementAcrossExperimentButton;
            private Button _viewVelocityCurveAcrossExperimentButton;
            private Label _totalDistanceTravelledLabel;
            private Label _sessionDurationLabel;
            #endregion
            
            #region [ Data ]
            
            #endregion
            
            
            
            
            public void Instantiate(AnalysisTool analysisTool) => _analysisTool = analysisTool;
            
            public void QueryElements(VisualElement root)
            {
                _dataVisualisationTabButton = root.Q<Button>("DataVisualisationTabButton");
                _dataVisualisationTabVisualElement = root.Q<VisualElement>("DataVisualisationTab");
                
                _viewYDisplacementAcrossExperimentButton = _dataVisualisationTabVisualElement.Q<Button>("ViewYDisplacementOverTimeButton");
                _viewVelocityCurveAcrossExperimentButton = _dataVisualisationTabVisualElement.Q<Button>("ViewVelocityOverTimeButton");
                
                _totalDistanceTravelledLabel = _dataVisualisationTabVisualElement.Q<Label>("TotalDistanceTravelledLabel");
                _sessionDurationLabel = _dataVisualisationTabVisualElement.Q<Label>("SessionDurationLabel");
            }
            
            public void RegisterCallbacks()
            {
                _dataVisualisationTabButton.clickable.clicked += OnDataVisualisationTabButtonPressed;
                
                _viewYDisplacementAcrossExperimentButton.clickable.clicked += OnViewYDisplacementOverTimeButtonPressed;
                _viewVelocityCurveAcrossExperimentButton.clickable.clicked += OnViewVelocityOverTimeButtonPressed;
            }

            public void UnregisterCallbacks()
            {
                _dataVisualisationTabButton.clickable.clicked -= OnDataVisualisationTabButtonPressed;
                
                _viewYDisplacementAcrossExperimentButton.clickable.clicked -= OnViewYDisplacementOverTimeButtonPressed;
                _viewVelocityCurveAcrossExperimentButton.clickable.clicked -= OnViewVelocityOverTimeButtonPressed;
            }


            private void OnDataVisualisationTabButtonPressed() => _analysisTool.ActiveTab = TabSelection.DataVisualisation;
            

            private void OnViewYDisplacementOverTimeButtonPressed()
            {
                DataDisplay display = GetWindow<DataDisplay>();
                if (display == null) return;
                
                display.Show();
            }

            private void OnViewVelocityOverTimeButtonPressed()
            {
                
            }
        }
    }    
}