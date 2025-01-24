using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Références et variables de configuration
    public Transform target;           // Assigne la cible à la main
    private Rigidbody2D rb;            // Le Rigidbody2D de l'ennemi

    [Header("Movement Settings")]
    public float moveSpeed = 3f;        // Vitesse de déplacement
    public float detectionRange = 10f;  // Distance de détection du joueur
    public float stoppingDistance = 1f; // Distance minimale avant d'arrêter de suivre

    [Header("Optional Settings")]
    public bool rotateTowardsPlayer = true;  // Si l'ennemi doit se tourner vers le joueur
    public float rotationSpeed = 5f;         // Vitesse de rotation

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Si target n'est pas assigné, on essaie de le trouver automatiquement
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("Player found automatically");
            }
            else
            {
                Debug.LogWarning("No player found in scene! Please assign the player reference manually.");
            }
        }

        // Configuration du Rigidbody2D
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
            // Vérifions que nous avons bien un Collider2D configuré en trigger
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;  // Important : on met le collider en trigger
            }
            else
            {
                Debug.LogError("No Collider2D found on enemy!");
            }
        }
        else
        {
            Debug.LogError("No Rigidbody2D found on enemy!");
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;  // Sécurité si le joueur n'existe plus

        // Calcul de la distance avec le joueur
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        // Si le joueur est dans la zone de détection
        if (distanceToPlayer <= detectionRange)
        {
            // Direction vers le joueur
            Vector2 direction = (target.position - transform.position).normalized;

            // Déplacement vers le joueur
            rb.velocity = direction * moveSpeed;

            // Rotation vers le joueur si activée
            if (rotateTowardsPlayer)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    rotationSpeed * Time.fixedDeltaTime
                );
            }

        }
        else
        {
            // Arrêt si le joueur est hors de portée
            rb.velocity = Vector2.zero;
        }
    }

    // Méthode pour visualiser la zone de détection dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }

    // Fonction pour détecter les collisions avec le trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifions si c'est bien le player qui est entré dans notre trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Dégâts subis");
            
            // Plus tard, nous appellerons ici la fonction pour infliger des dégâts
            // Par exemple : other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        }
    }

    // Optionnel : ajoutons une fonction pour détecter le temps passé en contact
    private void OnTriggerStay2D(Collider2D other)
    {
        // Utile si vous voulez infliger des dégâts continus
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Contact maintenu avec le player");
        }
    }
}