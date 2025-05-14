using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : MonoBehaviour
{
    public float health = 100f; // �ʱ� ü��
    public float currentHealth = 100f; // ���� ü��
    public float attackPower = 4f; // ���� ���ݷ�
    public float attackRange = 0.5f; // ���� ���� (�÷��̾���� �Ÿ�)
    public float attackCooldown = 2f; // ���� ��ٿ� �ð�
    public AudioClip attackSound; // ���� �Ҹ�
    public bool isDead = false; // ��� ����



    private float lastAttackTime; // ������ ���� �ð�
    private AudioSource audioSource; // AudioSource ������Ʈ
    private Vector3 initialPosition; // �ʱ� ��ġ ����
    private Quaternion initialRotation; // �ʱ� ȸ�� ����
    public Vector3 respawnPosition; // ������ ��ġ ����
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


        initialPosition = transform.position;
        initialRotation = transform.rotation;

        //if (anim != null)
        //    anim.SetBool("Move", true);
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    AttackPlayer(collider.gameObject);
                }
            }
        }
    }

    private void AttackPlayer(GameObject player)
    {
        Character playerCharacter = player.GetComponent<Character>();
        if (playerCharacter != null)
        {
            playerCharacter.TakeDamage(attackPower);
            Debug.Log($"{gameObject.name}�� {player.name}��(��) �����Ͽ� {attackPower}�� �������� �־����ϴ�.");

            if (audioSource != null && attackSound != null)
            {
                anim.SetTrigger("Attack");
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || currentHealth <= 0) return; // �̹� �׾��ų� ü���� 0 ������ ��� ����

        currentHealth -= damage;
        Debug.Log($"{gameObject.name}�� {damage}�� �������� �޾ҽ��ϴ�. ���� ü��: {currentHealth}");

        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("Die");
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        Debug.Log($"{gameObject.name}�� ����߽��ϴ�.");
        DisableEnemy();
        StartCoroutine(Respawn());
    }

    private void DisableEnemy()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        enabled = false;
    }

    private IEnumerator Respawn()
    {
        Debug.Log("Respawn �ڷ�ƾ ����.");

        yield return new WaitForSeconds(90f);

        RespawnEnemy();
    }

    private void RespawnEnemy()
    {
        Debug.Log("������ �����.");

        transform.position = respawnPosition;
        transform.rotation = initialRotation;

        currentHealth = health;
        isDead = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        enabled = true;
        lastAttackTime = Time.time;

        Debug.Log($"{gameObject.name} ������ �Ϸ�. ü��: {currentHealth}");
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}