using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnifiedInputSystem : MonoBehaviour {

    public UnitDataController Unit_Data_Controller;
    public TextDisplayMenager TDM;
    public bool UI_Button_Draged = false;
    public GameObject Draged_Obj;
    private RectTransform Draged_Obj_RectTransform;
    public Image Draged_Obj_Image;
    public List<GameObject> UI_Spawn_Slots;
    public List<Transform> UI_Slot_Position;
    private Vector3 Placeholder;
    private bool UI_Slot_LockedIN = false;
    private int SpawnLocation = 0;
    private int Unit_Id = 0;

  

	// Use this for initialization
	void Start () {
        //C = Camera.main;
        Draged_Obj_RectTransform = Draged_Obj.GetComponent<RectTransform>();
        Draged_Obj_Image = Draged_Obj.GetComponent<Image>();

        UI_Slot_Position.Add(UI_Spawn_Slots[0].GetComponent<RectTransform>().transform);
        UI_Slot_Position.Add(UI_Spawn_Slots[1].GetComponent<RectTransform>().transform);
        UI_Slot_Position.Add(UI_Spawn_Slots[2].GetComponent<RectTransform>().transform);
        Placeholder = Vector3.zero;



    }
	
	// Update is called once per frame
	void Update () {
        if (UI_Button_Draged)
        {
            UI_Slot_LockedIN = false;
            Draged_Obj_RectTransform.transform.position = Input.mousePosition + new Vector3(-Draged_Obj_RectTransform.rect.size.x/2, Draged_Obj_RectTransform.rect.size.y/2,0f);
            Placeholder = FetchClosestSlot();
            if (Placeholder.magnitude > 0f)
            {
                Draged_Obj_RectTransform.transform.position = Placeholder;
                UI_Slot_LockedIN = true;
            }
        }
        if (UI_Button_Draged && Input.GetMouseButtonUp(0))
        {
            Draged_Obj_Image.enabled = false;
            UI_Button_Draged = false;
            UI_Spawn_Slots[0].SetActive(false);
            UI_Spawn_Slots[1].SetActive(false);
            UI_Spawn_Slots[2].SetActive(false);
            if (UI_Slot_LockedIN)
            {
                Unit_Data_Controller.SpawnUnit(SpawnLocation, Unit_Id);
            }
        }
	}



    public void UI_Button_Drag(int Unit_ID)
    {
        Unit_Id = Unit_ID;
        if (Unit_Data_Controller.Check_Unit_Awailability(Unit_ID))
        {
            UI_Button_Draged = true;
            Draged_Obj_Image.sprite = Unit_Data_Controller.DragableSprites[Unit_ID];
            Draged_Obj_Image.enabled = true;
            UI_Spawn_Slots[0].SetActive(true);
            UI_Spawn_Slots[1].SetActive(true);
            UI_Spawn_Slots[2].SetActive(true);
        }
        else
        {
            //Print not enough gold
            //TDM.ToggleText(4,true);
        }

    }


    private Vector3 FetchClosestSlot()
    {
        Vector3 Temp = UI_Slot_Position[0].position;
        float Dist= 65f;
        for(int i = 0; i <=2; i++)
        {
            if (Vector3.Distance(Draged_Obj_RectTransform.transform.position, UI_Slot_Position[i].position) < Dist)
            {
                SpawnLocation = i;
                Temp = UI_Slot_Position[i].position;
                Dist = Vector3.Distance(Draged_Obj_RectTransform.transform.position, UI_Slot_Position[i].position);
            }
        }
        if (Dist < 65f)
            return Temp;
        else
            return Vector3.zero;
    }






}
