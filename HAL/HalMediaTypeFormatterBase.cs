using System;
using System.Net.Http.Formatting;

namespace HypertextRoots.HAL
{
    public class HalMediaTypeFormatterBase : BufferedMediaTypeFormatter
    {
        protected HalMediaTypeFormatterBase()
            : base()
        {
        }

        public override bool CanReadType(Type type)
        {
            return type.BaseType == typeof(LinkedResource) ||
                   (
                       type.IsGenericTypeDefinition &&
                       type.BaseType.GetGenericTypeDefinition() == typeof(LinkedResourceCollection<>)
                   );
        }

        public override bool CanWriteType(Type type)
        {
            return (type != null && type.BaseType != null) &&
                   (type.BaseType == typeof(LinkedResource) ||
                    (type.IsGenericTypeDefinition && type.BaseType.GetGenericTypeDefinition() == typeof(LinkedResourceCollection<>)));
        }
    }

}