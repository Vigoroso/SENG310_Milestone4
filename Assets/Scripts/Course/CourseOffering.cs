using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_Course_Offering", menuName = "Course Offering")]
public class CourseOffering : ScriptableObject
{
    public Course course;
    public string Section;
    public Semester semester;
    public List<Days> days;
    public Building building;
    public Wing wing;
    public int room;

    public Vector2Int startTime;
    public Vector2Int endTime;

    public string prof;
}

[System.Serializable]
public enum Semester
{
    Spring,
    Summer,
    Fall
}

[System.Serializable]
public enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday
}

[System.Serializable]
public enum CourseComponent
{
    Lecture,
    Lab,
    Tutorial
}

[System.Serializable]
public enum Building
{
    DTB,
    ECS,
    ELW,
    HHB
        //etc
}

public enum Wing
{
    None,
    A,
    B,
    C,
    D
}
