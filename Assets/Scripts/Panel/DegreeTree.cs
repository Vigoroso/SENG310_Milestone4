using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image linePrefab;

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
                current.Add(c);
                AddNode(c, 0);
            }
            else
            {
                bool skip = false;
                foreach(Course p in c.prereqs)
                {
                    if (!current.Contains(p))
                        skip = true;
                }

                if (skip)
                {
                    pending.Add(c);
                    continue;
                }
                else
                {
                    AddNode(c, 1);
                }
            }
        }

        int prevCount;
        do
        {
            prevCount = pending.Count;
            foreach (Course c in pending)
            {
                ResolvePrereqs();
                current.Add(c);
                pending.Remove(c);
            }
        }
        while (pending.Count != prevCount && pending.Count > 0);

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
        foreach (Course p in c.prereqs)
        {
            if (!current.Contains(p))
            {
                if (p.prereqs.Count == 0)
                {
                    AddNode(p, 0);
                }
                else
                    ResolvePrereqs(p);

                current.Add(p);
            }
        }
    }

    void AddColumn()
    {
        GameObject tc = Instantiate(treeColumn);
        tc.transform.SetParent(content);
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
        cn.transform.SetParent(columns[columnID].transform);
        cn.column = columns[columnID].GetComponent<RectTransform>();
        cn.columnID = columnID;
        nodes.Add(cn);

        if(cn.course.prereqs.Count > 0)
        {
            foreach(Course c in cn.course.prereqs)
            {
                foreach(CourseNode node in nodes)
                {
                    if(node.course == c)
                    {
                        DrawLine(node, cn);
                    }
                }
            }
        }
    }

    void DrawLine(CourseNode start, CourseNode end)
    {
        Texture2D tex = new Texture2D(lineResolution.x, lineResolution.y, TextureFormat.RGBA32, false);

        float dx = end.GetComponent<RectTransform>().position.x - start.GetComponent<RectTransform>().position.x;
        float dy = end.GetComponent<RectTransform>().position.y - start.GetComponent<RectTransform>().position.y;

        dy /= lineResolution.y;

        int curY = start.GetComponent<RectTransform>().position.y <= end.GetComponent<RectTransform>().position.y ?
                0 : lineResolution.y;

        for(int x = 0; x < lineResolution.x; x++)
        {
            curY = curY + Mathf.FloorToInt(x * dy);
            tex.SetPixel(x, curY, lineColour);

            for(int y = 0; y < lineResolution.y; y++)
            {
                if (curY == y)
                    continue;

                tex.SetPixel(x, y, Color.clear);
            }
        }

        tex.Apply();

        Image line = Instantiate(linePrefab);
        line.sprite  = Sprite.Create(tex, line.rectTransform.rect, new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(tex.width, tex.height));
        line.color = lineColour;
        line.transform.SetParent(lineContainer);

        RectTransform rect = line.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        rect.position = 0.5f * (end.rect.position + start.rect.position) + new Vector3(750, 750, 0);

        //need to determine height per item
        rect.sizeDelta = new Vector2(450f * (end.columnID - start.columnID), 32f);

        /*rect.sizeDelta = (RectTransformUtility.WorldToScreenPoint(null, end.rect.position)
                        - RectTransformUtility.WorldToScreenPoint(null, start.rect.position));*/

        /*if (start.rect.position.y <= end.rect.position.y)
        {
            rect.offsetMin = start.rect.position;
            rect.offsetMax = end.rect.position;
        }
        else
        {
            rect.offsetMin = new Vector2(start.rect.position.x, end.rect.position.y);
            rect.offsetMax = new Vector2(end.rect.position.x, start.rect.position.y);
        }*/
    }
}
