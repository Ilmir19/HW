using UnityEngine;

public class PlayerKinematic : MonoBehaviour
{
    [Header("Настройки движения")]
    public float speed = 6f;
    public float jumpForce = 14f;
    public float gravity = 40f;

    [Header("Проверка земли")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.15f; // Радиус проверки (не делай слишком большим)

    private float verticalVelocity;
    private bool isGrounded;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Фиксируем Z на 0, чтобы персонаж не исчезал за камерой
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {
        // 1. Проверка: на земле мы или нет
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 2. Считываем кнопки движения
        float moveX = 0;
        if (Input.GetKey(KeyCode.D)) moveX = 1;
        else if (Input.GetKey(KeyCode.A)) moveX = -1;

        // 3. Логика гравитации
        if (isGrounded)
        {
            // Если на земле и не прыгаем — скорость падения почти нулевая
            if (verticalVelocity < 0)
                verticalVelocity = -1f;

            // Прыжок
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                if (anim != null) anim.SetTrigger("Jump");
            }
        }
        else
        {
            // ЕСЛИ В ВОЗДУХЕ — всегда тянем вниз силой гравитации
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // 4. Применяем движение к позиции
        Vector3 moveDelta = new Vector3(moveX * speed, verticalVelocity, 0);
        transform.position += moveDelta * Time.deltaTime;

        // 5. Поворот персонажа влево/вправо
        if (moveX > 0) transform.eulerAngles = new Vector3(0, 0, 0);
        else if (moveX < 0) transform.eulerAngles = new Vector3(0, 180, 0);

        // 6. Обновление аниматора
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveX));
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    // Рисует красный шарик в редакторе, чтобы ты видел, где точка проверки земли
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
