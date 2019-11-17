using UnityEditor;
using UnityEngine;
using GridFramework.Renderers;

namespace GridFramework.Editor.Renderers {
	/// <summary>
	///   Base class for all renderer inspectors.
	/// </summary>
	/// <typeparam name="T">
	///   Type of renderer the inspector is for
	/// </typeparam>
	/// <remarks>
	///   <para>
	///     You should use this class as the base of your own renderer
	///     components. It displays the common fields and offers a function to
	///     override for your own fields.
	///   </para>
	///   <para>
	///     You do not have to inherit from this class, but it helps to have
	///     uniformity in the look of the inspectors. If you decide to write an
	///     inspector from scratch make sure you have fields for the renderer's
	///     <c>Meterial</c>, <c>Priority</c>, <c>LineWidth</c> and the axis
	///     colours (<c>ColorX</c>, <c>ColorY</c> and <c>ColorZ</c>).
	///   </para>
	/// </remarks>
	public abstract class RendererEditor<T> : UnityEditor.Editor where T: GridRenderer {
#region  Protected variables
		/// <summary>
		///   Reference to the target renderer.
		/// </summary>
		protected T _renderer;
#endregion  // Protected variables

#region  Callback methods
		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();

			// Record results from the inspector fields here
			var material = MaterialFields();
			var priority = PriorityFields();
			var lineWidth = LineWidthFields();
			var colors = ColorFields();
			SpecificFields();
			
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(target, "Edit " + typeof(T).Name);
				// Assign the recorded inspector fields here
				_renderer.Material = material;
				_renderer.Priority = priority;
				_renderer.LineWidth = lineWidth;
				_renderer.ColorX = colors[0];
				_renderer.ColorY = colors[1];
				_renderer.ColorZ = colors[2];
				AssignSpecific();
				EditorUtility.SetDirty(_renderer);
			}
		}

		void OnEnable () {
			_renderer = target as T;
		}
#endregion  // Callback methods

#region  Field methods
		private Material MaterialFields() {
			return (Material)EditorGUILayout.ObjectField(
				"Material",
				_renderer.Material,
				typeof(Material),
				false
			);
		}

		private float LineWidthFields() {
			var width = EditorGUILayout.FloatField("Line Width", _renderer.LineWidth);
			return Mathf.Max(width, 0f);
		}

		private int PriorityFields() {
			var priority = _renderer.Priority;
			return EditorGUILayout.IntField("Priority", priority);
		}

		private Color[] ColorFields() {
			Color[] colors = new Color[3];
			GUILayout.Label("Axis Colors");
			
			EditorGUILayout.BeginHorizontal();
			++EditorGUI.indentLevel; {
				colors[0] = EditorGUILayout.ColorField(_renderer.ColorX);
				colors[1] = EditorGUILayout.ColorField(_renderer.ColorY);
				colors[2] = EditorGUILayout.ColorField(_renderer.ColorZ);
			}
			--EditorGUI.indentLevel;
			EditorGUILayout.EndHorizontal();
			return colors;
		}
#endregion  // Field methods

#region  Protected methods
		/// <summary>
		///   Override this to show your own inspector fields.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method will be called after all common fields from within
		///     the <c>InspectorFields()</c> callback have been displayed. You
		///     should implement your own inspector fields here.
		///   </para>
		///   <para>
		///     Due to how Unity's undo system works you should only display
		///     the fields here and store the results in a private variable.
		///     Then assign the results inside the <see cref="AssignSpecific"/>
		///     method.
		///   </para>
		///   <example>
		///     <code>
		///       // Specific fields for the parallelepiped renderer
		///       private Vector3 from, to;
		///
		///       protected override void SpecificFields() {
		///           from = EditorGUILayout.Vector3Field("From", _renderer.From);
		///           to   = EditorGUILayout.Vector3Field("To"  , _renderer.To  );
		///       }
		///     </code>
		///   </example>
		///   <para>
		///     If you do not follow this advice things will still work fine,
		///     but the inspector fields will not play nice with Unity's undo.
		///   </para>
		/// </remarks>
		protected abstract void SpecificFields();

		/// <summary>
		///   Override this to assign your own inspector variables.
		/// </summary>
		/// <remarks>
		///   <code>
		///     This method should be used to assign the results from the field
		///     methods defined in <see cref="SpecificFields">.
		///   </code>
		///   <example>
		///     <code>
		///       // Specific fields for the parallelepiped renderer
		///       private Vector3 from, to;
		///
		///       protected override void AssignSpecific() {
		///           _renderer.From = from;
		///           _renderer.To   = to;
		///       }
		///     </code>
		///   </example>
		/// </remarks>
		protected abstract void AssignSpecific();
#endregion  // Protected methods
	}
}
