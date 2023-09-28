using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject EnemyContainerPrefab;
    Enemy[] enemies = new Enemy[3];
    [SerializeField] Transform[] EnemySpawnZoneTopLeftCorners;
    [SerializeField] Transform[] EnemySpawnZoneBottomRightCorners;
    [SerializeField] Character Character;
    [SerializeField] Inventory Inventory;
    [SerializeField] Sprite BulletSprite;

    float SaveInterval = 10f;
    float SaveTimer = 10f;

    private void Start()
    {
        string destination = Application.persistentDataPath + "/save.txt";
        if (File.Exists(destination))
            LoadGame();
        else
            Restart();
    }

    private void SaveGame()
    {
        List<Vector3> enemyPositions = new List<Vector3>();
        List<float> enemyHealth = new List<float>();
        foreach (var enemy in enemies)
            if (enemy != null)
            {
                enemyPositions.Add(enemy.transform.position);
                enemyHealth.Add(enemy.Health.CurrentHealth);
            }

        var pickups = FindObjectsOfType<PickupItem>();
        List<Vector3> pickupPositions = new List<Vector3>();
        foreach (var pickup in pickups)
            pickupPositions.Add(pickup.transform.position);

        var SaveData = new SaveData
        {
            Items = Inventory.items.Select(x => x == null ? null : x.Name).ToArray(),
            Amounts = Inventory.amounts,
            CharacterPosition = Character.transform.position,
            CharacterHealth = Character.Health.CurrentHealth,
            EnemyPositions = enemyPositions.ToArray(),
            EnemyHealth = enemyHealth.ToArray(),
            Pickups = pickups,
            PickupPositions = pickupPositions.ToArray()
        };

        string json = JsonUtility.ToJson(SaveData);
        string destination = Application.persistentDataPath + "/save.txt";

        using (StreamWriter outputFile = new StreamWriter(destination))
        {
            outputFile.Write(json);
        }
    }

    private void LoadGame()
    {
        string json;
        string destination = Application.persistentDataPath + "/save.txt";
        using (StreamReader file = new StreamReader(destination))
        {
            json = file.ReadToEnd();
        }
        var saveData = JsonUtility.FromJson<SaveData>(json);

        Inventory.items = saveData.Items.Select(x => new InventoryItem { Name = x }).ToArray();
        Inventory.amounts = saveData.Amounts;
        Inventory.UpdateAllSlots();

        Character.transform.position = saveData.CharacterPosition;
        Character.Health.CurrentHealth = saveData.CharacterHealth;
        Character.Health.UpdateHealth();

        for (int i = 0; i < saveData.EnemyPositions.Length; i++)
        {
            var spawnVector = saveData.EnemyPositions[i];
            enemies[i] = Instantiate(EnemyContainerPrefab, spawnVector, new Quaternion()).GetComponentInChildren<Enemy>();
            enemies[i].Health.CurrentHealth = saveData.EnemyHealth[i];
            enemies[i].Health.UpdateHealth();
        }
    }

    private void Update()
    {
        SaveTimer -= Time.deltaTime;
        if (SaveTimer <= 0)
        {
            SaveGame();
            SaveTimer += SaveInterval;
        }
    }

    public void Restart()
    {
        for (int i = 0; i < 3; i++)
        {
            if (enemies[i] != null)
                Destroy(enemies[i].transform.parent.gameObject);

            var spawnX = Random.Range(EnemySpawnZoneTopLeftCorners[i].position.x, EnemySpawnZoneBottomRightCorners[i].position.x);
            var spawnY = Random.Range(EnemySpawnZoneBottomRightCorners[i].position.y, EnemySpawnZoneTopLeftCorners[i].position.y);
            var spawnVector = new Vector3(spawnX, spawnY, 0);
            enemies[i] = Instantiate(EnemyContainerPrefab, spawnVector, new Quaternion()).GetComponentInChildren<Enemy>();
        }

        var pickups = FindObjectsOfType<PickupItem>();
        foreach (var pickup in pickups)
            Destroy(pickup.gameObject);

        Character.transform.position = new Vector3();
        Character.transform.parent.gameObject.SetActive(true);
        var health = Character.Health;
        health.SetHealth(health.MaxHealth);

        Inventory.DeleteAll();
        Inventory.TryAddItem(new InventoryItem { Name = "bullet" }, 30);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
