using System;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace UniqueIdentifiers.Editor
{
	using static EditorGUIUtility;

	/// <summary></summary>
    [CustomPropertyDrawer(typeof(Guid64))]
    public class Guid64PropertyDrawer : PropertyDrawer
    {
		/// <summary></summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			VisualElement root = new VisualElement();
			root.style.flexDirection = FlexDirection.Row;

			Guid64 guid = new Guid64(property.FindPropertyRelative(nameof(Guid64.value)).ulongValue);

			VisualElement label = new Label(property.displayName);
			label.style.width = labelWidth;
			root.Add(label);

			Label value = new Label(guid.ToString());
			value.style.width = fieldWidth;
			value.style.flexGrow = 1;

			root.TrackPropertyValue(property, property =>
			{
				value.text = new Guid64(property.FindPropertyRelative(nameof(Guid64.value)).ulongValue).ToString();
			});

			value.AddManipulator(new ContextualMenuManipulator(@event =>
			{
				@event.menu.AppendAction("Make Empty", action =>
				{
					AssignGuid64(property, Guid64.Empty); 
				});
				@event.menu.AppendAction("New Guid64", action =>
				{
					AssignGuid64(property, Guid64.NewGuid64());
				});
				@event.menu.AppendAction("Copy", action =>
				{
					systemCopyBuffer = guid.ToString();
				});
				@event.menu.AppendAction("Paste", action =>
				{
					if (Guid64.TryParse(systemCopyBuffer, out guid))
					{
						AssignGuid64(property, guid);
					}
				});
			}));

			root.Add(value);

			return root;
		}

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			Guid64 guid = new Guid64(property.FindPropertyRelative(nameof(Guid64.value)).ulongValue);

			EditorGUI.LabelField(rect, label.text, guid.ToString());

			if (null != Event.current && EventType.ContextClick == Event.current.type)
			{
				if (rect.Contains(Event.current.mousePosition))
				{
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("New Guid64"),
								 false,
								 prop => AssignGuid64(prop as SerializedProperty, Guid64.NewGuid64()),
								 property);

					if (Guid64.Empty != guid)
					{
						menu.AddItem(new GUIContent("Copy"),
									 false,
									 prop => systemCopyBuffer = ((Guid64)prop).ToString(),
									 guid);
					}
					else
					{
						menu.AddDisabledItem(new GUIContent("Copy"));
					}

					if (Guid64.TryParse(systemCopyBuffer, out Guid64 clipboard))
					{
						if (clipboard != guid)
						{
							menu.AddItem(new GUIContent("Paste"),
										 false,
										 prop =>
										 {
											 if (Guid64.TryParse(systemCopyBuffer, out Guid64 clipboard))
											 {
												 AssignGuid64(prop as SerializedProperty, clipboard);
											 }
										 },
										 property);
						}
						else
						{
							menu.AddDisabledItem(new GUIContent("Paste"));
						}
					}
					else
					{
						menu.AddDisabledItem(new GUIContent("Paste"));
					}

					if (Guid64.Empty == guid)
					{
						menu.AddDisabledItem(new GUIContent("Make Empty"));
					}
					else
					{
						menu.AddItem(new GUIContent("Make Empty"), false,
									 prop => AssignGuid64(prop as SerializedProperty, Guid64.Empty),
									 property);
					}

					menu.ShowAsContext();
				}
			}
		}

		/// <summary></summary>
		/// <param name="property"></param>
		/// <param name="guid"></param>
		public static void AssignGuid64(SerializedProperty property, Guid64 guid)
		{
			property.FindPropertyRelative(nameof(Guid64.value)).ulongValue = guid.value;
			property.serializedObject.ApplyModifiedProperties();
		}
	}
}
