using System;

namespace APIBaseTest.ExtensionsTest
{
    internal static class ExtensionType
    {
        public static bool IsPrimitiveType(this Type type)
        {
            var response =
                type == typeof(Char) || type == typeof(String) ||
                type == typeof(Boolean) ||
                type == typeof(SByte) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64) ||
                type == typeof(Byte) || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64) ||
                type == typeof(Single) || type == typeof(Double) || type == typeof(Decimal) ||
                type == typeof(DateTime) ||
                type == typeof(Guid) ||
                type == typeof(TimeSpan)
                //type == typeof(IntPtr) ||
                //type == typeof(HashCode) ||
                //type == typeof(Object) ||
                ;
            return response;

        }
    }
}
