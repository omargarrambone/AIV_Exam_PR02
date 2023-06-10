using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class NPCInteractable : MonoBehaviour
{
    public GameObject chatBubble;
    public Transform cameraTarget;
    public Transform playerTarget;

    public string[] sentence = { "", "", ""};
    public TMP_Text text;


    public void Interact()
    {
        Debug.Log("Interact!");
        if (chatBubble.gameObject.activeSelf == false)
        {
            chatBubble.SetActive(true);

            string sentenceToDisplay = RandomSentence();
            text.text = sentenceToDisplay;

            Vector3 relativePos = transform.position - cameraTarget.position;
            Vector3 playerPos = transform.position - playerTarget.position;

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
        string randomWord = sentence[Random.Range(0, sentence.Length)];
        return randomWord;
    }
}
