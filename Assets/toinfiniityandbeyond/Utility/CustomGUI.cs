using UnityEngine;
using UnityEditor;

namespace toinfiniityandbeyond.Utility
{
	public static class CustomGUILayout
	{
		private static readonly Color splitterColor = EditorGUIUtility.isProSkin ? new Color (0.157f, 0.157f, 0.157f) : new Color (0.5f, 0.5f, 0.5f);

		public static void Splitter (Color rgb, float thickness = 1)
		{
			Rect position = GUILayoutUtility.GetRect (GUIContent.none, CustomStyles.splitter, GUILayout.Height (thickness));

			if (Event.current.type == EventType.Repaint)
			{
				Color restoreColor = UnityEngine.GUI.color;
				GUI.color = rgb;
				CustomStyles.splitter.Draw (position, false, false, false, false);
				GUI.color = restoreColor;
			}
		}

		public static void Splitter (float thickness, GUIStyle splitterStyle)
		{
			Rect position = GUILayoutUtility.GetRect (GUIContent.none, splitterStyle, GUILayout.Height (thickness));

			if (Event.current.type == EventType.Repaint)
			{
				Color restoreColor = GUI.color;
				GUI.color = splitterColor;
				splitterStyle.Draw (position, false, false, false, false);
				GUI.color = restoreColor;
			}
		}

		public static void Splitter (float thickness = 1)
		{
			Splitter (thickness, CustomStyles.splitter);
		}
	}
	public static class CustomGUI { 
		private static readonly Color splitterColor = EditorGUIUtility.isProSkin ? new Color (0.157f, 0.157f, 0.157f) : new Color (0.5f, 0.5f, 0.5f);
		// GUI Style
		public static void Splitter (Rect position)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Color restoreColor = UnityEngine.GUI.color;
				UnityEngine.GUI.color = splitterColor;
				CustomStyles.splitter.Draw (position, false, false, false, false);
				UnityEngine.GUI.color = restoreColor;
			}
		}

	}

	public static class CustomStyles
	{
		public static readonly GUIStyle splitter;
		public static readonly GUIStyle centeredMiniLabel;
		public static readonly GUIStyle centeredBoldLabel;
		public static readonly GUIStyle centeredWhiteBoldLabel;
		public static readonly GUIStyle centeredWhiteMiniLabel;

		static CustomStyles ()
		{
			splitter = new GUIStyle ();
			splitter.normal.background = EditorGUIUtility.whiteTexture;
			splitter.stretchWidth = true;
			splitter.margin = new RectOffset (0, 0, 7, 7);

			centeredWhiteBoldLabel = new GUIStyle (EditorStyles.whiteBoldLabel);
			centeredWhiteBoldLabel.alignment = TextAnchor.MiddleCenter;
			centeredWhiteBoldLabel.wordWrap = true;

			centeredWhiteMiniLabel = new GUIStyle (EditorStyles.whiteMiniLabel);
			centeredWhiteMiniLabel.alignment = TextAnchor.MiddleCenter;
			centeredWhiteMiniLabel.wordWrap = true;

			centeredBoldLabel = new GUIStyle (EditorStyles.boldLabel);
			centeredBoldLabel.alignment = TextAnchor.MiddleCenter;
			centeredBoldLabel.wordWrap = true;

			centeredMiniLabel = new GUIStyle (EditorStyles.miniLabel);
			centeredMiniLabel.alignment = TextAnchor.MiddleCenter;
			centeredMiniLabel.wordWrap = true;
		}
	}
}