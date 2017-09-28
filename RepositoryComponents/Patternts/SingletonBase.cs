using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Patternts
{
    public abstract class SingletonBase<T> where T : class
    {
        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static T Instance
        {
            get { return SingletonFactory.Instance; }
        }

        /// <summary>
        /// The singleton class factory to create the singleton instance.
        /// </summary>
        class SingletonFactory
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SingletonFactory() { }

            // Prevent the compiler from generating a default constructor.
            SingletonFactory() { }

            internal static readonly T Instance = GetInstance();

            static T GetInstance()
            {
                var theType = typeof(T);

                T inst;

                try
                {
                    inst = (T)theType
                        .InvokeMember(theType.Name,
                        BindingFlags.CreateInstance | BindingFlags.Instance
                        | BindingFlags.NonPublic,
                        null, null, null,
                        CultureInfo.InvariantCulture);
                }
                catch (MissingMethodException ex)
                {
                    throw new TypeLoadException(string.Format(
                        CultureInfo.CurrentCulture,
                        "The type '{0}' must have a private constructor to " +
                        "be used in the Singleton pattern.", theType.FullName)
                        , ex);
                }

                return inst;
            }
        }
    }
}
