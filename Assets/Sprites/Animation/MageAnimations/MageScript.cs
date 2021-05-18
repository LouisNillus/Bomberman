using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageScript : MonoBehaviour
{
    private Vector3 _startPos = new Vector3(12, 0, 0);
    [SerializeField] private GameObject _prefabMage;

    private float _timer= 0.0f;

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.P) && _timer <= 0)
        {
            GameObject go = Instantiate(_prefabMage, _startPos, Quaternion.identity);
            Destroy(go, go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.2f);
            _timer = 1.5f;
        }
    }
}
