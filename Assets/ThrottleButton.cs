using UnityEngine;
using UnityEngine.EventSystems;

public class ThrottleButton : MonoBehaviour
{
    public bool isPressed;

    public float dampenPress = 0;

    public float sensitivity = 2f; 
    // Start is called before the first frame update
    void Start()
    {
        SetUpButton();
    }

    // Update is called once per frame
    void Update()
    {
        /*

        if(isPressed){
            dampenPress = Mathf.Lerp(dampenPress, 1, Time.deltaTime * sensitivity);
        }else{
            dampenPress = Mathf.Lerp(dampenPress, 0, Time.deltaTime * sensitivity);
        }
        
        */
        
        if(isPressed){
        dampenPress += Time.deltaTime*sensitivity;
        }else{
        dampenPress-= Time.deltaTime*sensitivity;
        }

        dampenPress = Mathf.Clamp01(dampenPress);
        
        
    }
    void SetUpButton(){
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
        pointerDown.callback.AddListener((e) => onClickDown());

        var pointerUp = new EventTrigger.Entry {eventID = EventTriggerType.PointerUp};
        pointerUp.callback.AddListener((e) => onClickUp());

        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);
    }

    public void onClickDown(){
        isPressed=true;
    }

    public void onClickUp(){
        isPressed=false;
    }
}
