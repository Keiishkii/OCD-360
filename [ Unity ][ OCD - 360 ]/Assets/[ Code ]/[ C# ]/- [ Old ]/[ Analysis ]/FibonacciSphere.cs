using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FibonacciSphere : MonoBehaviour
{
    #region [ Editor ]
    [UnityEditor.CustomEditor(typeof(FibonacciSphere))]
    public class FibonacciSphereInspector : KeiishkiiLib.CustomInspector<FibonacciSphere>
    {
        protected override void OnInspectorRender()
        {
            if (KeiishkiiLib.InspectorUtility.ChangeCheck(() => 
            {
                KeiishkiiLib.InspectorUtility.BooleanField("Use Constant Minimum and Maximum", ref _targetScript._useConstantMinimumAndMaximum);
                if (_targetScript._useConstantMinimumAndMaximum)
                {
                    KeiishkiiLib.InspectorUtility.IntField("Minimum", ref _targetScript._minimumConstantFrequency);
                    KeiishkiiLib.InspectorUtility.IntField("Maximum", ref _targetScript._maximumConstantFrequency);
                }

                KeiishkiiLib.InspectorUtility.FloatSlider("Render Alpha", ref _targetScript._renderAlpha, 0, 1);

                KeiishkiiLib.InspectorUtility.BooleanField("Use Linear Weighting", ref _targetScript._useLinearWeighting);
                if (!_targetScript._useLinearWeighting)
                {
                    KeiishkiiLib.InspectorUtility.FloatSlider("Heat Map Exponent", ref _targetScript._heatMapWeightingExponent, 0, 1);
                }
            }))
            {
                _targetScript.ColourVertices();
            }
        

            //KeiishkiiLib.InspectorUtility.Button("Generate Fibonacci Sphere Mesh", true, () => _targetScript.GenerateSphere());
            //KeiishkiiLib.InspectorUtility.Button("Generate Fibonacci Sphere Gizmos", true, () => _targetScript.GenerateFibonacciSphere());
            //KeiishkiiLib.InspectorUtility.Button("Reset Display Data", true, () => _targetScript.ResetData());
        }
    }
    #endregion

    #region [ Serialised Fields ]
    [SerializeField] public int pointCount;

        [SerializeField] private Gradient _heatMapGradient = new Gradient();
        [Range(0, 1)][SerializeField] private float _heatMapWeightingExponent = 0.25f;
        [SerializeField] private float _fibonacciSphereRadius;
        [SerializeField] private float _gizmoRenderRadius;
    #endregion

    #region [ Unserialised Fields ]
        private MeshFilter _meshFilter;
        private Mesh _mesh;
        private List<Vector3> _vertices = new List<Vector3>();

        private bool _useConstantMinimumAndMaximum;
        private bool _useLinearWeighting;

        private int[] _pointFrequencies;
        private int _maximumVariableFrequency;
        private int _maximumConstantFrequency;
        private int _minimumVariableFrequency;
        private int _minimumConstantFrequency;
        private float _renderAlpha;
    #endregion



    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }


    
    public void GenerateFibonacciSphere()
    {
        _mesh = new Mesh();

        _vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();

        List<Vector3> points = GenerateVertices().ToList();

        GK.ConvexHullCalculator calculator = new GK.ConvexHullCalculator();
        calculator.GenerateHull(points, false, ref _vertices, ref triangles, ref normals);

        ResetData(_vertices.Count);

        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = triangles.ToArray().Reverse().ToArray();
        _mesh.normals = normals.ToArray();

        _meshFilter.mesh = _mesh;
    }

    private Vector3[] GenerateVertices()
    {
        Vector3[] points = new Vector3[pointCount];

        float phi = Mathf.PI * (Mathf.Sqrt(5) - 1);
        for (int i = 0; i < pointCount; i++)
        {
            float y = 1 - (((float)i / (pointCount - 1)) * 2);
            float radius = Mathf.Sqrt(1 - y * y);

            float theta = phi * i;

            float x = Mathf.Cos(theta) * radius;
            float z = Mathf.Sin(theta) * radius;

            points[i] = new Vector3()
            {
                x = 0.5f * x,
                y = 0.5f * y,
                z = 0.5f * z,
            };
        }

        return points;
    }



    public void ResetData(int vertexCount)
    {
        _pointFrequencies = new int[vertexCount];

        _maximumVariableFrequency = 0;
        _minimumVariableFrequency = 0;
    }

    public void AddData(Vector3 normalisedGazeDirection)
    {
        float closestSqrDistance = Vector3.SqrMagnitude(normalisedGazeDirection - _vertices[0]);
        int closestPoint = 0;

        for (int i = 1; i < pointCount; i++)
        {
            float sqrMagnitude = Vector3.SqrMagnitude(normalisedGazeDirection - _vertices[i]);
            if (sqrMagnitude >= closestSqrDistance) continue;

            closestSqrDistance = sqrMagnitude;
            closestPoint = i;
        }

        _pointFrequencies[closestPoint]++;
    }

    public void LocateGlobalWeightingBounds()
    {
        _minimumVariableFrequency = _pointFrequencies[0];

        _maximumVariableFrequency = _pointFrequencies[0];

        for (int i = 1; i < _pointFrequencies.Length; i++)
        {
            if (_minimumVariableFrequency > _pointFrequencies[i]) _minimumVariableFrequency = _pointFrequencies[i];
            if (_maximumVariableFrequency < _pointFrequencies[i]) _maximumVariableFrequency = _pointFrequencies[i];
        }

        Debug.Log($"Minimum: {_minimumVariableFrequency}");
        Debug.Log($"Maximum: {_maximumVariableFrequency}");
    }

    public void ColourVertices()
    {
        Color[] colours = new Color[_vertices.Count];
        for (int i = 0; i < _vertices.Count; i++)
        {
            float min = (_useConstantMinimumAndMaximum) ? _minimumConstantFrequency : _minimumVariableFrequency;
            float max = (_useConstantMinimumAndMaximum) ? _maximumConstantFrequency : _maximumVariableFrequency;

            float weightValue = Mathf.InverseLerp(min, max, _pointFrequencies[i]);
            
            Color colour = (_useLinearWeighting) ? 
                _heatMapGradient.Evaluate(weightValue) : 
                _heatMapGradient.Evaluate(Mathf.Sin(Mathf.Pow(weightValue, _heatMapWeightingExponent) * (Mathf.PI / 2f)));

            colour.a = _renderAlpha;
            colours[i] = colour;
        }

        _mesh.colors = colours;
    }




    /*
    public void GenerateFibonacciSphere()
    {
        ResetData();

        _vertices = new Vector3[pointCount];

        float phi = Mathf.PI * (Mathf.Sqrt(5) - 1);
        for (int i = 0; i < pointCount; i++)
        {
            float y = 1 - (((float)i / (pointCount - 1)) * 2);
            float radius = Mathf.Sqrt(1 - y * y);

            float theta = phi * i;

            float x = Mathf.Cos(theta) * radius;
            float z = Mathf.Sin(theta) * radius;

            _vertices[i] = new Vector3()
            {
                x = 0.5f * x,
                y = 0.5f * y,
                z = 0.5f * z,
            };
        }
    }

    private void OnDrawGizmos()
    {
        Debug.Log($"Minimum: {_minimumFrequency}, Maximum: {_maximumFrequency}");

        if (ReferenceEquals(_vertices, null) || pointCount != _vertices.Length) return;
        for (int i = 0; i < pointCount; i++)
        {
            float weightValue = Mathf.InverseLerp(_minimumFrequency, _maximumFrequency, _pointFrequencies[i]);
            float scaledWeightedValue = Mathf.Sin(Mathf.Pow(weightValue, _heatMapWeightingExponent) * (Mathf.PI / 2f));

            Gizmos.color = _heatMapGradient.Evaluate(scaledWeightedValue);
            Gizmos.DrawSphere(_vertices[i] * _fibonacciSphereRadius, _gizmoRenderRadius);
        }
    }
    */
}
