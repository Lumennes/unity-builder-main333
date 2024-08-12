using UnityEngine;
using System.Collections;

    public class Swapper : MonoBehaviour
    {
  public bool swapper = true;
  public bool active = true;
        public GameObject[] character;
        public int index;
        public Texture btn_tex;
        void Awake()
        {
    if (character.Length > 0)
    {

      foreach (GameObject c in character)
      {
        if (c != null)
        c.SetActive(false);
      }
      if (active)
        character[0].SetActive(true);
    }
        }
        void OnGUI()
        {
    if (character.Length > 0 && swapper)
    {
      if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 100), btn_tex))
      {
        character[index].SetActive(false);
        index++;
        index %= character.Length;
        character[index].SetActive(true);
      }
    }
        }
    }

