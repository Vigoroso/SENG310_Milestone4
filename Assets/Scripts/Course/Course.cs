using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_Course", menuName = "Course")]
public class Course : ScriptableObject
{
    public int number;
    public Level level;
    public Subject subject;

    [TextArea]
    public string description;

    public List<Prereq> prereqs;

    public CourseMetric mean;
    public CourseMetric hoursPerWeek;
    public CourseMetric difficulty;

    public List<string> tags;
}

[System.Serializable]
public class Prereq
{
    public List<Course> equivalent;
}

[System.Serializable]
public enum Subject
{
    Any,
    CSC,
    MATH,
    SENG,
    STAT
}

public enum Level
{
    Any,
    L000,
    L100,
    L200,
    L300,
    L400,
    L500,
    L600
}

[System.Serializable]
public class CourseMetric
{
    public string attribute;
    public float normalizedScore { get { return score / (scoreMax - scoreMin); } }
    public float score;
    public float scoreMin;
    public float scoreMax;
}
