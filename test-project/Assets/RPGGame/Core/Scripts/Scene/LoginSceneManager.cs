using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePush;
using Examples.Console;
using Examples.Players;

public class LoginSceneManager : MonoBehaviour
{
    public static LoginSceneManager Singleton { get; private set; }
    public UIAuthentication loginDialog;
    public UIAuthentication registerDialog;
    public GameObject clickStartObject;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
        HideLoginDialog();
        ShowClickStart();
    }

  private void OnEnable()
  {
    gameInstance = GameInstance.Singleton;
    gameService = GameInstance.GameService;

    if (gameService.gamePush)
    {
      GP_Player.OnLoadComplete += GP_Player_OnLoadComplete;
      GP_Player.OnLoadError += GP_Player_OnLoadError;

      GP_Player.OnLoginComplete += GP_Player_OnLoginComplete;
      GP_Player.OnLoginError += GP_Player_OnLoginError;
    }
  }

  private void OnDisable()
  {
    if (gameService.gamePush)
    {
      GP_Player.OnLoadComplete -= GP_Player_OnLoadComplete;
      GP_Player.OnLoadError -= GP_Player_OnLoadError;

      GP_Player.OnLoginComplete -= GP_Player_OnLoginComplete;
      GP_Player.OnLoginError -= GP_Player_OnLoginError;
    }
  }

  private void Start()
  {
   
    //my
    //OnClickStart();
  }

  GameInstance gameInstance;
  BaseGameService gameService;

  public void OnClickStart()
    { 
        if (gameService.gamePush)
        {
          //Узнать, что на площадке доступна авторизация
          //(внутренняя, внешняя) можно через метод FREE:
          if (GP_Platform.HasIntegratedAuth())
          {
            //Узнать, что игрок авторизован через внутреннюю
            //или внешнюю систему авторизации можно через метод FREE:
            if (GP_Player.IsLoggedIn())
            {
              // Игрок использует один из способов входа (кука, авторизация, секретный код)
              if (GP_Player.HasAnyCredentials())
              {

                //GP_Player.Load();
              }
            }
            else
            {
              
              GP_Player.Login();
            }

          }
        }
        else
        {
          gameService.ValidateLoginToken(true, OnValidateLoginTokenSuccess, OnValidateLoginTokenError);
          HideClickStart();
        }
    }

  // Получить список полей
  private void PlayerFetchFields()
  {
    GP_Player.FetchFields(OnFetchFields);
  }

  #region playerData

  [System.Serializable]
  public class PlayersData
  {
    public PlayerState state;
    public PlayerAchievements[] achievements;
    public PlayerPurchases[] purchasesList;
  }

  [System.Serializable]
  public class PlayerState
  {
    // Добавлять глобальные значения которые создали в панели "Игроки" на сайте GamePush например public bool isOnline;
    public string avatar;
    public string credentials;
    public int id;
    public string name;
    public string platformType;
    public int projectId;
    public int score;
  }

  [System.Serializable]
  public class PlayerAchievements
  {
    public int id;
    public string tag;
    public string createdAt;
  }

  [System.Serializable]
  public class PlayerPurchases
  {
    public int productId;
    public string createdAt;
    public string expiredAt;
    public bool subscribed;
    public bool gift;
    public string tag;
  }

  [System.Serializable]
  public class LeaderBoardFetchData
  {
    public string avatar;
    public int id;
    public int score;
    public string name;
    public int position;
  }

  #endregion

  private List<int> _playersId = new List<int>();

  // Поля получены
  private void OnFetchFields(List<PlayerFetchFieldsData> data)
  {
    //foreach (PlayerFetchFieldsData d in data)
    //{
    //  if (d.type == )
    //    GP_Player.GetString(d.key);
    //}
  }

  private void FetchPlayers(List<int> playersId)
  {
    GP_Players.Fetch(playersId, OnPlayersFetchSuccess, OnPlayersFetchError);
  }

  private void OnPlayersFetchSuccess(GP_Data data)
  {
    var players = data.GetList<PlayersData>();
  }

  private void OnPlayersFetchError() => Debug.Log("PLAYER FETCH: ERROR");

  private void GP_Player_OnLoginError()
  {
    throw new System.NotImplementedException();
  }

  private void GP_Player_OnLoadError1()
  {
    throw new System.NotImplementedException();
  }

  private void GP_Player_OnLoginComplete()
  {
    throw new System.NotImplementedException();
  }

  private void GP_Player_OnLoadError()
  {
    throw new System.NotImplementedException();
  }

  private void GP_Player_OnLoadComplete()
  {
    throw new System.NotImplementedException();
  }

  private void OnValidateLoginTokenSuccess(PlayerResult result)
    {
       
        gameInstance.OnGameServiceLogin(result);
    }

    private void OnValidateLoginTokenError(string error)
    {
        
        ShowLoginDialog();
    }

    public void ShowLoginDialog()
    {
        if (loginDialog != null)
            loginDialog.Show();
    }

    public void HideLoginDialog()
    {
        if (loginDialog != null)
            loginDialog.Hide();
    }

    public void ShowRegisterDialog()
    {
        if (registerDialog != null)
            registerDialog.Show();
    }

    public void HideRegisterDialog()
    {
        if (registerDialog != null)
            registerDialog.Hide();
    }

    public void ShowClickStart()
    {
        if (clickStartObject != null)
            clickStartObject.SetActive(true);
    }

    public void HideClickStart()
    {
        if (clickStartObject != null)
            clickStartObject.SetActive(false);
    }
}
