using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public static UserInfo instance;

    public static UserInfo Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new UserInfo();
            }
            return instance;
        }
    }

    public string userName;

}
