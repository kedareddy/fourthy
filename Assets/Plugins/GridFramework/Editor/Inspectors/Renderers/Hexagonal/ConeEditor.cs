using UnityEngine;
using UnityEditor;
using GridFramework.Renderers.Hexagonal;

namespace GridFramework.Editor.Renderers.Hexagonal {
	/// <summary>
	///   Inspector for hexagonal cone renderers.
	/// </summary>
	[CustomEditor (typeof(Cone))]
	public class ConeEditor : RendererEditor<Cone> {
#region  Cached variables
		private int      originX,  originY;
		private int   radiusFrom, radiusTo;
		private int      hexFrom,    hexTo;
		private float  layerFrom,  layerTo;
#endregion  // Cached variables

#region  Implementation
		protected override void SpecificFields() {
			EditorGUIUtility.labelWidth = 80;
			Origin();
			Radius();
			Hex();
			Layer();
			EditorGUIUtility.labelWidth = 0;
		}
		protected override void AssignSpecific() {
			_renderer.OriginX    =    originX; _renderer.OriginY  =  originY;
			_renderer.RadiusFrom = radiusFrom; _renderer.RadiusTo = radiusTo;
			_renderer.HexFrom    =    hexFrom; _renderer.HexTo    =    hexTo;
			_renderer.LayerFrom  =  layerFrom; _renderer.LayerTo  =  layerTo;
		}
#endregion  // Implementation

#region  Fields
		private void Origin() {
			GUILayout.Label("Origin");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				originX = EditorGUILayout.IntField("X", _renderer.OriginX);
				originY = EditorGUILayout.IntField("Y", _renderer.OriginY);
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Radius() {
			GUILayout.Label("Radius");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				radiusFrom = EditorGUILayout.IntField("From", _renderer.RadiusFrom);
				radiusTo   = EditorGUILayout.IntField("To"  , _renderer.RadiusTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Hex() {
			GUILayout.Label("Hex");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				hexFrom = EditorGUILayout.IntField("From", _renderer.HexFrom);
				hexTo   = EditorGUILayout.IntField("To"  , _renderer.HexTo  );
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
