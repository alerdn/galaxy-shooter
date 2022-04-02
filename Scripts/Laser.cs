using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 15.0f;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.x > 11)
        {
            //Se o laser tem um pai (está dentro de outro objeto)
            if (transform.parent != null)
            {
                //Destroi o pai do laser
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
