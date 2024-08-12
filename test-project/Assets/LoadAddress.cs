using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

internal class LoadAddress : MonoBehaviour
{
  public string[] keys;
  AsyncOperationHandle<GameObject> opHandle;
  public int index = 0;

  public AnimatorController animatorController;

  public GameObject go;

  private void Start()
  {
    if(!animatorController)
      animatorController = FindAnyObjectByType(typeof(AnimatorController)) as AnimatorController;
    StartCoroutine(LoadCharacter(index));
  }

  public IEnumerator LoadCharacter(int index)
  {
    if (go != null)
      Destroy(go);

    if (opHandle.IsValid())
    {
      
      Addressables.Release(opHandle);
    }

    opHandle = Addressables.LoadAssetAsync<GameObject>(keys[index]);
    yield return opHandle;

    if (opHandle.Status == AsyncOperationStatus.Succeeded)
    {
      GameObject obj = opHandle.Result;
      go = Instantiate(obj, transform);
      
      if(animatorController)
      {
        if(animatorController.animators.Length != 2)
          Array.Resize(ref animatorController.animators, 2);

        animatorController.animators[1] = go.GetComponent<Animator>();
      }

      
    }
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Q))
    {
      if (index < keys.Length - 1)
        index++;
      else
        index = 0;
      StartCoroutine(LoadCharacter(index));
    }
  }

  void OnDestroy()
  {
    if (opHandle.IsValid())
      Addressables.Release(opHandle);
  }
}
