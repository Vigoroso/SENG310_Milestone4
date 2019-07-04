using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class HoverDetails : CourseDetails, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public List<CourseOffering> offerings = new List<CourseOffering>();

    [Header("Content")]
    public Semester semester = Semester.Fall;
    public Text semesterText;
    public Text sectionText;
    public Text daysText;
    public Text timeText;
    public CourseOffering nilOffering;
    
    [Header("Content Fit")]
    public ContentSizeFitter contentSizeFitter;
    public VerticalLayoutGroup verticalLayoutGroup;

    [Header("State")]
    public Course currentCourse;
    public bool hasFocus;
    public HoverPreset preset;

    int sectionIndex;

    public override void Awake()
    {
        base.Awake();
        semesterText.text = semester.ToString();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hasFocus = true;

        //sectionIndex = 0;

        PopulateOfferings();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasFocus = false;
        //offerings.Clear();
        gameObject.SetActive(false);
    }

    public void PopulateOfferings()
    {
        offerings.Clear();
        foreach (CourseOffering co in CourseSearch.instance.offerings)
        {
            if (currentCourse == co.course && semester == co.semester)
                offerings.Add(co);
        }
    }
    
    public void SetDetails()
    {
        if(offerings.Count == 0)
        {
            sectionText.text = "";
            daysText.text = "";
            timeText.text = "";
            return;
        }

        if (sectionIndex < 0)
            sectionIndex = offerings.Count - 1;
        else if(sectionIndex >= offerings.Count)
            sectionIndex = 0;

        SetDetails(offerings[sectionIndex]);
    }

    public void SetDetails(CourseOffering course)
    {
        base.SetDetails(course.course);
        sectionText.text = course.Section;

        if(course.days.Count > 0)
        {
            daysText.text = course.days[0].ToString();

            for (int i = 1; i < course.days.Count; i++)
            {
                daysText.text += ", ";
                daysText.text += course.days[i].ToString();
            }
        }

        timeText.text = course.startTime.x + ":" + course.startTime.y
                      + " - " + course.endTime.x + ":" + course.endTime.y;
    }

    public void SetDetails(CourseResult course)
    {
        if (hasFocus && currentCourse == course)
            return;

        currentCourse = course.course.course;

        preset = course.hoverPreset;

        if(preset != null)
        {
            image.color = preset.colour;
            image.sprite = preset.sprite;
            image.type = preset.imageType;

            contentSizeFitter.horizontalFit = preset.horizontalFit;
            contentSizeFitter.verticalFit = preset.verticalFit;
            verticalLayoutGroup.spacing = preset.spacing;

            rect.anchorMin = preset.anchorMin;
            rect.anchorMax = preset.anchorMax;

            transform.SetParent(course.column.transform);
            //transform.position = preset.offset;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
        }

        SetDetails(course.course);
    }

    public void ClearDetails()
    {
        currentCourse = null;
        gameObject.SetActive(false);
    }

    public void SetPreset(HoverPreset preset)
    {
        this.preset = preset;
    }

    public void ShiftSectionIndex(int shift)
    {
        sectionIndex += shift;

        SetDetails();
    }

    public void ShiftSemester(int shift)
    {
        int index = (int)semester + shift;
        if (index < 0)
            index = (int)Semester.Fall;
        else if (index > (int)Semester.Fall)
            index = 0;

        semester = (Semester)index;

        semesterText.text = semester.ToString();

        PopulateOfferings();
        SetDetails();
    }
}
