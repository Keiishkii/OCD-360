using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Parser : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private int _startIndex = 0;
    [SerializeField] private int _segmentLength = 0;
    [SerializeField] private float _sphereScale = 0;
    [SerializeField] private float _planeScale = 0;
    [SerializeField] private Material _gazeLineMaterial;
    #endregion

    #region [ Unserialised Fields ]
    private TextAsset _eyedata;
    private List<string> _objectNames = new List<string>();
    private List<Vector2> _uvCoordinates = new List<Vector2>();
    private List<Vector3> _gazeDirections = new List<Vector3>();
    private Mesh _sphereLineMesh;
    private Mesh _planeLineMesh;
    #endregion
    
    
    
    private void ParseEyeData()
    {
        _planeLineMesh = null;
        _sphereLineMesh = null;
        _gazeDirections.Clear();
        _uvCoordinates.Clear();
        _objectNames.Clear();
        
        if (_segmentLength <= 0) return;
        
        TextAsset data = Resources.Load<TextAsset>("2024-03-20 125119.eyedata");
        if (data == null) return;
        
        string[] rows = _eyedata.text.Split('\n');
        if (rows.Length <= 0 || _startIndex >= rows.Length) return;
        
        for (int i = Mathf.Max(1, _startIndex); i < Mathf.Min(rows.Length, _startIndex + _segmentLength); i++)
        {
            List<string> cells = ParseCsvLineFast(rows[i]);
            
            Vector3 normalisedGazeDirection = ParseFloat3Fast(cells[23]);
            
            // Skip false-flag rows
            if (Mathf.Approximately(normalisedGazeDirection.x, -1f) && Mathf.Approximately(normalisedGazeDirection.y, -1f) && Mathf.Approximately(normalisedGazeDirection.z, -1f))
                continue;

            _gazeDirections.Add(normalisedGazeDirection);
            
            Ray ray = new Ray(Vector3.zero, normalisedGazeDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                _objectNames.Add(hit.transform.gameObject.name);
                _uvCoordinates.Add(hit.textureCoord);
            }
            else
            {
                _objectNames.Add("");
                _uvCoordinates.Add(Vector2.zero);
            }
        }
        
        if (_gazeDirections.Count <= 0) return;
        
        int count = _gazeDirections.Count;
        int[] indices = new int[(count - 1) * 2];

        int idx = 0;
        for (int i = 0; i < count - 1; i++)
        {
            indices[idx++] = i;
            indices[idx++] = i + 1;
        }
        
        List<Vector3> verts = new List<Vector3>(_uvCoordinates.Count);
        verts.AddRange(_uvCoordinates.Select(uv => new Vector3(uv.x, uv.y, 0f)));

        _planeLineMesh = new Mesh();
        _planeLineMesh.SetVertices(verts);
        _planeLineMesh.SetIndices(indices, MeshTopology.Lines, 0);
        
        _sphereLineMesh = new Mesh();
        _sphereLineMesh.SetVertices(_gazeDirections);
        _sphereLineMesh.SetIndices(indices, MeshTopology.Lines, 0);
    }
    
    private static List<string> ParseCsvLineFast(string line)
    {
        List<string> result = new List<string>(32); // preallocate
        int len = line.Length;
        int i = 0;

        while (i < len)
        {
            bool quoted = false;
            if (line[i] == '"')
            {
                quoted = true;
                i++;
            }

            int start = i;

            if (quoted)
            {
                while (i < len)
                {
                    if (line[i] == '"' && (i + 1 == len || line[i + 1] == ','))
                    {
                        break;
                    }
                    i++;
                }

                string field = line.Substring(start, i - start);
                result.Add(field);

                i += 2;
            }
            else
            {
                while (i < len && line[i] != ',')
                    i++;

                string field = line.Substring(start, i - start);
                result.Add(field);

                i++;
            }
        }

        return result;
    }

    
    private static Vector3 ParseFloat3Fast(string s)
    {
        // Expect: float3(xf, yf, zf)

        int open = s.IndexOf('(');
        int close = s.LastIndexOf(')');
        if (open < 0 || close < 0) return Vector3.zero;

        int start = open + 1;
        int length = close - start;

        // Extract inside: "xf, yf, zf"
        string inner = s.Substring(start, length);

        // Manual parse without Split
        int c1 = inner.IndexOf(',');
        int c2 = inner.IndexOf(',', c1 + 1);

        float x = FastFloat(inner, 0, c1);
        float y = FastFloat(inner, c1 + 1, c2);
        float z = FastFloat(inner, c2 + 1, inner.Length);

        return new Vector3(x, y, z);
    }

    private static float FastFloat(string s, int start, int end)
    {
        // Trim manually
        while (start < end && s[start] == ' ') start++;
        while (end > start && s[end - 1] == ' ') end--;

        // Remove trailing 'f'
        if (s[end - 1] == 'f') end--;

        return float.Parse(s.Substring(start, end - start), CultureInfo.InvariantCulture);
    }



    
    
    


    private void OnValidate()
    {
        ParseEyeData();
    }

    private void Update()
    {
        DrawSphereGazeLine();
        DrawPlaneGazeLine();
    }

    private void DrawSphereGazeLine()
    {
        if (!_sphereLineMesh) return;

        Matrix4x4 matrix4X4 = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * _sphereScale);
        Graphics.DrawMesh(_sphereLineMesh, matrix4X4, _gazeLineMaterial, 0);
    }

    private void DrawPlaneGazeLine()
    {
        if (!_planeLineMesh) return;

        Matrix4x4 matrix4X4 = Matrix4x4.TRS(new Vector3(-5, -5, 0), Quaternion.identity, Vector3.one * _planeScale);
        Graphics.DrawMesh(_planeLineMesh, matrix4X4, _gazeLineMaterial, 0);
    }
}
