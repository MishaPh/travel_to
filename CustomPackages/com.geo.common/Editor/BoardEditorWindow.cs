using Geo.Common.Internal;
using Geo.Common.Internal.Boards;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Geo.Common.Editor
{
    public class BoardEditorWindow : EditorWindow
    {
        private const float gridStep = 0.5f;

        private BoardData _boardData;

        private int _countBoxes = 0;
        private int _percent = 10;

        [MenuItem("Window/Geo/Board Editor")]
        public static void ShowWindow()
        {
            GetWindow<BoardEditorWindow>("Board Editor");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Selected:", EditorStyles.boldLabel);
            if (_boardData == null)
            {
               EditorGUILayout.LabelField("No BoardData selected.");
               return;
            }

            var boardData = EditorGUILayout.ObjectField(_boardData, typeof(BoardData), false);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Count spaces in the board: {_countBoxes}");
            EditorGUILayout.EndVertical();

            _percent = EditorGUILayout.IntSlider("Special Space Percent", _percent, 0, 100);
            if (GUILayout.Button("Update Space Points"))
            {
                _boardData.Spaces = GenerateSpaces();
                EditorUtility.SetDirty(_boardData);
            }

            if (_boardData.Spaces.Count != _countBoxes)
            {
                EditorGUILayout.LabelField("The space count was changed.");
            }
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Gizmos information: ");
            EditorGUILayout.LabelField("Blue - is a moveable point");
            EditorGUILayout.LabelField("Cyan - is a moveable line");
            EditorGUILayout.LabelField("Green - is a special space");
            EditorGUILayout.LabelField("Red - indicate an invalid point");
            EditorGUILayout.EndVertical();
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSelectionChanged()
        {
            if (Selection.activeObject is BoardData data)
            {
                _boardData = data;
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (_boardData == null)
                return;

            for (var i = 0; i < _boardData.Points.Count; i++)
            {
                var nextIndex = (i + 1 == _boardData.Points.Count) ? 0 : i + 1;
                var next = (i + 1 == _boardData.Points.Count) ? _boardData.Points[0] : _boardData.Points[i + 1];
                var vectorToNext = _boardData.Points[i] - next;

                Handles.color = Color.blue;
                Handles.DrawLine(_boardData.Points[i].ToVector3XZ(), next.ToVector3XZ());
                Handles.color = IsVaildPoint(vectorToNext) ? Color.blue : Color.red;

                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.FreeMoveHandle(_boardData.Points[i].ToVector3XZ(), 0.1f, Vector3.up, Handles.DotHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    var delta = newPosition.ToVector2XZ(gridStep) - _boardData.Points[i];
                    if (delta.sqrMagnitude > float.Epsilon)
                    {
                        Undo.RecordObject(_boardData, "Change Board Target Position");
                        _boardData.Points[i] += delta;
                    }
                }
                Handles.color = Color.white;
                Handles.Label(_boardData.Points[i].ToVector3XZ(), i.ToString());
                DrawLineHandler(i, nextIndex);
            }

            _countBoxes = 0;
            var boxSize = 0.8f * gridStep * Vector3.one;
            Handles.color = Color.grey;
            foreach (var position in GetAllPositions())
            {
                _countBoxes++;
                Handles.DrawWireCube(position, boxSize);
            }

            foreach (var space in _boardData.Spaces)
            {
                Handles.color = space.Space == SpaceType.Empty ? Color.grey : Color.green;
                Handles.DrawSolidDisc(space.Position, Vector3.up, 0.05f);
            }
        }

        private List<SpaceInfo> GenerateSpaces()
        {
            var result = GetAllPositions()
                   .Select(p => new SpaceInfo() { Position = p, Space = SpaceTypeHelper.GetRndSpecial(_percent * 0.01f) }).ToList();

            return result;
        }

        private List<Vector3> GetAllPositions()
        {
            var list = new List<Vector3>();
            for (var i = 0; i < _boardData.Points.Count; i++)
            {
                var next = (i + 1 == _boardData.Points.Count) ? _boardData.Points[0] : _boardData.Points[i + 1];
                var current = _boardData.Points[i];
                list.AddRange(GetPositionsOnLine(current, next, gridStep));
            }
            return list;
        }

        private IEnumerable<Vector3> GetPositionsOnLine(Vector2 from, Vector2 to, float step)
        {
            var length = Vector2.Distance(from, to);
            var steps = (int)(length / step);
           
            for (var i = 0; i < steps; i++)
            {
                yield return Vector2.Lerp(from, to, (float)i / steps).ToVector3XZ();
            }
        }

        private void DrawLineHandler(int fromIndex, int nextIndex)
        {
            Handles.color = Color.cyan;
            var middle = Vector2.Lerp(_boardData.Points[fromIndex], _boardData.Points[nextIndex], 0.5f);
            EditorGUI.BeginChangeCheck();
            Vector3 newLinePosition = Handles.FreeMoveHandle(middle.ToVector3XZ(), 0.1f, Vector3.one, Handles.DotHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                var delta = newLinePosition.ToVector2XZ(gridStep) - middle;
                var lineVector = (_boardData.Points[fromIndex] - _boardData.Points[nextIndex]);
                if (lineVector.x != 0)
                    delta.x = 0;
                if (lineVector.y != 0)
                    delta.y = 0;

                if (delta.sqrMagnitude > gridStep * gridStep * 0.2f)
                {
                    Undo.RecordObject(_boardData, "Change Board Line Position");
                    _boardData.Points[fromIndex] += delta;
                    _boardData.Points[nextIndex] += delta;
                }
            }
        }

        private bool IsVaildPoint(Vector2 vectorToNext)
        {
            if (vectorToNext.x == 0)
                return true;
            if (vectorToNext.y == 0)
                return true;

            return false;
        }
    }
}
