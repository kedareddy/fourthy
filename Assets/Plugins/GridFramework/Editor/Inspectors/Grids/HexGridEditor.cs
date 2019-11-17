using UnityEditor;
using UnityEngine;
using GridFramework.Grids;

namespace GridFramework.Editor {
	/// <summary>
	///   Inspector for hexagonal grids.
	/// </summary>
	[CustomEditor(typeof(HexGrid))]
	public class HexGridEditor : UnityEditor.Editor {
#region  Private variables
		// Whether to display the computed properties
		[SerializeField]
		private static bool _showComputed = true;
		private static string _docsURL;
		private HexGrid _grid;
#endregion  // Private variables

#region  Callback methods
		void OnEnable() {
			_grid = target as HexGrid;
			_docsURL = "file://" + Application.dataPath
				+ "/Plugins/GridFramework/Documentation/html/"
				+ "class_grid_framework_1_1_grids_1_1_hex_grid.html";
		}

		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();

			var radius = EditorGUILayout.FloatField("Radius", _grid.Radius);
			var sides = (HexGrid.Orientation)EditorGUILayout.EnumPopup("Sides", _grid.Sides);
			var depth = EditorGUILayout.FloatField("Depth", _grid.Depth);

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(target, "Hexagonal Grid Change");
				_grid.Radius = radius;
				_grid.Sides = sides;
				_grid.Depth = depth;
				serializedObject.ApplyModifiedProperties();
				EditorUtility.SetDirty(target);
			}

			if (_showComputed = EditorGUILayout.Foldout(_showComputed, "Computed Properties")) {
				++EditorGUI.indentLevel;
				EditorGUILayout.LabelField("Height", ""+_grid.Height, GUIStyle.none, null);
				EditorGUILayout.LabelField("Width", ""+_grid.Width, GUIStyle.none, null);
				EditorGUILayout.LabelField("Side", ""+_grid.Side, GUIStyle.none, null);
				--EditorGUI.indentLevel;
			}
		}
#endregion  // Callback methods

#region  Menu items
		[MenuItem ("CONTEXT/HexGrid/Help")]
		private static void BrowseDocs(MenuCommand command) {
			Help.ShowHelpPage(_docsURL);
		}
#endregion  // Menu items
	}
}
