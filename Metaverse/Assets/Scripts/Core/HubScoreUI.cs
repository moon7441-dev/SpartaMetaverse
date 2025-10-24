using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubScoreUI : MonoBehaviour
{
    public TMP_Text stackHS;
    public TMP_Text lastScore;

    private void Start()
    {
        stackHS.text = $"Stack HS: {ScoreManager.GetHighScore( "Stack" )}";
        lastScore.text = $"Last : {ScoreManager.LastScore}";
    }
}
