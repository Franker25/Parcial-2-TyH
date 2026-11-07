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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(); // Llamamos al Método
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
        int randomValue = Random.Range(0, 4); //Tomamos en cuenta los casos que creamos despues con este int nuevo
        SpawnPointType spawnType = (SpawnPointType)randomValue; //Llamamos al enum que nos ayuda a ver claramente que son estos casos
        switch(spawnType) //usamos un switch para marcar varios casos que pueden pasar, en este caso varios lugares que salen los enemigos
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


      return selectedTransform.position + (Vector3)Random.insideUnitCircle; // Los enemigos que aparecen en las mismas zonas están un poco más alejados 
    }


// Update is called once per frame
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
