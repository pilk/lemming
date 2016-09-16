using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonKey : MonoBehaviour
{
    public KeyCode m_keyCode = KeyCode.Backspace;

    private Button m_button = null;

    private void Awake()
    {
        m_button = this.GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_keyCode))
        {
            m_button.onClick.Invoke();
        }
    }
}
