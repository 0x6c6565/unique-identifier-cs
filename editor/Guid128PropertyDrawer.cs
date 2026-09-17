using System;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace UniqueIdentifiers.Editor
{
	using static EditorGUIUtility;

	/// <summary></summary>
	[CustomPropertyDrawer(typeof(Guid128))]
	public class Guid128PropertyDrawer : PropertyDrawer
	{
		/// <summary></summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			VisualElement root = new VisualElement();
			root.style.flexDirection = FlexDirection.Row;

			Guid128 guid = ToGuid128(property);

			VisualElement label = new Label(property.displayName);
			label.style.width = labelWidth;
			root.Add(label);

			Label value = new Label(guid.ToString());
			value.style.width = fieldWidth;
			value.style.flexGrow = 1;

			root.TrackPropertyValue(property, property =>
			{
				value.text = ToGuid128(property).ToString();
			});

			value.AddManipulator(new ContextualMenuManipulator(@event =>
			{
				@event.menu.AppendAction("Make Empty", action =>
				{
					AssignGuid128(property, Guid128.Empty);
				});
				@event.menu.AppendAction("New Guid128", action =>
				{
					AssignGuid128(property, Guid128.NewGuid128());
				});
				@event.menu.AppendAction("Copy", action =>
				{
					systemCopyBuffer = guid.ToString();
				});
				@event.menu.AppendAction("Paste", action =>
				{
					if (Guid128.TryParse(systemCopyBuffer, out Guid128 guid))
					{
						AssignGuid128(property, guid);
					}
				});
			}));

			root.Add(value);

			return root;
		}

		/// <summary></summary>
		/// <param name="property"></param>
		/// <param name="guid"></param>
		public static void AssignGuid128(SerializedProperty property, Guid128 guid)
		{
			property.FindPropertyRelative(nameof(Guid128._a)).intValue = guid._a;
			property.FindPropertyRelative(nameof(Guid128._b)).intValue = guid._b;
			property.FindPropertyRelative(nameof(Guid128._c)).intValue = guid._c;
			property.FindPropertyRelative(nameof(Guid128._d)).intValue = guid._d;
			property.FindPropertyRelative(nameof(Guid128._e)).intValue = guid._e;
			property.FindPropertyRelative(nameof(Guid128._f)).intValue = guid._f;
			property.FindPropertyRelative(nameof(Guid128._g)).intValue = guid._g;
			property.FindPropertyRelative(nameof(Guid128._h)).intValue = guid._h;
			property.FindPropertyRelative(nameof(Guid128._i)).intValue = guid._i;
			property.FindPropertyRelative(nameof(Guid128._j)).intValue = guid._j;
			property.FindPropertyRelative(nameof(Guid128._k)).intValue = guid._k;

			property.serializedObject.ApplyModifiedProperties();
		}

		/// <summary></summary>
		/// <param name="property"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static Guid128 ToGuid128(SerializedProperty property)
		{
			return null != property ?
				new Guid128(property.FindPropertyRelative(nameof(Guid128._a)).intValue,
							   (short)property.FindPropertyRelative(nameof(Guid128._b)).intValue,
							   (short)property.FindPropertyRelative(nameof(Guid128._c)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._d)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._e)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._f)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._g)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._h)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._i)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._j)).intValue,
							   (byte)property.FindPropertyRelative(nameof(Guid128._k)).intValue) :
							   throw new ArgumentNullException("ToGuid128() property is null");
		}
	}
}
