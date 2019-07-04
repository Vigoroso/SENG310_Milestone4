using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class CourseNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Course course;
    public Text title;
    public HoverPreset hoverPreset;
    public RectTransform rect;
    public RectTransform column;
    public int columnID;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        title.text = course.subject.ToString() + " " + course.number;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter " + name);
        MainPanel.instance.hoverDetails.SetDetails(course);
        MainPanel.instance.hoverDetails.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit " + name);
        StartCoroutine(DelayedDisable());
    }

    IEnumerator DelayedDisable()
    {
        yield return new WaitForFixedUpdate();

        if (MainPanel.instance.hoverDetails.hasFocus)
            yield break;

        MainPanel.instance.hoverDetails.gameObject.SetActive(false);
    }

    public void SetCourse(Course course)
    {
        this.course = course;
        title.text = course.subject + " " + course.number;
    }
}
