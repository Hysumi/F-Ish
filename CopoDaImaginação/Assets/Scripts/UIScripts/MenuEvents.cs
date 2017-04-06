using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    public delegate void ExitCall();
    public static event ExitCall ExitButtonEvent;

    public delegate void RankingCall();
    public static event RankingCall RankingButtonEvent;

    public delegate void StartCall();
    public static event StartCall StartButtonEvent;

    internal void Exit()
    {
        ExitButtonEvent();
    }
    internal void Ranking()
    {
        RankingButtonEvent();
    }
    internal void Start()
    {
        StartButtonEvent();
    }

}
