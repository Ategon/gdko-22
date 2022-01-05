using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //input values
    private Action jump = new Action();
    private Action passive = new Action();
    private Action attack = new Action();
    private Action aggressive = new Action();
    private Action taunt = new Action();
    private Action pause = new Action();
    private Action swapItem = new Action();
    private Action useItem = new Action();
    private Action dodge = new Action();
    private Action toss = new Action();
    private Action swapWeapon = new Action();
    private Vector2 movement = new Vector2();

    //properties
    public Action Jump { get { return jump; } private set {; } }
    public Action Passive { get { return passive; } private set {; } }
    public Action Attack { get { return attack; } private set {; } }
    public Action Aggressive { get { return aggressive; } private set {; } }
    public Action Taunt { get { return taunt; } private set {; } }
    public Action Pause { get { return pause; } private set {; } }
    public Action SwapItem { get { return swapItem; } private set {; } }
    public Action Dodge { get { return dodge; } private set {; } }
    public Action Toss { get { return toss; } private set {; } }
    public Action SwapWeapon { get { return swapWeapon; } private set {; } }
    public Vector2 Movement { get { return movement; } private set {; } }

    //references
    private PlayerInput playerInput;
    [SerializeField] GameObject playerCharacter;

    //variables
    [SerializeField] private float tapWindow;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if (!playerInput) return;

        gameObject.name = $"Player {playerInput.user.id} Controller";
        playerInput.onActionTriggered += ReadAction;

        GameObject pc = Instantiate(playerCharacter, new Vector3(0, 0, 0), Quaternion.identity);
        pc.GetComponent<PlayerMovement>().playerInput = this;
        pc.name = $"Player {playerInput.user.id} Character";
    }

    void FixedUpdate()
    {
        if (attack.timer > 0) attack.timer -= Time.deltaTime;
        if (aggressive.timer > 0) aggressive.timer -= Time.deltaTime;
        if (passive.timer > 0) passive.timer -= Time.deltaTime;
        if (swapItem.timer > 0) swapItem.timer -= Time.deltaTime;
        if (useItem.timer > 0) useItem.timer -= Time.deltaTime;
        if (swapWeapon.timer > 0) swapWeapon.timer -= Time.deltaTime;
        if (toss.timer > 0) toss.timer -= Time.deltaTime;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= ReadAction;
    }

    private void OnDestroy()
    {
        playerInput.onActionTriggered -= ReadAction;
    }

    public void ReadAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Jump":
                ReadBool(context, jump);
                break;
            case "Passive":
                ReadAction(context, passive);
                break;
            case "Attack":
                ReadAction(context, attack);
                break;
            case "Aggressive":
                ReadAction(context, aggressive);
                break;
            case "Taunt":
                ReadBool(context, taunt);
                break;
            case "Pause":
                ReadBool(context, pause);
                break;
            case "Swap Item":
                ReadAction(context, swapItem);
                break;
            case "Use Item":
                ReadAction(context, useItem);
                break;
            case "Dodge":
                ReadBool(context, dodge);
                break;
            case "Toss":
                ReadAction(context, toss);
                break;
            case "Swap Weapon":
                ReadAction(context, swapWeapon);
                break;
            case "Movement":
                ReadMovement(context);
                break;
            default:
                break;
        }
    }

    public void ReadAction(InputAction.CallbackContext context, Action action)
    {
        if (context.performed)
        {
            action.timer = tapWindow;
            action.state = ActionState.Holding;
        }
        else if (context.canceled)
        {
            if (action.timer <= 0)
            {
                action.state = ActionState.Held;
            }
            else
            {
                action.state = ActionState.Tapped;
            }
        }
    }

    public void ReadBool(InputAction.CallbackContext context, Action action)
    {
        if (context.performed)
        {
            action.state = ActionState.Tapped;
        }
    }

    public void ReadMovement(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            movement = context.ReadValue<Vector2>();
        }
    }

    [System.Serializable]
    public class Action
    {
        public ActionState state = ActionState.None;
        public float timer = 0;
    }

    public enum ActionState
    {
        None,
        Holding,
        Tapped,
        Held
    }
}