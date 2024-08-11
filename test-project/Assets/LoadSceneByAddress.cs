using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

internal class LoadSceneByAddress : MonoBehaviour
{
  int curKey = 0;
  public string[] keys; // address string
  private AsyncOperationHandle<SceneInstance> loadHandle;

  void Start()
  {
    DontDestroyOnLoad(gameObject);
    StartCoroutine(Load());
    //loadHandle = Addressables.LoadSceneAsync(keys[0], LoadSceneMode.Additive);
  }

  IEnumerator Load()
  {
    if (curKey < keys.Length - 1)
      curKey++;
    else
      curKey = 0;
    loadHandle = Addressables.LoadSceneAsync(keys[curKey], LoadSceneMode.Single);
    yield return loadHandle;
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.Q))
    {
      StartCoroutine(Load());
    }
  }

  void OnDestroy()
  {
    Addressables.UnloadSceneAsync(loadHandle);
  }
}
