using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace MyTools
{
		namespace MyExtensions
		{
				/**
		Provides some utility functions for Colors.
	*/
				public static class ColorExtensions
				{
						private const float LightOffset = 0.0625f;
						private const float DarkerFactor = 0.9f;

						/**
			Returns a color lighter than the given color.
		*/
						public static Color Lighter (this Color color)
						{
								return new Color (
				color.r + LightOffset,
				color.g + LightOffset,
				color.b + LightOffset,
				color.a);
						}

						/**
			Returns a color darker than the given color.
		*/
						public static Color Darker (this Color color)
						{
								return new Color (
				color.r - LightOffset,
				color.g - LightOffset,
				color.b - LightOffset,
				color.a);
						}

						/**
			Returns the brightness of the color, 
			defined as the average off the three color channels.
		*/
						public static float Brightness (this Color color)
						{
								return (color.r + color.g + color.b) / 3;
						}

						/**
			Returns a new color with the RGB values scaled so that the color 
			has the given brightness. 

			If the color is too dark, a grey is returned with the right brighness.

			The alpha is left uncanged.
		*/
						public static Color WithBrightness (this Color color, float brightness)
						{
								if (color.IsApproximatelyBlack ()) {
										return new Color (brightness, brightness, brightness, color.a);
								}
			
								float factor = brightness / color.Brightness ();

								float r = color.r * factor;
								float g = color.g * factor;
								float b = color.b * factor;

								float a = color.a;

								return new Color (r, g, b, a);
						}

						/**
			Returns whether the color is black or almost black.
		*/
						public static bool IsApproximatelyBlack (this Color color)
						{
								return color.r + color.g + color.b <= Mathf.Epsilon;
						}

						/**
			Returns whether the color is white or almost white.
		*/
						public static bool IsApproximatelyWhite (this Color color)
						{
								return color.r + color.g + color.b >= 1 - Mathf.Epsilon;
						}
		
						/**
			Returns an opaque version of the given color.
		*/
						public static Color Opaque (this Color color)
						{
								return new Color (color.r, color.g, color.b);
						}
		
						//Color Methods
						public static Color SetAlpha (this Color color, float alpha)
						{
								return new Color (color.r, color.g, color.b, alpha);
						}
		
						public static Color SetRed (this Color color, float red)
						{
								return new Color (red, color.g, color.b, color.a);
						}
		
						public static Color SetBlue (this Color color, float blue)
						{
								return new Color (color.r, color.g, blue, color.a);
						}
		
						public static Color SetGreen (this Color color, float green)
						{
								return new Color (color.r, green, color.b, color.a);
						}
		
		
						//Color to Hexadecimal String
						public static string ColorToHexString (Color aColor)
						{
								return ColorToHexString ((Color32)aColor, false);
						}
		
						public static string ColorToHexString (Color aColor, 
		                                       bool includeAlpha)
						{
								return ColorToHexString ((Color32)aColor, includeAlpha);
						}
		
						public static string ColorToHexString (Color32 aColor, 
		                                       bool includeAlpha)
						{
								string rs = Convert.ToString (aColor.r, 16).ToUpper ();
								string gs = Convert.ToString (aColor.g, 16).ToUpper ();
								string bs = Convert.ToString (aColor.b, 16).ToUpper ();
								string a_s = Convert.ToString (aColor.a, 16).ToUpper ();
								while (rs.Length < 2)
										rs = "0" + rs;
								while (gs.Length < 2)
										gs = "0" + gs;
								while (bs.Length < 2)
										bs = "0" + bs;
								while (a_s.Length < 2)
										a_s = "0" + a_s;
								if (includeAlpha)
										return "#" + rs + gs + bs + a_s;
								return "#" + rs + gs + bs;
						}
						//Hexadecimal String to Color
						public static Color HexStringToColor (string aStr)
						{
								Color clr = new Color (0, 0, 0);
								if (aStr != null && aStr.Length > 0) {
										try {
												string str = aStr.Substring (1, aStr.Length - 1);
												clr.r = (float)System.Int32.Parse (str.Substring (0, 2), 
					                                   NumberStyles.AllowHexSpecifier) / 255.0f;
												clr.g = (float)System.Int32.Parse (str.Substring (2, 2), 
					                                   NumberStyles.AllowHexSpecifier) / 255.0f;
												clr.b = (float)System.Int32.Parse (str.Substring (4, 2), 
					                                   NumberStyles.AllowHexSpecifier) / 255.0f;
												if (str.Length == 8)
														clr.a = System.Int32.Parse (str.Substring (6, 2), 
						                            NumberStyles.AllowHexSpecifier) / 255.0f;
												else
														clr.a = 1.0f;
										} catch (Exception e) {
												Debug.Log ("Could not convert " + aStr + " to Color. " + e);
												return new Color (0, 0, 0, 0);
										}
								}
								return clr;
						}
		
						public static Color FloatColorToColor (string aStr)
						{
								Color clr = new Color (0, 0, 0);
								if (aStr != null && aStr.Length > 0) {
										try {
												if (aStr.Substring (0, 1) == "#") {  // #FFFFFF format
														string str = aStr.Substring (1, aStr.Length - 1);
														clr.r = (float)System.Int32.Parse (str.Substring (0, 2), 
						                                   NumberStyles.AllowHexSpecifier) / 255.0f;
														clr.g = (float)System.Int32.Parse (str.Substring (2, 2), 
						                                   NumberStyles.AllowHexSpecifier) / 255.0f;
														clr.b = (float)System.Int32.Parse (str.Substring (4, 2), 
						                                   NumberStyles.AllowHexSpecifier) / 255.0f;
														if (str.Length == 8)
																clr.a = System.Int32.Parse (str.Substring (6, 2), 
							                            NumberStyles.AllowHexSpecifier) / 255.0f;
														else
																clr.a = 1.0f;
												} else if (aStr.IndexOf (",", 0) >= 0) {  // 0.3, 1.0, 0.2 format
														int p0 = 0;
														int p1 = 0;
														int c = 0;
														p1 = aStr.IndexOf (",", p0);
														while (p1>p0 && c<4) {
																clr [c++] = float.Parse (aStr.Substring (p0, p1 - p0));
																p0 = p1 + 1;
																if (p0 < aStr.Length)
																		p1 = aStr.IndexOf (",", p0);
																if (p1 < 0)
																		p1 = aStr.Length;
														}
														if (c < 4)
																clr.a = 1.0f;
												}
										} catch (Exception e) {
												Debug.Log ("Could not convert " + aStr + " to Color. " + e);
												return new Color (0, 0, 0, 0);
										}
								}
								return clr;
						}
		
						//To fade something to grey, simply reduce the Saturation, To makesomething darket, reduce the Brightness. 
						//The Vector's x, y, and z now represent hue, saturation and brightness.
						public static Vector3 ColorToHSBVector3 (Color c)
						{
								float minValue = Mathf.Min (c.r, Mathf.Min (c.g, c.b));
								float maxValue = Mathf.Max (c.r, Mathf.Max (c.g, c.b));
								float delta = maxValue - minValue;
								float h = 0.0f;
								float s = 0.0f;
								float b = maxValue;
			
								// # Calculate the hue (in degrees of a circle, between 0 and 360)
								if (maxValue == c.r) {
										if (c.g >= c.b) {
												if (delta == 0.0f)
														h = 0.0f;
												else
														h = 60.0f * (c.g - c.b) / delta;
										} else if (c.g < c.b) {
												h = 60.0f * (c.g - c.b) / delta + 360f;
										}
								} else if (maxValue == c.g) {
										h = 60.0f * (c.b - c.r) / delta + 120f;
								} else if (maxValue == c.b) {
										h = 60.0f * (c.r - c.g) / delta + 240f;
								}
			
								// Calculate the saturation (between 0 and 1)
								if (maxValue == 0.0)
										s = 0.0f;
								else
										s = 1.0f - (minValue / maxValue);
								return new Vector3 (h / 360.0f, s, b);
						}
		
						//Color to HSB Vector with alpha
						public static Vector4 ColorToHSBVector4 (Color c)
						{
								float minValue = Mathf.Min (c.r, Mathf.Min (c.g, c.b));
								float maxValue = Mathf.Max (c.r, Mathf.Max (c.g, c.b));
								float delta = maxValue - minValue;
								float h = 0.0f;
								float s = 0.0f;
								float b = maxValue;
			
								// # Calculate the hue (in degrees of a circle, between 0 and 360)
								if (maxValue == c.r) {
										if (c.g >= c.b) {
												if (delta == 0.0f)
														h = 0.0f;
												else
														h = 60.0f * (c.g - c.b) / delta;
										} else if (c.g < c.b) {
												h = 60.0f * (c.g - c.b) / delta + 360f;
										}
								} else if (maxValue == c.g) {
										h = 60.0f * (c.b - c.r) / delta + 120f;
								} else if (maxValue == c.b) {
										h = 60.0f * (c.r - c.g) / delta + 240f;
								}
			
								// Calculate the saturation (between 0 and 1)
								if (maxValue == 0.0)
										s = 0.0f;
								else
										s = 1.0f - (minValue / maxValue);
								return new Vector4 (h / 360.0f, s, b, c.a);
						}
		
						public static Color HSBVectorToColor (Vector3 hsb)
						{
								return HSBVectorToColor (new Vector4 (hsb.x, hsb.y, hsb.z, 1.0f));
						}
		
						public static Color HSBVectorToColor (Vector4 hsba)
						{
								// When saturation = 0, then r, g, b represent grey value (= brightness (z)).
								float r = hsba.z;
								float g = hsba.z;
								float b = hsba.z;
								if (hsba.y > 0.0f) {  // saturation > 0
										// Calc sector
										float secPos = (hsba.x * 360.0f) / 60.0f;
										int secNr = Mathf.FloorToInt (secPos);
										float secPortion = secPos - secNr;
				
										// Calc axes p, q and t
										float p = hsba.z * (1.0f - hsba.y);
										float q = hsba.z * (1.0f - (hsba.y * secPortion));
										float t = hsba.z * (1.0f - (hsba.y * (1.0f - secPortion)));
				
										// Calc rgb
										if (secNr == 1) {
												r = q;
												g = hsba.z;
												b = p;
										} else if (secNr == 2) {
												r = p;
												g = hsba.z;
												b = t;
										} else if (secNr == 3) {
												r = p;
												g = q;
												b = hsba.z;
										} else if (secNr == 4) {
												r = t;
												g = p;
												b = hsba.z;
										} else if (secNr == 5) {
												r = hsba.z;
												g = p;
												b = q;
										} else {
												r = hsba.z;
												g = t;
												b = p;
										}
								}
								return new Color (r, g, b, hsba.w);
						}
		
				}
		}
}
