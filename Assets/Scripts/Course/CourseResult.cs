using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class CourseResult : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CourseOffering course;
    public Text title;
    public ResultColumn column;
    //public HoverPreset hoverPreset;

    public void Initialize(CourseOffering course)
    {
        this.course = course;
        //set ui values
        title.text = course.course.subject.ToString() + " " + course.course.number;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter " + name);

        MainPanel.instance.hoverDetails.offerings.Clear();
        MainPanel.instance.hoverDetails.SetDetails(this); //course.course);
        MainPanel.instance.hoverDetails.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit " + name);

        if(!MainPanel.instance.hoverDetails.hasFocus)
        {
            StartCoroutine(DelayedDisable());
        }
    }

    IEnumerator DelayedDisable()
    {
        yield return null;

        if (MainPanel.instance.hoverDetails.hasFocus)
            yield break;

        MainPanel.instance.hoverDetails.ClearDetails();
        MainPanel.instance.hoverDetails.ClearSpacer();
    }
}
