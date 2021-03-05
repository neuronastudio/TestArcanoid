using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject boxPrefab;
    public GameObject button;

    private bool isInit = false;
    private Vector3 speedBall = new Vector3(0.05f, 0, 0);
    private short GAME_WIDTH_HALF = 640;
    private bool isMove = false;
    private string directionMove;
    private GameObject ball;
    private float timeUpdateSpeed = 5f;
    private float timeCreateBox = 1f;
    private float gravity = 1f;
    private bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        ball = Instantiate(ballPrefab, new Vector3(0, -3.8f, 0), Quaternion.identity, gameObject.transform);
        ball.GetComponent<Ball>().Seek(this);
        isInit = true;
    }

    private void MouseDown(Vector2 _position)
    {
        if (_position[0] <= GAME_WIDTH_HALF)
        {
            directionMove = "left";
        }
        else
        {
            directionMove = "right";
        }
        isMove = true;
    }
    private void MouseUp()
    {
        isMove = false;
    }
    private void MoveBall()
    {
        if (isMove==true)
        {
            if (directionMove == "left" && ball.transform.position.x > -8.4)
            {
                ball.transform.position -= speedBall;
            }
            else if (directionMove == "right" && ball.transform.position.x < 8.4)
            {
                ball.transform.position += speedBall;
            }
        }
    }

    private void MoveBoxes()
    {
        //Коробки падают, если выходят за границу то удаляются, если сталкиваются с шаром то конец
    }

    private void CreateBox()
    {
        print("Create Box");
        //Строим коробку в рендомных координатах и она падает вниз за экран
        float randomX = Random.Range(-8f, 8f);
        GameObject box = Instantiate(boxPrefab, new Vector3(randomX, 6f, 0), Quaternion.identity, gameObject.transform);
        box.GetComponent<Rigidbody2D>().gravityScale = gravity;
    }
    public void Collision()
    {
        pause = true;
        button.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SwitchBoxesSpeed()
    {
        gravity++;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause == true) return;
        timeCreateBox -= Time.deltaTime;
        if (timeCreateBox <= 0)
        {
            timeCreateBox = 0.5f;
            CreateBox();
        }
        timeUpdateSpeed -= Time.deltaTime;
        if (timeUpdateSpeed <= 0) 
        {
            timeUpdateSpeed = 5f;
            SwitchBoxesSpeed();
        }
        MoveBall();
        MoveBoxes();
        ClickUpdate();
    }

    private void ClickUpdate()
    {
        if (!isInit) return;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
#else
        if (Input.touchCount > 0)
        {
            foreach (Touch tch in Input.touches)
            {
                if (tch.phase == TouchPhase.Began)
                {
                    MouseDown(tch.position);
                }
                if (tch.phase == TouchPhase.End)
                {
                    MouseUp();
                }
            }        
        }
#endif

    }
}
