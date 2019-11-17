using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( menuName = "Kinematic 2D/Implementation/Movement/Vertical Movement Profile" ) , System.Serializable ]
public class VerticalMovementProfile : ScriptableObject
{

	[Header("Jumping and Falling")]	

	[Range_NoSlider(true)] public float jumpDuration = 0.4f;
	[Range_NoSlider(true)] public float jumpHeight = 2.5f;

	[Range_NoSlider( true )] public float entrySpeedFactor = 1f;

	

}
