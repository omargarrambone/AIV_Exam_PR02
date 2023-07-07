using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class NPCInteractable : MonoBehaviour
{
    public GameObject chatBubble;
    public string[] sentence = { "", "", ""};
    public TMP_Text text;
    public int lastSentenceIndex;


    public void Interact()
    {
        if (chatBubble.gameObject.activeSelf == false)
        {
            chatBubble.SetActive(true);

            string sentenceToDisplay = RandomSentence();
            text.text = sentenceToDisplay;

            Vector3 relativePos = (transform.position - Camera.main.transform.position);
            Vector3 playerPos = (PlayerManager.PlayerGameObject.transform.position - transform.position).normalized;

            transform.rotation = Quaternion.LookRotation(new Vector3(playerPos.x, 0f, playerPos.z));
            chatBubble.transform.rotation = Quaternion.LookRotation(relativePos);

            StartCoroutine(RemoveAfterSeconds(2));
        }
    }

    IEnumerator RemoveAfterSeconds(int seconds)
    {
            yield return new WaitForSeconds(seconds);
            chatBubble.SetActive(false);
    }

    private string RandomSentence()
    {
        int newSentenceIndex;

        do
        {
            newSentenceIndex = Random.Range(0, sentence.Length);
        } while (newSentenceIndex == lastSentenceIndex);

        string randomWord = sentence[newSentenceIndex];

        lastSentenceIndex = newSentenceIndex;
        return randomWord;
    }
}
