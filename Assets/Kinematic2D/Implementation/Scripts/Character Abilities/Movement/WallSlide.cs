using UnityEngine;
using Lightbug.Kinematic2D.Core;
using Lightbug.CoreUtilities;

namespace Lightbug.Kinematic2D.Implementation
{

	

public enum WallSlideMode
{
     Constant ,
     Accelerated
}

[AddComponentMenu("Kinematic2D/Implementation/Abilities/Wall Slide")]
public class WallSlide : CharacterAbility
{
	
	[Tooltip("The tag that the objects must have for the character to interact with.")]
	[SerializeField]
	string wallSlideTag = "WallSlide";

	[Tooltip("Speed for the wall slide ability")]
	[SerializeField] 
	float wallSlideSpeed = 2f;

	[Range_NoSlider(true)] [SerializeField]
	float wallSlideAccelerationFactor = 2f;	
		
	[SerializeField] 
	WallSlideMode wallSlideMode = WallSlideMode.Constant;

	LayerMask layerMask;

	protected override void Awake()
	{
		base.Awake();

		layerMask = characterController2D.layerMaskSettings.profile.obstacles;
	}
	
     public override void Process(float dt)
     {
		if( movementController.isCurrentlyOnState( MovementState.Normal ) )
		{
			if( CheckWallSliding() &&  !characterBrain.CharacterAction.down )
			{
				movementController.SetState( MovementState.WallSlide );
				characterController2D.ResetVelocity();
			}
		}
		else if( movementController.isCurrentlyOnState( MovementState.WallSlide ) )
		{			
			ProcessWallSlide( dt );

			if( 	characterController2D.IsGrounded || 
				!CheckWallSliding() || 
				characterBrain.CharacterAction.down )
			{
				movementController.SetState( MovementState.Normal );
			}
		}

		
     }



	/// <summary>
	/// Check if the wall slide ability conditions are met (a wall in front of the character with the corresponding "Wall Sliding" Tag).
	/// </summary>
	public bool CheckWallSliding()
	{
		if(characterController2D.IsGrounded)
			return false;

		if(characterBrain.CharacterAction.down)
			return false;
		
		if( characterController2D.Velocity.y > 0 )
			return false;

		CollisionHitInfo info = new CollisionHitInfo();
		info.Reset();

		CardinalCollisionType cardinalCollisionType = 
		characterController2D.IsFacingRight ? 
		CardinalCollisionType.Right : 
		CardinalCollisionType.Left;

		float skin = characterBody.SkinWidth;
		
		info = characterController2D.CharacterCollisions.CardinalCollision( 
			cardinalCollisionType , 
			skin , 
			skin , 
			layerMask 
		);

		if(!info.collision)
			return false;

		float wallSignedAngle = Utilities.SignedAngle(characterBody.bodyTransform.Up , info.normal , characterBody.bodyTransform.Forward );
		float wallAngle = Mathf.Abs( wallSignedAngle );

		if ( !info.gameObject.CompareTag(wallSlideTag) || !Utilities.isCloseTo( wallAngle , 90 , 0.1f ) )
			return false;
			
		return true;
		
		
	}
	
	void ProcessWallSlide( float dt )
	{
		switch(wallSlideMode)
		{
			case WallSlideMode.Constant:
				characterController2D.SetVelocityY( - wallSlideSpeed );

			break;

			case WallSlideMode.Accelerated:

				characterController2D.SetVelocityY( 
                         Mathf.MoveTowards( 
                              characterController2D.Velocity.y , 
                              - wallSlideSpeed , 
                              wallSlideAccelerationFactor * dt 
                         ) 
                    );

			break;
		}
	}

	public override string GetInfo()
	{ 
		return "With this ability the character can grab onto a wall only if this is falling (negative vertical velocity) " +
		" and the wall is closely at 90 degrees respect to the ground. By doing this the state will change from \"Normal\"to \"WallSlide\""; 
	}
	
}

}