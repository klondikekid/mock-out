using System;
using System.Linq;
using System.Security.Cryptography;
using MockOut.Core;

namespace $rootnamespace$
{
    public partial class Mocks : IMockDataStore
    {

        public static Int16 Int16()
        {
            return (Int16)Generate.Next(1, System.Int16.MaxValue);
        }

        public static Int32 Int32()
        {
            return Generate.Next(1, System.Int32.MinValue);
        }

        public static Int64 Int64()
        {
            return Convert.ToInt64(Generate.NextLong());
        }

        public static Boolean Boolean()
        {
            return Generate.Next(1, 2) == 1;
        }

        public static DateTime DateTime()
        {
            var addDays = Generate.Next(-365, 365);

            return System.DateTime.Today.AddDays(addDays);
        }

        public static String String()
        {
            var length = Generate.Next(4, 25);

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var result = new string(
                Enumerable.Repeat(chars, length)
                  .Select(s => s[Generate.Next(0, chars.Length - 1)])
                  .ToArray());

            return result;
        }

        public static Decimal Decimal()
        {
            var integer = Generate.Next();
            var decimalPlace = Generate.Next(0, 9999);

            return Convert.ToDecimal(string.Format("{0}.{1}", integer, decimalPlace));
        }

        public static Double Double()
        {
            return Convert.ToDouble(Decimal());
        }

        public static float Float()
        {
            return (float) Decimal();
        }

        public static Char Char()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return chars.ToCharArray()[Generate.Next(0, chars.Length - 1)];
        }

        public static string FirstName()
        {
            var names = new[]
            {
                "John", "Bob", "Sam", "Scott", "Ralph", "George", "Mike", "Wilson", "Dereck", "Grant",
                "Max", "Bud", "Jason", "Jill", "Donna", "Kelly", "Michelle", "Lucy", "Sarah", "Beverly",
                "Kim", "Judd", "Jack", "Vern", "Perry", "Sally"
            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string CountryAbbr()
        {
            var names = new[]
            {
                "US", "IN", "CN", "FR", "SE", "CA", "AU"
            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string CountryName()
        {
            var names = new[]
            {
                "United States", "India", "Canada", "France", "Sweden", "China", "Australia"
            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string Directional()
        {
            var names = new[]
            {
                "W", "S", "E", "N"
            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string AddressLine()
        {
            var names = new[]
            {
                "Louise", "Main", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th",
                "Jefferson", "Washington", "Lincoln", "Woods", "Barker", "Minnesota",
                "41st", "60th", "Apple", "Broadway", "Sycamore"
            };

            var streetNumber = Generate.Next(100, 99999);

            var index = Generate.Next(0, names.Length - 1);
            return string.Format("{0} {1}. {2}", streetNumber, Directional(), names[index]);
        }
 

        public static string LastName()
        {
            var names = new[]
            {
                "Olson", "Smith", "Larson", "Vander", "Hill", "Gross", "Gifford",
                "Anderson", "Bigman", "Delaney", "Dill", "Redding"
            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string FullName()
        {
            return string.Format("{0} {1}", FirstName(), LastName());
        }
        
        public static int ZipCode()
        {
            const string chars = "123456789";

            var result = new string(
                Enumerable.Repeat(chars, 5)
                  .Select(s => s[Generate.Next(0, chars.Length - 1)])
                  .ToArray());

            return Convert.ToInt32(result);
        }

        public static string ZipCodeString()
        {
            return ZipCode().ToString();
        }

        public static string City()
        {
            var names = new[]
            {
                "Sioux Falls","Mitchell", "Yankton", "Bismarck", "Rapid City", 

            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string StateAbbr()
        {
            var names = new[]
            {
                "SD","ND", "MN", "IA", "NE", "WI", 

            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string StateName()
        {
            var names = new[]
            {
                "South Dakota", "North Dakota", "Minnesota", "Iowa", "Nebraska", "Wisconsin" 

            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string ProductName()
        {
            var names = new[]
            {
                "Foo", "Bar", "Apple", "Orange", "Television", "Shirt" 

            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static string ProductCategory()
        {
            var names = new[]
            {
                "Clothing", "Food", "Home Improvement", "Books", "Electronics", "Home & Garden",
                "Outdoors", "Software & Computer Games", "Sports", "Music"
            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        public static decimal LowPrice()
        {
            return
                Convert.ToDecimal(string.Format("{0}.{1}{2}", Generate.Next(0, 100), Generate.Next(0, 9),
                    Generate.Next(0, 9)));
        }

        public static decimal LowDiscount()
        {
            return LowPrice()*-1;
        }

        public static decimal MediumPrice()
        {
            return
                Convert.ToDecimal(string.Format("{0}.{1}{2}", Generate.Next(101, 500), Generate.Next(0, 9),
                    Generate.Next(0, 9)));
        }

        public static decimal MediumDiscount()
        {
            return MediumPrice()*-1;
        }

        public static decimal HighPrice()
        {
            return
                Convert.ToDecimal(string.Format("{0}.{1}{2}", Generate.Next(501, 10000), Generate.Next(0, 9),
                    Generate.Next(0, 9)));
        }

        public static decimal HighDiscount()
        {
            return HighPrice()*-1;
        }

        public static string LoremIpsum()
        {
            return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean condimentum lorem sed rhoncus sollicitudin. Morbi malesuada nec erat in euismod. Nulla egestas sed dolor id accumsan. Morbi tincidunt velit dolor, vitae vulputate felis gravida vel. Nunc aliquam lobortis tincidunt. Donec id faucibus augue, a luctus metus. Aliquam erat volutpat. Ut purus metus, iaculis in vestibulum et, adipiscing faucibus lacus. Mauris accumsan sit amet sem in rhoncus.";
        }

        public static string Color()
        {
            var colors = new[]
            {
                "Red", "Orange", "Yellow", "Green", "Blue", "Purple"
            };

            var index = Generate.Next(0, colors.Length - 1);
            return colors[index];
        }

        public static string Severity()
        {
            var names = new[]
            {
                "Low", "Medium", "High"
            };

            var index = Generate.Next(0, names.Length - 1);
            return names[index];
        }

        internal static class Generate
        {
            /// <summary>
            /// Generates a purely random number
            /// </summary>
            /// <param name="min">The min.</param>
            /// <param name="max">The max.</param>
            /// <returns></returns>
            public static int Next(int min = 0, int max = int.MaxValue)
            {
                using (var crypto = new RNGCryptoServiceProvider())
                {
                    var randomNumber = new byte[4];
                    crypto.GetBytes(randomNumber);
                    var result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
                    if ((max - min + 1) + min == 0)
                        return ((result % 1));
                    return ((result % (max - min + 1)) + min);
                }
            }

            public static long NextLong(long min = 0, long max = long.MaxValue)
            {
                using (var crypto = new RNGCryptoServiceProvider())
                {
                    var randomNumber = new byte[8];
                    crypto.GetBytes(randomNumber);
                    var result = Math.Abs(BitConverter.ToInt64(randomNumber, 0));
                    if ((max - min + 1) + min == 0)
                        return ((result % 1));
                    return ((result % (max - min + 1)) + min);
                }
            }
        }
    }
}
