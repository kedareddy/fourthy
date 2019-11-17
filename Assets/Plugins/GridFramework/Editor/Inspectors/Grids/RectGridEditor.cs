using UnityEngine;
using UnityEditor;
using GridFramework.Grids;
using Vector6 = GridFramework.Vectors.Vector6;

namespace GridFramework.Editor {
	/// <summary>
	///   Inspector for rectangular grids.
	/// </summary>
	[CustomEditor (typeof(RectGrid))]
	public class RectGridEditor : UnityEditor.Editor {
#region  Private variables
		private static string _docsURL;
		private RectGrid _grid;
#endregion  // Private variables

#region  Callback methods
		void OnEnable() {
			_grid = target as RectGrid;
			_docsURL = "file://" + Application.dataPath
				+ "/Plugins/GridFramework/Documentation/html/"
				+ "class_grid_framework_1_1_grids_1_1_rect_grid.html";
		}

		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();

			var spacing = EditorGUILayout.Vector3Field("Spacing", _grid.Spacing);
			var shearing = _grid.Shearing;

			GUILayout.Label("Shearing");

			EditorGUIUtility.labelWidth = 35f;
			++EditorGUI.indentLevel;
			EditorGUILayout.BeginHorizontal(); {
				shearing.XY = EditorGUILayout.FloatField("XY", shearing.XY);
				shearing.XZ = EditorGUILayout.FloatField("XZ", shearing.XZ);
				shearing.YX = EditorGUILayout.FloatField("YX", shearing.YX);
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndHorizontal();

			++EditorGUI.indentLevel;
			EditorGUILayout.BeginHorizontal(); {
				shearing.YZ = EditorGUILayout.FloatField("YZ", shearing.YZ);
				shearing.ZX = EditorGUILayout.FloatField("ZX", shearing.ZX);
				shearing.ZY = EditorGUILayout.FloatField("ZY", shearing.ZY);
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUIUtility.labelWidth = 0;

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(target, "Rectangular Grid Change");
				_grid.Spacing = spacing;
				_grid.Shearing = shearing;
				serializedObject.ApplyModifiedProperties();
				EditorUtility.SetDirty(target);
			}
		}
#endregion  // Callback methods

#region  Menu items
		[MenuItem ("CONTEXT/RectGrid/Help")]
		private static void BrowseDocs(MenuCommand command) {
			Help.ShowHelpPage(_docsURL);
		}
#endregion  // Menu items
	}
}
