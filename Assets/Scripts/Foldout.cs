using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Foldout : MonoBehaviour
{
    [Header("Foldout")]
    public bool disableChildren;
    public List<GameObject> disableExcludes;
    public List<Foldout> subFold;


    public bool preserveActiveState = true;
    public List<GameObject> inactiveChildren;

    [Tooltip("(1 / speed) seconds between steps.")]
    [Range(1,5000)]
    public float speed = 3f;

    [Tooltip("Number of keyframe steps from anchor A to anchor B")]
    [Range(1,60)]
    public int steps = 10;

    public RectTransform rect;
    public Vector2 anchorFullMax;
    public Vector2 anchorFullMin;
    public Vector2 anchorFoldedMax;
    public Vector2 anchorFoldedMin;
    public bool isFolded;
    bool transitioning;


    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void Fold()
    {
        if (transitioning)
            return;

        StartCoroutine(SmoothFold());
    }

    public IEnumerator SmoothFold()
    {
        if (transitioning)
            yield break;

        transitioning = true;
        
        if (isFolded)
        {
            isFolded = false;

            foreach(Foldout f in subFold)
                f.isFolded = !f.isFolded;

            for (float i = 1; i < steps; i++)
            {
                rect.anchorMin = anchorFoldedMin + (anchorFullMin - anchorFoldedMin) * i / steps;
                rect.anchorMax = anchorFoldedMax + (anchorFullMax - anchorFoldedMax) * i / steps;

                foreach(Foldout f in subFold)
                {
                    f.FoldStep(i, steps);
                }

                yield return null;
                //yield return new WaitForSeconds(1 / speed);
            }

            rect.anchorMin = anchorFullMin;
            rect.anchorMax = anchorFullMax;

            if (disableChildren)
                DisableChildren();

            foreach(Foldout f in subFold)
            {
                if(f.disableChildren)
                {
                    f.DisableChildren();
                }

                f.rect.anchorMin = f.isFolded ? f.anchorFoldedMin : f.anchorFullMin;
                f.rect.anchorMax = f.isFolded ? f.anchorFoldedMax : f.anchorFullMax;
            }
        }
        else
        {
            isFolded = true;

            if (disableChildren)
                DisableChildren();

            foreach (Foldout f in subFold)
            {
                f.isFolded = !f.isFolded;

                if (f.disableChildren)
                {
                    f.DisableChildren();
                }
            }

            for (float i = 1; i < steps; i++)
            {
                rect.anchorMin = anchorFullMin + (anchorFoldedMin - anchorFullMin) * i / steps;
                rect.anchorMax = anchorFullMax + (anchorFoldedMax - anchorFullMax) * i / steps;

                foreach (Foldout f in subFold)
                {
                    f.FoldStep(i, steps);
                }

                yield return null;
                //yield return new WaitForSeconds(1 / speed);
            }

            rect.anchorMin = anchorFoldedMin;
            rect.anchorMax = anchorFoldedMax;

            foreach (Foldout f in subFold)
            {
                f.rect.anchorMin = f.isFolded ? f.anchorFoldedMin : f.anchorFullMin;
                f.rect.anchorMax = f.isFolded ? f.anchorFoldedMax : f.anchorFullMax;
            }
        }

        transitioning = false;

        yield return null;
    }

    public void FoldStep(float i, float steps)
    {
        if (isFolded) //going from full -> folded
        {
            rect.anchorMin = anchorFullMin + (anchorFoldedMin - anchorFullMin) * i / steps;
            rect.anchorMax = anchorFullMax + (anchorFoldedMax - anchorFullMax) * i / steps;
        }
        else //going from folded -> full
        {
            rect.anchorMin = anchorFoldedMin + (anchorFullMin - anchorFoldedMin) * i / steps;
            rect.anchorMax = anchorFoldedMax + (anchorFullMax - anchorFoldedMax) * i / steps;
        }
    }

    public void DisableChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (disableExcludes.Contains(transform.GetChild(i).gameObject))
                continue;

            if (preserveActiveState && isFolded && transform.GetChild(i).gameObject.activeSelf == false)
            {
                inactiveChildren.Add(transform.GetChild(i).gameObject);
            }
            else if (preserveActiveState && !isFolded && inactiveChildren.Contains(transform.GetChild(i).gameObject))
            {
                inactiveChildren.Remove(transform.GetChild(i).gameObject);
                continue;
            }

            transform.GetChild(i).gameObject.SetActive(!isFolded);
        }
    }
}
