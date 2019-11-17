using UnityEngine;
using UnityEditor;
using GridFramework.Renderers.Hexagonal;
using Shift = GridFramework.Renderers.Hexagonal.Rectangle.OddColumnShift;

namespace GridFramework.Editor.Renderers.Hexagonal {
	/// <summary>
	///   Inspector for hexagonal rectangular renderers.
	/// </summary>
	[CustomEditor (typeof(Rectangle))]
	public class RectangleEditor : RendererEditor<Rectangle> {
#region  Cached variables
		private Shift shift;
		private int left, right, bottom, top;
		private float layerFrom, layerTo;
#endregion  // Cached variables

#region  Implementation
		protected override void SpecificFields() {
			Shift();
			Horizontal();
			Vertical();
			Layer();
		}
		protected override void AssignSpecific() {
			_renderer.Shift = shift;
			_renderer.Left = left; _renderer.Right = right;
			_renderer.Bottom = bottom; _renderer.Top = top;
			_renderer.LayerFrom = layerFrom; _renderer.LayerTo = layerTo;
		}
#endregion  // Implementation

#region  Fields
		private void Shift() {
			shift = (Shift)EditorGUILayout.EnumPopup("Shift", _renderer.Shift);
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
