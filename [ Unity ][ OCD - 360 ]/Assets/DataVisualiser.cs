#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DataVisualiser : MonoBehaviour
{
    #region [ Editor ]
    [CustomEditor(typeof(DataVisualiser))]
    public class DataVisualiserEditor : KeiishkiiLib.CustomInspector<DataVisualiser>
    {
        protected override void OnInspectorRender()
        {
            KeiishkiiLib.InspectorUtility.Button("Read Line", true, () => _targetScript.DisplayLine());
            for (int i = 0; i < _targetScript._titles.Count; i++)
            {
                int index = i;
                KeiishkiiLib.InspectorUtility.HorizontalScope(() =>
                {
                    KeiishkiiLib.InspectorUtility.TextLabel($"{((_targetScript._titles.Count > index) ? _targetScript._titles[index] : "undefined")}"); 
                    KeiishkiiLib.InspectorUtility.TextLabel($"{((_targetScript._content.Count > index) ? _targetScript._content[index] : "undefined")}"); 
                });
            }
        }
    }
    #endregion

    #region [ Serialised Field ]
    [SerializeField] private string _path;
    [SerializeField] private int _targetIndex;
    #endregion

    #region [ Unserailised Fields ]
    private List<string> _titles = new List<string>();
    private List<string> _content = new List<string>();
    private EyeFrameData _eyeFrameData;
    #endregion



    private void DisplayLine()
    {
        try
        {
            // Create a StreamReader
            using (StreamReader reader = new StreamReader(_path))
            {
                string titles = reader.ReadLine();
                _titles = titles?.Split(',').ToList();
                
                int lineIndex = 1;
                while (reader.ReadLine() is { } content)
                {
                    if (lineIndex == _targetIndex)
                    {
                        _content = SeparateLine(content);
                        break;
                    }
                    lineIndex++;
                }
            }
        }
        catch (Exception exp)
        {
            Debug.LogError(exp.Message);
        }
    }

    private List<string> SeparateLine(string content)
    {
        List<string> contentList = new List<string>();
        
        int startIndex = 0;
        int endIndex = 0;

        int bracketValue = 0;
        bool quoted = false;
        
        for (int i = 0; i < content.Length; i++)
        {
            char character = content[i];

            switch (content[i])
            {
                case '(':
                {
                    bracketValue++;
                } break;
                case ')':
                {
                    bracketValue--;
                } break;
                case '"':
                {
                    quoted = !quoted;
                } break;
                case ',' when (!quoted && bracketValue == 0):
                {
                    endIndex = i;

                    contentList.Add(content.Substring(startIndex, endIndex - startIndex));
                    
                    startIndex = endIndex + 1;
                } break;
            }
        }

        contentList.Add(content.Substring(startIndex));
        return contentList;
    }
}

public class EyeFrameData
{
    public bool UserPresent;
    public int FrameSequence;
    public int Timestamp;
    public int TimestampJ2000;
    
    public Eye LeftEye;
    public Eye RightEye;
    public Combined Combined;

    public int TrackingImprovementsCount;
    public List<string> TrackingImprovements = new List<string>();
}

public struct Eye
{
    public ExpressionData ExpressionData;

    public float PupilDiameterMm;
    public float EyeOpenness;
    public Vector3 GazeOriginMm;
    public Vector3 GazeDirectionNormalised;
    public Vector2 PupilPositionInSensorArea;
}

public struct ExpressionData
{
    public float EyeWide;
    public float EyeSqueeze;
    public float EyeFrown;
}

public struct Combined
{
    public float PupilDiameterMm;
    public float EyeOpenness;
    public Vector3 GazeOriginMm;
    public Vector3 GazeDirectionNormalised;
    public Vector2 PupilPositionInSensorArea;
    public bool ConvergenceDistanceValidity;
    public float ConvergenceDistanceMm;
}


#endif