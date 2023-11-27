using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KeiishkiiLib;
using static KeiishkiiLib.InspectorUtility;
using System;

public class UnitSphereGenerator : MonoBehaviour
{
    #region [ Editor ]
    [UnityEditor.CustomEditor(typeof(UnitSphereGenerator))]
    public class UnitSphereGeneratorEditor : CustomInspector<UnitSphereGenerator>
    {
        protected override void OnInspectorRender()
        {
            Button("Generate Sphere With Random Points", true, () => _targetScript.GenerateSphereWithRandomPoints());
            Button("Generate Sphere With Equidistant Points", true, () => _targetScript.GenerateSphereWithEquidistantPoints());

            Separator();

            
            Button("Load File", true, () => _targetScript._data = _targetScript.ReadFromFile());

            if (_targetScript._data is not { } || _targetScript._points.Count == 0) return;
            Button("Generate Sphere With Equidistant Points", true, () => _targetScript.ProcessSphereWithGazeWeights());
        }
    }
    #endregion

    #region [ Serialised Fields ]
        [SerializeField] private int _pointCount;
        [SerializeField] private float _gizmoRadius;
        [SerializeField] private float _gizmoRenderRadius;
        [Range(0, 1)][SerializeField] private float _weightDistributionExponent;

        [SerializeField] private TextAsset _dataAsset;
        [SerializeField] private List<(Vector3, int)> _points = new List<(Vector3, int)>();

        [SerializeField] Color _colourA;
        [SerializeField] Color _colourB;
        [SerializeField] Color _colourC;
    #endregion

    #region [ Unserialised Fields ]
        private GazeData _data;

        private int _maxWeighting;
        private int _minWeighting;
    #endregion



    private void GenerateSphereWithRandomPoints()
    {
        _points.Clear();
        _maxWeighting = 0;
        _minWeighting = 0;

        for (int i = 0; i < _pointCount; i++)
        {
            float theta = UnityEngine.Random.Range(0, Mathf.PI * 2);
            float phi = UnityEngine.Random.Range(0, Mathf.PI * 2);

            _points.Add((new Vector3()
            {
                x = Mathf.Sin(phi) * Mathf.Cos(theta),
                y = Mathf.Sin(phi) * Mathf.Sin(theta),
                z = Mathf.Cos(phi),
            }, 0));
        }
    }

    private void GenerateSphereWithEquidistantPoints()
    {
        _points.Clear();
        _maxWeighting = 0;
        _minWeighting = 0;

        float phi = Mathf.PI * (Mathf.Sqrt(5) - 1);
        for (int i = 0; i < _pointCount; i++)
        {
            float y = 1 - (((float) i / (_pointCount - 1)) * 2);
            float radius = Mathf.Sqrt(1 - y * y);

            float theta = phi * i;

            float x = Mathf.Cos(theta) * radius;
            float z = Mathf.Sin(theta) * radius;

            _points.Add((new Vector3()
            {
                x = 0.5f * x,
                y = 0.5f * y,
                z = 0.5f * z,
            }, 0));
        }
    }
    
    private void ProcessSphereWithGazeWeights()
    {
        for (int frameIndex = 0; frameIndex < _data.frameData.Count; frameIndex++)
        {
            Vector3 gazePoint = _data.frameData[frameIndex].lookDirection;

            float closestSqrDistance = Vector3.SqrMagnitude(gazePoint - _points[0].Item1);
            int closestPoint = 0;

            for (int i = 1; i < _points.Count; i++)
            {
                float sqrMagnitude = Vector3.SqrMagnitude(gazePoint - _points[i].Item1);
                if (sqrMagnitude >= closestSqrDistance) continue;
                
                closestSqrDistance = sqrMagnitude;
                closestPoint = i;
            }

            (Vector3, int) weightedPoint = _points[closestPoint];
            weightedPoint.Item2++;

            _points[closestPoint] = weightedPoint;
        }

        _minWeighting = _points[0].Item2; _maxWeighting = _points[0].Item2;
        for (int i = 1; i < _points.Count; i++)
        {
            if (_minWeighting > _points[i].Item2) _minWeighting = _points[i].Item2;
            if (_maxWeighting < _points[i].Item2) _maxWeighting = _points[i].Item2;
        }

        Debug.Log("Max Weight:" + _maxWeighting);
        Debug.Log("Min Weight:" + _minWeighting);
    }

    private GazeData ReadFromFile() => JsonUtility.FromJson<GazeData>(_dataAsset.text);



    private void OnDrawGizmos()
    {
        Debug.Log($"Point Count: {_points.Count}");
        foreach((Vector3, int) weightedPoint in _points)
        {
            float weightValue = Mathf.InverseLerp(_minWeighting, _maxWeighting, weightedPoint.Item2);
            float scaledWeightedValue = Mathf.Sin(Mathf.Pow(weightValue, _weightDistributionExponent) * (Mathf.PI / 2f));          
            
            Gizmos.color = (scaledWeightedValue < 0.5f) ?
                Color.Lerp(_colourA, _colourB, scaledWeightedValue) :
                Color.Lerp(_colourB, _colourC, scaledWeightedValue);

            Gizmos.DrawSphere(weightedPoint.Item1 * _gizmoRadius, _gizmoRenderRadius);
        }
    }
}
