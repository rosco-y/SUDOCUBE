using System;
using System.Collections.Generic;

using static eMovement;
public enum eMovement
{
    Up,
    Down,
    Right,
    Left
}

public enum eFace
{
    FRONT2BACK,
    LEFT2RIGHT,
    TOP2BOTTOM
}
public class cCur
{
    int _facing; //facing
    int _top; // dictages what the _sides are.
    int _bottom;
    const int NUMSIDES = 4;
    List<int> _sides;
    public cCur(int facing, int top)
    {
        _facing = facing;
        _top = top;
        _bottom = 7 - top;
        initializeSides();
    }

    public int Move(eMovement movement)
    {
        // dieFaces.cCur.Move()
        switch (movement)
        {
            case Up:
                // make sure and do these assignments in order!
                // (to avoid reassigning freshly assigned variables: a = b; c = a;
                _top = _facing;
                _facing = Bottom;
                _bottom = 7 - _top;
                initializeSides();
                break;
            case Down:
                int ttop = _top;
                _top = 7 - _facing;
                _bottom = _facing;
                _facing = ttop;
                initializeSides();
                break;
            case Right:
                int facingIndex = _sides.IndexOf(_facing);
                rotate(_sides, eMovement.Right);
                // rotate
                _facing = _sides[facingIndex];
                break;
            case Left:
                facingIndex = _sides.IndexOf(_facing);
                rotate(_sides, eMovement.Left);
                _facing = _sides[facingIndex];
                break;
            default:
                Console.Write($"Unhandled case: {movement.ToString()}");
                break;
        }

        return _facing;
    }

    void rotate(List<int> ls, eMovement movement)
    {

        if (movement == eMovement.Right)
        {
            int len = ls.Count - 1;
            int lastValue = ls[len];
            ls.RemoveAt(len);
            ls.Insert(0, lastValue);
        }
        else if (movement == eMovement.Left)
        {
            int firstValue = ls[0];
            ls.RemoveAt(0);
            ls.Add(firstValue);
        }

    }
    ~cCur()
    {
        _sides?.Clear();
        _sides = null;
    }

    private void initializeSides()
    {
        switch (Top)
        {
            case 1:
            case 6:
                _sides = new List<int> { 2, 3, 5, 4 };
                if (Top == 6)
                    _sides.Reverse();
                break;
            case 3:
            case 4:
                _sides = new List<int> { 1, 2, 6, 5 };
                if (Top == 4)
                    _sides.Reverse();
                break;
            case 2:
            case 5:
                _sides = new List<int> { 1, 3, 6, 4 };
                if (Top == 2)
                    _sides.Reverse();
                break;
        }
    }



    public int Face
    {
        set
        {
            _facing = value;
            _bottom = _facing;
        }
        get
        {
            return _facing;
        }
    }



    public int Top
    {
        get { return _top; }
        private set
        {
            _top = value;
            Bottom = _top;
        }
    }

    public int Bottom
    {
        get { return _bottom; }
        private set
        {
            _bottom = value;
            _top = _bottom;
        }
    }


    public List<int> Sides
    {
        get
        {
            return _sides;
        }
    }

    public eFace EFACE
    {
        get
        {
            eFace ret;
            switch (Face)
            {
                case 1:
                case 6:
                    ret = eFace.TOP2BOTTOM;
                    break;
                case 2:
                case 5:
                    ret = eFace.FRONT2BACK;
                    break;
                default:
                    ret = eFace.LEFT2RIGHT;
                    break;
            }

            return ret;
        }
    }

    //*
    public override string ToString()
    {
        string die = faceString(Face);
        return die;

        //int printLen = _sides.Count * 4;
        //string die = "----------------\n";
        //die += $"{_top}/{_bottom} \n";
        //die += new string('-', printLen);
        //die += "\n";
        //foreach (int face in _sides)
        //{
        //    if (face == Face)
        //        die += $"<*{face}*>";
        //    else
        //        die += $" {face}";
        //    if (face != _sides[_sides.Count - 1])
        //        die += ",";
        //}
        //die += "\n";
        //die += new string('-', printLen);
        //return die;
        //return Face.ToString() + "\n";
    }

    private string faceString(int face)
    {
        string ret = string.Empty;
        switch (face)
        {
            case 1:
                ret = "BOTTOM";
                break;
            case 2:
                ret = "BACK";
                break;
            case 3:
                ret = "LEFT";
                break;
            case 4:
                ret = "RIGHT";
                break;
            case 5:
                ret = "FRONT";
                break;
            default: // 6
                ret = "TOP";
                break;
        }
        return ret;

    }

    /// <summary>
    /// eCurFace is manually aligned with faceString(int face)
    /// </summary>
    public enum eCurFace
    {
        BOTTOM,
        BACK,
        LEFT,
        RIGHT,
        FRONT,
        TOP
    }

    /// <summary>
    /// return eCurFace corresponding to current face of cube.
    /// </summary>
    /// <returns></returns>
    public eCurFace GetCurFace()
    {
        eCurFace ret;
        switch (faceString(Face))
        {
            case "BOTTOM":
                ret = eCurFace.BOTTOM;
                break;
            case "BACK":
                ret = eCurFace.BACK;
                break;
            case "LEFT":
                ret = eCurFace.LEFT;
                break;
            case "RIGHT":
                ret = eCurFace.RIGHT;
                break;
            case "FRONT":
                ret = eCurFace.FRONT;
                break;
            default:
                ret = eCurFace.TOP;
                break;
        }
        return ret;
    }

    public string CameraAdjustmentAxis()
    {
        string retValue = string.Empty;
        switch (_facing)
        {
            case 5:
                retValue = "Z";
                break;
            case 2:
                retValue = "-Z";
                break;
            case 3:
                retValue = "X";
                break;
            case 4:
                retValue = "-X";
                break;
            case 6:
                retValue = "-Y";
                break;
            case 1:
                retValue = "Y";
                break;
        }
        return retValue;
    }


}