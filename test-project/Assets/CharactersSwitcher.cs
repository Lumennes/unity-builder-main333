using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSwitcher : MonoBehaviour
{
  public GameObject[] playerCharacters;
  public StatusC[] playerCharactersStatus;
  public int activeCharacterIndex;

  AttackTriggerC attackTriggerC;
  PlayerMecanimAnimationC playerMecanimAnimationC;

  // Start is called before the first frame update
  void Start()
  {
    attackTriggerC = GetComponent<AttackTriggerC>();
    playerMecanimAnimationC = GetComponent<PlayerMecanimAnimationC>();
    SwitchCharacter(activeCharacterIndex);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F1))
      SwitchCharacter(0);
    if (Input.GetKeyDown(KeyCode.F2))
      SwitchCharacter(1);
    if (Input.GetKeyDown(KeyCode.F3))
      SwitchCharacter(2);
    if (Input.GetKeyDown(KeyCode.F4))
      SwitchCharacter(3);
  }

  public void SwitchCharacter(int activeCharacterIndex)
  {
    if (playerCharacters[activeCharacterIndex] == null)
      return;

    this.activeCharacterIndex = activeCharacterIndex;

    for (int i = 0; i < playerCharacters.Length; i++)
    {
      if (playerCharacters[i] != null)
          playerCharacters[i].SetActive(i == activeCharacterIndex);
    }

    attackTriggerC.mainModel = playerCharacters[activeCharacterIndex];
    playerMecanimAnimationC.animator =
      playerCharacters[activeCharacterIndex].GetComponent<Animator>();
    
  }
}
