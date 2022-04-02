using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite[] spritesVida;
    public Image spriteAtual;
    public GameObject titleScreen;
    public GameObject startText;
    public Text scoreText;
    private int score;

    public void mostrarTelaInicial()
    {
        titleScreen.SetActive(true);
        startText.SetActive(true);
    }

    public void esconderTelaInicial()
    {
        titleScreen.SetActive(false);
        startText.SetActive(false);

        scoreText.text = "Score: 0";
        score = 0;
    }

    public void AtualizaVidas(int vidaAtual)
    {
        spriteAtual.sprite = spritesVida[vidaAtual];
    }

    public void AtualizaScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public int getScore()
    {
        return score;
    }
}
