using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CourseSearch : MonoBehaviour, IPointerEnterHandler
{

    public static CourseSearch instance;

    public CourseDB courseDB;

    [Header("CurrentResults")]
    public List<CourseOffering> offerings;
    public List<Course> courses;

    [Header("Sidebar")]
    //do union / intersect for tags?
    public List<string> tags;
    public List<Subject> subjects;
    public List<Level> levels;
    public Semester semester;
    public ToggleGroup semesterGroup;

    [Header("MainPanel")]
    public GameObject columnSpace;
    public GameObject prompt;

    [Header("Prefabs")]
    public ResultColumn column;
    public CourseResult courseResult;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MainPanel.instance.hoverDetails.ClearDetails();
    }

    public void FindCourses()
    {
        prompt.SetActive(false);
        MainPanel.instance.hoverDetails.transform.SetParent(MainPanel.instance.transform);

        offerings.Clear();
        courses.Clear();

        //check if criteria is dirty?

        //clear columns
        for(int i = 0; i < columnSpace.transform.childCount; i++)
        {
            Destroy(columnSpace.transform.GetChild(i).gameObject);
        }

        if(subjects.Count > 0)
        {
            //pre-filter by department for department specific columns
            foreach (Subject d in subjects)
            {
                switch (d)
                {
                    case Subject.CSC:
                        Filter(courseDB.csc);
                        break;

                    case Subject.MATH:
                        Filter(courseDB.math);
                        break;

                    case Subject.SENG:
                        Filter(courseDB.seng);
                        break;

                    case Subject.STAT:
                        Filter(courseDB.stat);
                        break;
                }
            }
        }
        else
        {
            //filter through all subjects
            Filter(courseDB.csc);
            Filter(courseDB.math);
            Filter(courseDB.seng);
            Filter(courseDB.stat);
        }
    }

    public void Filter(List<CourseOffering> subjectCourses)
    {
        if (subjectCourses.Count <= 0)
        {
            //write a message?
            return;
        }

        if(semesterGroup.AnyTogglesOn())
        {
            foreach (Toggle t in semesterGroup.ActiveToggles())
            {
                if (t.name == "Fall")
                {
                    semester = Semester.Fall;
                }
                else if (t.name == "Spring")
                {
                    semester = Semester.Spring;
                }
                else if (t.name == "Summer")
                {
                    semester = Semester.Summer;
                }
                else
                {
                    Debug.LogWarning("There are no active semester toggles?!");
                }
            }
        }
        else
        {
            //show message to choose semester
            return;
        }

        //List<Course> courses = new List<Course>();
        //List<CourseOffering> offerings = new List<CourseOffering>();
        foreach (CourseOffering c in subjectCourses)
        {
            if (c.semester != semester)
                continue;

            if (levels.Count > 0 && !levels.Contains(c.course.level))
                continue;

            bool skip = true;
            foreach (string t in tags)
            {
                if (c.course.tags.Contains(t))
                {
                    skip = false;
                    break;
                }
            }
            if (skip && tags.Count > 0)
                continue;

            if(!courses.Contains(c.course))
                courses.Add(c.course);

            offerings.Add(c);
        }

        if (offerings.Count > 0)
        {
            ResultColumn rColumn = Instantiate(column);
            rColumn.transform.SetParent(columnSpace.transform, false);

            List<Course> temp = new List<Course>();
            foreach (CourseOffering c in offerings)
            {
                if (temp.Contains(c.course))
                    continue;

                CourseResult cr = Instantiate(courseResult);
                cr.transform.SetParent(rColumn.transform.GetChild(1).GetChild(0), false);

                cr.Initialize(c);

                cr.column = rColumn;
                temp.Add(c.course);
            }
        }
    }

    public void ToggleSubject(SubjectToggle subject)
    {
        if (subjects.Contains(subject.subject))
            subjects.Remove(subject.subject);
        else
            subjects.Add(subject.subject);
    }

    public void ToggleLevel(LevelToggle level)
    {
        if (levels.Contains(level.level))
            levels.Remove(level.level);
        else
            levels.Add(level.level);
    }
}
