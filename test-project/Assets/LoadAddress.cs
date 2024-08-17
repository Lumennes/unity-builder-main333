using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

internal class LoadAddress : MonoBehaviour
{
  public string[] keys;
  AsyncOperationHandle<GameObject> opHandle;
  public int index = 0;

  public AnimatorController animatorController;

  public GameObject go;

  public GameObject loadCharPanel;
  public Image frontImage;
  public TMP_Text perText;

  private void Start()
  {
    loadCharPanel.SetActive(false);
    frontImage.fillAmount = 0;
    perText.text = "0%";

    if (!animatorController)
      animatorController = FindAnyObjectByType(typeof(AnimatorController)) as AnimatorController;

    //if (opHandle.IsValid())
    //{
    //  opHandle.Completed += OpHandle_Completed;
    //  opHandle.CompletedTypeless += OpHandle_CompletedTypeless;
    //  opHandle.Destroyed += OpHandle_Destroyed;
    //}

    // загружаем первого персонажа
    StartCoroutine(LoadCharacter(index));

    
  }

  private void OpHandle_Destroyed(AsyncOperationHandle obj)
  {
    Debug.Log("OpHandle_Destroyed: " + obj.Result.ToString());
    //throw new NotImplementedException();
  }

  private void OpHandle_CompletedTypeless(AsyncOperationHandle obj)
  {
    Debug.Log("OpHandle_CompletedTypeless: " + obj.Result.ToString());
    //throw new NotImplementedException();
  }

  private void OpHandle_Completed(AsyncOperationHandle<GameObject> obj)
  {
    Debug.Log("OpHandle_Completed: " + obj.Result.name.ToString());

    if (opHandle.Status == AsyncOperationStatus.Succeeded)
    {
      

      loadCharPanel.SetActive(false);
      frontImage.fillAmount = 0;
      perText.text = "0%";

      // создаем экз. объекта из ассета
      GameObject ob = opHandle.Result;
      go = Instantiate(ob, transform);
      // если есть ссылка на контролер аниматора
      if (animatorController)
      {
        // если массив аниматоров не равен двум переопределяем массив с 2-мя ссылками на них
        if (animatorController.animators.Length != 2)
          Array.Resize(ref animatorController.animators, 2);
        // вторая ссылка аниматора - аниматор этого игрового объекта
        animatorController.animators[1] = go.GetComponent<Animator>();
      }
    }
    else
    {
      opHandle.Release();
    }

    //throw new NotImplementedException();
  }

  public IEnumerator LoadCharacter(int index)
  {
    if (go != null) Destroy(go); // если гэймобжект уже есть удаляем его

    // если хандл валидный (существет) осовбождаем его
    ReleaseHandle();

    // загружаем объект ассета асинхронно 
    opHandle = Addressables.LoadAssetAsync<GameObject>(keys[index]);

    if (opHandle.IsValid())
    {
      opHandle.Completed += OpHandle_Completed;
      opHandle.CompletedTypeless += OpHandle_CompletedTypeless;
      opHandle.Destroyed += OpHandle_Destroyed;
    }

    loadCharPanel.SetActive(true);

    //Debug.Log(Addressables.GetDownloadSizeAsync(keys[index]).ToString());



    // ждём загрузки
    do
    {
      frontImage.fillAmount = opHandle.PercentComplete >= 0.99f ? 1 :
        opHandle.PercentComplete;
      perText.text = opHandle.PercentComplete >= 0.99f ? "100%" :
        (opHandle.PercentComplete * 100f).ToString("F0") + ("%");
      yield return null;
    }
    while (!opHandle.IsDone);

    //while (!opHandle.IsDone)
    //{
    //  frontImage.fillAmount = opHandle.PercentComplete;
    //  var per = (int)opHandle.PercentComplete * 100;
    //  perText.text = per + "%";
    //}

    // если загрузили
    
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Q))
    {
      Prev();
    }
    if (Input.GetKeyDown(KeyCode.E))
    {
      Next();
    }
  }


  public void Next()
  {
    if (index < keys.Length - 1)
      index++;
    else
      index = 0;
    StartCoroutine(LoadCharacter(index));
  }

  public void Prev()
  {
    if (index > 0)
      index--;
    else
      index = keys.Length - 1;
    StartCoroutine(LoadCharacter(index));
  }

  void OnDestroy()
  {
    ReleaseHandle();
  }

  void ReleaseHandle()
  {
    if (opHandle.IsValid())
    {
      opHandle.Completed -= OpHandle_Completed;
      opHandle.CompletedTypeless -= OpHandle_CompletedTypeless;
      opHandle.Destroyed -= OpHandle_Destroyed;
      Addressables.Release(opHandle);
    }
  }
}
