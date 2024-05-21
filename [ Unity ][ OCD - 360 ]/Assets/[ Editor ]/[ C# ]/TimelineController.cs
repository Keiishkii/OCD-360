#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MINELab
{
    class TimelineController : EditorWindow 
    {
        #region [ Static Instances ]
        private static TimelineController _instance;
        public static TimelineController Instance => _instance ? _instance : _instance = GetWindow<TimelineController>();
        #endregion
        
        #region [ Serialised Fields ]

        #endregion
        
        #region [ Unserialised Fields ]
        private AnalysisTool _analysisTool;
        private VisualTreeAsset _UXMLDocument;
        private VisualElement _rootVisualElement;
        private Button _playButton;
        private Button _pauseButton;
        private Button _stopButton;
        private Slider _timelineSlider;
        public bool active;
        
        private int _timeIndex;
        public UnityEvent<int> timeIndexChangeEvent = new UnityEvent<int>();
        public int TimeIndex
        {
            get => _timeIndex;
            set
            {
                _timeIndex = value;
                timeIndexChangeEvent.Invoke(value);
            }
        }
        #endregion

        #region [ Data ]
        
        #endregion


        [MenuItem ("[ MINE Lab ]/Timeline Controller")]
        public static void  ShowWindow () => GetWindow<TimelineController>("Timeline Controller");
        
        private void CreateGUI()
        {
            _analysisTool = AnalysisTool.Instance;
            
            _UXMLDocument = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/[ Editor ]/[ UXML ]/TimelineControllerUIDocument.uxml");
            _rootVisualElement = _UXMLDocument.Instantiate();

            _playButton = _rootVisualElement.Q<Button>("PlayButton");
            _pauseButton = _rootVisualElement.Q<Button>("PauseButton");
            _stopButton = _rootVisualElement.Q<Button>("StopButton");

            _timelineSlider = _rootVisualElement.Q<Slider>("TimelineSlider");
            
            rootVisualElement.Add(_rootVisualElement);
            active = true;

            RegisterCallbacks();
        }

        private void OnDestroy()
        {
            UnregisterCallbacks();
            active = false;
        }

        private void RegisterCallbacks()
        {
            timeIndexChangeEvent.AddListener(OnTimeIndexChanged);
            
            _playButton.clickable.clicked += OnPlayButtonPressed;
            _pauseButton.clickable.clicked += OnPauseButtonPressed;
            _stopButton.clickable.clicked += OnStopButtonPressed;
            
            _timelineSlider.RegisterValueChangedCallback(OnTimelineSliderValueChanged);
        }

        private void UnregisterCallbacks()
        {
            timeIndexChangeEvent.RemoveListener(OnTimeIndexChanged);

            
            _playButton.clickable.clicked -= OnPlayButtonPressed;
            _pauseButton.clickable.clicked -= OnPauseButtonPressed;
            _stopButton.clickable.clicked -= OnStopButtonPressed;
            _timelineSlider.UnregisterValueChangedCallback(OnTimelineSliderValueChanged);
        }

        

        private void OnPlayButtonPressed()
        {
            Debug.Log("Play");
            AnalysisTool.RuntimeObject.timeline.Play();
        }

        private void OnPauseButtonPressed()
        {
            Debug.Log("Pause");   
            AnalysisTool.RuntimeObject.timeline.Pause();
        }

        private void OnStopButtonPressed()
        {
            Debug.Log("Stop");   
            AnalysisTool.RuntimeObject.timeline.Stop();
        }

        private void OnTimelineSliderValueChanged(ChangeEvent<float> changeEvent)
        {
            Debug.Log($"Value: {changeEvent.newValue}");
            TimeIndex = Mathf.RoundToInt(changeEvent.newValue);
        }

        private void OnTimeIndexChanged(int index) => _timelineSlider.SetValueWithoutNotify(index);
        private void OnGUI()
        {
            
        }
    }
}
#endif

