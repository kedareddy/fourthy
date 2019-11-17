using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace MyTools
{
		namespace MyExtensions
		{
				public static class StringExtensions
				{

	
						//String Extensions
						public static int StringToInt (this string str)
						{
								int parsedInt = 0;
								if (str != null && int.TryParse (str, out parsedInt))
										return parsedInt;
								return 0;
						}
	
						public static float StringToFloat (this string str)
						{
								float parsedFloat = 0f;
								if (str != null && float.TryParse (str, out parsedFloat))
										return parsedFloat;
								return 0f;
						}
	
						public static Vector3 StringToVector3 (this string aStr)
						{
								Vector3 v = new Vector3 (0, 0, 0);
								if (aStr != null && aStr.Length > 0) {
										try {
												if (aStr.IndexOf (",", 0) >= 0) {  // 0.3, 1.0, 0.2 format
														int p0 = 0;
														int p1 = 0;
														int c = 0;
														p1 = aStr.IndexOf (",", p0);
														while (p1>p0 && c<=3) {
																v [c++] = float.Parse (aStr.Substring (p0, p1 - p0));
																p0 = p1 + 1;
																if (p0 < aStr.Length)
																		p1 = aStr.IndexOf (",", p0);
																if (p1 < 0)
																		p1 = aStr.Length;
														}
												}
										} catch (Exception e) {
												Debug.Log ("Could not convert " + aStr + " to Vector3. " + e);
												return new Vector3 (0, 0, 0);
										}
								}
								return v;
						}
	
						public static string FloatToString (this float aFloat, int decimals)
						{
								if (decimals <= 0)
										return "" + Mathf.RoundToInt (aFloat);
								string format = "{0:F" + decimals + "}";
								return string.Format (format, aFloat);
						}
	
						public static string Vector3ToString (this Vector3 v, int decimals)
						{
								if (decimals <= 0)
										return "<" + Mathf.RoundToInt (v.x) + "," + Mathf.RoundToInt (v.y) + "," + Mathf.RoundToInt (v.z) + ">";
								string format = "{0:F" + decimals + "}";
								return "<" + string.Format (format, v.x) + "," + string.Format (format, v.y) + "," + string.Format (format, v.z) + ">";
						}
				}
		}
}