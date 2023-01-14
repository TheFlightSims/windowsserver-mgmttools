using System;

namespace RemoteMsiManager
{
    public class Localization
    {
        private static Localization _instance = null;
        private readonly System.Resources.ResourceManager _resMan = null;

        private Localization()
        {
            _resMan = new System.Resources.ResourceManager("RemoteMsiManager.Localization.Resources", typeof(FrmRemoteMsiManager).Assembly);
        }

        internal static Localization GetInstance()
        {
            if (_instance == null)
                _instance = new Localization();
            return _instance;
        }

        /// <summary>
        /// Returns a localized string depending of the current culture.
        /// </summary>
        /// <param name="unlocalizedString">string to localized.</param>
        /// <returns>the localized string.</returns>
        internal string GetLocalizedString(string unlocalizedString)
        {
            try
            {
                string result = _resMan.GetString(unlocalizedString);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            catch (Exception) { }

            return "Missing_Localized_String_For(" + (unlocalizedString ?? "null") + ")";
        }

    }
}
