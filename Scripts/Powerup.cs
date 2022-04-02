using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 *Para objetos colidirem, ambos precisam ter um Collider
 *Um deles precisa ter um rigidbody
 *
 */


public class Powerup : MonoBehaviour
{
    //0 == tiro triplo; 1 == velocidade; 2 == escudos
    [SerializeField]
    private int _powerUpID;
    [SerializeField]
    private AudioClip _clip;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private float _speed = 2.0f;
    
    void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (transform.position.x < -11 || _gameManager.gameOver)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.tag == "Player")
        {
            //Acesssa a classe Player
            Player player = outro.GetComponent<Player>();

            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);

                if (_powerUpID == 0)
                {
                    //Habilita o tiro triplo
                    player.habilitarTiroTriplo();
                }
                else if (_powerUpID == 1)
                {
                    //Habilita boost de velocidade
                    player.habilitarBoostVelocidade();
                }
                else if (_powerUpID == 2)
                {
                    player.HabilitarEscudo();
                }
                else if (_powerUpID == 3)
                {
                    if (player.vidas < 3)
                    {
                        player.vidas++;
                        player.hitCount--;
                        _uiManager.AtualizaVidas(player.vidas);
                    }
                }
            }

            //Destroi o power-up
            Destroy(this.gameObject);
        }
        
    }
}
