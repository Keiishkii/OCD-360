#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class EyeDataAnalyisis : MonoBehaviour
{
    #region [ Editor ]
    [UnityEditor.CustomEditor(typeof(EyeDataAnalyisis))]
    public class EyeDataAnalyisisInspector : KeiishkiiLib.CustomInspector<EyeDataAnalyisis>
    {
        protected override void OnInspectorRender() 
        { 
            KeiishkiiLib.InspectorUtility.Button("Process File", false, () => _targetScript.Process());
        }
    }
    #endregion

    #region [ Serialised ]
        [SerializeField] private TextAsset _eyeDataFile;
        [SerializeField] private string _rowTitle;
    #endregion

    #region [ Unserialised ]
        private FibonacciSphere _fibonacciSphere;
    #endregion



    private void Awake() => _fibonacciSphere = FindObjectOfType<FibonacciSphere>();

    private void Process()
    {
        string filepath = AssetDatabase.GetAssetPath(_eyeDataFile);
        Debug.Log($"Asset Path: {filepath}");

        int row = 0;
        int coloumn = -1;

        _fibonacciSphere.GenerateFibonacciSphere();

        using (var fileStream = new FileStream(filepath, FileMode.Open))
        using (var streamReader = new StreamReader(fileStream))
        {
            while (!streamReader.EndOfStream)
            {
                string lineContent = streamReader.ReadLine();
                if (row == 0)
                {
                    string[] titles = lineContent.Split(',');
                    for (int i = 0; i < titles.Length; i++)
                    {
                        if (!String.Equals(_rowTitle, titles[i])) continue;

                        coloumn = i;
                        break;
                    }
                }
                else
                {
                    if (coloumn == -1) break;

                    char[] charArray = lineContent.ToCharArray();
                    List<int> commaIndexes = new List<int>();

                    int bracketIndetation = 0;
                    bool stringIndetation = false;
                    bool characterIndetation = false;
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        if (!(stringIndetation || characterIndetation || bracketIndetation > 0) && charArray[i] == ',') commaIndexes.Add(i);
                        else if (charArray[i] == '"') stringIndetation = !stringIndetation;
                        else if (charArray[i] == '\'') characterIndetation = !characterIndetation;
                        else if (charArray[i] == '(' || charArray[i] == '[' || charArray[i] == '{') bracketIndetation++;
                        else if (charArray[i] == ')' || charArray[i] == ']' || charArray[i] == '}') bracketIndetation--;
                    }

                    int substringStart = 0;

                    List<string> coloumnData = new List<string>();
                    for (int i = 0; i < commaIndexes.Count; i++)
                    {
                        coloumnData.Add(lineContent.Substring(substringStart, commaIndexes[i] - substringStart));
                        substringStart = commaIndexes[i] + 1;
                    }

                    if (commaIndexes.Count > 0) coloumnData.Add(lineContent.Substring(commaIndexes[^1] + 1, lineContent.Length - (commaIndexes[^1] + 2)));
                    
                    // remove '"float3(' + ')"'
                    string substring = coloumnData[coloumn].Substring(8, coloumnData[coloumn].Length - 10);
                    string[] elements = substring.Split(',');
                    for (int i = 0; i < elements.Length; i++)
                    {
                        // remove the f at the end of the string
                        elements[i] = elements[i].Substring(0, elements[i].Length - 1);
                    }

                    if (!(float.TryParse(elements[0], out float x) && float.TryParse(elements[1], out float y) && float.TryParse(elements[2], out float z))) continue;
                    Vector3 gazeDirection = Vector3.Normalize(new Vector3(x, y, z));

                    _fibonacciSphere.AddData(gazeDirection);
                }

                row++;
            }
        }

        _fibonacciSphere.LocateGlobalWeightingBounds();
        _fibonacciSphere.ColourVertices();
    }
}
#endif
