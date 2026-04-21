using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public int maxQueue = 0;

    public GameObject personPrefab;
    private List<GameObject> queue = new List<GameObject>();

    public float spawnInterval = 0.5f;
    public Transform[] slots;

    void Awake()
    {
        slots = GetComponentsInChildren<Transform>();
        List<Transform> temp = new List<Transform>(slots);
        temp.RemoveAt(0);
        slots = temp.ToArray();
        maxQueue = slots.Length;
    }

    void Start()
    {
        InvokeRepeating(nameof(AddPerson), 2f, spawnInterval);
    }

    void AddPerson()
    {
        if (queue.Count >= slots.Length) return;

        Transform slot = slots[queue.Count];

        GameObject person = Instantiate(personPrefab, slot.position, Quaternion.identity);
        queue.Add(person);

        if (queue.Count >= maxQueue)
        {
            FindFirstObjectByType<GameManager>().GameOver();
        }
    }

    public void ProcessNext()
    {
        if (queue.Count == 0) return;

        Destroy(queue[0]);
        queue.RemoveAt(0);

        for (int i = 0; i < queue.Count; i++)
        {
            queue[i].transform.position = slots[i].position;
        }
    }
}