using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text hookedStateText;
    [SerializeField] private TMP_Text winText;
    private static TMP_Text currentHookedText = null;
    private static TMP_Text currentWinText = null;

    private void Start()
    {
        currentHookedText = hookedStateText;
        currentWinText = winText;
    }

    public static void SetTextHookedText(string str)=> currentHookedText.text = str;

    public static void SetTextFinalText(string str) => currentWinText.text = str;
}