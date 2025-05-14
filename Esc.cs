using UnityEngine;

public class ESC : MonoBehaviour
{
    public GameObject settingsUI;  // ����â UI
    public GameObject player;      // �÷��̾� (Movement ������Ʈ�� ���� ���� �ʿ�)

    private bool isSettingsOpen = false;

    void Update()
    {
        // ����â�� ���� ���� ���� ESC�� �� �� ����
        if (!isSettingsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSettingsUI();
        }
    }

    public void OpenSettingsUI()
    {
        settingsUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;

        // �÷��̾� ���� ��Ȱ��ȭ
        if (player != null)
        {
            Movement movement = player.GetComponent<Movement>();
            if (movement != null) movement.enabled = false;

            FirstPersonController fpc = player.GetComponent<FirstPersonController>();
            if (fpc != null) fpc.enabled = false;
        }

        isSettingsOpen = true;
    }

    /// <summary>
    /// ����â �ݱ� ��ư���� ȣ��.
    /// </summary>
    /// <param name="returnToGameplay">�������� ���ư� ������ ����</param>
    public void CloseSettingsUI(bool returnToGameplay)
    {
        settingsUI.SetActive(false);
        Time.timeScale = 1;

        // �÷��̾� ���� �ٽ� Ȱ��ȭ
        if (player != null)
        {
            Movement movement = player.GetComponent<Movement>();
            if (movement != null) movement.enabled = true;

            FirstPersonController fpc = player.GetComponent<FirstPersonController>();
            if (fpc != null) fpc.enabled = true;
        }

        if (returnToGameplay)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        isSettingsOpen = false;
    }
}