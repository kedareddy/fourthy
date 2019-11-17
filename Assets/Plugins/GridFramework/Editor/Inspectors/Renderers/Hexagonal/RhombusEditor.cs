using UnityEngine;
using UnityEditor;
using GridFramework.Renderers.Hexagonal;
using Direction = GridFramework.Renderers.Hexagonal.Rhombus.RhombDirection;

namespace GridFramework.Editor.Renderers.Hexagonal {
	/// <summary>
	///   Inspector for hexagonal rhombus renderers.
	/// </summary>
	[CustomEditor (typeof(Rhombus))]
	public class RhombusEditor : RendererEditor<Rhombus> {
#region  Cached variables
		private Direction direction;
		private int left, right, bottom, top;
		private float layerFrom, layerTo;
#endregion  // Cached variables

#region  Implementation
		protected override void SpecificFields() {
			Direction();
			Horizontal();
			Vertical();
			Layer();
		}

		protected override void AssignSpecific() {
			_renderer.Direction = direction;
			_renderer.Left      =      left; _renderer.Right   =   right;
			_renderer.Bottom    =    bottom; _renderer.Top     =     top;
			_renderer.LayerFrom = layerFrom; _renderer.LayerTo = layerTo;
		}
#endregion  // Implementation

#region  Fields
		private void Direction() {
			direction = (Direction)EditorGUILayout.EnumPopup("Direction", _renderer.Direction);
		}

		private void Horizontal() {
			GUILayout.Label("Horizontal");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				left  = EditorGUILayout.IntField("Left" , _renderer.Left );
				right = EditorGUILayout.IntField("Right", _renderer.Right);
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Vertical() {
			GUILayout.Label("Vertical");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				bottom = EditorGUILayout.IntField("Bottom", _renderer.Bottom);
				top    = EditorGUILayout.IntField("Top"   , _renderer.Top   );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Layer() {
			GUILayout.Label("Layer");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				layerFrom = EditorGUILayout.FloatField("LayerFrom", _renderer.LayerFrom);
				layerTo   = EditorGUILayout.FloatField("LayerTo"  , _renderer.LayerTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}
#endregion  // Fields
	}
}
