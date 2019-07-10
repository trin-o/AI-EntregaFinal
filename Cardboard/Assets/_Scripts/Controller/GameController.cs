using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AI;

public enum MENU_STATE { INICIO, MENU, SALIDA };

public class GameController : MonoBehaviour
{
    public static GameController GC;
    public GAME_STATE state;
    public Transform Player;

    private bool loadComponent;
    private string sceneName;

    //transition
    Animator transitionFadeAnim;

    //HUD
    Slider health;
    Text healthText;

    Vector3 healthScale;

    //AI
    public static List<Transform> VisibleAgents = new List<Transform>();



    void Awake()
    {
        if (GC == null)
        {
            GC = this;
            DontDestroyOnLoad(this.gameObject);

            //state = GAME_STATE.M_INICIO;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //state = GAME_STATE.JUGANDO; //Cambiar a INICIO una vez que se incluya algun tipo de introduccion idk
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        /* SOLO PARA DESARROLLO */
        if (!loadComponent)
        {
            sceneName = SceneManager.GetActiveScene().name;

            transitionFadeAnim = GameObject.FindGameObjectWithTag("Transition/Fade").GetComponent<Animator>();

            switch (sceneName)
            {
                case "Menu":
                    state = GAME_STATE.MENU_INICIO;
                    break;
                case "ShootingScene":
                    state = GAME_STATE.INICIO;
                    Player = GameObject.FindGameObjectWithTag("Player").transform;
                    health = GameObject.FindGameObjectWithTag("Player/Health").GetComponent<Slider>();
                    healthScale = health.transform.localScale;
                    healthText = GameObject.FindGameObjectWithTag("Player/HealthText").GetComponent<Text>();
                    break;
                case "TestScene":
                    state = GAME_STATE.INICIO;
                    health = GameObject.FindGameObjectWithTag("Player/Health").GetComponent<Slider>();
                    healthScale = health.transform.localScale;
                    healthText = GameObject.FindGameObjectWithTag("Player/HealthText").GetComponent<Text>();
                    Player = GameObject.FindGameObjectWithTag("Player").transform;
                    Player.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }

            loadComponent = true;
        }
        /* SOLO PARA DESARROLLO */

        /*STATE MACHINE*/
        switch (state)
        {
            case GAME_STATE.MENU_INICIO:
                if (!AnimationActive(transitionFadeAnim, 0, "FadeIn"))
                {
                    state = GAME_STATE.MENU;
                }
                break;
            case GAME_STATE.MENU:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ExitMenu();
                }
                break;
            case GAME_STATE.MENU_SALIDA:
                if (!AnimationActive(transitionFadeAnim, 0, "FadeOut"))
                {
                    ChangeScene("TestScene", false);
                    state = GAME_STATE.INICIO;
                }
                break;
            case GAME_STATE.INICIO:
                if (!AnimationActive(transitionFadeAnim, 0, "FadeIn"))
                {
                    state = GAME_STATE.JUGANDO;
                }
                break;
            case GAME_STATE.JUGANDO:
                break;
            case GAME_STATE.PAUSA:
                break;
            case GAME_STATE.REINICIO:
                if (!AnimationActive(transitionFadeAnim, 0, "FadeOut"))
                {
                    Debug.Log("entro");
                    state = GAME_STATE.INICIO;
                    ChangeScene("TestScene", false);
                }
                break;
            case GAME_STATE.FIN:
                // if (Input.GetKeyDown(KeyCode.Space))
                // {
                // }
                transitionFadeAnim.SetTrigger("out");
                state = GAME_STATE.REINICIO;
                break;
            default:
                break;
        }
    }

    public void ExitMenu()
    {
        state = GAME_STATE.MENU_SALIDA;
        transitionFadeAnim.SetTrigger("out");
    }

    void ChangeScene(string sceneName, bool load)
    {
        SceneManager.LoadScene(sceneName);
        loadComponent = load;
    }

    bool AnimationActive(Animator anim, int layer, string name)
    {
        return anim.GetCurrentAnimatorStateInfo(layer).IsName(name);
    }

    public void UpdateHealthBar(float value)
    {
        health.value = value;
        healthText.text = health.value + " / 100";

        health.transform.localScale = healthScale * 1.1f;
        StartCoroutine(ResetScaleHealth());

    }

    IEnumerator ResetScaleHealth()
    {
        health.transform.localScale = healthScale;
        yield return new WaitForSeconds(0.25f);
    }

    // IEnumerator ResetGame()
    // {
    //     yield return new WaitForSeconds(1.5f);
        
    // }


    public void EndGame()
    {
        state = GAME_STATE.FIN;
    }
}

public enum GAME_STATE { /*MENU*/MENU_INICIO, MENU, MENU_SALIDA, /*GAME*/ INICIO, JUGANDO, PAUSA, REINICIO, FIN }
