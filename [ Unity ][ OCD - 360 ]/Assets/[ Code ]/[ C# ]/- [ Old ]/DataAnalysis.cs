using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KeiishkiiLib;
using static KeiishkiiLib.InspectorUtility;

public class DataAnalysis : MonoBehaviour
{
    #region [ Editor ]
    [CustomEditor(typeof(DataAnalysis))]
    private class DataRecorderEditor : CustomInspector<DataAnalysis>
    {
        protected override void OnInspectorRender()
        {
            if (!Application.isPlaying) return;
            Button("Load Display Data", true, () => _targetScript.LoadNewDataToDisplay());

            if (_targetScript._data is not { }) return;
            Button($"{((_targetScript._displayAsFull) ? "Display At Time Point Data" : "Display Full Data")}", true, () => 
            {
                _targetScript._displayAsFull = !_targetScript._displayAsFull;
                _targetScript.RefreshDataDisplay();
            });

            RenderDisplay(_targetScript._data);
        }

        private void RenderDisplay(GazeData data)
        {
            if (!_targetScript._displayAsFull)
            {
                if (ChangeCheck(() =>
                {
                    FloatSlider("Time", ref _targetScript._timePoint, 0, (data.frameData.Count - 1) * Time.fixedDeltaTime);
                    IntField("Render Offset", ref _targetScript._partialDispayOffset);
                    _targetScript._partialDispayOffset = Mathf.Clamp(_targetScript._partialDispayOffset, 0, data.frameData.Count - 1);
                }))
                {
                    _targetScript.RefreshDataDisplay();
                }
            }
        }
    }
    #endregion

    #region [ Serialised Field ]
    [SerializeField] private TextAsset _dataAsset;
    [SerializeField] private LineRenderer _gazeDirectionLineRenderer;
    [SerializeField] private LineRenderer _headDirectionLineRenderer;
    [SerializeField] private GameObject _timePointGaze;
    [SerializeField] private GameObject _timePointLook;
    [SerializeField] private Transform _roomTransform;

    [SerializeField] private float _lineDisplayDistanceForGazeFromCenter;
    [SerializeField] private float _lineDisplayDistanceForHeadDirectionFromCenter;

    [SerializeField] private float _targetDisplayDistanceForGazeFromCenter;
    [SerializeField] private float _targetDisplayDistanceForHeadDirectionFromCenter;

    [SerializeField] private int _partialDispayOffset = 0;
    #endregion

    #region [ Unserialised Fields ]
        private bool _displayAsFull;
        private GazeData _data;
        private float _timePoint;
        private Transform _timePointGazeTransform;
        private Transform _timePointLookTransform;
    #endregion




    private void Awake()
    {
        _timePointGazeTransform = _timePointGaze.transform;
        _timePointLookTransform = _timePointLook.transform;
    }

    private void LoadNewDataToDisplay()
    {
        _data = ReadFromFile();
        RefreshDataDisplay();
    }

    private void RefreshDataDisplay()
    {
        if (_data is not {}) return;

        if (_displayAsFull) DisplayAsFull();
        else DisplayAtTime();
    }

    private void DisplayAsFull()
    {
        SetTargetState(false);

        SetSampleCount(_data.frameData.Count);
        for (int i = 0; i < _data.frameData.Count; i++)
        {
            _gazeDirectionLineRenderer.SetPosition(i, _roomTransform.position + _data.frameData[i].gazeDirection * _lineDisplayDistanceForGazeFromCenter);
            _headDirectionLineRenderer.SetPosition(i, _roomTransform.position + _data.frameData[i].lookDirection * _lineDisplayDistanceForHeadDirectionFromCenter);
        }
    }

    private void DisplayAtTime()
    {
        int index = Mathf.FloorToInt(_timePoint / Time.fixedDeltaTime);
        index = Mathf.Clamp(index, 0, _data.frameData.Count - 1);

        SetTargetState(true);

        _timePointGazeTransform.SetPositionAndRotation(
            _data.frameData[index].gazeDirection * _targetDisplayDistanceForGazeFromCenter,
            Quaternion.LookRotation(_data.frameData[index].gazeDirection));

        _timePointLookTransform.SetPositionAndRotation(
            _data.frameData[index].lookDirection * _targetDisplayDistanceForHeadDirectionFromCenter,
            Quaternion.LookRotation(_data.frameData[index].lookDirection));

        List<(Vector3, Vector3)> vectorDirections = new List<(Vector3, Vector3)>();
        for (int i = Mathf.Max(0, index - _partialDispayOffset); i < Mathf.Min(_data.frameData.Count - 1, index + _partialDispayOffset); i++) vectorDirections.Add((
            _roomTransform.position + _data.frameData[i].gazeDirection * _lineDisplayDistanceForGazeFromCenter,
            _roomTransform.position + _data.frameData[i].lookDirection * _lineDisplayDistanceForHeadDirectionFromCenter
        ));

        SetSampleCount(vectorDirections.Count);
        for (int i = 0; i < vectorDirections.Count; i++)
        {
            _gazeDirectionLineRenderer.SetPosition(i, vectorDirections[i].Item1);
            _headDirectionLineRenderer.SetPosition(i, vectorDirections[i].Item2);
        }
    }

    private void SetTargetState(bool enabled)
    {
        _timePointGaze.SetActive(enabled);
        _timePointLook.SetActive(enabled);
    }

    private void SetSampleCount(int sampleCount)
    {
        _gazeDirectionLineRenderer.positionCount = sampleCount;
        _headDirectionLineRenderer.positionCount = sampleCount;
    }


    private GazeData ReadFromFile() => JsonUtility.FromJson<GazeData>(_dataAsset.text);
}
