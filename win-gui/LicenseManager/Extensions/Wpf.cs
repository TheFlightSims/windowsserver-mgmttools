using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HGM.Hotbird64.LicenseManager.Extensions
{
    public static class Wpf
    {
        /// <summary>
        /// Generates a BitmapSource from a control
        /// </summary>
        /// <param name="renderTarget">Control where you want to render (for dpi adjustment)</param>
        /// <param name="control">UIElement (usually Control or higher) you want to render</param>
        /// <param name="width">dpi independent width</param>
        /// <param name="height">dpi independent height</param>
        /// <returns></returns>
        public static BitmapSource GenerateImage(this Visual renderTarget, UIElement control, double width, double height)
        {
            var source = PresentationSource.FromVisual(renderTarget);
            if (source?.CompositionTarget == null) throw new InvalidOperationException("Unrendered Control specified");

            var scaleX = source.CompositionTarget.TransformToDevice.M11;
            var scaleY = source.CompositionTarget.TransformToDevice.M22;
            var rect = new Rect(0, 0, width * scaleX * scaleX, height * scaleY * scaleY);

            var rtb = new RenderTargetBitmap
            (
                (int)Math.Round(width * scaleX * scaleX * scaleX),
                (int)Math.Round(height * scaleY * scaleY * scaleY),
                96 * scaleX,
                96 * scaleY,
                PixelFormats.Pbgra32
            );

            control.Arrange(rect);
            rtb.Render(control);
            return rtb;
        }

        [SuppressMessage("ReSharper", "PartialTypeWithSinglePart")]
        public static void ExpandAll(this ItemsControl control, bool isExpanded)
        {
            foreach (var item in control.Items)
            {
                if (!(item is TreeViewItem))
                {
                    continue;
                }

                var treeViewItem = (TreeViewItem)item;
                treeViewItem.IsExpanded = isExpanded;
                ExpandAll(treeViewItem, isExpanded);
            }
        }
    }
}
