using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseDetails : MonoBehaviour
{
    public RectTransform rect;
    public Semester semester = Semester.Fall;
    public Text semesterText;
    public Text courseText;
    public Text descriptionText;

    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        semesterText.text = semester.ToString();
    }

    public void SetDetails(Course course, RectTransform trigger)
    {
        rect.position = trigger.position;
        courseText.text = course.department + " " + course.number;
        descriptionText.text = course.description;
    }
}
