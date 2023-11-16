#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;
    using static KeiishkiiLib.InspectorUtility;

namespace KeiishkiiLib
{
    public abstract class CustomInspector<T> : Editor
    {
        #region References
        // - - -
            protected T _targetScript;
        // - - -
        #endregion
        
        #region Behaviour
        // - - -
            protected Rect _inspectorRect;
            private bool _showBaseInspector;
        // - - -
        #endregion

        
        
        

        protected void OnEnable()
        {
            if (target is T targetScript)
            {
                _targetScript = targetScript;
            }
        } 
        
        
        
        protected virtual void OnInspectorRender() { }
        public override void OnInspectorGUI()
        {
            //VerticalScope(GUILayout.FlexibleSpace);
            //if (Event.current.type == EventType.Repaint) _inspectorRect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.Repaint)
            {
                _inspectorRect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, 0);
            }
            
            
            
            //Debug.Log($"Rect Size: {_inspectorRect.ToString()}");
            
            OnInspectorRender();
            
            Separator();
            Foldout($"{((_showBaseInspector) ? ("Hide") : ("Show"))} {typeof(T).ToString()} Base Inspector:", ref _showBaseInspector, () =>
            {
                base.OnInspectorGUI();
            });
        }
        
        protected virtual void OnSceneRender() { }
        public void OnSceneGUI()
        {
            OnSceneRender();
        }
    }
}
#endif