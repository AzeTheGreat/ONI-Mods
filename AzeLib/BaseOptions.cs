using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace AzeLib
{
    public abstract class BaseOptions<T> where T : class, new()
    {
        private static T _opts;
        public static T Opts
        {
            get
            {
                if (_opts == null)
                    _opts = POptions.ReadSettings<T>() ?? new T();

                return _opts;
            }
            set { _opts = value; }
        }

        static protected void Load()
        {
            PUtil.InitLibrary(true);
            POptions.RegisterOptions(typeof(T));
        }
    }
}
