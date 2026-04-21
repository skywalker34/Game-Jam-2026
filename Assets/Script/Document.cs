using System;
using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Document : MonoBehaviour
{
    public DateTime expireDate;

    public TMP_Text dateText;
    void Start()
    {
        GenerateRandomDate();
        dateText.text = expireDate.ToString("yyyy-MM-dd");
    }

    public void GenerateRandomDate()
    {
        DateTime today = DateTime.Today;
        int years = UnityEngine.Random.Range(-5, 6);
        int randomDays = UnityEngine.Random.Range(-365 * years, 366 * years);

        expireDate = today.AddDays(randomDays);
    }

    public IEnumerator Disappear(Vector3 direction, float distance, float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * distance;

        float timer = 0f;

        SpriteRenderer image = GetComponent<SpriteRenderer>();
        Color startColor = image.color;
        Color dateColor = dateText.color;


        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, timer / duration);

            float alpha = Mathf.Lerp(1f, 0f, 1f - Mathf.Pow(1f - t, 3));
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            dateText.color = new Color(dateColor.r, dateColor.g, dateColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
