using UnityEngine;
using UnityEngine.Events;  // Pour créer des événements personnalisés

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Death Effect")]
    public GameObject explosionPrefab;    // Le prefab de l'explosion qu'on a créé
    private bool isDead = false;          // Pour éviter de déclencher la mort plusieurs fois
    private float health;


    public UnityEvent<float> onHealthChanged; // Événement qui sera déclenché quand les PV changent

    public UnityEvent onPlayerDeath; // Événement pour la mort du joueur

    // Références aux composants
    private SpriteRenderer playerSprite;
    private PlayerController playerController;
    private Rigidbody2D rb;

    private void Awake()
    {
        // Initialisation des événements s'ils sont null
        if (onHealthChanged == null) onHealthChanged = new UnityEvent<float>();
        if (onPlayerDeath == null) onPlayerDeath = new UnityEvent();
    }

    void Start()
    {
        // Initialisation des composants
        playerSprite = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        // Initialisation des événements
        if (onHealthChanged == null) onHealthChanged = new UnityEvent<float>();
        if (onPlayerDeath == null) onPlayerDeath = new UnityEvent();

        onHealthChanged.Invoke(currentHealth / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        // Ne fait rien si déjà mort
        if (isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        // Déclenche l'événement avec le pourcentage de vie restant
        onHealthChanged.Invoke(currentHealth / maxHealth);

        Debug.Log($"Il vous reste {currentHealth} Point de vie");

        if (currentHealth <= 0)
        {
            HandlePlayerDeath();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged.Invoke(currentHealth / maxHealth);
    }

    private void HandlePlayerDeath()
    {
        isDead = true;

        // Désactive le contrôle du joueur
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Arrête tout mouvement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Crée l'effet d'explosion
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Récupère la durée de l'animation pour une destruction automatique
            Animator explosionAnimator = explosion.GetComponent<Animator>();
            if (explosionAnimator != null)
            {
                AnimatorClipInfo[] clipInfo = explosionAnimator.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length > 0)
                {
                    float clipDuration = clipInfo[0].clip.length;
                    Destroy(explosion, clipDuration);
                }
            }
        }

        // Cache le sprite du joueur
        if (playerSprite != null)
        {
            playerSprite.enabled = false;
        }

        onPlayerDeath.Invoke();
    }
}