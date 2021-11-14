using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SetFurnitureScript : NetworkBehaviour
{ 
    [Header("Settings")]
    [SerializeField] private bool currentlyPlacing;                            //Status of placing
    [SerializeField] private float lastUpdateTime;
    [SerializeField] private float placementIndicatorUpdateRate = 0.05f;
    [SerializeField] public GameObject placementIndicator;                   //Point to placement indicator object
    //[SerializeField] private GameObject furnitureObj;                         //Store selected furniture object
    [SerializeField] private Vector3 curPlacementPos;                         //Store an instance of current placement position
    [SerializeField] private int curFurnitureChosen;

    [Header("References")]
    [SerializeField] public static SetFurnitureScript inst;                     //Instantiate FurniturePlacer
    [SerializeField] private UIBedGrey bedStateChanger;
    [SerializeField] private UIChairGrey chairStateChanger;
    [SerializeField] private UITableGrey tableStateChanger;
    [SerializeField] private ModelManager updateChanger;
    [SerializeField] private int i = 1;

    [Header("Furniture")]
    [SerializeField] private int selectedFurnitureLocal = 0;
    [SerializeField] public GameObject[] furnitureArray;
    [SerializeField] private Furniture activeFurniture;

    [SyncVar(hook = nameof(OnFurnitureChanged))]
    public int activeFurnitureSynced = 1;

    //Sync to all players the current furniture to create
    void OnFurnitureChanged(int _Old, int _New)
    {
        // disable old furniture
        // in range and not null
        if (0 < _Old && _Old < furnitureArray.Length && furnitureArray[_Old] != null)
            furnitureArray[_Old].SetActive(false);

        // enable new furniture
        // in range and not null
        if (0 < _New && _New < furnitureArray.Length && furnitureArray[_New] != null)
        {
            furnitureArray[_New].SetActive(true);
            activeFurniture = furnitureArray[activeFurnitureSynced].GetComponent<Furniture>();
        }
    }

    public override void OnStartLocalPlayer()
    {
        bedStateChanger.fpInst = this;
        chairStateChanger.fpInst = this;
        tableStateChanger.fpInst = this;
        updateChanger.playerInst = this;
    }

    #region Server
    [Command]
    public void CmdChangeActiveFurniture(int newIndex)
    {
        activeFurnitureSynced = newIndex;
    }

    [Command]
    /*Called when user clicks on grid to place furniture down*/
    void CmdPlaceFurniture3D(Vector3 placementPos)
    {
        //Send Rpc Command
        Debug.Log("Sending Command");
        RpcPlaceFurniture(placementPos);
    }

    [ClientRpc]
    void RpcPlaceFurniture(Vector3 placementPos)
    {
        Debug.Log("Command RPC");
        GameObject furniture3D = Instantiate(activeFurniture.furnitureObj, placementPos, activeFurniture.rotationRef.rotation);
        furniture3D.gameObject.name += "(" + i.ToString() + ")";
        furniture3D.tag = "Drag";
        var mats = furniture3D.GetComponent<MeshRenderer>().materials;
        furniture3D.GetComponent<colorPicker>().newmat = mats[1];
        Debug.Log("Success Sync");
        i++;
    }
    
    [Command]
    void CmdUpdateScales(string instanceName, float xScale, float yScale, float zScale)
    {
        Debug.Log("Changing Scales");
        RpcUpdateScales(instanceName, xScale, yScale, zScale);
    }

    [ClientRpc]
    void RpcUpdateScales(string instanceName, float xScale, float yScale, float zScale)
    {
        Debug.Log("Command UpdateScales");
        GameObject gameObjectInst = GameObject.Find(instanceName);
        gameObjectInst.transform.localScale = new Vector3(xScale, yScale, zScale);
        Debug.Log("Success Sync");
    }

    [Command]
    void CmdUpdateColor(string instanceName, float r, float g, float b, float a)
    {
        Debug.Log("Changing Color");
        RpcUpdateColor(instanceName, r, g, b, a);
    }

    [ClientRpc]
    void RpcUpdateColor(string instanceName, float r, float g, float b, float a)
    {
        Debug.Log("Command UpdateColor");
        GameObject gameObjectInst = GameObject.Find(instanceName);
        gameObjectInst.GetComponent<colorPicker>().newmat.color = new Color(r, g, b, a);
        Debug.Log("Success Sync");
    }

    [Command]
    void CmdUpdatePosition(string instanceName, float xPos, float yPos, float zPos)
    {
        Debug.Log("Changing Position");
        RpcUpdatePosition(instanceName, xPos, yPos, zPos);
    }

    [ClientRpc]
    void RpcUpdatePosition(string instanceName, float xPos, float yPos, float zPos)
    {
        Debug.Log("Command UpdatePosition");
        GameObject gameObjectInst = GameObject.Find(instanceName);
        gameObjectInst.transform.position = new Vector3(xPos, yPos, zPos);
        Debug.Log("Success Sync");
    }

    [Command]
    void CmdUpdateRotation(string instanceName, float xRot, float yRot, float zRot)
    {
        Debug.Log("Changing Rotation");
        RpcUpdateRotation(instanceName, xRot, yRot, zRot);
    }

    [ClientRpc]
    void RpcUpdateRotation(string instanceName, float xRot, float yRot, float zRot)
    {
        Debug.Log("Command UpdateRotation");
        GameObject gameObjectInst = GameObject.Find(instanceName);
        gameObjectInst.transform.eulerAngles = new Vector3(xRot, yRot, zRot);
        Debug.Log("Success Sync");
    }

    [Command]
    void CmdDeleteObject(string instanceName)
    {
        Debug.Log("Changing Scales");
        RpcDeleteObject(instanceName);
    }

    [ClientRpc]
    void RpcDeleteObject(string instanceName)
    {
        Debug.Log("Command DeleteObject");
        GameObject gameObjectInst = GameObject.Find(instanceName);
        updateChanger.deleteFloatControl();
        Destroy(gameObjectInst);
        Debug.Log("Success Sync");
    }

    void CmdFlipObject(string instanceName)
    {
        Debug.Log("Changing Scales");
        RpcFlipObject(instanceName);
    }

    [ClientRpc]
    void RpcFlipObject(string instanceName)
    {
        Debug.Log("Command FlipObject");
        GameObject gameObjectInst = GameObject.Find(instanceName);
        gameObjectInst.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        Debug.Log("Success Sync");
    }

    #endregion

    #region Client

    /*Instiate FurniturePlacer. Start at Program Start*/
    void Awake()
    {
        //inst = this;

        // disable all furnitures
        foreach (var item in furnitureArray)
            if (item != null)
                item.SetActive(false);

        //allows all players to run this
        bedStateChanger = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScriptBed;
        tableStateChanger = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScriptTable;
        chairStateChanger = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScriptChair;
        updateChanger = GameObject.Find("SceneReference").GetComponent<SceneReference>().modelManagerScript;

    }

    private void Update()
    {
        if (!isLocalPlayer) { return; }

        //Capture esc key sequence
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelFurniturePlacement();

        //If we are placing a furniture down, make it follow the cursor via the placement indicator object
        if (Time.time - lastUpdateTime > placementIndicatorUpdateRate && currentlyPlacing)
        { 
            lastUpdateTime = Time.time;
            curPlacementPos = Selector.inst.GetCurTilePosition();
            placementIndicator.transform.position = curPlacementPos;
        }

        //When user press left mouse button – place it down.
        if (currentlyPlacing && Input.GetMouseButtonDown(0))
        {
            //PlaceFurniture3D();

            CmdPlaceFurniture3D(curPlacementPos);
            Debug.Log("Sent command to server");
            CancelFurniturePlacement();
        }
    }

    /*Function called when clicking on the UI*/
    public void BeginNewFurniturePlacement(int furniturePreset)
    {
        //Activate Placement Indicator Controls
        currentlyPlacing = true;
        //Debug.Log("currentlyPlacing = true");
        selectedFurnitureLocal = furniturePreset + 1;       //Store an instance of the Furniture Preset 
        CmdChangeActiveFurniture(selectedFurnitureLocal);
        placementIndicator.SetActive(true);         //Activate PlacementIndicator Object
        //Debug.Log("Begin placement");
    }

    /*Cancel Furniture Placement when escape is pressed*/
    public void CancelFurniturePlacement()
    {
        currentlyPlacing = false;
        Debug.Log("currentlyPlacing = false");
        placementIndicator.SetActive(false);

        if (selectedFurnitureLocal == 2)        //2 is for bed furniture
        {
            bedStateChanger.FinishPlacement();
            Debug.Log("Cancel placement");
        }
        else if (selectedFurnitureLocal == 6)   //6 is for table furniture
        {
            tableStateChanger.FinishPlacement();
            Debug.Log("Cancel Placement");
        }
        else if (selectedFurnitureLocal == 4)   //4 is for chair furniture
        {
            chairStateChanger.FinishPlacement();
            Debug.Log("Cancel placement");
        }
        
    }

    public void SendUpdates(string selectedModelName, float x, float y, float z, bool isRefSelectedModel)
    {
        if (isRefSelectedModel == true)
        {
            CmdUpdateScales(selectedModelName, x, y, z);
        }
        Debug.Log("Update Sent: " + isRefSelectedModel.ToString());
    }

    public void SendPosUpdates(string selectedModelName, float xPos, float yPos, float zPos)
    {
        CmdUpdatePosition(selectedModelName, xPos, yPos, zPos);
        Debug.Log("Update Sent: Position ");
    }

    public void SendRotUpdates(string selectedModelName, float xRot, float yRot, float zRot)
    {
        CmdUpdateRotation(selectedModelName, xRot, yRot, zRot);
        Debug.Log("Update Sent: Rot ");
    }

    public void DeleteObject(string selectedModelName)
    {
        CmdDeleteObject(selectedModelName);
    }

    public void FlipObject(string selectedModelName)
    {
        CmdFlipObject(selectedModelName);
    }

    public void UpdateColor(string instanceName, float r, float g, float b, float a)
    {
        Debug.Log("Changing Color");
        CmdUpdateColor(instanceName, r, g, b, a);
    }

    #endregion
}
