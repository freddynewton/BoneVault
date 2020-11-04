using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    public MiniMapGenerator MiniMapGenerator;
    public Camera mainCam;

    private Vector3 oldMousePos;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void openMinimap(bool active)
    {
        MiniMapGenerator.miniMapCamera.transform.position = new Vector3(MiniMapGenerator.playerMiniMap.transform.position.x, MiniMapGenerator.playerMiniMap.transform.position.y, -256f);
        MiniMapGenerator.minimapTextureCanvas.SetActive(active);
    }

    private void Update()
    {
        // TODO: FIX
        // if (MiniMapGenerator.minimapTextureCanvas.activeSelf) moveMinimap();
    }
    public void moveMinimap()
    {
        Vector3 prevMousePos = new Vector3();

        if (MiniMapGenerator.minimapTextureCanvas.activeSelf)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 dir = new Vector3((prevMousePos - mainCam.ScreenToWorldPoint(Input.mousePosition)).x, (prevMousePos - mainCam.ScreenToWorldPoint(Input.mousePosition)).y, 0);

                MiniMapGenerator.miniMapCamera.transform.position = dir;
            }
            else
            {
                prevMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
}
