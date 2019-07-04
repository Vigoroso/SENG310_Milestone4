using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Major", menuName = "Major")]
public class Major : ScriptableObject
{
    public List<Elective> electives;

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

[System.Serializable]
public class Elective
{
    public Subject subject;
    public Level level;
    public int count;
}
