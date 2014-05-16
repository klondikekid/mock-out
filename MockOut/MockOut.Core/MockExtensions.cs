using System;

namespace MockOut.Core
{
    public static class MockExtensions
    {
        public static long Negative(this long l)
        {
            return Math.Abs(l)*- 1;
        }
        public static int Negative(this int i)
        {
            return Math.Abs(i) * -1;
        }
        public static decimal Negative(this decimal d)
        {
            return Math.Abs(d) * -1;
        }
        public static double Negative(this double d)
        {
            return Math.Abs(d) * -1;
        }
        public static float Negative(this float f)
        {
            return Math.Abs(f) * -1;
        }
        //public static void Mock(this object type, params Action<IMockOptions>[] options)
        //{
        //    Core.Mock.Create(type.GetType(), (x) => { type = x; }, options);
        //}
    }
}
