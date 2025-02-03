using Fluit.Resources;

namespace Fluit.Classes
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static Resources.AppResources localizedResources = new AppResources();

        public Resources.AppResources LocalizedResources { get { return localizedResources; } }
    }
}
