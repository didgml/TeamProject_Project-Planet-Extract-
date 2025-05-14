using SavedSettings;
using UnityEngine;
using UnityEngine.UI;

public class keysetting : MonoBehaviour
{
    [SerializeField] PlayerKeyBindings keyBindings;
    [SerializeField] Button saveButton;
    [SerializeField] Button resetButton;
    [SerializeField] Button loadButton;

    void Start()
    {
        saveButton.onClick.AddListener(() => keyBindings.Save());
        resetButton.onClick.AddListener(() => keyBindings.ResetKeyBindings());
        loadButton.onClick.AddListener(() => keyBindings.Load());
    }
}
