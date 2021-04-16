using System;

namespace Unity.InteractiveTutorials
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class SerializedTypeFilterAttributeBase : Attribute
    {
        public Type baseType { get; protected set; }

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SerializedTypeFilterAttribute : SerializedTypeFilterAttributeBase
    {

        public SerializedTypeFilterAttribute(Type baseType)
        {
            this.baseType = baseType;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SerializedTypeGUIViewFilterAttribute : SerializedTypeFilterAttributeBase
    {

        public SerializedTypeGUIViewFilterAttribute()
        {
            this.baseType = GUIViewProxy.guiViewType;
        }
    }
}
