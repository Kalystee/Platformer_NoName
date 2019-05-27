using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowResizer
{
    public Vector2 minWindowSize = new Vector2(100f, 100f);

    private bool isResizing;

    private Rect resizeStart = default(Rect);

    private const float ResizeButtonSize = 20f;

    public Rect DoResizeControl(Rect winRect)
    {
        Vector2 mousePosition = Event.current.mousePosition;
        Rect rect = new Rect(winRect.width - 24f, winRect.height - 24f, 24f, 24f);
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            this.isResizing = true;
            this.resizeStart = new Rect(mousePosition.x, mousePosition.y, winRect.width, winRect.height);
        }
        if (this.isResizing)
        {
            winRect.width = this.resizeStart.width + (mousePosition.x - this.resizeStart.x);
            winRect.height = this.resizeStart.height + (mousePosition.y - this.resizeStart.y);
            if (winRect.width < this.minWindowSize.x)
            {
                winRect.width = this.minWindowSize.x;
            }
            if (winRect.height < this.minWindowSize.y)
            {
                winRect.height = this.minWindowSize.y;
            }
            winRect.xMax = Mathf.Min((float)Screen.width, winRect.xMax);
            winRect.yMax = Mathf.Min((float)Screen.height, winRect.yMax);
            if (Event.current.type == EventType.MouseUp)
            {
                this.isResizing = false;
            }
        }
        return new Rect(winRect.x, winRect.y, (float)((int)winRect.width), (float)((int)winRect.height));
    }
}
