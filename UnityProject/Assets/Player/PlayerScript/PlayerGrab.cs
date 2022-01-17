using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("�I�u�W�F�N�g")]
    [Tooltip("��̈ʒu")] public GameObject _HandPosition;
    [Tooltip("�莝���̂���")]public GameObject _HandBallObject;
    [Tooltip("������A�C�e��")] public GameObject _ThrowBallObject;

    [Header("�{���͈�")]
    public float _SearchRange;

    [Header("��")]
    public float _ThrowForce;

    [Header("���C���[�}�X�N")]
    public LayerMask _ItemLayer;

    [SerializeField]Vector3 lastPos;
    bool isGrab;

    

    // Start is called before the first frame update
    void Start()
    {
        isGrab = false;
    }

    // Update is called once per frame
    void Update()
    {
        //_HandBallObject.transform.position = _HandPosition.transform.position;
        //Debug.Log(_HandPosition.transform.position == _HandBallObject.transform.position);
        Vector3 nowPos = new Vector3(transform.position.x, 0f, transform.position.z);

        //�ړ��ʂ�0.1f�ȉ��̏ꍇ(�~�܂��Ă���Ƃ�)
        if ((nowPos - lastPos).magnitude < 0.005f)
        {
            if (isGrab)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ThrowItem();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    GrabItem();
                }
            }
        }

        lastPos = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    bool SearchItem()
    {
        var colliders = Physics.OverlapSphere(transform.position, _SearchRange, _ItemLayer);
        if (colliders.Length <= 0) return false;

        for (int i = 0; i < colliders.Length; ++i)
            Debug.Log("find item" + colliders[i].tag.ToString());

        if (colliders.Length > 1)
        {
            for(int i = colliders.Length - 1; i > 0; --i)
            {

                for (int j = 0; j <= i - 1; ++j)
                {
                    if(Vector3.Distance(transform.position, colliders[j].transform.position) > Vector3.Distance(transform.position, colliders[j + 1].transform.position))
                    {
                        var temp = colliders[j];
                        colliders[j] = colliders[j + 1];
                        colliders[j + 1] = temp;
                    }
                }
            }
        }
        colliders[0].gameObject.GetComponent<ItemBehavior>().PickUp();
        _HandBallObject.SetActive(true);
        return true;
    }

    void GrabItem()
    {
        if (SearchItem())
        {
            isGrab = true;
        }
    }

    void ThrowItem()
    {
        isGrab = false;
        Vector3 temp = transform.position + transform.forward * 2;
        temp.y += 3.0f;
        GameObject item = Instantiate(_ThrowBallObject, temp, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * _ThrowForce, ForceMode.Impulse);
        _HandBallObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _SearchRange);
    }
}
