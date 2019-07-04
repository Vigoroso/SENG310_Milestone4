using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public static MainPanel instance;

    public HoverDetails hoverDetails;

    public RectTransform rect;

    public List<GameObject> content;

    public mono_gmail mailLog;

    public int contentID;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        mailLog.log = "";
        //mailLog.SendMail("Test", "Do we have success?");

        instance = this;
        rect = GetComponent<RectTransform>();
    }

    public void NavigateTo(GameObject panel)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetInstanceID() == panel.GetInstanceID())
                continue;

            transform.GetChild(i).gameObject.SetActive(false);
        }

        panel.SetActive(true);
    }
}
