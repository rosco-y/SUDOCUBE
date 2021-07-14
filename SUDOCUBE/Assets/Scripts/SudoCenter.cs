using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class SudoCenter : MonoBehaviour
{
    // Start is called before the first frame update
    bool _doneRotating = true; // to begin with, no rotation is needed.
    Quaternion _newRotation;
    [SerializeField] float _rotateSpeed = 9f;
    [SerializeField] Camera _camera;
    GameObject[] CameraLocations;
    [SerializeField] TMP_Text StatusBar;

    private void FixedUpdate()
    {
        if (!_doneRotating)
        {
            //_camera.transform.position = new Vector3(0, 0, -20f);
            transform.rotation = Quaternion.Slerp(transform.rotation, _newRotation, _rotateSpeed * Time.deltaTime);
            _doneRotating = DoneRotating(transform.rotation, _newRotation);
        }
        else
            sideReport();

    }

   
    
    private void Start()
    {

        if (g.Instance.CurrentSide == null)
            g.Instance.CurrentSide = new cCur(5, 6); // front,top
        sideReport();
    }

    private void sideReport()
    {
        StatusBar.text = g.Instance.CurrentSide.ToString();
    }

    /// <summary>
    /// DoneRotating()
    /// If a new rotation is begun before the rotation is finished, then the
    /// rotations can become out of square. (the sudocube can be tilted on an 
    /// axis.)  So this routine checks to see that the difference between the
    /// current rotation and the target rotation are "close" enough
    /// </summary>
    /// <param name="rotation">current rotation</param>
    /// <param name="toRotation">target rotation</param>
    /// <returns></returns>
    public float _rotationIsCloseEnough = 0.1f;
    private bool DoneRotating(Quaternion rotation, Quaternion toRotation)
    {
        Vector3 A = rotation.eulerAngles;
        Vector3 B = toRotation.eulerAngles;
        bool doneRotating = true; // set false if any checks fail
        doneRotating &= Mathf.Abs(A.x - B.x) <= _rotationIsCloseEnough;
        doneRotating &= Mathf.Abs(A.y - B.y) <= _rotationIsCloseEnough;
        doneRotating &= Mathf.Abs(A.z - B.z) <= _rotationIsCloseEnough;
        if (doneRotating)
            sideReport();
        return doneRotating;
    }

    /// <summary>
    /// Check for Direction Key Presses which indicate that
    /// the center sphere (SudoCenter) should be rotated in the
    /// indicated direction.
    /// Calculate a _newRotation to Quaternion.Slerp to in the
    /// FixedUpdate() method.
    /// if a _newRotation is specified, then the module level
    /// _doneRotating is set false, to only be set true when
    /// DoneRotating() indicates that the rotation is 
    /// close enough (within the specified epsilon.)
    /// /**************
    ///  * NOTE:
    ///  * new rotations can't be calculated unless all 
    ///  * Slerping has been finished, because calculating
    ///  * a new rotation based on the current rotation will
    ///  * result in off-kilter rotations if the current rotation
    ///  * isn't finished.  
    ///  * Rely on DoneRotating to maintain the _doneRotating
    ///  * module level boolean.
    ///  ************************
    /// </summary>
    private void Update()
    {
        bool _newRotation = false;
        float _rotateAngle = 90f; // angle to rotate center sphere 
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            /**********************************************
             * can't recalculate until finished with current rotation.
             * if not done, then return
             *********************************************/
            if (!_doneRotating)
                return;
            /*
             * first unhide layers as rotations with hidden layers tends to 
             * put everything out of whack.
             */
            //g.Instance.ShowAllLayers(); // unhide hidden layers.

            _newRotation = true;  // SHOW ALL LAYERS.
            g.Instance.CurrentSide.Move(eMovement.Right);
            this._newRotation = transform.rotation * Quaternion.AngleAxis(-_rotateAngle, Vector3.up);
            _doneRotating = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!_doneRotating)
                return;
            _newRotation = true;
            //g.Instance.ShowAllLayers();

            g.Instance.CurrentSide.Move(eMovement.Left);
            this._newRotation = transform.rotation * Quaternion.AngleAxis(_rotateAngle, Vector3.up);
            _doneRotating = false;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!_doneRotating)
                return;

            //g.Instance.ShowAllLayers();
            _newRotation = true;
            g.Instance.CurrentSide.Move(eMovement.Up);
            this._newRotation = transform.rotation * Quaternion.AngleAxis(-_rotateAngle, Vector3.right);
            _doneRotating = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!_doneRotating)
                return;

            //g.Instance.ShowAllLayers();
            _newRotation = true;
            g.Instance.CurrentSide.Move(eMovement.Down);
            this._newRotation = transform.rotation * Quaternion.AngleAxis(_rotateAngle, Vector3.right);
            _doneRotating = false;
        }

        if (_newRotation)
        {
            _newRotation = false;
        }
    }
}

