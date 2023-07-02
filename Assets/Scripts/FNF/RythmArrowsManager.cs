using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmArrowsManager : MonoBehaviour
{
    // da 0 a 0,2 = perfetto 5
    // da 0,2 a 0,6 = buono 3 
    // da 0,5 a 1f = male 0
    // mancato = mancato -2

   
    public float PlayerPoints
    { 
        get { 
            return actualPlayerPoints; 
        }
        set { 
          
            actualPlayerPoints = ClampPointsValue(value);
            UpdateSliderPlayer();
        }
    }
    public float EnemyPoints
    {
        get {
            return actualEnemyPoints; 
        }
        set {
            actualEnemyPoints = ClampPointsValue(value);
            UpdateSliderEnemy();
        }
    }
    [Header("Points Setup")]
    [SerializeField] float maxPoints;
    [SerializeField] float perfectPoint, goodPoint, badPoint, missedPoint, enemyGainPoint, stolenPointsFromEnemyByPlayer, stolenPointsFromPlayerByEnemy;

    [Header("References")]
    [SerializeField] bool shouldRemoveText;
    [SerializeField] private float actualPlayerPoints, actualEnemyPoints;
    [SerializeField] float textTimer, textCounter;
    [SerializeField] UnityEngine.UI.Slider playerSlider, enemySlider;
    [SerializeField] TMPro.TMP_Text pointText, accuracyText;

    void Start()
    {
        ArrowsCheckHit.OnMissedNote.AddListener(MissNote);
        ArrowsCheckHit.OnPlayerHittedNote.AddListener(HittedNote);
        BossArrowsManager.OnBossHittedNote.AddListener(BossHittedNote);

    }

    void Update()
    {
        if (shouldRemoveText)
        {
            textCounter -= Time.deltaTime;

            if (textCounter < 0){
                shouldRemoveText = false;
                accuracyText.SetText("");
            }
        }
    }

    float ClampPointsValue(float value)
    {
        return Mathf.Clamp(value, 0, maxPoints);
    }

    void UpdateSliderPlayer()
    {
        playerSlider.value = actualPlayerPoints / maxPoints;
    }

    void UpdateSliderEnemy()
    {
        enemySlider.value = actualEnemyPoints / maxPoints;
    }

    void BossHittedNote()
    {
        EnemyPoints+= enemyGainPoint;
        UpdateSliderEnemy();
        PlayerPoints -= stolenPointsFromPlayerByEnemy;
    }

    void MissNote()
    {
        accuracyText.SetText("Che piritu!");
        PlayerPoints -= missedPoint;

        pointText.SetText(actualPlayerPoints.ToString());
    }

    public void HittedNote(float distanceFromNote)
    {
        RythmResult result = AccuracyTest(distanceFromNote);

        string resultText = "";

        switch (result)
        {
            case RythmResult.Perfect:
                resultText = "Scopa!";
                PlayerPoints += perfectPoint;
                break;
            case RythmResult.Good:
                resultText = "7 bello!";
                PlayerPoints += goodPoint;
                break;
            case RythmResult.Bad:
                resultText = "Asso di coppe";
                PlayerPoints += badPoint;
                break;
        }

        SetAccuracyText(resultText);
        pointText.SetText(actualPlayerPoints.ToString());
        EnemyPoints -= stolenPointsFromEnemyByPlayer;
    }

    void SetAccuracyText(string text)
    {
        textCounter = textTimer;
        accuracyText.SetText(text);
        shouldRemoveText = true;
    }


    RythmResult AccuracyTest(float myAccuracy)
    {
        if (myAccuracy >= 0f && myAccuracy <= 0.2f) return RythmResult.Perfect;
        else if (myAccuracy > 0.2f && myAccuracy <= 0.6f) return RythmResult.Good;

        return RythmResult.Bad;
    }
}
