using System.Threading.Tasks;
using Foundation;
using Sufni.Bridge.Services;
using UIKit;

namespace Sufni.Bridge.iOS;

public class ShareService : IShareService
{
    public Task ShareFileAsync(string filePath)
    {
        var url = NSUrl.FromFilename(filePath);
        var activityController = new UIActivityViewController(
            new NSObject[] { url }, null);

        var presenter = GetTopmostViewController();
        if (presenter == null) return Task.CompletedTask;

        // On iPad, UIActivityViewController needs a source rect
        if (activityController.PopoverPresentationController != null)
        {
            activityController.PopoverPresentationController.SourceView = presenter.View;
            var b = presenter.View.Bounds;
            activityController.PopoverPresentationController.SourceRect =
                new CoreGraphics.CGRect(b.X + b.Width / 2, b.Y + b.Height / 2, 0, 0);
        }

        return presenter.PresentViewControllerAsync(activityController, true);
    }

    private static UIViewController? GetTopmostViewController()
    {
        UIViewController? root = null;

        // iOS 15+: use window scenes
        foreach (var scene in UIApplication.SharedApplication.ConnectedScenes)
        {
            if (scene is UIWindowScene windowScene)
            {
                foreach (var window in windowScene.Windows)
                {
                    if (window.IsKeyWindow)
                    {
                        root = window.RootViewController;
                        break;
                    }
                }
            }
            if (root != null) break;
        }

        // Traverse to the topmost presented view controller
        var top = root;
        while (top?.PresentedViewController != null)
            top = top.PresentedViewController;
        return top;
    }
}
