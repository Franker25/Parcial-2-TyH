using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int enemyCount = 5;
    [SerializeField]
    private Transform spawnTopLeft, spawnTopRight, spawnBottomLeft, spawnBottomRight;
    [SerializeField]
    private GameManager gameManager;

   
    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(); 
        }
    }

    private void SpawnEnemy()
    {
        if (gameManager.HasWon)
        {
            return;
        }

        Vector3 spawnPosition = SelectRandomPosition();
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnDie += gameManager.AddKill;
            enemy.OnDie += SpawnEnemy;
        }
    }

    private Vector3 SelectRandomPosition()
    { 
      Transform selectedTransform = null;
        int randomValue = Random.Range(0, 4); 
        SpawnPointType spawnType = (SpawnPointType)randomValue; 
        switch(spawnType) 
        {
            case SpawnPointType.TopLeft:
                {
                    selectedTransform = spawnTopLeft;
                    break;
                }
            case SpawnPointType.TopRight:
                {
                    selectedTransform = spawnTopRight;
                    break;
                }
            case SpawnPointType.BottomLeft:
                {
                    selectedTransform = spawnBottomLeft;
                    break;
                }
            case SpawnPointType.BottomRight:
                {
                    selectedTransform = spawnBottomRight;
                    break;
                }
            default:
                {
                    selectedTransform = spawnTopLeft;
                    break;
                }
        }


      return selectedTransform.position + (Vector3)Random.insideUnitCircle; 
    }



void Update()
    {
        
    }

    public enum SpawnPointType
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3,
    }

}
