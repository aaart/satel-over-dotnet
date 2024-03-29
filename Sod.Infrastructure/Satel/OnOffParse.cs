﻿using System;

namespace Sod.Infrastructure.Satel;

public static class OnOffParse
{
    public static string ToString(bool val)
    {
        return val ? "ON" : "OFF";
    }

    public static bool ToBoolean(string val)
    {
        switch (val.ToUpperInvariant())
        {
            case "ON":
                return true;
            case "OFF":
                return false;
            default:
                throw new ArgumentOutOfRangeException($"Must be ON/OFF. Was: {val}");
        }
    }
}