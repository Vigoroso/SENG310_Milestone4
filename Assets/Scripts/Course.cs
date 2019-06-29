using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_Course", menuName = "Course")]
[System.Serializable]
public class Course : ScriptableObject
{
    public int number;
    public Department department;
    //public List<string> tags;

    [TextArea]
    public string description;

    public CourseMetric mean;
    public CourseMetric hoursPerWeek;
    public CourseMetric difficulty;
}

[System.Serializable]
public enum Department
{
    CSC,
    MATH,
    SENG,
    STAT
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
