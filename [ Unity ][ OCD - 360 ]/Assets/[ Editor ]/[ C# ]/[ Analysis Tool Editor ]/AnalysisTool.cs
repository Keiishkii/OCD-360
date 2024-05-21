#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace MINELab
{
    public partial class AnalysisTool : EditorWindow
    {
        #region [ Static Instances ]
        private static AnalysisTool _instance;
        public static AnalysisTool Instance => _instance ? _instance : _instance = GetWindow<AnalysisTool>();
        public static AnalysisToolRuntimeObject RuntimeObject => AnalysisToolRuntimeObject.Instance;
        #endregion
        
        #region [ Serialised Fields ]

        #endregion
        
        #region [ Unserialised Fields ]
        private VisualTreeAsset _UXMLDocument;

        private readonly DataImportTab _dataImportTab = new ();
        private readonly DataVisualisationTab _dataVisualisationTab = new ();
        private readonly DataExportTab _dataExportTab = new ();
        public bool active;

        private TabSelection _activeTab;
        private TabSelection ActiveTab
        {
            get => _activeTab;
            set
            {
                switch (value)
                {
                    case TabSelection.DataImport:
                    {
                        _dataImportTab.TabEnabled = true;
                        _dataVisualisationTab.TabEnabled = false;
                        _dataExportTab.TabEnabled = false;
                    } break;
                    case TabSelection.DataVisualisation:
                    {
                        _dataImportTab.TabEnabled = false;
                        _dataVisualisationTab.TabEnabled = true;
                        _dataExportTab.TabEnabled = false;
                    } break;
                    case TabSelection.DataExport:
                    {
                        _dataImportTab.TabEnabled = false;
                        _dataVisualisationTab.TabEnabled = false;
                        _dataExportTab.TabEnabled = true;
                    } break;
                }
            }
        }
        private enum TabSelection
        {
            DataImport,
            DataVisualisation,
            DataExport
        }
        #endregion
        
        
        
        [MenuItem ("[ MINE Lab ]/Analysis Tool")]
        public static void  ShowWindow () => GetWindow<AnalysisTool>("Analysis Tool");

        private void CreateGUI()
        {
            active = true;

            InstantiateTabs();
            QueryElements();
            RegisterCallbacks();

            ActiveTab = TabSelection.DataImport;
        }

        public void OnDestroy()
        {
            UnregisterCallbacks();
            active = false;
        }
        
        private void InstantiateTabs()
        {
            _dataImportTab.Instantiate(this);
            _dataVisualisationTab.Instantiate(this);
            _dataExportTab.Instantiate(this);
        }
        
        private void QueryElements()
        {
            _UXMLDocument = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/[ Editor ]/[ UXML ]/AnalysisToolUIDocument.uxml");
            VisualElement root = _UXMLDocument.Instantiate();
            
            _dataImportTab.QueryElements(root);
            _dataVisualisationTab.QueryElements(root);
            _dataExportTab.QueryElements(root);
            
            rootVisualElement.Add(root);
        }
        
        private void RegisterCallbacks()
        {
            _dataImportTab.RegisterCallbacks();
            _dataVisualisationTab.RegisterCallbacks();
            _dataExportTab.RegisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            _dataImportTab.UnregisterCallbacks();
            _dataVisualisationTab.UnregisterCallbacks();
            _dataExportTab.UnregisterCallbacks();
        }
    }
}
#endif