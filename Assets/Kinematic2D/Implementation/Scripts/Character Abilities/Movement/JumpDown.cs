﻿using UnityEngine;
using Lightbug.Kinematic2D.Core;
using Lightbug.CoreUtilities;

namespace Lightbug.Kinematic2D.Implementation
{

[AddComponentMenu("Kinematic2D/Implementation/Abilities/Jump Down")]
public class JumpDown : CharacterAbility
{     
	const float CastDistance = 0.1f;

	[Tooltip("The distance the character will descend when the jump down ability is performed.\n"
	+ "A lowest value may not ensure that the character will properly jump down (because of the inherent nature of the collision). "
	+ "A higher value may result in an ugly transition.")]
	[Range_NoSlider( 0.01f, Mathf.Infinity)]
	[SerializeField] 
	float jumpDownDistance = 0.05f;

	[Tooltip("whether or not the horizontal velocity should reset before jumping down the platform.")]
	[SerializeField]
	bool resetHorizontalVelocity = false;

	LayerMask layerMask;

	protected override void Awake()
	{
		base.Awake();

		layerMask = characterController2D.layerMaskSettings.profile.oneWayPlatforms;
	}


     public override void Process(float dt)
     {			
		if( !movementController.isCurrentlyOnState( MovementState.Normal ) )
			return;

		if( characterBrain.CharacterAction.down)
		{
			if( characterBrain.CharacterAction.jumpPressed )
			{
				if( CheckGround() )
					ProcessJumpDown();
			}

		}			
				


     }

	void ProcessJumpDown()
	{
		characterController2D.ForceNotGroundedState();

		Vector3 deltaPosition = Vector3.down * ( characterBody.SkinWidth + characterBody.BoxThickness + jumpDownDistance);

		characterController2D.Teleport( characterBody.bodyTransform.Position + deltaPosition , characterBody.bodyTransform.Rotation );

		characterController2D.SetVelocityY( 0 );

		if( resetHorizontalVelocity )
			characterController2D.SetVelocityX( 0 );
			
	}

	
	bool CheckGround()
	{
		CollisionHitInfo hitInfo = new CollisionHitInfo();
		
		hitInfo = characterController2D.CharacterCollisions.VerticalNotGroundedCollision( 
			- CastDistance ,
			layerMask 
		);

		if(hitInfo.collision)
			if( Utilities.BelongsToLayerMask( hitInfo.gameObject.layer , layerMask) )
				return true;		
				

		return false;
	}

	public override string GetInfo()
	{ 
		return "This ability allows the character to descend vertically from a one way platform by pressing \"Down\" + \"Jump\""; 
	}
	
}

}