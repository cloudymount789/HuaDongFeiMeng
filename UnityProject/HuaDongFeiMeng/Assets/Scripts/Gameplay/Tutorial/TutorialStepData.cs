using UnityEngine;

[CreateAssetMenu(menuName = "HuadongFeimeng/Tutorial/Step", fileName = "TutorialStep_")]
public class TutorialStepData : ScriptableObject
{
    public string stepId;
    [TextArea] public string instructionText;

    [Tooltip("Optional: name of tool/part to highlight")]
    public string highlightKey;
}

