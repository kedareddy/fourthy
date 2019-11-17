using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace MyTools
{
		namespace MyExtensions
		{
				public static class GameObjectExtensions
				{			

						public static bool HasRigidbody (this GameObject gobj)
						{
								return (gobj.GetComponent<Rigidbody>() != null);
						}
	
						public static bool HasAnimation (this GameObject gobj)
						{
								return (gobj.GetComponent<Animation>() != null);
						}
	
						public static void SetAnimationSpeed (this Animation anim, float newSpeed)
						{
								anim [anim.clip.name].speed = newSpeed; 
						}



						// Set the layer of this GameObject and all of its children.
						public static void SetLayerRecursively (this GameObject gameObject, int layer)
						{
								gameObject.layer = layer;
								foreach (Transform t in gameObject.transform)
										t.gameObject.SetLayerRecursively (layer);
						}

						public static void SetCollisionRecursively (this GameObject gameObject, bool tf)
						{
								Collider[] colliders = gameObject.GetComponentsInChildren<Collider> ();
								foreach (Collider collider in colliders) {
										collider.enabled = tf;
								}
						}

						public static void SetVisualRecursively (this GameObject gameObject, bool tf)
						{
								Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer> ();
								foreach (Renderer renderer in renderers)
										renderer.enabled = tf;
						}

						public static T GetComponentInParents<T> (this GameObject gameObject)
		where T : Component
						{
								for (Transform t = gameObject.transform; t != null; t = t.parent) {
										T result = t.GetComponent<T> ();
										if (result != null)
												return result;
								}
		
								return null;
						}

						public static T[] GetComponentsInChildrenWithTag<T> (this GameObject gameObject, string tag)
		where T: Component
						{
								List<T> results = new List<T> ();
		
								if (gameObject.CompareTag (tag)) {
										results.Add (gameObject.GetComponent<T> ());
								}

								foreach (Transform t in gameObject.transform) {
										results.AddRange (t.gameObject.GetComponentsInChildrenWithTag<T> (tag));
								}
								return results.ToArray ();
						}

						public static T[] GetComponentsInParents<T> (this GameObject gameObject)
		where T: Component
						{
								List<T> results = new List<T> ();
								for (Transform t = gameObject.transform; t != null; t = t.parent) {
										T result = t.GetComponent<T> ();
										if (result != null)
												results.Add (result);
								}
		
								return results.ToArray ();
						}

				}
		}
}