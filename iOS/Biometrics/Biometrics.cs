using System;
using System.Threading.Tasks;
using LocalAuthentication;
using UIKit;

  public class Biometrics:IBiometrics
  {
    private static int _osMajorVersion = int.Parse(UIDevice.CurrentDevice.SystemVersion.Split('.')[0]);

    public bool OnBiometrics { get; set; }


    private AuthenticationTypes _Current { get; set; } = AuthenticationTypes.None;


    public void Authenticate(Func<Task> action,string title)
    {
      try
      {
        OnBiometrics = true;
        var context = new LAContext();
        if(_Current == AuthenticationTypes.FaceID || _Current == AuthenticationTypes.TouchID)
        {
          if(context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics,out var authError))
          {
            context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics,$"{title ?? "Logging in"} with {_Current}",GetReplyHandler(action));
          }
          else
          {
            //DisplayAlert($"{_Current} is temporarily unavailable. Please check if you have set up {_Current} in your phone Settings.");
            OnBiometrics = false;
            return;
          }
        }
        else
        {
          //DisplayAlert($"Please check if you have set up any available biometrics in your phone Settings.");
          OnBiometrics = false;
          return;
        }
      }
      catch(Exception ex)
      {
        //DisplayAlertError("Something went wrong while using biometric authentication.");
      }
    }

    
    
    public AuthenticationTypes GetAuthenticationType()
    {
      var m = _osMajorVersion;
      var context = new LAContext();
      if(context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics,out var authError))
      {
        if(_osMajorVersion >= 11 && context.BiometryType == LABiometryType.FaceId)
        {
          return _Current = AuthenticationTypes.FaceID;
        }
        return _Current = AuthenticationTypes.TouchID;
      }
      return _Current = AuthenticationTypes.None;
    }
    
    

    private LAContextReplyHandler GetReplyHandler(Func<Task> action)
    {
      var replyHandler = new LAContextReplyHandler((success,error) =>
      {
        if(success)
        {
          action();
          OnBiometrics = false;
        }
        else
        {
          OnBiometrics = false;
        }
      });
      return replyHandler;
    }

}
