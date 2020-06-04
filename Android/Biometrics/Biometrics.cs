using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Widget;
using AndroidX.Biometric;
using AndroidX.Core.Content;



  public class Biometrics:IBiometrics
  {
    private readonly Context _context = Android.App.Application.Context;

    public static MainActivity MainActivity { get; set; }



    public void Authenticate(Func<Task> action,string title)
    {
      try
      {
        switch(BiometricManager.From(_context).CanAuthenticate())
        {
          case BiometricManager.BiometricSuccess:
            var biometricPrompt = new BiometricPrompt(MainActivity, ContextCompat.GetMainExecutor(_context),GetBiometricAuthenticationCallback(action));
            var promptInfo = new BiometricPrompt.PromptInfo.Builder()
                  .SetTitle(title == null ? "Biometric login for Falcon" : $"{title}...")
                  .SetNegativeButtonText("Cancel")
                  .Build();
            biometricPrompt.Authenticate(promptInfo);
            return;

          case BiometricManager.BiometricErrorHwUnavailable:
            Tools.DisplayAlert(message: "Biometric hardware is currently unavailable. Try again later.");
            return;

          case BiometricManager.BiometricErrorNoneEnrolled:
            Tools.DisplayAlert(message: "The device does not have any biometrics enrolled. Please make sure you have set up any available biometrics in your phone Settings.");
            return;

          default:
            return;
        }
      }
      catch(Exception ex)
      {
        //DisplayAlertError("Something went wrong while using biometric authentication.");
      }
    }



    private BiometricPrompt.AuthenticationCallback GetBiometricAuthenticationCallback(Func<Task> action)
    {
      var callback = new BiometricAuthenticationCallback
      {
        Success = (result) =>
        {
          Toast.MakeText(_context,"Authentication succeeded.",ToastLength.Short).Show();
          action();
        },
        Failed = () =>
        {
        },
        Error = (errorCode,errString) =>
        {
        },
      };
      return callback;
    }
}
