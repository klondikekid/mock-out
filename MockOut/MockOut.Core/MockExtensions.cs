#region License
// Copyright (c) 2014 John Robinson
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

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
