using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DegreeTree : MonoBehaviour
{
    [Header("Major and courses")]
    public Major major;
    public List<Course> current;
    public List<Course> prereqs;

    [Header("Content")]
    public Transform content;
    public List<GameObject> columns;
    public List<CourseNode> nodes;

    [Header("Line")]
    public Transform lineContainer;
    public float thickness = 3f;
    public Vector2Int lineResolution = new Vector2Int(32, 32);
    public Color lineColour = Color.black;

    [Header("Prefabs")]
    public GameObject treeColumn;
    public CourseNode courseNode;
    public UILineRenderer linePrefab;

    bool initialized;

    private void Awake()
    {
        SetupTree();
    }

    private void OnEnable()
    {
        if (!initialized)
            SetupTree();
    }

    public void SetupTree()
    {
        for(int i = content.childCount - 1; i >= 0; i--)
        {
            if (content.GetChild(i).transform == lineContainer)
                continue;

            Destroy(content.GetChild(i).gameObject);
        }

        columns = new List<GameObject>();
        AddColumn(4);

        List<Course> current = new List<Course>();
        List<Course> pending = new List<Course>();
        foreach(Course c in major.firstYearCore)
        {
            if(c.prereqs.Count == 0)
            {
                Debug.Log("Add " + c.name);
                current.Add(c);
                AddNode(c, 0);
            }
            else
            {
                bool skip = false;
                foreach(Prereq prereq in c.prereqs)
                {
                    for(int i = 0; i < prereq.equivalent.Count; i++)
                    {
                        Debug.Log(prereq.equivalent[i].name + " in tree?");

                        if (current.Contains(prereq.equivalent[i]))
                            break;

                        if (i == prereq.equivalent.Count - 1)
                            skip = true;
                    }
                }

                if (skip)
                {
                    Debug.Log("Skip " + c.name);
                    pending.Add(c);
                    continue;
                }
                else
                {
                    Debug.Log("Add " + c.name);
                    AddNode(c, 1);
                }
            }
        }

        int prevCount;
        do
        {
            prevCount = pending.Count;
            List<Course> removeMe = new List<Course>();
            foreach (Course c in pending)
            {
                ResolvePrereqs();
                current.Add(c);
                removeMe.Add(c);
            }

            foreach(Course c in removeMe)
            {
                pending.Remove(c);
            }

            removeMe.Clear();
        }
        while (pending.Count != prevCount && pending.Count > 0);

        foreach (Course c in pending)
            Debug.Log(c.name + " pending");

        initialized = true;
    }

    public void ResolvePrereqs()
    {
        foreach (Course p in prereqs)
        {
            if (!current.Contains(p))
            {
                if(p.prereqs.Count > 0)
                    ResolvePrereqs(p);
                else
                {
                    AddNode(p, 0);
                }

                current.Add(p);
            }
        }
    }

    public void ResolvePrereqs(Course c)
    {
        foreach (Prereq prereq in c.prereqs)
        {
            for (int i = 0; i < prereq.equivalent.Count; i++)
            {
                if (!current.Contains(prereq.equivalent[i]))
                {
                    if (prereq.equivalent[i].prereqs.Count == 0)
                    {
                        AddNode(prereq.equivalent[i], 0);
                    }
                    else
                        ResolvePrereqs(prereq.equivalent[i]);

                    current.Add(prereq.equivalent[i]);
                }
            }
        }
    }

    void AddColumn()
    {
        GameObject tc = Instantiate(treeColumn);
        tc.transform.SetParent(content, false);
        //tc.transform.localScale = Vector3.one;
        columns.Add(tc);
        tc.transform.position = Vector3.zero;
    }

    void AddColumn(int count)
    {
        for(int i = 0; i < count; i++)
        {
            AddColumn();
        }
    }

    void AddNode(Course course, int columnID)
    {
        CourseNode cn = Instantiate(courseNode);
        cn.SetCourse(course);
        cn.transform.SetParent(columns[columnID].transform, false);
        cn.column = columns[columnID].GetComponent<RectTransform>();
        cn.columnID = columnID;
        nodes.Add(cn);

        if(cn.course.prereqs.Count > 0)
        {
            foreach(Prereq prereq in cn.course.prereqs)
            {
                foreach(Course c in prereq.equivalent)
                {
                    foreach (CourseNode node in nodes)
                    {
                        if (node.course == c)
                        {
                            DrawLine(node, cn);
                        }
                    }
                }
            }
        }
    }

    void DrawLine(CourseNode start, CourseNode end)
    {
        GameObject go = Instantiate(linePrefab.gameObject);

        go.transform.SetParent(lineContainer.GetComponent<RectTransform>(), false);

        UILineRenderer line = go.GetComponent<UILineRenderer>();
        line.enabled = true;
        //Vector2[] endpoints = new Vector2[] { (Vector2)start.rect.position, (Vector2)end.rect.position };
        //line.Points = endpoints;

        UILineConnector connector = line.GetComponent<UILineConnector>();
        connector.transforms = new RectTransform[] { start.rect, end.rect };
        connector.enabled = true;

        //line.transform.position = Vector3.zero;
    }
}
