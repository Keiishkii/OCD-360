using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MINELab
{
    public class DataManager : MonoBehaviour
    {
        #region [ Instance ]
        public static DataManager Instance => _instance ? _instance : _instance = FindObjectOfType<DataManager>();
        private static DataManager _instance;
        #endregion

        #region [ Serialised Fields ]
        public ExperimentData experimentData;
        #endregion


        public void WriteToFile()
        {
            string directory = $"{Application.persistentDataPath}/Data";
            string filename = $"Experiment Data - {($"{experimentData.experimentStartTime:d/MM/yyyy}").Replace('/', '-')}-{($"{experimentData.experimentStartTime:T}").Replace(':', '-')}";
            
            string json = JsonUtility.ToJson(experimentData, true);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            
            File.WriteAllText($"{directory}/{filename}.json", json);
        }
    }

    [Serializable]
    public class ExperimentData
    {
        public DateTime experimentStartTime;
        public DateTime experimentEndTime;
        
        public List<BlockData> blockData = new ();
    }

    [Serializable]
    public class BlockData
    {
        public List<TrialData> trialData = new ();
    }
    
    [Serializable]
    public class TrialData
    {
        public float rotationOffset;
        public List<Vector2> affectScaleValues = new List<Vector2>();
    }
}