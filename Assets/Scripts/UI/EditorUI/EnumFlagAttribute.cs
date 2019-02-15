﻿//http://dotsquid.com/2017/04/05/enum-flag-attribute-plus-property-drawer/

using UnityEngine;

public class EnumFlagAttribute : PropertyAttribute
{
    public string name;

    public EnumFlagAttribute() { }

    public EnumFlagAttribute(string name)
    {
        this.name = name;
    }
}