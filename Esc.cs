using UnityEngine;

public class ESC : MonoBehaviour
{
    public GameObject settingsUI;  // 설정창 UI
    public GameObject player;      // 플레이어 (Movement 컴포넌트를 끄기 위해 필요)

    private bool isSettingsOpen = false;

    void Update()
    {
        // 설정창이 닫혀 있을 때만 ESC로 열 수 있음
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

        // 플레이어 조작 비활성화
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
    /// 설정창 닫기 버튼에서 호출.
    /// </summary>
    /// <param name="returnToGameplay">게임으로 돌아갈 것인지 여부</param>
    public void CloseSettingsUI(bool returnToGameplay)
    {
        settingsUI.SetActive(false);
        Time.timeScale = 1;

        // 플레이어 조작 다시 활성화
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