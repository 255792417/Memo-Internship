using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemsManager : MonoBehaviour
{
    public AudioManager audioManager;
    public Animator anim;
    public StatusController playerStatus;
    public GameObject bombPrefab;
    public Transform playerTransform;

    // �����
    private ObjectPool<GameObject> bombPool;

    void Awake()
    {
        // ���������
        bombPool = new ObjectPool<GameObject>(
            createFunc: CreateBomb,       // ��������ķ���
            actionOnGet: OnGetBomb,       // ��ȡ����ʱ�Ĳ���
            actionOnRelease: OnReleaseBomb, // �ͷŶ���ʱ�Ĳ���
            actionOnDestroy: OnDestroyBomb, // ���ٶ���ʱ�Ĳ���
            collectionCheck: true,          // �Ƿ�������Ƿ��Ѿ��ڳ���
            defaultCapacity: 10,            // Ĭ������
            maxSize: 100                    // �������
            );
    }

    private GameObject CreateBomb()
    {
        GameObject bombObj = Instantiate(bombPrefab);
        bombObj.GetComponent<Bomb>().audioManager = audioManager;

        return bombObj;
    }

    private void OnGetBomb(GameObject  bomb)
    {
        bomb.transform.position = playerTransform.position;
        bomb.SetActive(true);
    }

    private void OnReleaseBomb(GameObject bomb)
    {
        bomb.SetActive(false);
    }

    private void OnDestroyBomb(GameObject bomb)
    {
        Destroy(bomb);
    }

    private void Update()
    {
        // �� 1 �ָ�����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerStatus.AddOxygen(playerStatus.maxOxygen);
            anim.SetTrigger("OxygenCapsule");
            audioManager.Play("OxygenCapsule",false);
        }

        // �� 2 �ָ�����
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerStatus.Heal(playerStatus.maxHealth);
            anim.SetTrigger("MedicalKit");
            audioManager.Play("MedicalKit",false );
        }

        // �� 3 ����ը��
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject bomb = bombPool.Get();
        }

        // �� 4 ���ͻص���
        if (Input.GetKeyDown(KeyCode.Alpha4) && playerTransform.position.y < 0)
        {
            playerStatus.transform.position = new Vector3(0, 0, 0);
            anim.SetTrigger("Teleport");
            audioManager.Play("Teleport", false);
        }
    }
}
