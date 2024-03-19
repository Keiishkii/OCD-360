using System.Collections;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using KeiishkiiLib;
using static KeiishkiiLib.InspectorUtility;
#endif

public class DataRecorder : MonoBehaviour
{
    #if UNITY_EDITOR
    #region [ Editor ]
    [CustomEditor(typeof(DataRecorder))]
    private class DataRecorderEditor : CustomInspector<DataRecorder>
    {
        protected override void OnInspectorRender()
        {
            if (!_targetScript._dataRecording) Button("Begin Data Collection", false, () => _targetScript.BeginDataCollection());
            else Button("End Data Collection", false, () => _targetScript.EndDataCollection());
        }
    }
    #endregion
    #endif

    #region [ Serialised Field ]
        [SerializeField] private Transform _cameraTransform;
    #endregion

    #region [ Unserialised Fields ]
        private GazeData _data;
        private IEnumerator _dataCollectionCoroutine;
        private bool _dataRecording;
    #endregion



    private void BeginDataCollection()
    {
        Debug.Log("Starting Data Collection");

        _data = new GazeData();
        _dataRecording = true;

        if (_dataCollectionCoroutine != null) StopCoroutine(_dataCollectionCoroutine);
        StartCoroutine(_dataCollectionCoroutine = ProcessDataCollection());
    }

    private IEnumerator ProcessDataCollection()
    {
        float timeElapsed = 0;
        while (_dataRecording)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 0.1f;
            Vector3 gazeDirection = Quaternion.LookRotation(Vector3.Normalize(new Vector3(randomOffset.x, randomOffset.y, 1f))) * _cameraTransform.forward;

            _data.frameData.Add(new FrameData()
            {
                time = timeElapsed,
                gazeDirection = gazeDirection,
                lookDirection = _cameraTransform.forward
            });

            yield return new WaitForFixedUpdate();
            timeElapsed += Time.fixedDeltaTime;
        }
    }

    private void EndDataCollection()
    {
        if (_dataCollectionCoroutine != null) StopCoroutine(_dataCollectionCoroutine);
        _dataRecording = false;

        Debug.Log("Ending Data Collection");

        WriteToFile();
    }

    private void WriteToFile()
    {
        string filename = "GazeData.json";
        string directory = $"{Application.dataPath}/[ Data ]";

        string data = JsonUtility.ToJson(_data, true);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        File.WriteAllText($"{directory}/{filename}", data);
        Debug.Log("Data Saved");
    }
}
