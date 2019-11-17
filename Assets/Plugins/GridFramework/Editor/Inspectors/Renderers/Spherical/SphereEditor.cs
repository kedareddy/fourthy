using UnityEngine;
using UnityEditor;
using GridFramework.Renderers.Spherical;

namespace GridFramework.Editor.Renderers.Spherical {
	/// <summary>
	///   Inspector for spherical sphere renderers.
	/// </summary>
	[CustomEditor (typeof(Sphere))]
	public class SphereEditor : RendererEditor<Sphere> {
#region  Cached variables
		private float  altitudeFrom,  altitudeTo;
		private float longitudeFrom, longitudeTo;
		private float  latitudeFrom,  latitudeTo;
		private int smoothnessP, smoothnessM;
#endregion  // Cached variables

#region  Implementation
		protected override void SpecificFields() {
			EditorGUIUtility.labelWidth = 75;
			Altitude();
			Longitude();
			Latitude();
			Smoothness();
			EditorGUIUtility.labelWidth = 0;
		}

		protected override void AssignSpecific() {
			_renderer.AltFrom =  altitudeFrom; _renderer.AltTo =  altitudeTo;
			_renderer.LonFrom = longitudeFrom; _renderer.LonTo = longitudeTo;
			_renderer.LatFrom =  latitudeFrom; _renderer.LatTo =  latitudeTo;

			_renderer.SmoothP = smoothnessP;
			_renderer.SmoothM = smoothnessM;
		}
#endregion  // Implementation

#region  Fields
		private void Altitude() {
			GUILayout.Label("Altitude");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				altitudeFrom = EditorGUILayout.FloatField("From", _renderer.AltFrom);
				altitudeTo   = EditorGUILayout.FloatField("To"  , _renderer.AltTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Longitude() {
			GUILayout.Label("Longitude");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				longitudeFrom = EditorGUILayout.FloatField("From", _renderer.LonFrom);
				longitudeTo   = EditorGUILayout.FloatField("To"  , _renderer.LonTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Latitude() {
			GUILayout.Label("Latitude");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				latitudeFrom = EditorGUILayout.FloatField("From", _renderer.LatFrom);
				latitudeTo   = EditorGUILayout.FloatField("To"  , _renderer.LatTo  );
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}

		private void Smoothness() {
			GUILayout.Label("Smoothness");
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				smoothnessP = EditorGUILayout.IntField("Parallels", _renderer.SmoothP);
				smoothnessM = EditorGUILayout.IntField("Meridians" , _renderer.SmoothM);
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
		}
#endregion  // Fields
	}
}
