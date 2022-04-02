using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _inimigoPrefab;
    [SerializeField]
    private GameObject[] _powerupsPrefab;

    private UIManager _uiManager;
    private GameManager _gameManager;

    private float _enemySpawnRate;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _enemySpawnRate = 3;
    }

    void Update()
    {
        if (_gameManager.gameOver) _enemySpawnRate = 3;
    }

    public void IniciarCorotinas()
    {
        StartCoroutine(CorotinaSpawnInimigo());
        StartCoroutine(CorotinaSpawnPowerup());
    }

    private IEnumerator CorotinaSpawnInimigo()
    {
        while (!_gameManager.gameOver)
        {
            Instantiate(_inimigoPrefab, new Vector3(12, Random.Range(-3.8f, 5.9f), 0), _inimigoPrefab.transform.rotation);

            if (_enemySpawnRate > 1.2f)
                _enemySpawnRate -= _uiManager.getScore() * 0.01f;

            yield return new WaitForSeconds(_enemySpawnRate);
        }

    }

    private IEnumerator CorotinaSpawnPowerup()
    {
        while (!_gameManager.gameOver)
        {
            //Com inteiro, o Random.Range não retorna o valor máximo. Exemplo: Random.Range(0, 3) retornará: 0, 1 e 2
            //Com float, o Random.Range inclui o número máximo. Exemplo: Random.Range(0, 3) retornará: 0, 1, 2 e 3
            int randomPowerup = Random.Range(0, 4);
            Instantiate(_powerupsPrefab[randomPowerup], new Vector3(11, Random.Range(-3.8f, 5.7f), 0), _powerupsPrefab[randomPowerup].transform.rotation);
            float randomTimePowerup = Random.Range(10.0f, 20.0f);
            yield return new WaitForSeconds(randomTimePowerup);
        }
    }
}
