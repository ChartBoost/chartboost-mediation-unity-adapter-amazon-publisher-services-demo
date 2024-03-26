using Chartboost;
using UnityEditor;
using UnityEngine;

namespace Sample
{
   [InitializeOnLoad]
   public class DemoSetupOnEditorLaunch
   {
      private const string CompanyName = "Chartboost";
      private const string ProductName = "Chartboost Mediation Unity SDK - Amazon Publisher Services Demo";
      private const string ApplicationBundleIdentifier = "com.chartboost.mediation.unity.demo.aps";

      static DemoSetupOnEditorLaunch() => SetupDemoApp();

      [MenuItem("Chartboost Mediation/APS/Setup Demo")]
      private static void SetupDemoApp()
      {
         Debug.Log($"Configuring {ProductName}.");
         
         PlayerSettings.companyName = CompanyName;
         PlayerSettings.productName = ProductName;
         PlayerSettings.bundleVersion = ChartboostMediation.Version;

         PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
         PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)33;
         PlayerSettings.iOS.targetOSVersionString = "12.5";

         PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, ApplicationBundleIdentifier);
         PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, ApplicationBundleIdentifier);
         PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, ApplicationBundleIdentifier);
      }
   }
}
