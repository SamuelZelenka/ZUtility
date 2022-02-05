using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLog : MonoBehaviour
{
    public static CommandLog instance;
    public GameObject textPrefab = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Command.NewCommand("clear", new VoidCommand(() => Clear()), "Clear all console log messages.");
    }
    public static void PrintMessage(string message)
    {
        Text newText = Instantiate(instance.textPrefab, instance.transform).transform.GetChild(0).GetComponent<Text>();
        newText.text = message;
        Debug.Log(message);
    }
    private void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
