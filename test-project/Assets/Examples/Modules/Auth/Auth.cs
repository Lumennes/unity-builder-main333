using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GamePush;
using System.Collections.Generic;
using Examples.Console;

namespace Examples.Auth
{
  public class Auth : MonoBehaviour
  {

    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _secretCodeInput;
    [Space]
    [SerializeField] private Button _buttonHasIntegratedAuth;
    [SerializeField] private Button _buttonIsLoggedIn;
    [SerializeField] private Button _buttonLogin;
    [SerializeField] private Button _buttonLogout;
    [SerializeField] private Button _buttonLoad;
    [SerializeField] private Button _buttonFetch;
    [SerializeField] private Button _buttonSync;

    public void HasIntegratedAuth()
    {
      ConsoleUI.Instance.Log("HasIntegratedAuth: " +
        GP_Platform.HasIntegratedAuth().ToString());
    }

    public void IsLoggedIn()
    {
      ConsoleUI.Instance.Log("IsLoggedIn: " +
        GP_Player.IsLoggedIn().ToString());
    }

    public void Login()
    {
      GP_Player.Login();
    }

    public void Logout()
    {
      GP_Player.Logout();
    }

    public void Load()
    {
      GP_Player.Load();

      GetPlayerData();
    }

    public void Fetch()
    {
      GP_Player.FetchFields(/*OnPlayerFetchFieldsComplete*/);
    }

    public void Sync()
    {
      if(_nameInput.text != GP_Player.GetName())
      {        
        GP_Player.SetName(_nameInput.text);
        ConsoleUI.Instance.Log("Player change name: " + _nameInput.text);
      }

      GP_Player.Sync();
    }


    public void Start()
    {
      //if(GP_Platform.HasIntegratedAuth())// поддерживает ли
      //{

      //}
      

    }

    // Подписка на события
    private void OnEnable()
    {


      GP_Player.OnLoginComplete += OnLoginComplete;
      GP_Player.OnLoginError += OnLoginError;
      GP_Player.OnLogoutComplete += OnLogoutComplete;
      GP_Player.OnLogoutError += OnLogoutError;
      GP_Player.OnConnect += OnConnect;
      GP_Player.OnPlayerChange += OnPlayerChange;
      GP_Player.OnLoadComplete += OnLoadComplete;
      GP_Player.OnLoadError += OnLoadError;
      GP_Player.OnSyncError += OnSyncError;
      GP_Player.OnSyncComplete += OnSyncComplete;
      GP_Player.OnPlayerFetchFieldsComplete += OnPlayerFetchFieldsComplete;
      GP_Player.OnPlayerFetchFieldsError += OnPlayerFetchFieldsError;

      _buttonHasIntegratedAuth.onClick.AddListener(HasIntegratedAuth);
      _buttonIsLoggedIn.onClick.AddListener(IsLoggedIn);
      _buttonLogin.onClick.AddListener(Login);
      _buttonLogout.onClick.AddListener(Logout);
      _buttonLoad.onClick.AddListener(Load);
      _buttonFetch.onClick.AddListener(Fetch);
      _buttonSync.onClick.AddListener(Sync);

      GetPlayerData();
    }

    void GetPlayerData()
    {
      if (GP_Player.IsLoggedIn())
      {
        _nameInput.text = GP_Player.GetName();
        _secretCodeInput.text = GP_Player.GetString("secretCode");
      }
      else
      {
        _nameInput.text = string.Empty;
        _secretCodeInput.text = string.Empty;
      }
    }

    private void OnPlayerFetchFieldsError()
    {
      ConsoleUI.Instance.Log("Player Fetch Fields Error");
    }

    private void OnPlayerFetchFieldsComplete(List<PlayerFetchFieldsData> datas)
    {
      ConsoleUI.Instance.Log("\nOnPlayerFetchFieldsComplete:");
      //throw new System.NotImplementedException();
      foreach (var data in datas)
      {
        ConsoleUI.Instance.Log("NAME: " + data.name);
        ConsoleUI.Instance.Log("KEY: " + data.key);
        ConsoleUI.Instance.Log("TYPE: " + data.type);
        ConsoleUI.Instance.Log("DEFAULT VALUE: " + data.defaultValue);
        ConsoleUI.Instance.Log("IMPORTANT: " + data.important);
        ConsoleUI.Instance.Log("@PUBLIC: " + data.@public);
        ConsoleUI.Instance.Log("INTERVAL INCREMENT: " + data.intervalIncrement);
        ConsoleUI.Instance.Log("LIMITS: " + data.limits);        
        for (int i = 0; i < data.variants.Length; i++)
          ConsoleUI.Instance.Log("VARIANT ["+ i + "]: " + data.variants[i]);
        //public string name;
        //public string key;
        //public string type;
        //public string defaultValue; // string | bool | number
        //public bool important;
        //public bool @public;
        //public PlayerFieldIncrement intervalIncrement;
        //public PlayerFieldLimits limits;
        //public PlayerFieldVariant[] variants;
      }
    }

    private void OnSyncComplete()
    {
      ConsoleUI.Instance.Log("Sync Complete");
    }

    private void OnSyncError()
    {
      ConsoleUI.Instance.Log("Sync Error");
    }

    private void OnLoadError()
    {
      ConsoleUI.Instance.Log("Load Error");
    }

    private void OnLoadComplete()
    {
      ConsoleUI.Instance.Log("Load Complete");
    }

  

    // Отписка от событий
    private void OnDisable()
    {
      GP_Player.OnLoginComplete -= OnLoginComplete;
      GP_Player.OnLoginError -= OnLoginError;
      GP_Player.OnLogoutComplete -= OnLogoutComplete;
      GP_Player.OnLogoutError -= OnLogoutError;
      GP_Player.OnConnect -= OnConnect;
      GP_Player.OnPlayerChange -= OnPlayerChange;
      GP_Player.OnLoadComplete -= OnLoadComplete;
      GP_Player.OnLoadError -= OnLoadError;
      GP_Player.OnSyncError -= OnSyncError;
      GP_Player.OnSyncComplete -= OnSyncComplete;
      GP_Player.OnPlayerFetchFieldsComplete -= OnPlayerFetchFieldsComplete;
      GP_Player.OnPlayerFetchFieldsError -= OnPlayerFetchFieldsError;

      _buttonHasIntegratedAuth.onClick.RemoveListener(HasIntegratedAuth);
      _buttonIsLoggedIn.onClick.RemoveListener(IsLoggedIn);
      _buttonLogin.onClick.RemoveListener(Login);
      _buttonLogout.onClick.RemoveListener(Logout);
      _buttonLoad.onClick.RemoveListener(Load);
      _buttonFetch.onClick.RemoveListener(Fetch);
      _buttonSync.onClick.RemoveListener(Sync);
    }

    private void OnConnect() => ConsoleUI.Instance.Log("Player Connect");

    private void OnLoad() => ConsoleUI.Instance.Log("Player Load");

    private void OnLoginComplete() => ConsoleUI.Instance.Log("Login Complete");
    private void OnLoginError() => ConsoleUI.Instance.Log("Login Error");

    private void OnLogoutComplete() => ConsoleUI.Instance.Log("Logout Complete");
    private void OnLogoutError() => ConsoleUI.Instance.Log("Logout Error");

    private void OnPlayerChange() => ConsoleUI.Instance.Log("Player Change");
  }
}
