using UnityEngine;
using System.Collections;
using System.Text;


public class PlayerStateManager : MonoBehaviour
{
    // Attach this script to any object in both scenes (like your HealthBar or Player)
    
    // Keys for PlayerPrefs
    private const string HEALTH_KEY = "PlayerHealth";
    private const string MAX_HEALTH_KEY = "PlayerMaxHealth";
    private const string CURRENT_AMMO_KEY = "CurrentAmmo";
    private const string MAX_AMMO_SIZE_KEY = "MaxAmmoSize";
    private const string CURRENT_CLIP_KEY = "CurrentClip";
    private const string MAX_CLIP_SIZE_KEY = "MaxClipSize";
    private const string STATE_EXISTS_KEY = "PlayerStateExists";
    
    // References to components
    private HealthBar healthBar;
    private ShootScript shootScript;
    
    // Flag to determine if this is a new game session or continuing from another scene
    [SerializeField] private bool isFirstScene = false;
    
    // For inspection and debugging
    [Header("Debug Info")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private string healthBarStatus = "Not Found";
    [SerializeField] private string shootScriptStatus = "Not Found";
    
    // Delay finding components to ensure everything is initialized
    [SerializeField] private float componentSearchDelay = 0.2f;
    [SerializeField] private float componentsRetryInterval = 1.0f;
    [SerializeField] private int maxSearchRetries = 3;
    private int currentRetry = 0;
    
    void Start()
    {
        if (isFirstScene)
        {
            // This is the first scene, so clear any existing saved state
            // This ensures a fresh start when beginning a new game
            ResetPlayerState();
            Debug.Log("First scene detected - PlayerPrefs reset for fresh start");
        }
        
        // Delay the component search to ensure everything is loaded
        StartCoroutine(DelayedInitialization());
    }
    
    private IEnumerator DelayedInitialization()
    {
        // Wait a short time for all scene objects to initialize
        yield return new WaitForSeconds(componentSearchDelay);
        
        // Try finding components
        currentRetry = 0;
        yield return StartCoroutine(RetryFindComponents());
        
        // Now load state if needed
        if (!isFirstScene && PlayerPrefs.GetInt(STATE_EXISTS_KEY, 0) == 1)
        {
            LoadPlayerState();
            Debug.Log("Player state loaded from PlayerPrefs");
        }
        else if (!isFirstScene)
        {
            Debug.Log("No saved player state found");
        }
    }
    
    private IEnumerator RetryFindComponents()
    {
        bool allComponentsFound = false;
        
        while (!allComponentsFound && currentRetry < maxSearchRetries)
        {
            // First try standard search
            FindComponents();
            
            if (currentRetry > 0)
            {
                // If this is a retry, try deep search
                DeepSearchComponents();
            }
            
            // Update component status display
            UpdateDebugStatus();
            
            // Check if we found what we needed
            allComponentsFound = (healthBar != null && shootScript != null);
            
            if (!allComponentsFound)
            {
                currentRetry++;
                Debug.Log($"Component search attempt {currentRetry}/{maxSearchRetries}. Missing components. Waiting before retry...");
                yield return new WaitForSeconds(componentsRetryInterval);
            }
        }
        
        if (!allComponentsFound)
        {
            // Final attempt - try to find components by tag
            TryFindByTag();
            UpdateDebugStatus();
        }
        
        LogComponentStatus();
    }
    
    private void FindComponents()
    {
        // Standard component search
        if (healthBar == null)
            healthBar = FindObjectOfType<HealthBar>();
            
        if (shootScript == null)
            shootScript = FindObjectOfType<ShootScript>();
    }
    
    private void DeepSearchComponents()
    {
        // More aggressive search through all GameObjects including inactive
        Debug.Log("Performing deep search for components...");
        
        MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>(true);
        foreach (MonoBehaviour mb in allMonoBehaviours)
        {
            if (healthBar == null && mb is HealthBar)
            {
                healthBar = mb as HealthBar;
                Debug.Log("Found HealthBar through deep search on: " + mb.gameObject.name);
            }
            
            if (shootScript == null && mb is ShootScript)
            {
                shootScript = mb as ShootScript;
                Debug.Log("Found ShootScript through deep search on: " + mb.gameObject.name);
            }
            
            // Exit early if we found everything
            if (healthBar != null && shootScript != null)
                break;
        }
    }
    
    private void TryFindByTag()
    {
        Debug.Log("Trying to find components by tag...");
        
        // Try to find player by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Debug.Log("Found player GameObject with tag 'Player'");
            
            // Try to find components on player
            if (healthBar == null)
                healthBar = player.GetComponent<HealthBar>();
                
            if (shootScript == null)
            {
                // Look for ShootScript on player or its children
                shootScript = player.GetComponent<ShootScript>();
                if (shootScript == null)
                {
                    shootScript = player.GetComponentInChildren<ShootScript>();
                    if (shootScript != null)
                        Debug.Log("Found ShootScript in child of Player");
                }
            }
        }
        
        // Try to find gun by tag (if your gun has a tag)
        GameObject gun = GameObject.FindWithTag("Gun");
        if (gun != null && shootScript == null)
        {
            shootScript = gun.GetComponent<ShootScript>();
            if (shootScript != null)
                Debug.Log("Found ShootScript on GameObject with tag 'Gun'");
        }
        
        // Try to find weapon components by tag
        GameObject weapon = GameObject.FindWithTag("Weapon");
        if (weapon != null && shootScript == null)
        {
            shootScript = weapon.GetComponent<ShootScript>();
            if (shootScript != null)
                Debug.Log("Found ShootScript on GameObject with tag 'Weapon'");
        }
    }
    
    private void UpdateDebugStatus()
    {
        healthBarStatus = healthBar != null ? 
            $"Found on {healthBar.gameObject.name}" :
            "NOT FOUND";
            
        shootScriptStatus = shootScript != null ? 
            $"Found on {shootScript.gameObject.name}" :
            "NOT FOUND";
    }
    
    private void LogComponentStatus()
    {
        string status = "Component search completed.\n";
        status += $"HealthBar: {(healthBar != null ? "Found" : "NOT FOUND")}\n";
        status += $"ShootScript: {(shootScript != null ? "Found" : "NOT FOUND")}";
        Debug.Log(status);
        
        if (healthBar == null || shootScript == null)
        {
            // List all active game objects with their components for debugging
            ListAllActiveGameObjects();
        }
    }
    
    private void ListAllActiveGameObjects()
    {
        if (!showDebugInfo) return;
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Active GameObjects in scene:");
        
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in rootObjects)
        {
            ListGameObjectHierarchy(root, 0, sb);
        }
        
        Debug.Log(sb.ToString());
    }
    
    private void ListGameObjectHierarchy(GameObject obj, int depth, StringBuilder sb)
    {
        string indent = new string(' ', depth * 2);
        sb.AppendLine($"{indent}- {obj.name} (Active: {obj.activeSelf})");
        
        // List components
        Component[] components = obj.GetComponents<Component>();
        foreach (var component in components)
        {
            if (component != null) // Protection against missing scripts
                sb.AppendLine($"{indent}  ↳ {component.GetType().Name}");
        }
        
        // Process children
        foreach (Transform child in obj.transform)
        {
            if (depth < 3) // Limit depth to avoid huge logs
                ListGameObjectHierarchy(child.gameObject, depth + 1, sb);
        }
    }
    
    // Call this method when transitioning scenes
    public void SavePlayerState()
    {
        // Make sure we have the latest references
        if (healthBar == null || shootScript == null)
        {
            FindComponents();
            DeepSearchComponents();
            UpdateDebugStatus();
        }
        
        if (healthBar != null)
        {
            PlayerPrefs.SetInt(HEALTH_KEY, healthBar.health);
            PlayerPrefs.SetInt(MAX_HEALTH_KEY, healthBar.maxHealth);
            Debug.Log($"Saved health: {healthBar.health}/{healthBar.maxHealth}");
        }
        else
        {
            Debug.LogWarning("Cannot save health - HealthBar component not found!");
        }
        
        if (shootScript != null)
        {
            PlayerPrefs.SetInt(CURRENT_AMMO_KEY, shootScript.currentAmmo);
            PlayerPrefs.SetInt(MAX_AMMO_SIZE_KEY, shootScript.maxAmmoSize);
            PlayerPrefs.SetInt(CURRENT_CLIP_KEY, shootScript.currentClip);
            PlayerPrefs.SetInt(MAX_CLIP_SIZE_KEY, shootScript.maxClipSize);
            Debug.Log($"Saved ammo: {shootScript.currentClip}/{shootScript.maxClipSize} (clip) and {shootScript.currentAmmo}/{shootScript.maxAmmoSize} (reserve)");
        }
        else
        {
            Debug.LogWarning("Cannot save ammo - ShootScript component not found!");
            
            // Output all GameObjects with MonoBehaviours to help debugging
            if (showDebugInfo)
            {
                MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>();
                Debug.Log($"Scene contains {allMonoBehaviours.Length} MonoBehaviours. First 10:");
                for (int i = 0; i < Mathf.Min(10, allMonoBehaviours.Length); i++)
                {
                    MonoBehaviour mb = allMonoBehaviours[i];
                    Debug.Log($"  - {mb.GetType().Name} on {mb.gameObject.name}");
                }
            }
        }
        
        // Mark that we have saved state
        PlayerPrefs.SetInt(STATE_EXISTS_KEY, 1);
        
        // Save immediately
        PlayerPrefs.Save();
    }
    
    private void LoadPlayerState()
    {
        // Make sure we have the latest references
        if (healthBar == null || shootScript == null)
        {
            FindComponents();
            DeepSearchComponents();
            UpdateDebugStatus();
        }
        
        if (healthBar != null)
        {
            healthBar.health = PlayerPrefs.GetInt(HEALTH_KEY, healthBar.health);
            healthBar.maxHealth = PlayerPrefs.GetInt(MAX_HEALTH_KEY, healthBar.maxHealth);
            healthBar.slider.value = healthBar.health;
            Debug.Log($"Loaded health: {healthBar.health}/{healthBar.maxHealth}");
        }
        else
        {
            Debug.LogWarning("Cannot load health - HealthBar component not found!");
        }
        
        if (shootScript != null)
        {
            shootScript.currentAmmo = PlayerPrefs.GetInt(CURRENT_AMMO_KEY, shootScript.currentAmmo);
            shootScript.maxAmmoSize = PlayerPrefs.GetInt(MAX_AMMO_SIZE_KEY, shootScript.maxAmmoSize);
            shootScript.currentClip = PlayerPrefs.GetInt(CURRENT_CLIP_KEY, shootScript.currentClip);
            shootScript.maxClipSize = PlayerPrefs.GetInt(MAX_CLIP_SIZE_KEY, shootScript.maxClipSize);
            Debug.Log($"Loaded ammo: {shootScript.currentClip}/{shootScript.maxClipSize} (clip) and {shootScript.currentAmmo}/{shootScript.maxAmmoSize} (reserve)");
        }
        else
        {
            Debug.LogWarning("Cannot load ammo - ShootScript component not found!");
        }
    }
    
    // Call this if you want to reset the player state (e.g., for a new game)
    public void ResetPlayerState()
    {
        PlayerPrefs.DeleteKey(HEALTH_KEY);
        PlayerPrefs.DeleteKey(MAX_HEALTH_KEY);
        PlayerPrefs.DeleteKey(CURRENT_AMMO_KEY);
        PlayerPrefs.DeleteKey(MAX_AMMO_SIZE_KEY);
        PlayerPrefs.DeleteKey(CURRENT_CLIP_KEY);
        PlayerPrefs.DeleteKey(MAX_CLIP_SIZE_KEY);
        PlayerPrefs.DeleteKey(STATE_EXISTS_KEY);
        PlayerPrefs.Save();
        
        Debug.Log("Player state reset");
    }
    
    // This ensures PlayerPrefs are reset when exiting Play mode in Unity Editor
    #if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        // Clear PlayerPrefs when exiting Play mode to ensure a fresh start next time
        ResetPlayerState();
        Debug.Log("Application quitting - PlayerPrefs reset");
    }
    #endif
}