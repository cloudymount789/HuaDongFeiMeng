using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<TutorialStepData> steps = new List<TutorialStepData>();
    public int currentIndex;

    [Header("Debug")]
    public KeyCode nextKey = KeyCode.N;

    private void Start()
    {
        ShowCurrent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextKey)) Next();
    }

    public void Next()
    {
        currentIndex = Mathf.Clamp(currentIndex + 1, 0, Mathf.Max(0, steps.Count - 1));
        ShowCurrent();
    }

    private void ShowCurrent()
    {
        if (steps.Count == 0)
        {
            Debug.Log("Tutorial: no steps.");
            return;
        }

        var step = steps[currentIndex];
        if (step == null)
        {
            Debug.LogWarning("Tutorial: step is null.");
            return;
        }

        Debug.Log($"Tutorial[{currentIndex + 1}/{steps.Count}]: {step.instructionText}");
    }
}

