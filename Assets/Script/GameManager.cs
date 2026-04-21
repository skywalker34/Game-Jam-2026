using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public int mistakeCount = 0;
    public int maxMistake = 3;

    public DateTime today;
    public TMP_Text dateText;
    public TMP_Text mistakeText;

    public GameObject documentPrefab;
    public Document currentDoc;
    public Transform DocSpawn;

    public GameObject GameOverUI;

    bool isSpawning = false;

    void Start()
    {
        today = DateTime.Today;
        dateText.text = DateTime.Today.ToString("yy-MMM-dd", CultureInfo.InvariantCulture);
        SpawnNewDocument();
    }

    public void OnApproveButton()
    {
        OnPlayerDecision(true);
    }

    public void OnRejectButton()
    {
        OnPlayerDecision(false);
    }

    void SpawnNewDocument()
    {
        currentDoc = Instantiate(documentPrefab, DocSpawn.position, Quaternion.identity).GetComponent<Document>();
        isSpawning = false;
    }

    public void OnPlayerDecision(bool approve)
    {
        if (isSpawning) return;

        bool isValid = currentDoc.expireDate >= today;

        if (approve != isValid)
        {
            mistakeCount++;
            mistakeText.text = mistakeCount.ToString();
            Debug.Log("Mistake Count¡G" + mistakeCount);

            if (mistakeCount >= maxMistake)
            {
                GameOver();
            }
        }

        FindFirstObjectByType<QueueManager>().ProcessNext();

        StartCoroutine(NextDocument(approve));
    }

    IEnumerator NextDocument(bool approve)
    {
        isSpawning = true;

        Vector3 dir = approve ? Vector3.up : Vector3.down;
        float flyDistance = 10f;
        float flyDuration = 0.2f;
        yield return StartCoroutine(
            currentDoc.Disappear(dir, flyDistance, flyDuration)
        );

        FindFirstObjectByType<QueueManager>().ProcessNext();

        SpawnNewDocument();
    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
}