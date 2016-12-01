using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TOD_MinAttribute))]
public class TOD_MaxDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var attr = attribute as TOD_MaxAttribute;

		EditorGUI.PropertyField(position, property, label);

		if (property.propertyType == SerializedPropertyType.Float)
		{
			property.floatValue = Mathf.Min(property.floatValue, attr.max);
		}
		else if (property.propertyType == SerializedPropertyType.Integer)
		{
			property.intValue = Mathf.Min(property.intValue, Mathf.RoundToInt(attr.max));
		}
	}
}

[CustomPropertyDrawer(typeof(TOD_MinAttribute))]
public class TOD_MinDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var attr = attribute as TOD_MinAttribute;

		EditorGUI.PropertyField(position, property, label);

		if (property.propertyType == SerializedPropertyType.Float)
		{
			property.floatValue = Mathf.Max(property.floatValue, attr.min);
		}
		else if (property.propertyType == SerializedPropertyType.Integer)
		{
			property.intValue = Mathf.Max(property.intValue, Mathf.RoundToInt(attr.min));
		}
	}
}

[CustomPropertyDrawer(typeof(TOD_RangeAttribute))]
public class TOD_RangeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var attr = attribute as TOD_RangeAttribute;

		EditorGUI.PropertyField(position, property, label);

		if (property.propertyType == SerializedPropertyType.Float)
		{
			property.floatValue = Mathf.Clamp(property.floatValue, attr.min, attr.max);
		}
		else if (property.propertyType == SerializedPropertyType.Integer)
		{
			property.intValue = Mathf.Clamp(property.intValue, Mathf.RoundToInt(attr.min), Mathf.RoundToInt(attr.max));
		}
	}
}
