using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outcode
{
    public bool above;
    public bool below;
    public bool left;
    public bool right;

    public Outcode(Vector2 u)
    {
        above = (u.y > 1.0f);
        below = (u.y < -1.0f);
        left = (u.x < -1.0f);
        right = (u.x > 1.0f);
    }

    public Outcode()
    {
        above = false;
        below = false;
        left = false;
        right = false;
    }

    public static bool getEdge(int e, Outcode code)
    {
        if (e == 0)
        {
            return code.above;
        }
        else if (e == 1)
        {
            return code.below;
        }
        else if (e == 2)
        {
            return code.left;
        }
        else if (e == 3)
        {
            return code.right;
        }
        else return false;
    }
    public static Outcode operator & (Outcode left, Outcode right)
    {

        Outcode ans = new Outcode();

        ans.above = left.above && right.above;
        ans.below = left.below && right.below;
        ans.left = left.left && right.left;
        ans.right = left.right && right.right;

        return ans;


    }

    public static bool operator ==(Outcode left, Outcode right)
    {

        return ((left.above == right.above) && (left.below == right.below) && (left.left == right.left) && (left.right == right.right));
    }

    public static bool operator !=(Outcode left, Outcode right)
    {

        return !(left == right);
    }

    public static Outcode operator |(Outcode left, Outcode right)
    {

        Outcode ans = new Outcode();

        ans.above = left.above || right.above;
        ans.below = left.below || right.below;
        ans.left = left.left || right.left;
        ans.right = left.right || right.right;

        return ans;


    }


    public static bool trivialAcceptance(Outcode left, Outcode right)
    {
        Outcode zero = new Outcode();
        if ((left == zero) && (right == zero))
            return true;
        else
            return false;
    }

    public static bool trivialRejection(Outcode left, Outcode right)
    {
        Outcode zero = new Outcode();

        if ((left & right)!=zero)
            return true;
        else
            return false;
    }


    public static bool checkOutcodeEquals(Outcode left, Outcode right)
    {
        
        if (left.above == right.above &&
            left.below == right.below &&
            left.left ==right.left &&
            left.right == right.right)
        {
            return true;
        }
        else return false;


    }


    public string boolToCode(bool element)
    {

        if (element == true)
            return "1";

        else return "0";
    }


    public override string ToString()
    {
        return (boolToCode(above) + boolToCode(below) + boolToCode(left) + boolToCode(right));
    }

  
}
