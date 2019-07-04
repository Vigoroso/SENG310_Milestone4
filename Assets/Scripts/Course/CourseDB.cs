using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Course lists organized by subject
/// </summary>
[CreateAssetMenu(fileName = "CourseDB", menuName = "Course Database")]
public class CourseDB : ScriptableObject
{
    public List<CourseOffering> csc;
    public List<CourseOffering> seng;
    public List<CourseOffering> math;
    public List<CourseOffering> stat;
}
