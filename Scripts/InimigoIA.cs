using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoIA : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosaoInimigoPrefab;
    [SerializeField]
    private AudioClip _clip;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private float _speed = 5.0f;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x < -13)
        {
            transform.position = new Vector3(12, Random.Range(-3.8f, 5.9f), 0);
        }

        if (_gameManager.gameOver)
            Destroy(this.gameObject);
    }

    private void RodarAnimacaoExplosao()
    {
        Instantiate(_explosaoInimigoPrefab, transform.position, _explosaoInimigoPrefab.transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.tag == "Player")
        {
            Player player = outro.GetComponent<Player>();
            if (player != null)
            {
                player.SofrerDano();
            }

            RodarAnimacaoExplosao();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);
            Destroy(this.gameObject);
        }
        else if (outro.tag == "Laser")
        {
            /*if (outro.transform.parent != null)
            {
                Destroy(outro.transform.parent.gameObject);
            }*/
            Destroy(outro.gameObject);

            _uiManager.AtualizaScore();

            RodarAnimacaoExplosao();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);
            Destroy(this.gameObject);
        }
    }
}
