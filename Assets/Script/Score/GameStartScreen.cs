using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartScreen : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public GameObject astronaut;

    private Vector3 moveDirection;
    private float MOVE_SPEED = 3f;
    private float spinSpeed;
    private bool isLeftToRight = true;

    private Vector3 endPos;

    private void Start()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene("InGame"));
        exitButton.onClick.AddListener(() => Application.Quit());
        SetNewPath();
    }

    private void Update()
    {
        if (astronaut == null) return;

        astronaut.transform.position += moveDirection * Time.deltaTime * MOVE_SPEED;
        astronaut.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);

        Vector3 toEnd = endPos - astronaut.transform.position;
        if (Vector3.Dot(moveDirection, toEnd) <= 0f)
        {
            isLeftToRight = !isLeftToRight;
            SetNewPath();
        }
    }

    private void SetNewPath()
    {
        float startX = isLeftToRight ? -12f : 12f;
        float endX = isLeftToRight ? 12f : -12f;

        float startY = Random.Range(-4.5f, 4.5f);
        float endY = Random.Range(-4.5f, 4.5f);

        Vector3 startPos = new Vector3(startX, startY, 0f);
        endPos = new Vector3(endX, endY, 0f);

        astronaut.transform.position = startPos;
        moveDirection = (endPos - startPos).normalized;

        astronaut.transform.rotation = Quaternion.Euler(
            Random.Range(-30f, 30f),
            Random.Range(0f, 360f),
            Random.Range(-30f, 30f)
        );

        spinSpeed = Random.Range(30f, 60f) * (Random.value > 0.5f ? 1f : -1f);
    }
}
