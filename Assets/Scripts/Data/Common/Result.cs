using UnityEngine;
using System;

public struct Result
{
    public bool boolValue;
    public string info;
    public string code;


    public static Result Success(string info = "", string code = ""){
        Result successRe = new Result();
        successRe.boolValue = true;
        successRe.info = info;
        successRe.code = code;
        return successRe;
    }

    public static Result Error(string info = "", string code = "")
    {
        Result ErrorRe = new Result();
        ErrorRe.boolValue = false;
        ErrorRe.info = info;
        ErrorRe.code = code;
        return ErrorRe;
    }

    public static Result ExptionErr(Exception e, string code = "")
    {
        Result ErrorRe = new Result();
        ErrorRe.boolValue = false;
        ErrorRe.info = e.ToString();
        ErrorRe.code = code;
        return ErrorRe;
    }
}


