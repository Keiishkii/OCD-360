#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor;

[ExecuteInEditMode]
public class LookConversionTest : MonoBehaviour
{
    [CustomEditor(typeof(LookConversionTest))]
    public class LookConversionTestEditor : KeiishkiiLib.CustomInspector<LookConversionTest>
    {
        protected override void OnInspectorRender()
        {
            KeiishkiiLib.InspectorUtility.Button("Deselect Glyph:", true, () => _targetScript._glyphIndex = -1);
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.HorizontalScope(() =>
            {
                KeiishkiiLib.InspectorUtility.Button("<-", true, () =>
                {
                    _targetScript._glyphIndex = (_targetScript._glyphIndex >= 0) ? _targetScript._glyphIndex - 1 : _targetScript.glyphs.Count - 1;
                    EditorWindow.GetWindow<SceneView>().Repaint();
                });

                if (_targetScript._glyphIndex < 0)
                {
                    KeiishkiiLib.InspectorUtility.TextLabel("No glyph is selected.");
                }
                else
                {
                    Glyph glyph = _targetScript.glyphs[_targetScript._glyphIndex];
                    KeiishkiiLib.InspectorUtility.TextField("", ref glyph.name);
                }
                KeiishkiiLib.InspectorUtility.Button("->", true, () =>
                {
                    _targetScript._glyphIndex = (_targetScript._glyphIndex < _targetScript.glyphs.Count - 1) ? _targetScript._glyphIndex + 1 : -1;
                    EditorWindow.GetWindow<SceneView>().Repaint();
                });
            });
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.Button("Add New Item:", true, () =>
            {
                _targetScript.glyphs.Add(new Glyph()
                {
                    name = "new glyph",
                    contours = new List<Contour>()
                    {
                        new Contour()
                        {
                            points = new List<Vector2>()
                            {
                                new Vector2(0.45f, 0.45f),
                                new Vector2(0.45f, 0.55f),
                                new Vector2(0.55f, 0.55f),
                                new Vector2(0.55f, 0.45f)
                            }
                        }
                    }
                });
                
                _targetScript._glyphIndex = _targetScript.glyphs.Count - 1;
                EditorWindow.GetWindow<SceneView>().Repaint();
            });
            
            KeiishkiiLib.InspectorUtility.FloatField("Handle Scale:", ref _targetScript.handleScale);
            if (KeiishkiiLib.InspectorUtility.ChangeCheck(() => KeiishkiiLib.InspectorUtility.VectorField("Look Direction:", ref _targetScript.direction)))
            {
                EditorWindow.GetWindow<SceneView>().Repaint();
            }
        }

        protected override void OnSceneRender()
        {
            if (_targetScript._glyphIndex == -1 || _targetScript._glyphIndex >= _targetScript.glyphs.Count) return;
            Glyph glyph = _targetScript.glyphs[_targetScript._glyphIndex];
            
            float halfScale = _targetScript.displayScale / 2f;
            Vector2 displayOffset = new (-halfScale, -halfScale);

            RenderGlyphVertexTransformHandles(glyph, displayOffset);
            RenderGlyphAddVertexButtons(glyph, displayOffset);
            RenderGlyphRemoveVertexButtons(glyph, displayOffset);
        }

        private void RenderGlyphVertexTransformHandles(in Glyph glyph, in Vector2 displayOffset)
        {
            float size = _targetScript.handleScale;
            for (int i = 0; i < glyph.contours.Count; i++)
            {
                for (int j = 0; j < glyph.contours[i].points.Count; j++)
                {
                    Vector2 handlePosition = displayOffset + (glyph.contours[i].points[j] * _targetScript.displayScale);
                    Vector2 glyphPosition = (Handles.PositionHandle(handlePosition, Quaternion.identity) - new Vector3(displayOffset.x, displayOffset.y, 0)) / _targetScript.displayScale;
                    glyph.contours[i].points[j] = glyphPosition;
                }
            }
        }

        private void RenderGlyphAddVertexButtons(in Glyph glyph, in Vector2 displayOffset)
        {
            float size = _targetScript.handleScale;
            for (int i = 0; i < glyph.contours.Count; i++)
            {
                for (int j = 0; j < glyph.contours[i].points.Count - 1; j++)
                {
                    Vector2 midPoint = (glyph.contours[i].points[j] + glyph.contours[i].points[j + 1]) / 2f;
                    Vector2 handlePosition = displayOffset + (midPoint * _targetScript.displayScale);

                    Handles.color = new Color(0.32f, 1f, 0.38f);
                    if (Handles.Button(handlePosition, Quaternion.identity, size, size, Handles.RectangleHandleCap))
                    {
                        glyph.contours[i].points.Insert(j + 1, midPoint);
                    }
                }
            }
        }

        private void RenderGlyphRemoveVertexButtons(in Glyph glyph, in Vector2 displayOffset)
        {
            float size = _targetScript.handleScale;
            Vector2 offset = new Vector2(0.0025f, 0.0025f) * _targetScript.displayScale;
            
            for (int i = 0; i < glyph.contours.Count; i++)
            {
                for (int j = 0; j < glyph.contours[i].points.Count; j++)
                {
                    Vector2 handlePosition = displayOffset + offset + (glyph.contours[i].points[j] * _targetScript.displayScale);
                    
                    Handles.color = new Color(0.77f, 0.05f, 0.05f);
                    if (Handles.Button(handlePosition, Quaternion.identity, size, size, Handles.RectangleHandleCap))
                    {
                        glyph.contours[i].points.RemoveAt(j);
                    }
                }
            }
        }
    }
    
    #region [ Serialised Fields ]
    [SerializeField] private Vector2 direction;
    [SerializeField] private float displacement;
    [SerializeField] private float displayScale;
    [SerializeField] private float handleScale = 1f;
    [SerializeField] private List<Glyph> glyphs = new List<Glyph>();
    [SerializeField] private float displayGlyphOffset = 0;
    [SerializeField] private Transform planeTransform;
    [SerializeField] private Texture _addVertexTexture;
    [SerializeField] private Texture _removeVertexTexture;
    #endregion

    #region [ Unserialised Fields ]
    private int _glyphIndex;
    #endregion

    
    
    private void Start()
    {
        _glyphIndex = -1;
    }

    private void Update()
    {
        if (planeTransform is not { }) return;
        
        planeTransform.localScale = Vector3.one * displayScale;
    }
    
    private void OnDrawGizmos()
    {
        DrawGizmosDisplay(out Vector2 textureCoordinate);
        DrawGizmosGlyphs();
        DrawGizmosIntersections(textureCoordinate);
    }

    private void DrawGizmosDisplay(out Vector2 textureCoordinate)
    {
        textureCoordinate = Vector2.zero;
        
        Gizmos.color = Color.gray;

        float halfScale = displayScale / 2f;
        Vector2 displayOffset = new Vector2(-halfScale, -halfScale);
        
        Gizmos.DrawCube(new Vector3(-halfScale, -halfScale, 0), new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(new Vector3(halfScale, -halfScale, 0), new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(new Vector3(-halfScale, halfScale, 0), new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(new Vector3(halfScale, halfScale, 0), new Vector3(0.05f, 0.05f, 0.05f));

        Vector3 normalisedDirection = Quaternion.Euler(new Vector3(direction.x, direction.y)) * Vector3.forward;
        Gizmos.DrawLine(Vector3.zero, normalisedDirection * displacement);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(normalisedDirection * displacement, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(normalisedDirection * displacement, 0.05f);

        Ray ray = new Ray(Vector3.zero, normalisedDirection);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            textureCoordinate = hit.textureCoord;

            Vector2 offsetTextureCoordinate = displayOffset + (textureCoordinate * displayScale);
            
            Gizmos.DrawCube(new Vector3(-halfScale, offsetTextureCoordinate.y, 0), new Vector3(0.025f, 0.025f, 0.025f));
            Gizmos.DrawCube(new Vector3(halfScale, offsetTextureCoordinate.y, 0), new Vector3(0.025f, 0.025f, 0.025f));
            
            Gizmos.DrawCube(new Vector3(offsetTextureCoordinate.x, -halfScale, 0), new Vector3(0.025f, 0.025f, 0.025f));
            Gizmos.DrawCube(new Vector3(offsetTextureCoordinate.x, halfScale, 0), new Vector3(0.025f, 0.025f, 0.025f));
            
            Gizmos.DrawCube(new Vector3(offsetTextureCoordinate.x, offsetTextureCoordinate.y, 0), new Vector3(0.025f, 0.025f, 0.025f));
        
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(new Vector3(offsetTextureCoordinate.x, offsetTextureCoordinate.y, 0), new Vector3(0.035f, 0.035f, 0.035f));
        }
    }

    private void DrawGizmosGlyphs()
    {
        float halfScale = displayScale / 2f;
        Vector3 displayOffset = new Vector3(-halfScale, -halfScale, displayGlyphOffset);
        
        Random.InitState(0);
        foreach (Glyph glyph in glyphs)
        {
            Gizmos.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            foreach (Contour contour in glyph.contours)
            {
                for (int i = 0; i < contour.points.Count - 1; i++)
                {
                    Vector3 vertexA = new Vector3(contour.points[i].x, contour.points[i].y, 0);   
                    Vector3 vertexB = new Vector3(contour.points[i + 1].x, contour.points[i + 1].y, 0);

                    Vector3 offsetVertexA = displayOffset + (vertexA * displayScale);
                    Vector3 offsetVertexB = displayOffset + (vertexB * displayScale);
                    
                    Gizmos.DrawLine(offsetVertexA, offsetVertexB);
                }

                if (contour.points.Count > 1)
                {
                    Vector3 vertexA = new Vector3(contour.points[0].x, contour.points[0].y, 0);
                    Vector3 vertexB = new Vector3(contour.points[^1].x, contour.points[^1].y, 0);   

                    Vector3 offsetVertexA = displayOffset + (vertexA * displayScale);
                    Vector3 offsetVertexB = displayOffset + (vertexB * displayScale);
                    
                    Gizmos.DrawLine(offsetVertexA, offsetVertexB);
                }
            }
        }
    }

    private void DrawGizmosIntersections(in Vector2 textureCoordinate)
    {
        float halfScale = displayScale / 2f;
        Vector3 displayOffset = new Vector3(-halfScale, -halfScale, displayGlyphOffset);
        
        Vector2 intersectionVertexA = new (textureCoordinate.x, textureCoordinate.y);
        Vector2 intersectionVertexB = new (textureCoordinate.x, 0);
        
        Vector3 intersectionVertexA3D = new (intersectionVertexA.x, intersectionVertexA.y, displayGlyphOffset);
        Vector3 intersectionVertexB3D = new (intersectionVertexB.x, intersectionVertexB.y, displayGlyphOffset);   
            
        Vector3 offsetVertexA = displayOffset + (intersectionVertexA3D * displayScale);
        Vector3 offsetVertexB = displayOffset + (intersectionVertexB3D * displayScale);

        (bool collision, List<Glyph> glyphs) = CheckCollision(textureCoordinate);
        Gizmos.color = (collision) ? Color.red : Color.green;
        Gizmos.DrawLine(offsetVertexA, offsetVertexB);

        if (!collision) return;
        foreach (Glyph glyph in glyphs)
        {
            Debug.Log(glyph.name);
        }
    }



    private (bool, List<Glyph>) CheckCollision(in Vector2 textureCoordinate)
    {
        Vector2 intersectionVertexA = new (textureCoordinate.x, textureCoordinate.y);
        Vector2 intersectionVertexB = new (textureCoordinate.x, 0);

        List<Glyph> collidedGlyphs = new List<Glyph>();
        foreach (Glyph glyph in glyphs)
        {
            int intersections = 0;
            foreach (Contour contour in glyph.contours)
            {
                for (int i = 0; i < contour.points.Count - 1; i++)
                {
                    Vector2 vertexA = new (contour.points[i].x, contour.points[i].y);   
                    Vector2 vertexB = new (contour.points[i + 1].x, contour.points[i + 1].y);

                    if (FasterLineSegmentIntersection(intersectionVertexA, intersectionVertexB, vertexA, vertexB)) intersections++;
                }

                if (contour.points.Count > 1)
                {
                    Vector2 vertexA = new (contour.points[0].x, contour.points[0].y);
                    Vector2 vertexB = new (contour.points[^1].x, contour.points[^1].y);

                    if (FasterLineSegmentIntersection(intersectionVertexA, intersectionVertexB, vertexA, vertexB)) intersections++;
                }
            }
            
            if (intersections % 2 == 1) collidedGlyphs.Add(glyph);
        }

        return (collidedGlyphs.Count == 0) ? (false, null) : (true, collidedGlyphs);
    }
    
    bool FasterLineSegmentIntersection (Vector2 line1point1, Vector2 line1point2, Vector2 line2point1, Vector2 line2point2) {
 
        Vector2 a = line1point2 - line1point1;
        Vector2 b = line2point1 - line2point2;
        Vector2 c = line1point1 - line2point1;
 
        float alphaNumerator = b.y * c.x - b.x * c.y;
        float betaNumerator  = a.x * c.y - a.y * c.x;
        float denominator    = a.y * b.x - a.x * b.y;
 
        if (denominator == 0) {
            return false;
        } else if (denominator > 0) {
            if (alphaNumerator < 0 || alphaNumerator > denominator || betaNumerator < 0 || betaNumerator > denominator) {
                return false;
            }
        } else if (alphaNumerator > 0 || alphaNumerator < denominator || betaNumerator > 0 || betaNumerator < denominator) {
            return false;
        }
        return true;
    }
}

[Serializable]
public class Glyph
{
    public string name;
    public List<Contour> contours = new List<Contour>();
}

[Serializable]
public class Contour
{
    public List<Vector2> points = new List<Vector2>();
}
#endif