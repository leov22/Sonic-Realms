﻿using System;
using System.Collections.Generic;
using Hedgehog.Actors;
using Hedgehog.Utils;
using UnityEditor;
using UnityEngine;

namespace Hedgehog.Editor
{
    [Serializable]
    [CustomEditor(typeof(HedgehogController))]
    public class HedgehogControllerEditor : UnityEditor.Editor
    {
        [SerializeField] private HedgehogController _instance;

        [SerializeField]
        private bool _showCollision;
        [SerializeField]
        private bool _showSensors;
        [SerializeField]
        private bool _showSensorsGenerator;

        [SerializeField] private bool _showDebugInfo;

        [SerializeField]
        private Renderer _fromRenderer;
        [SerializeField]
        private Collider2D _fromCollider2D;
        [SerializeField]
        private Bounds _fromBounds;

        [SerializeField]
        private bool _showAdvancedSensors;
        [SerializeField]
        private bool _showControls;
        [SerializeField]
        private bool _showPhysics;
        [SerializeField]
        private bool _showAdvancedPhysics;

        public void OnEnable()
        {
            _instance = target as HedgehogController;

            _fromRenderer = _instance.GetComponent<Renderer>();
            _fromCollider2D = _instance.GetComponent<Collider2D>();
        }

        public override void OnInspectorGUI()
        {
            if (_instance == null) return;

            #region GUI Styles
            var titleStyle = new GUIStyle
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                richText = true
            };

            var headerStyle = new GUIStyle
            {
                fontSize = 11,
                alignment = TextAnchor.MiddleCenter,
            };

            var foldoutStyle = new GUIStyle(EditorStyles.foldout);
            #endregion

            #region Title
            GUILayout.Label("<color=\"#0000ff\">Hedgehog Controller</color>", titleStyle);
            #endregion

            #region Controls Foldout
            _showControls = EditorGUILayout.Foldout(_showControls, "Controls", foldoutStyle);
            if (_showControls)
            {
                _instance.JumpKey = (KeyCode)EditorGUILayout.EnumPopup("Jump Key", _instance.JumpKey);
                _instance.LeftKey = (KeyCode)EditorGUILayout.EnumPopup("Left Key", _instance.LeftKey);
                _instance.RightKey = (KeyCode)EditorGUILayout.EnumPopup("Right Key", _instance.RightKey);
            }
            #endregion
            #region Collision Foldout
            _showCollision = EditorGUILayout.Foldout(_showCollision, "Collision", foldoutStyle);
            if (_showCollision)
            {
                EditorGUILayout.LabelField("Layers", headerStyle);

                _instance.InitialTerrainMask = LayerMaskField("Initial Terrain Mask", _instance.InitialTerrainMask);
            }
            #endregion
            #region Sensors Foldout
            _showSensors = EditorGUILayout.Foldout(_showSensors, "Sensors", foldoutStyle);
            if (_showSensors)
            {
                EditorGUILayout.LabelField("Create", headerStyle);

                EditorGUILayout.BeginHorizontal();
                _fromRenderer =
                    EditorGUILayout.ObjectField("From Renderer", _fromRenderer, typeof(Renderer), true) as Renderer;
                GUI.enabled = _fromRenderer != null && _fromRenderer.bounds != default(Bounds);
                if (GUILayout.Button("Create"))
                {
                    HedgehogUtils.GenerateSensors(_instance, _fromRenderer.bounds);
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _fromCollider2D =
                    EditorGUILayout.ObjectField("From Collider2D", _fromCollider2D, typeof(Collider2D), true) as Collider2D;
                GUI.enabled = _fromCollider2D != null;
                if (GUILayout.Button("Create"))
                {
                    HedgehogUtils.GenerateSensors(_instance, _fromCollider2D.bounds);
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();

                _fromBounds = EditorGUILayout.BoundsField("From Bounds", _fromBounds);
                GUI.enabled = _fromBounds != default(Bounds);
                if (GUILayout.Button("Create from Bounds"))
                {
                    HedgehogUtils.GenerateSensors(_instance, _fromBounds, true);
                }
                GUI.enabled = true;

                EditorGUILayout.Space();
            
                _instance.SensorTopLeft = EditorGUILayout.ObjectField("Top Left", _instance.SensorTopLeft,
                    typeof(Transform), true) as Transform;
                _instance.SensorTopMiddle = EditorGUILayout.ObjectField("Top Center", _instance.SensorTopMiddle,
                    typeof(Transform), true) as Transform;
                _instance.SensorTopRight = EditorGUILayout.ObjectField("Top Right", _instance.SensorTopRight,
                    typeof(Transform), true) as Transform;
                _instance.SensorMiddleLeft = EditorGUILayout.ObjectField("Center Left", _instance.SensorMiddleLeft,
                    typeof(Transform), true) as Transform;
                _instance.SensorMiddleMiddle = EditorGUILayout.ObjectField("Center", _instance.SensorMiddleMiddle,
                    typeof(Transform), true) as Transform;
                _instance.SensorMiddleRight = EditorGUILayout.ObjectField("Center Right", _instance.SensorMiddleRight,
                    typeof(Transform), true) as Transform;
                _instance.SensorBottomLeft = EditorGUILayout.ObjectField("Bottom Left", _instance.SensorBottomLeft,
                    typeof(Transform), true) as Transform;
                _instance.SensorBottomMiddle = EditorGUILayout.ObjectField("Bottom Center", _instance.SensorBottomMiddle,
                    typeof(Transform), true) as Transform;
                _instance.SensorBottomRight = EditorGUILayout.ObjectField("Bottom Right", _instance.SensorBottomRight,
                    typeof(Transform), true) as Transform;
            }
            #endregion
            #region Physics Foldout
            _showPhysics = EditorGUILayout.Foldout(_showPhysics, "Physics", foldoutStyle);
            if (_showPhysics)
            {
                EditorGUILayout.BeginHorizontal();
                _instance.MaxSpeed = EditorGUILayout.FloatField("Max Speed",
                    _instance.MaxSpeed);
                EditorGUILayout.PrefixLabel("units/s");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _instance.JumpSpeed = EditorGUILayout.FloatField("Jump Speed",
                    _instance.JumpSpeed);
                EditorGUILayout.PrefixLabel("units/s");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                _instance.GroundAcceleration = EditorGUILayout.FloatField("Ground Acceleration",
                    _instance.GroundAcceleration);
                EditorGUILayout.PrefixLabel("units/s²");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _instance.GroundDeceleration = EditorGUILayout.FloatField("Ground Deceleration",
                    _instance.GroundDeceleration);
                EditorGUILayout.PrefixLabel("units/s²");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _instance.AirAcceleration = EditorGUILayout.FloatField("Air Acceleration",
                    _instance.AirAcceleration);
                EditorGUILayout.PrefixLabel("units/s²");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                _instance.AirGravity = EditorGUILayout.FloatField("Air Gravity",
                    _instance.AirGravity);
                EditorGUILayout.PrefixLabel("units/s²");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _instance.SlopeGravity = EditorGUILayout.FloatField("Slope Gravity",
                    _instance.SlopeGravity);
                EditorGUILayout.PrefixLabel("units/s²");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                #region Advanced Physics
                _showAdvancedPhysics = EditorGUILayout.Toggle("Show Advanced", _showAdvancedPhysics);
                if (_showAdvancedPhysics)
                {
                    EditorGUILayout.BeginHorizontal();
                    _instance.HorizontalLockTime = EditorGUILayout.FloatField("Horizontal Lock",
                        _instance.HorizontalLockTime);
                    EditorGUILayout.PrefixLabel("seconds");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    _instance.LedgeClimbHeight = EditorGUILayout.FloatField("Ledge Climb Height",
                        _instance.LedgeClimbHeight);
                    EditorGUILayout.PrefixLabel("units");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    _instance.LedgeDropHeight = EditorGUILayout.FloatField("Ledge Drop Height",
                        _instance.LedgeDropHeight);
                    EditorGUILayout.PrefixLabel("units");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    _instance.SlopeGravityBeginAngle = EditorGUILayout.FloatField("Slope Gravity Begin Angle",
                        _instance.SlopeGravityBeginAngle);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    _instance.ForceJumpAngleDifference = EditorGUILayout.FloatField("Force Jump Angle",
                        _instance.ForceJumpAngleDifference);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    _instance.AntiTunnelingSpeed = EditorGUILayout.FloatField("Anti-Tunneling Speed",
                        _instance.AntiTunnelingSpeed);
                    EditorGUILayout.PrefixLabel("units/s");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    _instance.DetachSpeed = EditorGUILayout.FloatField("Detach Speed",
                        _instance.DetachSpeed);
                    EditorGUILayout.PrefixLabel("units/s");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    #region Forbidden Constants
                    EditorGUILayout.LabelField("Surface Constants", headerStyle);
                    EditorGUILayout.HelpBox("The following constants are best left untouched!", MessageType.Warning);

                    EditorGUILayout.BeginHorizontal();
                    _instance.MinWallmodeSwitchSpeed = EditorGUILayout.FloatField("Min Wallmode Switch Speed",
                        _instance.MinWallmodeSwitchSpeed);
                    EditorGUILayout.PrefixLabel("units/s");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    _instance.HorizontalWallmodeAngleWeight = EditorGUILayout.FloatField("Horizontal Wallmode Angle Weight",
                        _instance.HorizontalWallmodeAngleWeight);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    _instance.MaxSurfaceAngleDifference = EditorGUILayout.FloatField("Max Surface Angle Difference",
                        _instance.MaxSurfaceAngleDifference);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    _instance.MinOverlapAngle = EditorGUILayout.FloatField("Min Overlap Angle",
                        _instance.MinOverlapAngle);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    _instance.MinFlatOverlapRange = EditorGUILayout.FloatField("Min Flat Overlap Range",
                        _instance.MinFlatOverlapRange);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    _instance.MinFlatAttachAngle = EditorGUILayout.FloatField("Min Flat Attach Angle",
                        _instance.MinFlatAttachAngle);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    _instance.MaxVerticalDetachAngle = EditorGUILayout.FloatField("Max Vertical Detach Angle",
                        _instance.MaxVerticalDetachAngle);
                    EditorGUILayout.PrefixLabel("degrees");
                    EditorGUILayout.EndHorizontal();
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Debug Foldout
            _showDebugInfo = EditorGUILayout.Foldout(_showDebugInfo, "Debug", foldoutStyle);
            if (_showDebugInfo)
            {
                if (!Application.isPlaying)
                {
                    EditorGUILayout.HelpBox("This section becomes active in Play Mode.", MessageType.Info);
                }

                GUI.enabled = Application.isPlaying;
                EditorGUILayout.LabelField("Surface", headerStyle);
                _instance.Grounded = EditorGUILayout.Toggle("Grounded", _instance.Grounded);
                GUI.enabled = Application.isPlaying && _instance.Grounded;
                EditorGUILayout.FloatField("Surface Angle", _instance.SurfaceAngle);
                EditorGUILayout.EnumPopup("Wallmode", _instance.Wallmode);
                GUI.enabled = Application.isPlaying;
                _instance.TerrainMask = LayerMaskField("Terrain Mask", _instance.TerrainMask);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Velocity", headerStyle);
            
                _instance.Vx = EditorGUILayout.FloatField("X", _instance.Vx);
                _instance.Vy = EditorGUILayout.FloatField("Y", _instance.Vy);
                GUI.enabled = Application.isPlaying && _instance.Grounded;
                _instance.Vg = EditorGUILayout.FloatField("Ground", _instance.Vg);
                GUI.enabled = Application.isPlaying;

                EditorGUILayout.LabelField("Control", headerStyle);

                EditorGUILayout.Toggle("Lock Upon Landing", _instance.LockUponLanding);
                EditorGUILayout.Toggle("Horizontal Lock", _instance.HorizontalLock);
                GUI.enabled = Application.isPlaying && _instance.HorizontalLock;
                EditorGUILayout.FloatField("Countdown", _instance.HorizontalLockTimer);
                GUI.enabled = Application.isPlaying;
            
                EditorGUILayout.Space();

                EditorGUILayout.Toggle("Jump Key Pressed", _instance.JumpKeyDown);
                EditorGUILayout.Toggle("Left Key Pressed", _instance.LeftKeyDown);
                EditorGUILayout.Toggle("Right Key Pressed", _instance.RightKeyDown);

                GUI.enabled = true;
            }
            #endregion

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_instance);
                EditorUtility.SetDirty(this);
            }
        }

        // Source: http://answers.unity3d.com/questions/42996/how-to-create-layermask-field-in-a-custom-editorwi.html
        private static LayerMask LayerMaskField(string label, LayerMask layerMask)
        {
            List<string> layers = new List<string>();
            List<int> layerNumbers = new List<int>();
 
            for (int i = 0; i < 32; i++) {
                string layerName = LayerMask.LayerToName(i);
                if (layerName != "") {
                    layers.Add(layerName);
                    layerNumbers.Add(i);
                }
            }
            int maskWithoutEmpty = 0;
            for (int i = 0; i < layerNumbers.Count; i++) {
                if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                    maskWithoutEmpty |= (1 << i);
            }
            maskWithoutEmpty = EditorGUILayout.MaskField(label, maskWithoutEmpty, layers.ToArray());
            int mask = 0;
            for (int i = 0; i < layerNumbers.Count; i++) {
                if ((maskWithoutEmpty & (1 << i)) > 0)
                    mask |= (1 << layerNumbers[i]);
            }
            layerMask.value = mask;
            return layerMask;
        }
    }
}
