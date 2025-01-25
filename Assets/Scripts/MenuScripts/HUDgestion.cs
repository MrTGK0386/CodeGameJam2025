using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Pour le texte avec TextMeshPro (si vous l'utilisez)

public class HUDManager : MonoBehaviour
{
    [Header("Health Bar References")]
    public Slider healthBarSlider;         // La barre de vie sous forme de Slider UI
    public Image healthBarFill;            // L'image de remplissage de la barre de vie
    public TextMeshProUGUI healthText;     // Le texte affichant les PV (optionnel)
    
    [Header("Health Bar Colors")]
    public Color fullHealthColor = Color.green;     // Couleur quand la vie est pleine
    public Color lowHealthColor = Color.red;        // Couleur quand la vie est basse
    public float lowHealthThreshold = 0.3f;         // Seuil en dessous duquel la vie est considérée comme basse (30%)

    // Référence au PlayerStats
    private PlayerStats playerStats;

    void Start()
    {
        // Cherche le composant PlayerStats dans la scène
        playerStats = FindObjectOfType<PlayerStats>();
        
        if (playerStats == null)
        {
            Debug.LogError("Aucun PlayerStats trouvé dans la scène!");
            return;
        }

        // Configure la barre de vie
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = 1f;  // Car on travaille avec des pourcentages
            healthBarSlider.value = 1f;
        }

        // S'abonne aux événements de PlayerStats
        playerStats.onHealthChanged.AddListener(UpdateHealthBar);
        playerStats.onPlayerDeath.AddListener(HandlePlayerDeath);

        // Initialise l'affichage
        UpdateHealthBar(1f);
    }

    private void UpdateHealthBar(float healthPercentage)
    {
        // Met à jour la valeur de la barre de vie
        if (healthBarSlider != null)
        {
            healthBarSlider.value = healthPercentage;
        }

        // Met à jour la couleur de la barre de vie
        if (healthBarFill != null)
        {
            // Interpole la couleur entre rouge et vert selon le niveau de vie
            healthBarFill.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
        }

        // Met à jour le texte des PV si présent
        if (healthText != null)
        {
            float currentHealth = playerStats.currentHealth;
            float maxHealth = playerStats.maxHealth;
            healthText.text = $"{Mathf.Ceil(currentHealth)}/{maxHealth} PV";
        }

        // Effet visuel quand la vie est basse
        if (healthPercentage <= lowHealthThreshold)
        {
            // Ici vous pourriez ajouter des effets visuels pour alerter le joueur
            Debug.Log("Vie faible !");
        }
    }

    private void HandlePlayerDeath()
    {
        // Vous pouvez ajouter ici des effets visuels pour la mort
        Debug.Log("Le joueur est mort - Mise à jour du HUD");
        
        // Par exemple, faire clignoter la barre de vie en rouge
        if (healthBarFill != null)
        {
            healthBarFill.color = Color.red;
        }
    }
}