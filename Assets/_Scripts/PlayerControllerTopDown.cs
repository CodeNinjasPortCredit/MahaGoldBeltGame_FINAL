using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerTopDown : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Rigidbody rb;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator animatorController;
    private Vector3 currentMovementDirection;
    private bool isRunning = false;

    [Header("Health System")]
    [SerializeField] private Image[] healthHearts = new Image[3];
    private int currentHealth = 3;
    private int maxHealth = 3;

    [Header("Stamina System")]
    [SerializeField] private Image staminaBar;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 30f;
    [SerializeField] private float staminaRechargeRate = 20f;
    [SerializeField] private float staminaBarMaxWidth = 400f;
    private float currentStamina;
    private bool canSprintAgain = true;

    [Header("Interaction System")]
    [SerializeField] private bool isInInteraction = false;
    private bool canMove = true;

    private bool isInvincible = false; //start with ability to receieve damage
    public float invincibilityDuration = 2.0f;

    private void Start()
    {
        isInvincible = false;

        // Initialize Rigidbody if not assigned
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Ensure Rigidbody is properly configured for top-down movement
        if (rb != null)
        {
            rb.freezeRotation = true; // Prevent physics rotation
            rb.constraints = RigidbodyConstraints.FreezeRotationX | 
                            RigidbodyConstraints.FreezeRotationY | 
                            RigidbodyConstraints.FreezeRotationZ;
        }

        // Initialize health UI
        UpdateHealthDisplay();

        // Initialize stamina
        currentStamina = maxStamina;
        UpdateStaminaDisplay();
    }

    private void Update()
    {
        if (!canMove)
            return;

        // Get input
        HandleMovementInput();
        HandleRunInput();

        // Update stamina
        UpdateStamina();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Apply movement
        ApplyMovement();

        // Rotate character toward camera direction
        RotateCharacterTowardCamera();
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create movement direction based on camera orientation
        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Flatten camera forward for top-down movement
            cameraForward.y = 0f;
            cameraForward.Normalize();

            currentMovementDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        }
        else
        {
            // Fallback to world space if no camera assigned
            currentMovementDirection = new Vector3(horizontal, 0f, vertical).normalized;
        }

        // Update animation parameters
        if (animatorController != null)
        {
            animatorController.SetFloat("Speed", currentMovementDirection.magnitude);
            animatorController.SetBool("IsRunning", isRunning);
        }
    }

    private void HandleRunInput()
    {
        bool wantToRun = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        // Can sprint if: stamina is available AND not locked out from depletion AND player is moving
        if (wantToRun && canSprintAgain && currentStamina > 0 && currentMovementDirection.magnitude > 0.1f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        Debug.Log($"WantToRun: {wantToRun}, CanSprintAgain: {canSprintAgain}, Stamina: {currentStamina:F1}, Moving: {currentMovementDirection.magnitude > 0.1f}, IsRunning: {isRunning}");
    }

    private void UpdateStamina()
    {
        if (isRunning)
        {
            // Drain stamina while running
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina < 0)
                currentStamina = 0;

            // Lock out sprinting if stamina completely depleted
            if (currentStamina <= 0)
                canSprintAgain = false;

            Debug.Log($"Draining Stamina: {currentStamina:F1} / {maxStamina}");
        }
        else
        {
            // Recharge stamina while walking or idling
            currentStamina += staminaRechargeRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            // Can sprint again once fully recharged
            if (currentStamina >= maxStamina)
                canSprintAgain = true;
        }

        UpdateStaminaDisplay();
    }

    private void UpdateStaminaDisplay()
    {
        if (staminaBar != null)
        {
            // Calculate width based on stamina percentage (0 to maxWidth)
            float staminaPercent = currentStamina / maxStamina;
            float newWidth = staminaPercent * staminaBarMaxWidth;
            
            // Update RectTransform width
            RectTransform rectTransform = staminaBar.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 sizeDelta = rectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                rectTransform.sizeDelta = sizeDelta;
                Debug.Log($"Stamina Display - Current: {currentStamina:F1}, Max: {maxStamina}, Width: {newWidth:F1} / {staminaBarMaxWidth}");
            }
        }
        else
        {
            Debug.LogWarning("Stamina Bar is not assigned!");
        }
    }

    private void ApplyMovement()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 velocity = currentMovementDirection * currentSpeed;
        velocity.y = rb.velocity.y; // Preserve gravity

        rb.velocity = velocity;
    }

    private void RotateCharacterTowardCamera()
    {
        if (currentMovementDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentMovementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible) {  //when is not invincible
            TakeDamage(); 
        }
    }

    IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration); //the amount of time he is invincible for
        isInvincible = false;


    }

    /// <summary>
    /// Reduces health by 1 and updates UI. Call when player takes damage.
    /// </summary>
    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateHealthDisplay();

            if (currentHealth <= 0)
            {
                OnPlayerDeath();
            }
        }

        StartCoroutine(InvincibilityTimer()); //start invincibility
    }

    /// <summary>
    /// Increases health by 1 up to max health and updates UI.
    /// </summary>
    public void GainHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHealthDisplay();
        }
    }

    private void UpdateHealthDisplay()
    {
        for (int i = 0; i < healthHearts.Length; i++)
        {
            if (healthHearts[i] != null)
            {
                healthHearts[i].enabled = i < currentHealth;
            }
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player has died!");
        // TODO: Implement death logic (respawn, game over screen, etc.)
    }

    /// <summary>
    /// Call this when entering an interaction (e.g., minigame, dialogue).
    /// Freezes player movement and animation.
    /// </summary>
    public void EnterInteractionState()
    {
        isInInteraction = true;
        canMove = false;
        
        // Stop animations
        if (animatorController != null)
        {
            animatorController.SetFloat("Speed", 0f);
        }

        // Stop movement
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        Debug.Log("Entered Interaction State - Show Minigame Menu");
        // TODO: Spawn minigame menu UI here
    }

    /// <summary>
    /// Call this when exiting an interaction state.
    /// Resumes normal player control.
    /// </summary>
    public void ExitInteractionState()
    {
        isInInteraction = false;
        canMove = true;

        Debug.Log("Exited Interaction State - Resume gameplay");
        // TODO: Close minigame menu UI here
    }

    public bool IsInInteraction() => isInInteraction;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
