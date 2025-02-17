using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 200f;
    public float deadZoneRadius = 0.5f;
    private Vector2 velocity;

    public Animator screen;

    private bool isActive;

    public GameObject endScreen;

    public GameObject sfx;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        float distance = Vector2.Distance(transform.position, mousePosition);
        if (distance > deadZoneRadius)
        {
            Vector2 direction = ((Vector2)mousePosition - (Vector2)transform.position).normalized;
            velocity = direction * speed * Time.deltaTime;
            transform.position += (Vector3)velocity;
        }

        Vector2 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            if (!isActive)
            {
                screen.gameObject.SetActive(true);
                screen.SetBool("on", true);
                Invoke("Next", 0.6f);

                isActive = true;
            }
        }

        if (collision.gameObject.tag == "DeadZone")
        {
            if (!isActive)
            {
                screen.gameObject.SetActive(true);
                screen.SetBool("on", true);
                Invoke("Restart", 0.6f);

                isActive = true;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 0f;
    }

    public void Next()
    {
        if(SceneManager.GetActiveScene().buildIndex < 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 0f;
        }
        else
        {
            endScreen.SetActive(true);
        }

        Instantiate(sfx);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }
}