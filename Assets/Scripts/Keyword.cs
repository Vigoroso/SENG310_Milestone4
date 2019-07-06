using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[RequireComponent(typeof(Text))]
public class Keyword : MonoBehaviour
{
    public Text keyword;

    public void Remove()
    {
        Destroy(gameObject);
    }
}
