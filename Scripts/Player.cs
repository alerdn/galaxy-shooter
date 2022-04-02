using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tiroTriploPrefab;
    [SerializeField]
    private GameObject _explosaoPrefab;
    [SerializeField]
    private GameObject _escudosGameObject;
    [SerializeField]
    private GameObject[] _engines;

    public int vidas = 3;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;

    private float _speed = 5.0f;
    private float _fireRate = 0.25f;
    private float _canFire = 0.0f;
    private AudioSource _audioSource;
    public int hitCount;

    public bool TiroTriploAtivado = false;
    public bool boostVelocidadeAtivado = false;
    public bool escudoAtivado = false;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        transform.position = new Vector3(0, 0, 0);
        _spawnManager.IniciarCorotinas();

        if (_uiManager != null)
        {
            _uiManager.AtualizaVidas(vidas);
        }

        hitCount = 0;
    }
        
    void Update()
    {
        Movimento();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Tiro();
        }

        verificaMotores();
    }

    public void SofrerDano()
    {
        if (escudoAtivado)
        {
            escudoAtivado = false;
            _escudosGameObject.SetActive(false);
            //Usa o return para sair da função sem precisar processar mais nada desnecessariamente
            return;
        }

        hitCount++;
        //Verifica os motores no void Update()

        vidas--;
        _uiManager.AtualizaVidas(vidas);

        if (vidas < 1)
        {
            _gameManager.gameOver = true;
            _uiManager.mostrarTelaInicial();

            Destroy(this.gameObject);
            Instantiate(_explosaoPrefab, transform.position, _explosaoPrefab.transform.rotation);
        }
    }

    private void verificaMotores()
    {
        if (hitCount == 0)
        {
            _engines[0].SetActive(false);
            _engines[1].SetActive(false);
        }
        else if (hitCount == 1)
        {
            _engines[0].SetActive(true);
            _engines[1].SetActive(false);
        }            
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }
            
    }

    private void Tiro()
    {
        _audioSource.Play();

        if (Time.time > _canFire)
        {
            if (TiroTriploAtivado)
            {
                Instantiate(_tiroTriploPrefab, transform.position, _tiroTriploPrefab.transform.rotation);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0.97f, 0, 0), _laserPrefab.transform.rotation);
            }
            _canFire = Time.time + _fireRate;
        }
    }

    private void Movimento()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        if (boostVelocidadeAtivado)
        {
            transform.Translate(Vector3.up * _speed*1.75f * inputHorizontal * Time.deltaTime);
            transform.Translate(Vector3.left * _speed*.75f * inputVertical * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * _speed * inputHorizontal * Time.deltaTime);
            transform.Translate(Vector3.left * _speed * inputVertical * Time.deltaTime);
        }
        

        if (transform.position.y > 6)
            transform.position = new Vector3(transform.position.x, 6, 0);
        else if (transform.position.y < -4.0f)
            transform.position = new Vector3(transform.position.x, -4.0f, 0);

        if (transform.position.x > 9.45f)
            transform.position = new Vector3(9.45f, transform.position.y, 0);
        else if (transform.position.x < -9.5f)
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
    }

    /* IEnumerator é uma co-rotina, com ele podemos usar o comando YIELD return new WaitForSeconds, que faz
     * a próxima instrução ser pausada pelo número de segundos definidos
     * 
     * Não podemos chamar o StarCoroutine no objeto Power up porque quando o destruirmos, as co-rotinas neles serão
     * destruídas também
     * 
     * Por esse motivo, vamos usar a função habilitarTiroTriplo para ativar o poder e já começar a contar o cooldown
     * Ela será chamada no objeto power up
     */
    public void habilitarTiroTriplo()
    {
        TiroTriploAtivado = true;
        StartCoroutine(CorotinaCooldownTiroTriplo());
    }

    public IEnumerator CorotinaCooldownTiroTriplo() 
    {
        //O jogo irá esperar 5 segundos até setar TiroTriploAtivado para false
        yield return new WaitForSeconds(10.0f);
        TiroTriploAtivado = false;
    }

    public void habilitarBoostVelocidade()
    {
        boostVelocidadeAtivado = true;
        StartCoroutine(CorotinaCooldownBoostVelocidade());
    }

    public IEnumerator CorotinaCooldownBoostVelocidade()
    {
        yield return new WaitForSeconds(5.0f);
        boostVelocidadeAtivado = false;
    }

    public void HabilitarEscudo()
    {
        escudoAtivado = true;
        _escudosGameObject.SetActive(true);
    }
}
