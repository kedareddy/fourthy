using UnityEngine;
using UnityEditor;
using SphereGrid = GridFramework.Grids.SphereGrid;

namespace GridFramework.Editor {
	/// <summary>
	///   Inspector for spherical grids.
	/// </summary>
	[CustomEditor (typeof(SphereGrid))]
	public class SphereGridEditor : UnityEditor.Editor {
#region  Private variables
		// Whether to display the computed properties
		[SerializeField]
		private static bool _showComputed = true;
		private static string _docsURL;
		private SphereGrid _grid;
#endregion  // Private variables

#region  Callback methods
		void OnEnable() {
			_grid = target as SphereGrid;
			_docsURL = "file://" + Application.dataPath
				+ "/Plugins/GridFramework/Documentation/html/"
				+ "class_grid_framework_1_1_grids_1_1_sphere_grid.html";
		}

		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();

			var radius    = EditorGUILayout.FloatField("Radius" , _grid.Radius   );
			var parallels = EditorGUILayout.IntField("Parallels", _grid.Parallels);
			var meridians = EditorGUILayout.IntField("Meridians", _grid.Meridians);

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(target, "Sphere Grid Change");
				_grid.Radius = radius;
				_grid.Parallels = parallels;
				_grid.Meridians = meridians;
				serializedObject.ApplyModifiedProperties();
				EditorUtility.SetDirty(target);
			}

			if (_showComputed = EditorGUILayout.Foldout(_showComputed, "Computed Properties")) {
				++EditorGUI.indentLevel;
				EditorGUILayout.LabelField("Polar", ""+_grid.Polar, GUIStyle.none, null);
				EditorGUILayout.LabelField(" ", +_grid.Polar/Mathf.PI+"\u03c0", GUIStyle.none, null);
				EditorGUILayout.LabelField(" ", _grid.PolarDeg+"\u00B0", GUIStyle.none, null);
				EditorGUILayout.LabelField("Azimuth", ""+_grid.Azimuth, GUIStyle.none, null);
				EditorGUILayout.LabelField(" ", ""+_grid.Azimuth/Mathf.PI+"\u03c0", GUIStyle.none, null);
				EditorGUILayout.LabelField(" ", ""+_grid.AzimuthDeg+"\u00B0", GUIStyle.none, null);
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
