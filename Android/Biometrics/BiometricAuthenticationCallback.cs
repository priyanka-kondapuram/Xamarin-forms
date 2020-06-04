using System;
using AndroidX.Biometric;
using Java.Lang;


  public class BiometricAuthenticationCallback:BiometricPrompt.AuthenticationCallback
  {
    public Action Failed;
    public Action<BiometricPrompt.AuthenticationResult> Success;
    public Action<int,ICharSequence> Error;


    public override void OnAuthenticationFailed()
    {
      base.OnAuthenticationFailed();
      Failed();
    }


    public override void OnAuthenticationError(int errorCode,ICharSequence errString)
    {
      base.OnAuthenticationError(errorCode,errString);
      Error(errorCode,errString);
    }


    public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
    {
      base.OnAuthenticationSucceeded(result);
      Success(result);
    }

  }
