using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour 
{
    public event KillConfirmed killConfirmedEvent;

    private static GameManager instance;

    [SerializeField]
    private Player player;

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;

    private Enemy currentTarget;

    private Camera mainCamera;
    private int targetIndex;

    private bool touchLastFrame = false;

    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    public static GameManager MyInstance 
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public HashSet<Vector3Int> Blocked { get => blocked; set => blocked = value; }

    private void Start()
    {
        mainCamera = Camera.main;
    }
   
    void Update()
    {
        if (touchLastFrame && Input.touchCount == 0)
        {
            touchLastFrame = false;
        }
        else if (!touchLastFrame && Input.touchCount > 0)
        {
            ClickTarget();
            touchLastFrame = true;
        }
        
        NextTarget();
    }

    private void ClickTarget()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.touches[i].position), Vector3.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

                if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
                {
                    if (Input.touchCount <= 1)
                    {
                        entity.Interact();
                        Debug.Log("ll");
                    }


                }


            }


        }



        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("HEloooo");

            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                DeselectTarget();

                SelectTarget(hit.collider.GetComponent<Enemy>());
            }

            else
            {
                UiManager.MyInstance.HideTargetFrame();

                DeselectTarget();

                currentTarget = null;
                player.MyTarget = null;
            }
        }

        //if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        //{

        //    Touch touch = Input.GetTouch(0);
        //    RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(touch.position), Vector2.zero, Mathf.Infinity, clickableLayer);

        //    if (hit.collider != null)
        //    {
        //        IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

        //        if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
        //        {
        //            entity.Interact();
        //            //  indicateW.SetActive(true);

        //        }
        //        else
        //        {
        //            hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(touch.position), Vector2.zero, Mathf.Infinity, groundLayer);

        //            if (hit.collider != null)
        //            {
        //                player.GetPath(mainCamera.ScreenToWorldPoint(touch.position));
        //            }
        //        }

        //    }
        //}

        //    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

        //    if(hit.collider != null)
        //    {
        //        IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();
        //        if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
        //        {
        //            entity.Interact();

        //        }

        //    }
        //else
        //{
        //    hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

        //    if (hit.collider != null)
        //    {
        //        player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        //    }


        //}


        // }

        //else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
        //    IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();
        //    if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
        //    {
        //        entity.Interact();

        //    }

        //}

    }

    

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeselectTarget();

            if(Player.MyInstance.Attackers.Count > 0)
            {
                if(targetIndex < Player.MyInstance.Attackers.Count)
                {
                    SelectTarget(Player.MyInstance.Attackers[targetIndex] as Enemy);
                    targetIndex++;
                    if (targetIndex >= Player.MyInstance.Attackers.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0;
                }

               
            }
        }
    }

    private void DeselectTarget()
    {
        if(currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }

    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UiManager.MyInstance.ShowTargetFrame(currentTarget);
    }

   
}
