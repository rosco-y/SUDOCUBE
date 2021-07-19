using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum eLayer
{
    Front2Back,
    Top2Bottom,
    Right2Left
}

/// <summary>
/// ShowHideLayer is attached to the GameManager, and shows / hides layers as needed,
/// keeping the camera at a constant distance from the nearest visible layer.
/// </summary>
public class ShowHideLayer : MonoBehaviour
{
    // Start is called before the first frame update
    bool[] HiddenLayers = new bool[g.PSIZE];
    [SerializeField] float _centerAdjustSpeed = 9;
    [SerializeField] GameObject _sudoCenter;
    //[SerializeField] LayerLabels lyrLabels;
    public struct stCenterLocations
    {
        public Vector3 centerLocation { get; set; }
        public bool isSet { get; set; }
    };
    stCenterLocations[] CenterLocations;
    GameObject _centerPositionGameObject;
    [SerializeField] LayerLabels _layerLabels;

    // Update is called once per frame
    private void Awake()
    {
    }
    private void Start()
    {
        _centerPositionGameObject = new GameObject(); // one time instead of repeatedly
        CenterLocations = new stCenterLocations[g.PSIZE];
        saveCenterPosition(0);
    }

    private void saveCenterPosition(int layer)
    {
        try
        {
            if (layer < 0 || layer > g.PSIZE - 1)
                return;
            CenterLocations[layer].centerLocation = _sudoCenter.transform.position;
            CenterLocations[layer].isSet = true;
        }
        catch (Exception x)
        {
            throw x;
        }

    }

    void restoreCenterPosition(int layer)
    {
        if (_centerTargetPosition != null && CenterLocations[layer].isSet)
        {
            if (g.Instance.CurrentLayer == 0)
                _centerTargetPosition.transform.position = CenterLocations[layer].centerLocation;
        }

    }


    public bool ShowLayer(int layerNo)
    {
        saveCenterPosition(layerNo);
        bool layerShown = false;
        if (layerNo < 0 || layerNo > g.PSIZE - 1)
            return layerShown;
        if (g.Instance.CurrentLayer < 0 || g.Instance.CurrentLayer > g.PSIZE - 1)
            return layerShown;
        HiddenLayers[layerNo] = false;
        g.Instance.CurrentLayer--;
        layerNo = normalizedLayerNo(layerNo);
        SetVisible(layerNo, true);
        layerShown = true;
        return layerShown;

    }


    void showAllLayers()
    {
        // current layer is already visible
        for (int layer = 0; layer < g.PSIZE; layer++)
        {
            ShowLayer(layer);
        }
        g.Instance.CurrentLayer = 0;
        restoreCenterPosition(0);
    }

    float getLayerDistance()
    {
        eLayer layerType = getCurrentLayerType();
        Dictionary<int, List<SudoCube>> layer;
        layer = getLayer(layerType);
        int startIndex = normalizedLayerNo(0);
        bool ascending = false;
        float distance = float.MinValue;
        if (startIndex == 0)
        {
            ascending = true;
        }
        if (ascending)
        {
            for (int ilayer = 0; ilayer < g.PSIZE; ilayer++)
            {
                distance = findCenterCellDistance(layer, ilayer);
                if (distance != float.MinValue)
                    break;
            }
        }
        else
        {
            for (int ilayer = g.PSIZE - 1; ilayer >= 0; ilayer--)
            {
                distance = findCenterCellDistance(layer, ilayer);
                if (distance != float.MinValue)
                    break;
            }
        }
        return distance;
    }

    private float findCenterCellDistance(
        Dictionary<int, List<SudoCube>> layer, int ilayer)
    {
        float distance = float.MinValue;
        for (int iCell = 0; iCell < (layer.Count * g.PSIZE) - 1; iCell++)
        {

            SudoCube cell = (SudoCube)layer[ilayer][iCell];

            int row, col;
            if (cell.isActiveAndEnabled)
            {
                row = iCell % g.PSIZE;
                col = iCell / g.PSIZE;
                if (row == 4 && col == 4)
                {
                    distance =
                        Vector3.Distance(_sudoCenter.transform.position,
                                         cell.transform.position);
                    break;
                }
            }
            if (distance != float.MinValue)
                break;
        }
        return distance;
    }

    public void SetVisible(int layerNo, bool visible)
    {

        eLayer layerType = getCurrentLayerType();
        Dictionary<int, List<SudoCube>> layer;
        layer = getLayer(layerType);

        int cellCount = 0;
        foreach (SudoCube cell in layer[layerNo])
        {
            cell.gameObject.SetActive(visible);
            cellCount++;
        }
    }

    private Dictionary<int, List<SudoCube>> getLayer(eLayer layertype)
    {
        // ensure that dictionaries are not null (don't know why it happens.)
        if (g.Instance.FRONT2BACK_LAYERS == null || g.Instance.LEFT2RIGHT_LAYERS == null || g.Instance.TOP2BOTTOM_LAYERS == null)
            g.Instance.CreateLayers();
        Dictionary<int, List<SudoCube>> layer = null;
        if (layertype == eLayer.Front2Back)
            layer = g.Instance.FRONT2BACK_LAYERS;
        else if (layertype == eLayer.Right2Left)
            layer = g.Instance.LEFT2RIGHT_LAYERS;
        else
            layer = g.Instance.TOP2BOTTOM_LAYERS;
        return layer;
    }


    /// <summary>
    /// Reverse layerNo's when viewing the reverse sides of the SudoKube.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns>Normalized layer number.</returns>
    int normalizedLayerNo(int layer)
    {

        switch (g.Instance.CurrentSide.GetCurFace())
        {
            case cCur.eCurFace.BACK:
            case cCur.eCurFace.RIGHT:
            case cCur.eCurFace.TOP:
                layer = (g.PSIZE - 1) - layer;
                break;
            default:
                break;
        }
        return layer;
    }

    /// <summary>
    /// Checks the currently facing side of the cube, and returns the
    /// corresponding eLayer value.
    /// </summary>
    /// <returns></returns>
    /// 
    private eLayer getCurrentLayerType()
    {
        eLayer layerType;
        switch (g.Instance.CurrentSide.EFACE)
        {
            case eFace.FRONT2BACK:
                layerType = eLayer.Front2Back;
                break;
            case eFace.LEFT2RIGHT:
                layerType = eLayer.Right2Left;
                break;
            default:
                layerType = eLayer.Top2Bottom;
                break;
        }
        return layerType;
    }


    GameObject _centerTargetPosition;
    //GameObject _beginCameraPosition;
    bool _adjustCamera = false; // prevent FixedUpdate from attempting to make invalid adjustments
    float _newCameraDistance;
    float _beginCameraDistance;
    bool _fshowThisLayer = false;
    bool _invertDistanceAdjustment = false;
    void Update()
    {
        bool hideLayer = false;


        /* test for rotation.
         * if rotation then show all layers first.
         */
        if (g.Instance.CurrentLayer > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow))
            {
                showAllLayers();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (g.Instance.CurrentLayer < g.PSIZE - 1)
                showThisLayer(g.Instance.CurrentLayer + 2);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (g.Instance.CurrentLayer > 0)
                showThisLayer(g.Instance.CurrentLayer);
        }
        if (hideLayer || _invertDistanceAdjustment)
        {
            _newCameraDistance = getDistanceFromCenterToActiveLayer(); // STEP 3: RETRIEVE ADJUSTED DISTANCE.
            float distanceAdjustment = Mathf.Abs(_beginCameraDistance - _newCameraDistance);
            if (_invertDistanceAdjustment)
                distanceAdjustment = -distanceAdjustment;

            _centerTargetPosition = centerPosition();
            if (g.Instance.CurrentLayer == 0)
                _centerTargetPosition.transform.position = g.Instance.InitialCameraPosition;
            else
                _centerTargetPosition.transform.position = V3CameraPositionAdjustment(distanceAdjustment);
            _adjustCamera = true;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            //_invertDistanceAdjustment = true; // 1 can only be showing a layer.
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            //_fshowThisLayer = true;
            showThisLayer(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            // CurrentLayer is zero-based, so keep that in mind when setting "showLayer"
            //_invertDistanceAdjustment = g.Instance.CurrentLayer < 1;
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            showThisLayer(2);
            //_fshowThisLayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            //_invertDistanceAdjustment = g.Instance.CurrentLayer < 2;
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            //_fshowThisLayer = true;
            showThisLayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            //_invertDistanceAdjustment = g.Instance.CurrentLayer < 3;
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            showThisLayer(4);
            //_fshowThisLayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            //_invertDistanceAdjustment = g.Instance.CurrentLayer < 4;
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            showThisLayer(5);
            _fshowThisLayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            //_invertDistanceAdjustment = g.Instance.CurrentLayer < 5;
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            showThisLayer(6);
            //_fshowThisLayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            //_invertDistanceAdjustment = g.Instance.CurrentLayer < 6;
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            showThisLayer(7);
            //_fshowThisLayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            //_invertDistanceAdjustment = g.Instance.CurrentLayer < 7;
            //_beginCameraDistance = getDistanceFromCenterToActiveLayer();
            showThisLayer(8);
            //_fshowThisLayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            showThisLayer(9);
        }

        ////////////////////////////////////////////////////////////////
        /// '*', '0' and ENTER KEYS
        ////////////////////////////////////////////////////////////////

        // * = set/unset home layer
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            if (g.Instance.HomeLayer == g.Instance.CurrentLayer)
                g.Instance.HomeLayer = -1;
            else
                g.Instance.HomeLayer = g.Instance.CurrentLayer;
            _layerLabels.setLayerLabels();
        }
        // '0' = GOTO Home Layer
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            showThisLayer(g.Instance.HomeLayer + 1);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (g.Instance.HomeLayer > -1)
            {
                int layerToShow = navigateHomeLayers();
                showThisLayer(layerToShow);
            }
            else
            {
                g.Instance.HomeLayer = g.Instance.CurrentLayer;
                _layerLabels.setLayerLabels();
            }
        }

        // IMPORTANT, adjustCameraDistance has to take place after the 
        // layer visibility adjustments are made!
        // (i.e., in reponse to Input.GetKeyDown() tests.)
        adjustCameraDistance(_invertDistanceAdjustment, _fshowThisLayer);

    }
    private void adjustCameraDistance(bool showLayer, bool fshowThisLayer)
    {
        if (fshowThisLayer)
        {
            _newCameraDistance = getDistanceFromCenterToActiveLayer(); // STEP 3: RETRIEVE ADJUSTED DISTANCE.
            float distanceAdjustment = Mathf.Abs(_beginCameraDistance - _newCameraDistance);
            if (!showLayer)
                distanceAdjustment = -distanceAdjustment;

            _centerTargetPosition = centerPosition();
            _centerTargetPosition.transform.position = V3CameraPositionAdjustment(distanceAdjustment);
            _adjustCamera = true;
        }
    }

    private int navigateHomeLayers()
    {
        int curLayer = g.Instance.CurrentLayer;
        int homeLayer = g.Instance.HomeLayer;
        int minLayer, maxLayer;
        getLayerMinMax(homeLayer, out minLayer, out maxLayer);
        if (curLayer <= maxLayer && curLayer >= minLayer)
        {
            if (curLayer + 1 <= maxLayer)
                return curLayer + 2;
            else
                return minLayer + 1;
        }
        else
            return homeLayer + 1; // out of bounds, return home.
    }

    private void getLayerMinMax(int homeLayer, out int minLayer, out int maxLayer)
    {
        switch (homeLayer)
        {
            case 0:
            case 1:
            case 2:
                minLayer = 0; maxLayer = 2;
                break;
            case 3:
            case 4:
            case 5:
                minLayer = 3; maxLayer = 5;
                break;
            case 6:
            case 7:
            case 8:
                minLayer = 6; maxLayer = 8;
                break;
            default:
                minLayer = -1; maxLayer = g.PSIZE + 1; // illegal values
                break;
        }
        return;
    }

    private void showThisLayer(int layer)
    {
        _invertDistanceAdjustment = g.Instance.CurrentLayer < (layer - 1);
        _beginCameraDistance = getDistanceFromCenterToActiveLayer();
        layer--;

        int i = g.Instance.CurrentLayer;
        if (g.Instance.CurrentLayer < layer)
        {
            // hide layers until layer is currentlayer

            while (g.Instance.CurrentLayer < layer)
            {
                HideLayer(i++);

            }
        }
        else
        {
            while (g.Instance.CurrentLayer > layer)
            {
                ShowLayer(--i); // infinate loop if ShowLayer fails to decrement CurrentLayer.
            }

        }
        restoreCenterPosition(g.Instance.CurrentLayer);
        _fshowThisLayer = true;
    }

    /// <summary>
    /// Hide (deActivate) the specified layer
    /// </summary>
    /// <param name="layerNo"></param>
    /// <returns></returns>
    public bool HideLayer(int layerNo)
    {

        saveCenterPosition(layerNo);

        bool layerHidden = false;
        if (g.Instance.CurrentLayer < 0 || g.Instance.CurrentLayer > g.PSIZE - 2)
            return layerHidden;
        HiddenLayers[layerNo] = true;
        g.Instance.CurrentLayer++;
        layerNo = normalizedLayerNo(layerNo);
        SetVisible(layerNo, false);
        layerHidden = true;
        return layerHidden;

    }

    /// <summary>
    /// return Camera transform's postion, rotation and scale in a module-scoped GameObject.
    /// (scope of "_newGameObject" is module, so a new one doesn't need to be instantiated
    /// every time centerPosition() is called--proliferating memory with used-once GameObjects.)
    /// </summary>
    /// <returns></returns>
    GameObject centerPosition()
    {
        Vector3 position = _sudoCenter.transform.position;

        _centerPositionGameObject.transform.position = position;

        Quaternion rotation = _sudoCenter.transform.rotation;

        _centerPositionGameObject.transform.rotation = rotation;
        // scale defaults
        _centerPositionGameObject.transform.localScale = Vector3.one;

        return _centerPositionGameObject;
    }

    /// <summary>
    /// Get the nearest distance to an Active SudoCube, in g.Instance.SudoCubes[][][]
    /// </summary>
    /// <returns></returns>
    float getDistanceFromCenterToActiveLayer()
    {
        float foundDistance = getLayerDistance();

        return foundDistance;
    }


    /// <summary>
    /// calculate adjusment needed so FixedUpdate can move the camera to it's correct distance.
    /// </summary>
    Vector3 V3CameraPositionAdjustment(float distanceAdjustment)
    {
        // get axis to adjust camera along
        string cameraAdjustmentAxis = g.Instance.CurrentSide.CameraAdjustmentAxis();
        //print($"cameraAdjustmentAxis = {cameraAdjustmentAxis}");
        Vector3 v3cameraPos = centerPosition().transform.position;

        bool negative = false;
        char axis;

        if (cameraAdjustmentAxis.StartsWith("-"))
        {
            negative = true;
            axis = cameraAdjustmentAxis[1];
        }
        else
            axis = cameraAdjustmentAxis[0];

        switch (axis)
        {
            case 'X':
                if (negative)
                    v3cameraPos.x -= distanceAdjustment;
                else
                    v3cameraPos.x += distanceAdjustment;

                break;
            case 'Y':
                if (negative)
                    v3cameraPos.y -= distanceAdjustment;
                else
                    v3cameraPos.y += distanceAdjustment;
                break;

            default: // 'Z'
                if (negative)
                    v3cameraPos.z -= distanceAdjustment;
                else
                    v3cameraPos.z += distanceAdjustment;
                break;
        }
        return v3cameraPos;
    }
    private void FixedUpdate()
    {
        if (_adjustCamera && _centerTargetPosition != null)
        {
            // ensure that sudoCenter keeps it's center position.
            if (g.Instance.CurrentLayer == 0)
                _centerTargetPosition.transform.position = new Vector3(0.42f, 0.37f, 0.43f);

            _sudoCenter.transform.position = Vector3.Lerp(_sudoCenter.transform.position, _centerTargetPosition.transform.position, Time.deltaTime * _centerAdjustSpeed);
            _sudoCenter.transform.parent = _sudoCenter.transform;
        }
    }
}