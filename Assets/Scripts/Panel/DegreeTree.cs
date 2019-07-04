using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegreeTree : MonoBehaviour
{
    public Major major;

    public List<Course> current;
    public List<Course> prereqs;

    public void SetupTree()
    {
        List<Course> current = new List<Course>();
        List<Course> pending = new List<Course>();
        foreach(Course c in major.firstYearCore)
        {
            if(c.prereqs.Count == 0)
            {
                current.Add(c);
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

    }

    public void ResolvePrereqs()
    {
        foreach (Course p in prereqs)
        {
            if (!current.Contains(p))
            {
                ResolvePrereqs(p);
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
                current.Add(p);
            }
        }
    }

}
