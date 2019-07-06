using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class CourseSearch : MonoBehaviour, IPointerEnterHandler
{
    public static CourseSearch instance;

    public CourseDB courseDB;

    public HoverPreset hoverPreset;

    [Header("CurrentResults")]
    public List<CourseOffering> offerings;
    public List<Course> courses;

    [Header("Sidebar")]
    public InputField keywordInput;
    public GameObject keywordContentParent;
    public List<Subject> subjects;
    public List<Level> levels;
    public Semester semester;
    public ToggleGroup semesterGroup;

    [Header("MainPanel")]
    public GameObject columnSpace;
    public GameObject prompt;
    public List<RectTransform> columns = new List<RectTransform>();
    public float[] hoverAnchorOffsets = new float[10] { 0, 0, .56f, .5f, .4f, .35f, .25f, .15f, .15f, .15f };

    [Header("Prefabs")]
    public ResultColumn column;
    public CourseResult courseResult;
    public Keyword keyword;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        offerings = new List<CourseOffering>();
    }

    public void OnEnable()
    {
        MainPanel.instance.hoverDetails.SetPreset(hoverPreset);
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
        columns.Clear();

        //check if criteria is dirty?

        //clear columns
        for (int i = 0; i < columnSpace.transform.childCount; i++)
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
                        Filter(courseDB.cscOffering);
                        break;
                    case Subject.ENGL:
                        Filter(courseDB.englOffering);
                        break;
                    case Subject.ENGR:
                        Filter(courseDB.engrOffering);
                        break;
                    case Subject.GREE:
                        Filter(courseDB.greeOffering);
                        break;
                    case Subject.MATH:
                        Filter(courseDB.mathOffering);
                        break;
                    case Subject.MEDI:
                        Filter(courseDB.mediOffering);
                        break;
                    case Subject.PHIL:
                        Filter(courseDB.philOffering);
                        break;
                    case Subject.PSYCH:
                        Filter(courseDB.psychOffering);
                        break;
                    case Subject.SENG:
                        Filter(courseDB.sengOffering);
                        break;
                    case Subject.STAT:
                        Filter(courseDB.statOffering);
                        break;
                }
            }
        }
        else
        {
            //filter through all subjects
            Filter(courseDB.cscOffering);
            Filter(courseDB.englOffering);
            Filter(courseDB.engrOffering);
            Filter(courseDB.greeOffering);
            Filter(courseDB.mathOffering);
            Filter(courseDB.mediOffering);
            Filter(courseDB.philOffering);
            Filter(courseDB.psychOffering);
            Filter(courseDB.sengOffering);
            Filter(courseDB.statOffering);
<<<<<<< HEAD
            Filter(courseDB.psychOffering);
            Filter(courseDB.philOffering);
            Filter(courseDB.englOffering);
            Filter(courseDB.engrOffering);
            Filter(courseDB.mediOffering);
            Filter(courseDB.greeOffering);
=======
>>>>>>> 5d3053b211b79e76d7ea59e336f572c78757104a
        }

        int spacing = -2500 + 300 * columns.Count;
        if (spacing > 0)
            spacing = 0;

        columnSpace.GetComponent<HorizontalLayoutGroup>().spacing = spacing;
    }

    public void Filter(List<CourseOffering> subjectCourses)
    {
        offerings.Clear();

        if (subjectCourses.Count <= 0)
        {
            Debug.Log("No courses for subject :(");
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
            Debug.Log("no toggles on!");
            //show message to choose semester
            return;
        }

        //List<Course> courses = new List<Course>();
        //List<CourseOffering> offerings = new List<CourseOffering>();

        List<Keyword> keywords = new List<Keyword>();

        for(int i = 0; i < keywordContentParent.transform.childCount; i++)
        {
            keywords.Add(keywordContentParent.transform.GetChild(i).GetComponent<Keyword>());
        }

        Debug.Log(keywords.Count);

        foreach (CourseOffering c in subjectCourses)
        {
            if (c.semester != semester)
                continue;

            if (levels.Count > 0 && !levels.Contains(c.course.level))
                continue;

            bool skip = true;
            foreach(Keyword k in keywords)
            {
                Debug.Log("Looking for  '" + k.keyword.text + "' in: " + c.course.description + " and " + c.course.name);

                if (Regex.IsMatch(c.course.description, k.keyword.text, RegexOptions.IgnoreCase)
                    || Regex.IsMatch(c.course.name, k.keyword.text, RegexOptions.IgnoreCase) )
                {
                    Debug.Log("Matched " + k.keyword.text);
                    skip = false;
                    break;
                }
            }
            if (skip && keywords.Count > 0)
                continue;

            if(!courses.Contains(c.course))
                courses.Add(c.course);

            offerings.Add(c);
        }

        if (offerings.Count > 0)
        {
            ResultColumn rColumn = Instantiate(column);
            columns.Add(rColumn.GetComponent<RectTransform>());
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

    public void AddKeyword()
    {
        foreach (Keyword k in keywordContentParent.GetComponentsInChildren<Keyword>())
        {
            if (k.keyword.text == keywordInput.text)
            {
                keywordInput.text = "";
                return;
            }
        }

        Keyword keyword = Instantiate(this.keyword);
        keyword.keyword.text = keywordInput.text;
        keyword.transform.SetParent(keywordContentParent.transform, false);

        keywordInput.text = "";
    }

}
