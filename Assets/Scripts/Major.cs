using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Major", menuName = "Major")]
public class Major : ScriptableObject
{
    [Header("First Year")]
    public List<Course> firstYearCore;
    public List<Course> firstYearRecommended;

    [Header("Second Year")]
    public List<Course> secondYearCore;
    public List<Course> secondYearRecommended;

    [Header("Third Year")]
    public List<Course> thirdYearCore;
    public List<Course> thirdYearRecommended;

    [Header("Fourth Year")]
    public List<Course> fourthYearCore;
    public List<Course> fourthYearRecommended;
}
