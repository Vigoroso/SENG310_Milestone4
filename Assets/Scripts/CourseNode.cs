﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CourseNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Course course;
    public Text text;

    private void Awake()
    {
        text.text = course.department.ToString() + " " + course.number;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter " + name);
        MainPanel.instance.hoverDetails.SetDetails(course, GetComponent<RectTransform>());
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
}
