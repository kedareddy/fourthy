using UnityEngine;
using UnityEditor;
using GridFramework.Renderers.Polar;

namespace GridFramework.Editor.Renderers.Polar {
	/// <summary>
	///   Inspector for polar cylinder renderers.
	/// </summary>
	[CustomEditor(typeof(Cylinder))]
	public class CylinderEditor : RendererEditor<Cylinder> {
#region  Cached variables
		private float radialFrom, radialTo; 
		private float sectorFrom, sectorTo; 
		private float  layerFrom,  layerTo; 
		private int smoothness;
#endregion  // Cached variables

#region  Implementation
		protected override void SpecificFields() {
			Smoothness();
			EditorGUIUtility.labelWidth = 50;
			Radial();
			Sector();
			Layer();
			EditorGUIUtility.labelWidth = 0;
		}
		protected override void AssignSpecific() {
			_renderer.Smoothness = smoothness;

			_renderer.RadialFrom = radialFrom; _renderer.RadialTo = radialTo;
			_renderer.SectorFrom = sectorFrom; _renderer.SectorTo = sectorTo;
			_renderer.LayerFrom  =  layerFrom; _renderer.LayerTo  =  layerTo;
		}
#endregion  // Implementation

#region  Fields
		private void Smoothness() {
			smoothness = EditorGUILayout.IntField("Smoothness", _renderer.Smoothness);
		}

		private void Radial() {
			GUILayout.Label("Radial");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				radialFrom = EditorGUILayout.FloatField("From", _renderer.RadialFrom);
				radialTo   = EditorGUILayout.FloatField("To"  , _renderer.RadialTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Sector() {
			GUILayout.Label("Sector");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				sectorFrom = EditorGUILayout.FloatField("From", _renderer.SectorFrom);
				sectorTo   = EditorGUILayout.FloatField("To"  , _renderer.SectorTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Layer() {
			GUILayout.Label("Layer");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				layerFrom = EditorGUILayout.FloatField("From", _renderer.LayerFrom);
				layerTo   = EditorGUILayout.FloatField("To"  , _renderer.LayerTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}
#endregion  // Fields
	}
}
