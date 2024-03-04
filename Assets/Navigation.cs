using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // 引入命名空间以访问场景管理功能

public class Navigation : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public GameObject gameover;
    public GameObject restart;
    public bool getconfused = false;


    private Vector3 stopPosition;
    private bool playerStopped = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(player.position);
        gameover.SetActive(false);
        restart.SetActive(false);

    }

    void Update()
    {
        if (player != null && !getconfused)
        {
            if (!playerStopped)
            {
                agent.SetDestination(player.position);
            }
            //caught player, stop moving, show game over
            if (Vector3.Distance(player.position, transform.position) < 0.8f)
            {
                agent.isStopped = true;
                gameover.SetActive(true);
                restart.SetActive(true);
                // 保存玩家的当前位置，并将playerStopped设置为true
                stopPosition = player.position;
                playerStopped = true;
            }
        }
        // 如果playerStopped为true，则强制玩家停留在stopPosition位置
        if (playerStopped)
        {
            player.position = stopPosition;
        }
        // 如果重启的UI显示，并且玩家按下了F键，则重新加载当前场景
        if (restart.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ReloadCurrentScene();
        }
    }
    void ReloadCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex; // 获取当前场景的索引
        SceneManager.LoadScene(sceneIndex); // 根据索引重新加载场景
    }
}

