using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( menuName = "Kinematic 2D/Implementation/Movement/Horizontal Movement Data" ) , System.Serializable ]
public class HorizontalMovementProfile : ScriptableObject
{
	[Header("Movement")]
	
	[Tooltip( "Walk speed in units per second.")]
	[Range_NoSlider(true)] public float walkSpeed = 5f;

	[Tooltip( "Time for the character to reach the walk speed.")]
	[Range_NoSlider(true)] public float walkDuration = 0.2f;	
   
	[Tooltip( "Air control = 0 -> no control while the character is not grounded." + 
	"Air control = 1 -> full control while the character is not grounded.")]
	[Range_NoSlider( 0f , 1f )] public float airControl = 0.7f; 

	[Range_NoSlider( true )] public float entrySpeedFactor = 1f;

	

	

	
	
}
