  public interface IBiometrics
  {
    #region Public Methods

    void Authenticate(Func<Task> action1,string title = null);

    #endregion Public Methods
  }
