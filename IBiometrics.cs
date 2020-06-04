  public interface IBiometrics
  {
    void Authenticate(Func<Task> action1,string title = null);
  }
