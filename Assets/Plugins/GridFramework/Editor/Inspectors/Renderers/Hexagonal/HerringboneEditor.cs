using UnityEditor;
using GridFramework.Renderers.Hexagonal;
using Vector3 = UnityEngine.Vector3;
using Shift = GridFramework.Renderers.Hexagonal.Herringbone.OddColumnShift;

namespace GridFramework.Editor.Renderers.Hexagonal {
	/// <summary>
	///   Inspector for hexagonal herringbone renderers.
	/// </summary>
	[CustomEditor (typeof(Herringbone))]
	public class HerringboneEditor : RendererEditor<Herringbone> {
#region  Cached variables
		private Vector3 from, to;
		private Shift shift;
#endregion  // Cached variables

#region  Implementation
		protected override void SpecificFields() {
			shift = (Shift)EditorGUILayout.EnumPopup("Shift", _renderer.Shift);
			from = EditorGUILayout.Vector3Field("From", _renderer.From);
			to   = EditorGUILayout.Vector3Field( "To" , _renderer.To  );
		}

		protected override void AssignSpecific() {
			_renderer.Shift = shift;
			_renderer.From = from;
			_renderer.To = to;
		}
#endregion  // Implementation
	}
}
