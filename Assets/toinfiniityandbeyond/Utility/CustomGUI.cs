using UnityEngine;
using UnityEditor;

using System;
using System.Reflection;

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
		public static readonly GUIStyle centerMiniLabel;
		public static readonly GUIStyle leftBoldLabel;
		public static readonly GUIStyle centerBoldLabel;
		public static readonly GUIStyle centerWhiteBoldLabel;
		public static readonly GUIStyle centerWhiteMiniLabel;

		static CustomStyles ()
		{
			splitter = new GUIStyle ();
			splitter.normal.background = EditorGUIUtility.whiteTexture;
			splitter.stretchWidth = true;
			splitter.margin = new RectOffset (0, 0, 7, 7);

			centerWhiteBoldLabel = new GUIStyle (EditorStyles.whiteBoldLabel);
			centerWhiteBoldLabel.alignment = TextAnchor.MiddleCenter;
			centerWhiteBoldLabel.wordWrap = true;

			centerWhiteMiniLabel = new GUIStyle (EditorStyles.whiteMiniLabel);
			centerWhiteMiniLabel.alignment = TextAnchor.MiddleCenter;
			centerWhiteMiniLabel.wordWrap = true;

			leftBoldLabel = new GUIStyle (EditorStyles.boldLabel);
			leftBoldLabel.alignment = TextAnchor.MiddleLeft;
			leftBoldLabel.wordWrap = true;

			centerBoldLabel = new GUIStyle (EditorStyles.boldLabel);
			centerBoldLabel.alignment = TextAnchor.MiddleCenter;
			centerBoldLabel.wordWrap = true;

			centerMiniLabel = new GUIStyle (EditorStyles.miniLabel);
			centerMiniLabel.alignment = TextAnchor.MiddleCenter;
			centerMiniLabel.wordWrap = true;
		}
	}
}