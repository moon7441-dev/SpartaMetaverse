using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class StackGame : MonoBehaviour
{
    public string scoreKey = "Stack";
    public Text scoreText;
    public GameObject gameOverUI;
    public StackBlocks baseBlock;
    public StackBlocks movingPrefab;

    StackBlocks current;
    int score;
    bool over;
    int dir = 1;
    float speed = 3f;

    private void Start()
    {
        score = 0;
        over = false;
        SpawnNext();
        UpdateUI();
        if (gameOverUI) gameOverUI.SetActive(false);
    }

    private void Update()
    {
        if(over || current == null) return;
        current.transform.position += Vector3.right * dir * speed * Time.deltaTime;
        if (current.transform.position.x > 3f) dir = -1;
        if (current.transform.position.x < -3f) dir = -1;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Place();
        }
    }

    void Place()
    {
        //오버랩 계산 단순화
        float dx = current.transform.position.x - baseBlock.transform.position.x;
        float overlap = Mathf.Clamp01(1f-Mathf.Abs(dx) / current.width);
        if (overlap <= 0.05f)
        {
            GameOver();
            return;
        }
        score++;
        UpdateUI();

        float newWidth = current.width * overlap;
        var next = Instantiate(movingPrefab, new Vector3(-3f, current.transform.position.y + 0.5f, 0), Quaternion.identity);
        next.SetWidth(newWidth);
        baseBlock = current; // 방금 놓은것이 기준이됨
        current = next;
    }

    void SpawnNext()
    {
        current = Instantiate(movingPrefab, new Vector3(-3f, baseBlock.transform.position.y + 0.5f, 0), Quaternion.identity) ;
        current.SetWidth(baseBlock.width);
    }

    void UpdateUI()
    {
        if(scoreText) scoreText.text = score.ToString();
    }
    void GameOver()
    {
        over = true;
        ScoreManager.submitScore(scoreKey, score);
        if(gameOverUI) gameOverUI.SetActive(true);
    }

    public void OnClick_BackToHub()
    {
        SceneLoader.LoadHub("HubScene");
    }
}


