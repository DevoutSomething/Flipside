
using TMPro;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    [Header("WayPoints")]
    public MapPoint up;
    public MapPoint right, down, left;
    [Header("Scene Options")]
    [SerializeField] int Levelindext = 0;
    [HideInInspector] public string sceneToLoad;
    [TextArea(1, 2)]
    public string LevelName;
    [Header("MapPointOp")]
    [HideInInspector] public bool isLocked;
    public bool isLevel;
    public bool isCorner;
    public bool isWarp;
    [Header("Warp Options")]
    public bool autoWarp;
    [HideInInspector] public bool hasWarped; 
    public MapPoint warpPoint;
    [Header("Image Options")]
    [SerializeField] Sprite unlockedSprite = null;
    [SerializeField] Sprite lockedSprite = null;
    [Header("Level UI Objects")]
    [SerializeField] TextMeshProUGUI levelText = null;
    [SerializeField] GameObject levelPanel = null;

    SpriteRenderer spriteRenderer;

     void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
        if (levelPanel != null)
        {
            levelPanel.SetActive(false);
        }

        if (!isLevel && !isWarp)
        {
            if (isLocked && lockedSprite != null)
            {
                spriteRenderer.sprite = lockedSprite;
            }
               
            else
            {
                  spriteRenderer.sprite = null;
            } 
        }

        else
        {
            if (isLevel)
            {
                sceneToLoad = DataManager.instance.gameData.lockedLevels[Levelindext].sceneToLoad;
                isLocked = DataManager.instance.gameData.lockedLevels[Levelindext].isLocked;
            }
            if (isLocked)
            {
                if(spriteRenderer.sprite != null)
                {
                    spriteRenderer.sprite = lockedSprite;
                }
                else
                {
                    if(spriteRenderer.sprite != null)
                    {
                        spriteRenderer.sprite = unlockedSprite;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag  == "Player")
        {
            if (isLocked)
            {
                if(levelPanel != null)
                {
                    levelPanel.SetActive(true);
                }
                if (levelText != null)
                {
                    levelText.text = "Level Locked";
                }
            }
            else
            {
                if (levelPanel != null)
                {
                    levelPanel.SetActive(true);
                }
                if(levelText != null)
                {
                    levelText.text = LevelName;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (levelPanel != null)
            {
                levelPanel.SetActive(false);
            }
            if (levelText != null)
            {
                levelText.text = "";
            }
        }
    }

























}

