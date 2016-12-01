using UnityEngine;

/// Utility method class.
///
/// All of those methods should really be in the Unity API, but they're not.

public static class TOD_Util
{
	/// Multiply the RGB components of a color.
	/// \param color The color.
	/// \param multiplier The multiplier.
	/// \returns The input color with its RGB components multiplied by multiplier.
	public static Color MulRGB(Color color, float multiplier)
	{
		if (multiplier == 1) return color;
		return new Color(color.r * multiplier, color.g * multiplier, color.b * multiplier, color.a);
	}

	/// Multiply the RGBA components of a color.
	/// \param color The color.
	/// \param multiplier The multiplier.
	/// \returns The input color with its RGB components multiplied by multiplier.
	public static Color MulRGBA(Color color, float multiplier)
	{
		if (multiplier == 1) return color;
		return new Color(color.r * multiplier, color.g * multiplier, color.b * multiplier, color.a * multiplier);
	}

	/// Power of the RGB components of a color.
	/// \param color The color.
	/// \param power The power.
	/// \returns The input color with its RGB components raised by the exponent power.
	public static Color PowRGB(Color color, float power)
	{
		if (power == 1) return color;
		return new Color(Mathf.Pow(color.r, power), Mathf.Pow(color.g, power), Mathf.Pow(color.b, power), color.a);
	}

	/// Power of the RGBA components of a color.
	/// \param color The color.
	/// \param power The power.
	/// \returns The input color with its RGBA components raised by the exponent power.
	public static Color PowRGBA(Color color, float power)
	{
		if (power == 1) return color;
		return new Color(Mathf.Pow(color.r, power), Mathf.Pow(color.g, power), Mathf.Pow(color.b, power), Mathf.Pow(color.a, power));
	}
}
