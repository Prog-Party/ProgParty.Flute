using System.Windows.Threading;

namespace Fluit.Classes
{
    public class Threads
    {
        private static Dispatcher _foregroundDispatcher;
        public static Dispatcher ForegroundDispatcher
        {
            get { return _foregroundDispatcher; }
            set
            {
                if (_foregroundDispatcher == null)
                {
                    _foregroundDispatcher = value;
                }
            }
        }
    }
}
