#if UNITY_EDITOR
    using System;
    using UnityEditor;
    using UnityEngine;
    using static UnityEditor.EditorGUILayout;
    using Object = UnityEngine.Object;

namespace KeiishkiiLib
{
    public static class InspectorUtility
    {
        #region Component
        // - - - 
            public static void Separator()
            {
                EditorGUILayout.Separator();
            }

            public static void TextLabel(in string text) => LabelField(text);
            public static void TextLabel(in string text, GUIStyle style) => LabelField(text, style);
            public static void TextLabel(in string text, GUILayoutOption[] layoutOption) => LabelField(text, layoutOption);

            public static void Button(in string buttonName, in bool visibleInEditMode, in Action action)
            {
                if ((visibleInEditMode || Application.isPlaying) && GUILayout.Button(buttonName)) action.Invoke();
            }
            
            public static void Button(in string buttonName, in GUIStyle style, in bool visibleInEditMode, in Action action)
            {
                if ((visibleInEditMode || Application.isPlaying) && GUILayout.Button(buttonName, style)) action.Invoke();
            }
            
            public static void Foldout(in string foldoutName, ref bool foldoutState, in Action action)
            {
                foldoutState = EditorGUILayout.Foldout(foldoutState, foldoutName);
                if (foldoutState)
                {
                    action.Invoke();
                }
            }
        // - - - 
        #endregion

        #region Fields
        // - - -
        
            public static void TextField(in string stringName, ref string stringReference)
            {
                stringReference = EditorGUILayout.TextField(stringName, stringReference);
            }
        
            public static void TextArea(in string stringName, ref string stringReference)
            {
                stringReference = EditorGUILayout.TextArea(stringName, stringReference);
            }
            
            public static void IntField(in string intName, ref int intReference)
            {
                intReference = EditorGUILayout.IntField(intName, intReference);
            }
            
            public static void BooleanField(in string booleanName, ref bool booleanReference)
            {
                booleanReference = Toggle(booleanName, booleanReference);
            }

            public static void FloatSlider(in string sliderName, ref float floatReference, float min, float max)
            {
            floatReference = EditorGUILayout.Slider(sliderName, floatReference, min, max); 
            }

            public static void FloatField(in string floatName, ref float floatReference)
            {
                string label = floatName;
                float temp = floatReference;
                
                HorizontalScope(() =>
                {
                    float width = EditorGUIUtility.currentViewWidth;
                    
                    TextLabel(label, new []{GUILayout.Width(0.5f * width)});
                    temp = EditorGUILayout.FloatField("", temp, new []{GUILayout.Width(0.5f * width)});  
                });

                floatReference = temp;
            }
            
            public static void FloatField(in string floatName, ref float floatReference, float containerWidth)
            {
                string label = floatName;
                float temp = floatReference;
                
                HorizontalScope(() =>
                {
                    TextLabel(label, new []{GUILayout.Width(0.5f * containerWidth)});
                    temp = EditorGUILayout.FloatField("", temp, new []{GUILayout.Width(0.5f * containerWidth)});  
                });

                floatReference = temp;
            }

            public static void AnimationCurveField(in string label, ref AnimationCurve curveReference)
            {
                EditorGUILayout.CurveField(label, curveReference);
            }
            
            
            public static void FloatField(in string floatName, ref float floatReference, GUILayoutOption[] layoutOption) => floatReference = EditorGUILayout.FloatField(floatName, floatReference, layoutOption);

            public static void BoundsField(in string boundsName, ref float lowerBoundFloatReference, ref float upperBoundFloatReference)
            {
                string label = boundsName;
                float tempMinimum = lowerBoundFloatReference;
                float tempMaximum = upperBoundFloatReference;

                float width = EditorGUIUtility.currentViewWidth;
                float containerWidth = width;

                float labelWidth = containerWidth * 0.2f;
                float fieldWidth = containerWidth * 0.4f;

                HorizontalScope(() =>
                {
                    TextLabel(label, new []{GUILayout.Width(labelWidth)});
                    FloatField("Min:", ref tempMinimum, fieldWidth );
                    FloatField("Max:", ref tempMaximum, fieldWidth );
                });
                
                lowerBoundFloatReference = tempMinimum;
                upperBoundFloatReference = tempMaximum;
            }
            
            public static void ObjectField<T>(in string objectName, ref T objectReference, in bool allowSceneObjects) where T : Object
            {
                objectReference = (T) EditorGUILayout.ObjectField(objectName, objectReference, typeof(T), allowSceneObjects);
            }
            
            public static void VectorField(in string vectorName, ref Vector2 vectorReference)
            {
                vectorReference = Vector2Field(vectorName, vectorReference);
            }
            
            public static void VectorField(in string vectorName, ref Vector3 vectorReference)
            {
                vectorReference = Vector3Field(vectorName, vectorReference);
            }
            
            public static void VectorField(in string vectorName, ref Vector4 vectorReference)
            {
                vectorReference = Vector4Field(vectorName, vectorReference);
            }
        // - - - 
        #endregion
        
        #region Scopes
        // - - - 
            public static void HorizontalScope(in Action action)
            {
                using GUILayout.HorizontalScope horizontalScope = new GUILayout.HorizontalScope();
                action.Invoke();
            }
            
            public static void VerticalScope(in Action action)
            {
                using GUILayout.VerticalScope verticalScope = new GUILayout.VerticalScope();
                action.Invoke();
            }
            
            public static void IndentedScope(in Action action)
            {
                EditorGUI.indentLevel++;
                action.Invoke();
                EditorGUI.indentLevel--;
            }
            
            public static bool ChangeCheck(in Action action)
            {
                using EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope();
                
                action.Invoke();
                return changeCheckScope.changed;
            }
        // - - - 
        #endregion
    }
}
#endif