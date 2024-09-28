using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Poliment_UI.BundleConfigurationActivator), "Activate")]
namespace Poliment_UI
{
    public static class BundleConfigurationActivator
    {
        public static void Activate()
        {
            BundleTable.Bundles.RegisterConfigurationBundles();
        }
    }
}