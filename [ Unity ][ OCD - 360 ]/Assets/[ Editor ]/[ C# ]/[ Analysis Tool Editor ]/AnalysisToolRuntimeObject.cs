using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MINELab
{
    public class AnalysisToolRuntimeObject : MonoBehaviour
    {
        #region [ Static Instances ]
        private static AnalysisToolRuntimeObject _instance;
        public static AnalysisToolRuntimeObject Instance
        {
            get
            {
                if (!Application.isPlaying) return null;
                if (_instance is null) FindObjectOfType<AnalysisToolRuntimeObject>();
                if (_instance is not null) return _instance;

                GameObject runtimeObjectPrefab = Instantiate(Resources.Load("[ Editor ]/Analysis Tool Runtime Object")) as GameObject;
                if (runtimeObjectPrefab is null) return null;

                return _instance ??= runtimeObjectPrefab.GetComponent<AnalysisToolRuntimeObject>();
            }
        }
        #endregion

        #region [ Serialised Fields ]
        public Timeline timeline = new ();
        #endregion

        private void Awake() => timeline.Instantiate(this);
        private void OnEnable() => timeline.OnEnable();
        private void OnDisable() => timeline.OnDisable();
    }

    [Serializable]
    public class Timeline
    {
        #region [ Serailised Fields ]
        [SerializeField] private Transform _hmdMeshTransform;
        [SerializeField] private Transform _leftHandMeshTransform;
        [SerializeField] private Transform _rightHandMeshTransform;
        #endregion
        
        #region [ Unserialised Fields ]
        private AnalysisTool _analysisTool;
        private AnalysisToolRuntimeObject _analysisToolRuntimeObject;
        private TimelineController _timelineController;
        private IEnumerator _playbackCoroutine;
        private bool _playing = false;
        #endregion
        
        
        
        public void Instantiate(AnalysisToolRuntimeObject analysisToolRuntimeObject)
        {
            TimelineController.Instance.TimeIndex = 0;
            
            _analysisTool = AnalysisTool.Instance;
            _analysisToolRuntimeObject = analysisToolRuntimeObject;
            _timelineController = TimelineController.Instance;
        }

        public void OnEnable() => _timelineController.timeIndexChangeEvent.AddListener(OnTimeIndexChanged);
        public void OnDisable() => _timelineController.timeIndexChangeEvent.RemoveListener(OnTimeIndexChanged);
        
        private void OnTimeIndexChanged(int timeIndex)
        {
            /*
            _hmdMeshTransform.SetPositionAndRotation(sample.playerHeadPosition.ConvertToVector3(), sample.playerLeftHandRotation.ConvertToQuaternion());
            _leftHandMeshTransform.SetPositionAndRotation(sample.playerLeftHandPosition.ConvertToVector3(), sample.playerLeftHandRotation.ConvertToQuaternion());
            _rightHandMeshTransform.SetPositionAndRotation(sample.playerRightHandPosition.ConvertToVector3(), sample.playerRightHandRotation.ConvertToQuaternion());
            */
        }
        
        
        
        public void Play()
        {
            if (_playing) return;
            _playing = true;

            //_analysisToolRuntimeObject.StartCoroutine(_playbackCoroutine = PlaybackCoroutine());
        }

        public void Pause()
        {
            Debug.Log("Pause: 1");
            if (!_playing) return;
            _playing = false;

            Debug.Log("Pause: 2");
            if (_playbackCoroutine is not null) _analysisToolRuntimeObject.StopCoroutine(_playbackCoroutine);
            Debug.Log("Ended");
        }

        public void Stop()
        {
            Debug.Log("Pause: 1");
            _playing = false;
            
            Debug.Log("Pause: 2");
            TimelineController.Instance.TimeIndex = 0;

            if (_playbackCoroutine is not null) _analysisToolRuntimeObject.StopCoroutine(_playbackCoroutine);
            Debug.Log("Ended");
        }
    }
}