using System;
using UnityEngine;

namespace TopDownCamera.Scripts.Editor
{
    using UnityEditor;
    
    [CustomEditor(typeof(TopDownCameraMono))]
    public class TopDownCameraMonoEditor : Editor
    {
        private TopDownCameraMono _target;
        private GUIStyle _labelStyle;
        
        private void OnEnable()
        {
            _target = target as TopDownCameraMono;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
        }

        private void OnSceneGUI()
        {
            // Properties and checkers
            var cameraTargetProp = serializedObject.FindProperty("_target");
            if (cameraTargetProp == null || cameraTargetProp.objectReferenceValue == null)
            {
                return;
            }
            
            var cameraTarget = cameraTargetProp.objectReferenceValue as Transform;
            if (cameraTarget == null)
            {
                return;
            }
            
            var targetPosition = cameraTarget.position;
            var distance = serializedObject.FindProperty("_distance");
            var height = serializedObject.FindProperty("_height");
            var forward = cameraTarget.forward;

            // Distance disc
            Handles.color = new Color(1f, 0f, 0f, 0.15f);
            Handles.DrawSolidDisc(targetPosition, Vector3.up, distance.floatValue);
            
            Handles.color = new Color(0f, 1f, 0f, 0.75f);
            Handles.DrawWireDisc(targetPosition, Vector3.up, distance.floatValue);

            // Create Slider handles to adjust camera properties
            Handles.color = new Color(0f, 1f, 0f, 0.5f);
            distance.floatValue = Handles.ScaleSlider(
                distance.floatValue, 
                targetPosition, 
                -forward,
                Quaternion.identity, 
                distance.floatValue, 
                1f);
            distance.floatValue = Mathf.Clamp(distance.floatValue, 5f, float.MaxValue);
                
            Handles.color = new Color(0f, 0f, 1f, 0.5f);
            height.floatValue = Handles.ScaleSlider(
                height.floatValue, 
                targetPosition, 
                Vector3.up, 
                Quaternion.identity, 
                height.floatValue, 
                1f);
            height.floatValue = Mathf.Clamp(height.floatValue, 5f, float.MaxValue);

            InitStyles();

            _labelStyle.alignment = TextAnchor.UpperCenter;
            Handles.Label(targetPosition + (-forward * distance.floatValue), "Distance");
            
            _labelStyle.alignment = TextAnchor.MiddleRight;
            Handles.Label(targetPosition + (Vector3.up * height.floatValue), "Height");

            _target.HandleCamera();
            
            // save and update values
            serializedObject.ApplyModifiedProperties();
        }

        private void InitStyles()
        {
            if (_labelStyle != null)
            {
                return;
            }

            _labelStyle = new GUIStyle
            {
                fontSize = 15, 
                normal = {textColor = Color.white}
            };
        }
    }
}