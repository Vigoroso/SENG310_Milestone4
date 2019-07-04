using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidePanel : Foldout
{
    public MainPanel mainPanel;
    public Foldout foldoutButton;

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        rect.anchorMin.Set(anchorFullMin.x, anchorFullMin.y);
        rect.anchorMax.Set(anchorFullMax.x, anchorFullMax.y);
    }

    public override void Fold()
    {
        base.Fold();

        if (isFolded)
        {
            foldoutButton.GetComponentInChildren<Text>().text = ">\n>\n>";
        }
        else
        {
            foldoutButton.GetComponentInChildren<Text>().text = "<\n<\n<";
        }
    }

    public void ToggleContent(GameObject go)
    {
        if(go.activeSelf)
        {
            go.SetActive(false);
            GetComponent<VerticalLayoutGroup>().spacing -= go.GetComponent<RectTransform>().rect.height;
        }
        else
        {
            go.SetActive(true);
            GetComponent<VerticalLayoutGroup>().spacing += go.GetComponent<RectTransform>().rect.height;
        }

    }

    public void ShowHide(Text text)
    {
        if (text.text == "Show")
            text.text = "Hide";
        else
            text.text = "Show";
    }
}
