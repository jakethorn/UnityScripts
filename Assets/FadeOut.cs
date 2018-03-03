using UnityEngine;

public class FadeOut : MonoBehaviour
{
	public Texture2D fadeTexture;
	public float fadeSpeed = .2f;

	private float alpha = 0;
	private int drawDepth = -1000;

	private static bool startFade;

	void OnGUI()
	{
		if (startFade)
		{
			alpha += fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);

			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = drawDepth;

			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
		}
	}

	public static void StartFade()
	{
		startFade = true;
	}
}
