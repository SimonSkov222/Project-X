using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnPointEnemies : NetworkBehaviour
{
    const int CATEGORY_NORMAL = 0;
    const int CATEGORY_ELITE = 1;
    const int CATEGORY_BOSS = 2;
        
    [SerializeField]
    private GameObject[] normals;
    [SerializeField]
    private GameObject[] elites;
    [SerializeField]
    private GameObject[] bosses;

    [SerializeField]
    private int numbOfEnemies = 10;

    [SerializeField]
    [Range(0.001f, 1)]
    private float changeForElites = 0.2f;

    [SerializeField]
    [Range(1, 25)]
    private float spawnRadius = 5;

    private List<GameObject>[] poolNormals;
    private List<GameObject>[] poolElites;
    private List<GameObject>[] poolBosses;

    private int poolSizeNormals = 10;
    private int poolSizeElites = 10;
    private int poolSizeBosses = 10;

    public int AliveEnemies { get; private set; }
    public bool CanSpawnElites { get { return elites.Length > 0; } }

    [Server]
    void Start()
    {

        poolNormals = new List<GameObject>[normals.Length];
        poolElites = new List<GameObject>[elites.Length];
        poolBosses = new List<GameObject>[bosses.Length];

        for (int m = 0; m < normals.Length; m++)
        {
            poolNormals[m] = new List<GameObject>();
            for (int i = 0; i < poolSizeNormals; i++)
            {
                CreateEnemy(CATEGORY_NORMAL, m);
            }
        }
        for (int m = 0; m < elites.Length; m++)
        {
            poolElites[m] = new List<GameObject>();
            for (int i = 0; i < poolSizeNormals; i++)
            {
                CreateEnemy(CATEGORY_ELITE, m);
            }
        }
        for (int m = 0; m < bosses.Length; m++)
        {
            poolBosses[m] = new List<GameObject>();
            for (int i = 0; i < poolSizeNormals; i++)
            {
                CreateEnemy(CATEGORY_BOSS, m);
            }
        }


        SpawnFullEnemies();
    }

    private int[] GetRandomEnemyType()
    {
        int cat = CATEGORY_NORMAL;
        int model = Random.Range(0, normals.Length);
        //Spawn an elite
        if (CanSpawnElites && Random.Range(0,1) <= changeForElites)
        {
            cat = CATEGORY_ELITE;
            model = Random.Range(0, elites.Length);
        }

        return new int[] { cat, model };
    }

    private void SpawnFullEnemies()
    {
        int tryTimes = 5;

        for (int i = 0; i < numbOfEnemies; i++)
        {
            for (int a = 0; a < tryTimes; a++)
            {
                if (RandomSpawn(spawnRadius))
                {
                    break;
                }
            }

        }

    }

    private bool RandomSpawn(float radius)
    {
        RaycastHit hit;
        Vector2 point = Random.insideUnitCircle * radius;
        float pointY = transform.position.y + 10;
        if (Physics.Raycast(new Vector3(point.x + transform.position.x, pointY, point.y + transform.position.z), Vector3.down, out hit))
        {
            if (hit.collider.tag == "Floor")
            {
                Vector3 position = hit.point + (Vector3.up * 2);//skal ikke spawne nede i jorden
                int[] type = GetRandomEnemyType();  //Hent kategori og model id
                SpawnEnemy(type[0], type[1], position);
                return true;
            }
        }

        return false;
    }
    [Server]
    private GameObject CreateEnemy(int category, int model)
    {
     //   Debug.Log("CreateEnemy: " + category + " -- " + model);
        //Hent kategori og pool
        GameObject[] enemies = null;
        List<GameObject>[] pool = null;
        switch (category)
        {
            case CATEGORY_NORMAL: enemies = normals; pool = poolNormals; break;
            case CATEGORY_ELITE: enemies = elites; pool = poolElites; break;
            case CATEGORY_BOSS: enemies = bosses; pool = poolBosses; break;
        }

        //Opret fjenden
        GameObject enemy = Instantiate<GameObject>(enemies[model]);
        IHealth health = (IHealth)enemy.GetComponent(typeof(IHealth));
        health.EventOnDeath += (s, o) => AliveEnemies--;
        enemy.SetActive(false);
        enemy.transform.SetParent(transform);

        pool[model].Add(enemy);
        
        NetworkServer.Spawn(enemy);
        //NetworkManager.singleton.spawnPrefabs.Add(enemy);
        //ClientScene.RegisterPrefab(enemy);
        return enemy;
    }
    

    private GameObject GetDisabledEnemy(int category, int model)
    {
        //Hent pool
        List<GameObject>[] pool = null;
        switch (category)
        {
            case CATEGORY_NORMAL:   pool = poolNormals;  break;
            case CATEGORY_ELITE:    pool = poolElites;  break;
            case CATEGORY_BOSS:     pool = poolBosses;  break;
        }
        GameObject enemy = pool[model].FirstOrDefault(m => m != null && !m.activeSelf);

        //hvis der ikke er nogle fjender de er disable, lav en ny
        if (enemy == null)
        {
            enemy = CreateEnemy(category, model);
        }


        return enemy;
    }
    
    private void SpawnEnemy(int category, int model, Vector3 position/*, float rotationDegreeY*/)
    {
        GameObject enemy = GetDisabledEnemy(category, model);
        AliveEnemies++;
        enemy.transform.position = position;
        //enemy.transform.Rotate(Vector3.up * rotationDegreeY);
        enemy.SetActive(true);
    }

}
