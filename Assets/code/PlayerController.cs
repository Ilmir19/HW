using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки")]
    public float speed = 6f;
    public float jumpForce = 12f;

    [Header("Связи")]
    public Rigidbody2D rb;
    public Animator anim;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontalInput;
    private bool isGrounded;

    void Start()
    {
        // Если забыл прикрепить в инспекторе — скрипт сам их найдет
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();

        // Запрещаем персонажу заваливаться на бок
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 1. ПРЯМОЙ ОПРОС КЛАВИШ (Игнорируем Глючные оси Unity)
        horizontalInput = 0;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1;

        // 2. ПРОВЕРКА ЗЕМЛИ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // 3. ПРЫЖОК
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (anim != null) anim.SetTrigger("Jump");
        }

        // 4. АНИМАЦИИ (передаем данные)
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
            anim.SetBool("isGrounded", isGrounded);
        }

        // 5. ПОВОРОТ ПЕРСОНАЖА
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput, 1, 1);
        }
    }

    void FixedUpdate()
    {
        // 6. ФИЗИЧЕСКОЕ ДВИЖЕНИЕ
        // Мы меняем только X, оставляя Y (прыжок/падение) нетронутым
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }
}
