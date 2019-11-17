using UnityEngine;
using UnityEditor;
using GridFramework.Grids;

namespace GridFramework.Editor {
	/// <summary>
	///   Inspector for polar grids.
	/// </summary>
	[CustomEditor (typeof(PolarGrid))]
	public class PolarGridEditor : UnityEditor.Editor {
#region  Private variables
		// Whether to display the computed properties
		[SerializeField]
		private bool _showComputed = true;
		private static string _docsURL;
		private PolarGrid _grid;
#endregion  // Private variables

#region  Callback methods
		void OnEnable() {
			_grid = target as PolarGrid;
			_docsURL = "file://" + Application.dataPath
				+ "/Plugins/GridFramework/Documentation/html/"
				+ "class_grid_framework_1_1_grids_1_1_polar_grid.html";
		}
		
		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();

			var radius = EditorGUILayout.FloatField("Radius", _grid.Radius);
			var sectors = EditorGUILayout.IntField("Sectors", _grid.Sectors);
			var depth = EditorGUILayout.FloatField("Depth", _grid.Depth);

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(target, "Polar Grid Change");
				_grid.Radius = radius;
				_grid.Sectors = sectors;
				_grid.Depth = depth;
				serializedObject.ApplyModifiedProperties();
				EditorUtility.SetDirty(target);
			}

			if (_showComputed = EditorGUILayout.Foldout(_showComputed, "Computed Properties")) {
				++EditorGUI.indentLevel;
				EditorGUILayout.LabelField("Degrees", ""+_grid.Degrees, GUIStyle.none, null);
				EditorGUILayout.LabelField("Radians", ""+_grid.Radians, GUIStyle.none, null);
				EditorGUILayout.LabelField("Radians in \u03c0", ""+_grid.Radians/Mathf.PI, GUIStyle.none, null);
				--EditorGUI.indentLevel;
			}
		}
#endregion  // Callback methods

#region  Menu items
		[MenuItem ("CONTEXT/PolarGrid/Help")]
		private static void BrowseDocs(MenuCommand command) {
			Help.ShowHelpPage(_docsURL);
		}
#endregion  // Menu items
	}
}
