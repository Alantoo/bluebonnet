
namespace system
{

    public static partial class Util
    {

        public static sun.misc.Unsafe JavaUnsafe
        {
            get
            {
                sun.misc.Unsafe result = _JavaUnsafe;
                if (result == null)
                {
                    result = GetHelper() as sun.misc.Unsafe;
                    if (result == null)
                        throw new System.NotSupportedException("missing Java Unsafe");
                    _JavaUnsafe = result;
                }
                return result;

                object GetHelper()
                {
                    var unsafeClass = java.lang.Class.forName("sun.misc.Unsafe");
                    java.lang.reflect.Field unsafeField = null;
                    try
                    {
                        unsafeField = unsafeClass.getDeclaredField("theUnsafe");
                    }
                    catch
                    {
                        try // alternate name in old versions of Android
                        {
                            unsafeField = unsafeClass.getDeclaredField("THE_ONE");
                        }
                        catch
                        {
                        }
                    }
                    if (unsafeField != null)
                    {
                        try
                        {
                            unsafeField.setAccessible(true);
                            return unsafeField.get(null);
                        }
                        catch
                        {
                        }
                    }
                    return null;
                }

            }
        }



        public static long ElementOffset32(int index)
        {
            var arrayBaseAndShift = _ArrayBaseAndShift32;
            if (arrayBaseAndShift == null)
            {
                arrayBaseAndShift = ArrayBaseAndShift((java.lang.Class) typeof(int[]));
                _ArrayBaseAndShift32 = arrayBaseAndShift;
            }

            return ((long) index << arrayBaseAndShift[1]) + arrayBaseAndShift[0];
        }



        public static long ElementOffset64(int index)
        {
            var arrayBaseAndShift = _ArrayBaseAndShift64;
            if (arrayBaseAndShift == null)
            {
                arrayBaseAndShift = ArrayBaseAndShift((java.lang.Class) typeof(long[]));
                _ArrayBaseAndShift64 = arrayBaseAndShift;
            }

            return ((long) index << arrayBaseAndShift[1]) + arrayBaseAndShift[0];
        }



        public static long ElementOffsetObj(int index)
        {
            var arrayBaseAndShift = _ArrayBaseAndShiftObj;
            if (arrayBaseAndShift == null)
            {
                arrayBaseAndShift = ArrayBaseAndShift((java.lang.Class) typeof(object[]));
                _ArrayBaseAndShiftObj = arrayBaseAndShift;
            }

            return ((long) index << arrayBaseAndShift[1]) + arrayBaseAndShift[0];
        }



        static int[] ArrayBaseAndShift(java.lang.Class arrayClass)
        {
            var @base = Util.JavaUnsafe.arrayBaseOffset(arrayClass);
            var scale = Util.JavaUnsafe.arrayIndexScale(arrayClass);
            if ((scale & (scale - 1)) != 0)
                throw new System.NotSupportedException();
            var baseAndShift = new int[2];
            baseAndShift[0] = @base;
            baseAndShift[1] = 31 - java.lang.Integer.numberOfLeadingZeros(scale);
            return baseAndShift;
        }



        [java.attr.RetainType] private static sun.misc.Unsafe _JavaUnsafe;
        [java.attr.RetainType] private static int[] _ArrayBaseAndShift32;
        [java.attr.RetainType] private static int[] _ArrayBaseAndShift64;
        [java.attr.RetainType] private static int[] _ArrayBaseAndShiftObj;
    }
}
