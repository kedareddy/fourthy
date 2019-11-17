using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightbug.Kinematic2D.Implementation
{

[AddComponentMenu("Kinematic2D/Implementation/Abilities/Horizontal Movement")]

public class HorizontalMovement : CharacterAbility
{     
	public HorizontalMovementProfile horizontalData;
	
	public bool isAffectedByMovementAreas = true;


	[SerializeField]
	[Range( 0f , 1f )]
	[Tooltip("This multiplier will affect the overall walk speed only if the character is crouching.")]
	float crouchSpeedMultiplier = 0.5f;

	HorizontalMovementProfile defaultHorizontalData;
	
	
	float horizontalSmoothDampSpeed = 0;

	GameObject collidedTrigger = null;	
	

    	protected override void Awake()
	{
		base.Awake();

		if(horizontalData == null)
		{
			Debug.Log("Missing movement data");
			return;
		}

		defaultHorizontalData = horizontalData;
	}     



     public override void Process( float dt )
	{
		if( !movementController.isCurrentlyOnState( MovementState.Normal ) && 
			!movementController.isCurrentlyOnState( MovementState.JetPack ) )
			return;

		if(horizontalData == null)
		{
			Debug.Log("Missing movement data");
			return;
		}
		
		ProcessHorizontalMovement();
		ProcessMovementArea();
	}

	void ProcessHorizontalMovement()
	{
		
		
		float movementControl = characterController2D.IsGrounded ? 1 : horizontalData.airControl;
		
		float duration = horizontalData.walkDuration * ( 1 / movementControl );
		
		float targetSpeed = 0;
		
		if( characterBrain.CharacterAction.right )
			targetSpeed = horizontalData.walkSpeed;
		else if( characterBrain.CharacterAction.left )
			targetSpeed = - horizontalData.walkSpeed;		
		
		if( characterController2D.PoseController.isCurrentlyOnState( PoseState.Crouch ) )
			targetSpeed *= crouchSpeedMultiplier;

		characterController2D.SetVelocityX( Mathf.SmoothDamp( characterController2D.Velocity.x , targetSpeed , ref horizontalSmoothDampSpeed , duration) );
	}

	void ProcessMovementArea()
	{
		if(!isAffectedByMovementAreas)
			return;

		if( characterController2D.CollidedTrigger == null )
		{
			collidedTrigger = null;
			RevertMovementParameters();
			return;
		}
		else
		{
			
			if( ( collidedTrigger == null ) || 
				( characterController2D.CollidedTrigger != collidedTrigger ))
			{
				MovementArea movementArea = characterController2D.CollidedTrigger.GetComponent<MovementArea>();
				if( movementArea != null )
				{
					SetMovementParameters( movementArea );
				}

				collidedTrigger = characterController2D.CollidedTrigger;
			}

			
		
		}
	}

	

	

	void SetMovementParameters( MovementArea movementArea )
	{
		CharacterMovementProfile data = movementArea.CharacterMovementData;
		if( data == null  )
			return;
		
		if( data.horizontalMovementData != null )
		{
			this.horizontalData = data.horizontalMovementData;

			characterController2D.SetVelocityX( characterController2D.Velocity.x * data.horizontalMovementData.entrySpeedFactor );			
		}
		
	}

	void RevertMovementParameters()
	{	
		this.horizontalData = defaultHorizontalData;
		
	}


	public override string GetInfo()
	{ 
		return "It handles the horizontal movement of the character in normal state."; 
	}
	
}

}
