using System;
using System.Drawing;

namespace DeploymentToolkit.Util
{
    public static class ColorUtil
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static Color GetColor(string color)
        {
            var result = default(Color);
            try
            {
                if(color.StartsWith("#"))
                {
                    result = ColorTranslator.FromHtml(color);
                }
                else
                {
                    result = Color.FromName(color);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to parse color ({color})");
            }
            return result;
        }
    }
}
