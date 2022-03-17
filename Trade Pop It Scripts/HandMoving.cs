using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMoving : MonoBehaviour
{
    public static HandMoving S;

    [SerializeField] private Animation _playerAnimation;
    [SerializeField] private Animation _botAnimation;
    [SerializeField] private List<AnimationClip> _listOfPlayerAnimation;
    [SerializeField] private List<AnimationClip> _listOfBotAnimation;


    private void Awake()
    {
        S = this;
    }

    public void MoveHandPlayer(int idAnimation) //0 - put, 1 - done, 2 - add, 3 - cancel
    {
        _playerAnimation.clip = _listOfPlayerAnimation[idAnimation];
        _playerAnimation.Play();

        if (idAnimation == 1)
            CoreGame.S.playerOk = true;
        else
            CoreGame.S.playerOk = false;

        if (CoreGame.S.playerOk && CoreGame.S.aiOk)
            CoreUI.S.FinishLevel();
    }

    public void MoveHandBot(int idAnimation) //0 - put, 1 - done, 2 - add, 3 - cancel
    {
        _botAnimation.clip = _listOfBotAnimation[idAnimation];
        _botAnimation.Play();

        if (idAnimation == 1)
            CoreGame.S.aiOk = true;
        else
            CoreGame.S.aiOk = false;

        if (CoreGame.S.playerOk && CoreGame.S.aiOk)
            CoreUI.S.FinishLevel();
    }
}
