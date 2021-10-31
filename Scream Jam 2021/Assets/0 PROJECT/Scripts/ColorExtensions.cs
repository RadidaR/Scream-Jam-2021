using UnityEngine;

public static class ColorExtensions
{
	public static string ToHex(this Color color)
	{
		return $"# {ColorUtility.ToHtmlStringRGBA(color)}";
	}
	
	public static Color SetAlpha(this Color color, float? a = 1f)
	{
		return new Color(color.r, color.g, color.b, a ?? 1f);
	}
	
	public static Color LerpToColor(this Color color, Color newColor, float percent)
	{
		return new Color(Mathf.Lerp(color.r, newColor.r, percent), Mathf.Lerp(color.g, newColor.g, percent), Mathf.Lerp(color.b, newColor.b, percent), Mathf.Lerp(color.a, newColor.a, percent));
	}
	//Lerp Colors
}
