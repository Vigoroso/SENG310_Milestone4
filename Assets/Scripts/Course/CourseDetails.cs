using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseDetails : MonoBehaviour
{
    public RectTransform rect;
    public Text courseText;
    public Text descriptionText;

    public float offsetScale = 200f;

    public virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void SetDetails(Course course)
    {
        courseText.text = course.subject + " " + course.number;
        descriptionText.text = course.description;
    }
}
