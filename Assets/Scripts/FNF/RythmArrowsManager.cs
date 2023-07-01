using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmArrowsManager : MonoBehaviour
{
    // da 0 a 0,2 = perfetto 5
    // da 0,2 a 0,6 = buono 3 
    // da 0,5 a 1f = male 0
    // mancato = mancato -2

    [SerializeField] TMPro.TMP_Text pointText, accuracyText;
    [SerializeField] int playerPoints, enemyPoints;

    void Start()
    {
        ArrowsCheckHit.OnMissedNote.AddListener(MissNote);
        ArrowsCheckHit.OnHittedNote.AddListener(HittedNote);
    }

    void Update()
    {
        
    }

    void MissNote()
    {
        accuracyText.SetText("Che medda");
        playerPoints -= 2;

        pointText.SetText(playerPoints.ToString());
    }

    void HittedNote(float distanceFromNote)
    {
        RythmResult result = AccuracyTest(distanceFromNote);

        string resultText = "";

        switch (result)
        {
            case RythmResult.Perfect:
                resultText = "Arancino";
                playerPoints += 5;
                break;
            case RythmResult.Good:
                resultText = "Arancina";
                playerPoints += 3;
                break;
            case RythmResult.Bad:
                resultText = "Cannolo";
                playerPoints += 0;
                break;
        }

        accuracyText.SetText(resultText);
        pointText.SetText(playerPoints.ToString());
    }

    RythmResult AccuracyTest(float myAccuracy)
    {
        if (myAccuracy >= 0f && myAccuracy <= 0.2f) return RythmResult.Perfect;
        else if (myAccuracy > 0.2f && myAccuracy <= 0.6f) return RythmResult.Good;

        return RythmResult.Bad;
    }
}
