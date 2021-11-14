using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeTimer : MonoBehaviour
{
    public Image imgGaze;
    public float totalTime = 2;
    bool gvrStatus;
    float gvrTimer;

    public int distanceOfRay = 10;
    private RaycastHit _hit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //when you gaze at the teleport cube then it will change the radial amount of the image
        if (gvrStatus)
        {
            gvrTimer += Time.deltaTime;
            imgGaze.fillAmount = gvrTimer / totalTime;
        }
       
        
         Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out _hit, distanceOfRay))
        {
            //to teleport
            if (imgGaze.fillAmount == 1 && _hit.transform.CompareTag("Teleport"))
            {
                _hit.transform.gameObject.GetComponent<Teleport>().TeleportPlayer();
            }

            //rotate cube with gaze
            if (imgGaze.fillAmount == 1 && _hit.transform.CompareTag("RotateCube") && gvrStatus)
            {
                _hit.transform.gameObject.GetComponent<RotateCube>().ChangeSpin();
                gvrStatus = false;
            }
        }

    } 

    public void GVROn()
    {
        gvrStatus = true;
    }
    public void GVROff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgGaze.fillAmount = 0;
    }
}
