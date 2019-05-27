using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtensions
{
    public static Rect IntRect(this Rect r)
    {
        return new Rect((int)r.x, (int)r.y, (int)r.width, (int)r.height);
    }

    public static Rect ExpandedBy(this Rect rect, float margin)
    {
        return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
    }

    public static Rect ContractedBy(this Rect rect, float margin)
    {
        return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
    }

    public static Rect AtDefaultPosition(this Rect rect)
    {
        return new Rect(0f, 0f, rect.width, rect.height);
    }
}
