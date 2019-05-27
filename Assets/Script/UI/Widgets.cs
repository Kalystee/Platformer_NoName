using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widgets
{
    //Couleurs utilisé pour les fenêtres
    public static readonly Color windowBackgroundColor;
    public static readonly Color windowBorderColor;
    public static readonly Color buttonBackgroundColor;
    public static readonly Color buttonHoverBackgroundColor;
    public static readonly Color buttonClickedBackgroundColor;
    public static readonly Color buttonBorderColor;
    public static readonly Color buttonDisabledBackgroundColor;
    public static readonly Color buttonDisabledBorderdColor;

    public static readonly Texture2D whiteTexture;
    public static readonly Texture2D redCrossTex;
    public static readonly Texture2D greenCheckTex;

    public static readonly Font font;

    public static readonly GUIStyle defaultStyle;
    public static readonly GUIStyle nullStyle;

    static Widgets()
    {
        windowBackgroundColor = new Color32(31, 31, 45, 255);
        windowBorderColor = new Color32(179, 179, 204, 255);
        buttonBackgroundColor = new Color32(115, 115, 150, 255);
        buttonHoverBackgroundColor = new Color32(88, 88, 122, 255);
        buttonClickedBackgroundColor = new Color32(45, 45, 63, 255);
        buttonBorderColor = new Color32(179, 179, 204, 255);
        buttonDisabledBackgroundColor = new Color32(85, 85, 120, 255);
        buttonDisabledBorderdColor = new Color32(129, 129, 154, 255);

        redCrossTex = Resources.Load<Texture2D>("Sprites/UI/RedCross");
        greenCheckTex = Resources.Load<Texture2D>("Sprites/UI/GreenCheck");
        whiteTexture = Resources.Load<Texture2D>("Sprites/UI/WhiteTexture");

        font = Resources.Load<Font>("Fonts/UI/Bangers");

        defaultStyle = new GUIStyle
        {
            font = Widgets.font,
            alignment = TextAnchor.MiddleLeft,
        };
        defaultStyle.normal.textColor = Color.white;
        nullStyle = new GUIStyle();
    }

    public static bool IsMouseOver(Rect rect)
    {
        return rect.Contains(Event.current.mousePosition);
    }

    /// <summary>
    /// Affiche une boite (contours) d'une épaisseur choisie
    /// </summary>
    /// <param name="rect">Rectangle définissant la boite</param>
    /// <param name="thickness">Epaisseur</param>
    public static void DrawBox(Rect rect, int thickness = 1)
    {
        Vector2 b = new Vector2(rect.x, rect.y);
        Vector2 a = new Vector2(rect.x + rect.width, rect.y + rect.height);
        if (b.x > a.x)
        {
            float x = b.x;
            b.x = a.x;
            a.x = x;
        }
        if (b.y > a.y)
        {
            float y = b.y;
            b.y = a.y;
            a.y = y;
        }
        Vector3 vector = a - b;
        GUI.DrawTexture(new Rect(b.x, b.y, (float)thickness, vector.y), whiteTexture);
        GUI.DrawTexture(new Rect(a.x - (float)thickness, b.y, (float)thickness, vector.y), whiteTexture);
        GUI.DrawTexture(new Rect(b.x + (float)thickness, b.y, vector.x - (float)(thickness * 2), (float)thickness), whiteTexture);
        GUI.DrawTexture(new Rect(b.x + (float)thickness, a.y - (float)thickness, vector.x - (float)(thickness * 2), (float)thickness), whiteTexture);
    }

    /// <summary>
    /// On affiche la fenêtre
    /// </summary>
    /// <param name="rect">Rectangle définissant la fenêtre</param>
    public static void DrawWindowBackground(Rect rect)
    {
        DrawBoxWithColors(rect, Widgets.windowBackgroundColor, Widgets.windowBorderColor, 2);
    }

    public static void DrawBoxWithColors(Rect rect, Color bg, Color border, int thickness = 1)
    {
        GUI.color = bg;
        GUI.DrawTexture(rect, whiteTexture);
        GUI.color = border;
        Widgets.DrawBox(rect, thickness);
        GUI.color = Color.white;
    }

    public static void DrawWindowContent(Rect rect)
    {
        GUI.color = Widgets.windowBorderColor;
        Widgets.DrawBox(rect, 1);
        GUI.color = Color.white;
    }

    public static void Label(Rect rect, string text, TextAnchor textAnchor = TextAnchor.MiddleLeft)
    {
        GUI.Label(rect, text, new GUIStyle(Widgets.defaultStyle) { alignment = textAnchor });
    }

    public static void InputField(Rect rect, ref string text)
    {
        DrawBox(rect);
        text = GUI.TextField(rect, text, 32, defaultStyle);
    }

    public static void TextArea(Rect rect, ref string text)
    {
        text = GUI.TextArea(rect, text, defaultStyle);
    }

    public static bool ButtonText(Rect rect, string label, bool active = true)
    {
        Color buttonColor = Widgets.buttonBackgroundColor;
        if (IsMouseOver(rect) && active)
        {
            buttonColor = Widgets.buttonHoverBackgroundColor;
            if (Input.GetMouseButton(0))
            {
                buttonColor = Widgets.buttonClickedBackgroundColor;
            }
        }
        else if(!active)
        {
            buttonColor = Widgets.buttonDisabledBackgroundColor;
        }
        Widgets.DrawBoxWithColors(rect, buttonColor, active ? Widgets.buttonBorderColor : Widgets.buttonDisabledBorderdColor);
        Widgets.Label(rect, label, TextAnchor.MiddleCenter);
        return active && Widgets.ButtonInvisible(rect);
    }

    public static void DrawSprite(Rect rect, Sprite sprite, bool keepAspect = true)
    {
        if (keepAspect)
        {
            if (rect.width > rect.height)
                rect = new Rect(rect.x, rect.y - rect.height / 2, rect.width, rect.width);
            else if (rect.width < rect.height)
                rect = new Rect(rect.x - rect.width / 2, rect.y, rect.height, rect.height);
        }
        GUI.DrawTexture(rect, sprite.texture);
    }

    public static bool DrawAbility(Rect rect, AbilityBase ability)
    {
        DrawBox(rect);
        rect = rect.ContractedBy(rect.width / 10f);
        DrawSprite(rect, ability.Sprite);
        if (InvisibleHover(rect))
        {
            if (!Find.WindowStack.IsWindowOpen<Window_Ability>())
            {
                Find.WindowStack.Add(new Window_Ability(ability));
            }
            return Input.GetMouseButtonDown(0);
        }
        else
        {
            if (Window_Ability.ability == ability)
            {
                Find.WindowStack.RemoveWindows<Window_Ability>();
            }
        }
        return false;
    }

    public static bool DrawItem(Rect rect, ItemBase item)
    {
        DrawBox(rect);
        rect = rect.ContractedBy(rect.width / 10f);
        DrawSprite(rect, item.Sprite);
        if (InvisibleHover(rect))
        {
            if (!Find.WindowStack.IsWindowOpen<Window_Item>())
            {
                Find.WindowStack.Add(new Window_Item(item));
            }
            return Input.GetMouseButtonDown(0);
        }
        else
        {
            if (Window_Item.item == item)
            {
                Find.WindowStack.RemoveWindows<Window_Item>();
            }
        }
        return false;
    }

    public static bool ButtonInvisible(Rect rect)
    {
        return GUI.Button(rect, string.Empty, nullStyle);
    }

    public static bool InvisibleHover(Rect rect)
    {
        return IsMouseOver(rect);
    }

    public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollBar = true)
    {
        Widgets.DrawBox(outRect);
        if (showScrollBar)
        {
            scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
        }
        else
        {
            scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
        }
    }

    public static void EndScrollView()
    {
        GUI.EndScrollView();
    }

    public static void BeginGroup(Rect rect)
    {
        GUI.BeginGroup(rect, "", nullStyle);
        Widgets.DrawBoxWithColors(rect, buttonBackgroundColor, buttonBorderColor);
    }

    public static void EndGroup()
    {
        GUI.EndGroup();
    }

    public static bool ButtonImage(Rect butRect, Sprite sprite, bool active = true)
    {
        return Widgets.ButtonImage(butRect, sprite.texture, active);
    }

    public static bool ButtonImage(Rect butRect, Sprite sprite, Color baseColor, Color mouseoverColor, bool active = true)
    {
        return Widgets.ButtonImage(butRect, sprite.texture, baseColor, mouseoverColor, active);
    }

    public static bool ButtonImage(Rect butRect, Texture2D tex, bool active = true)
    {
        return Widgets.ButtonImage(butRect, tex, Color.white, Color.gray, active);
    }

    public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor, bool active = true)
    {
        if (Widgets.IsMouseOver(butRect) && active)
        {
            GUI.color = mouseoverColor;
        }
        else if(!active)
        {
            GUI.color = Widgets.buttonDisabledBackgroundColor;
        }
        else
        {
            GUI.color = baseColor;
        }
        GUI.DrawTexture(butRect, tex);
        GUI.color = baseColor;
        return Widgets.ButtonInvisible(butRect);
    }

    public static bool CloseButtonFor(Rect rectToClose)
    {
        Rect butRect = new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f);
        return Widgets.ButtonImage(butRect, Widgets.redCrossTex);
    }

    public static void DrawInventory(Rect rect, Inventory inventory, ref Vector2 scrollInventory)
    {
        int height = inventory.Size * 50;
        Rect scrollRect = rect.ContractedBy(5);

        Widgets.BeginScrollView(scrollRect, ref scrollInventory, new Rect(0, 0, scrollRect.width - 20, height));
        for (int i = 0; i < inventory.Size; i++)
        {
            Int2 offset = new Int2(0, 50 * i);
            Widgets.DrawBoxWithColors(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 25, 40), Widgets.buttonBackgroundColor, Widgets.buttonBorderColor);
            Widgets.DrawItem(new Rect(10 + offset.x, 10 + offset.y, 30, 30), inventory[i].Item);
            int offY = 0;
            if (inventory[i].Quantity != 1)
            {
                offY += 20;
                Widgets.Label(new Rect(60 + offset.x, 10 + offset.y, 30, 30), $"x{inventory[i].Quantity.ToString()}", TextAnchor.MiddleLeft);
            }
            Widgets.Label(new Rect(60 + offset.x + offY, 10 + offset.y, 20, 30), inventory[i].Item.Name);
        }
        Widgets.EndScrollView();
    }

    public static void DrawShopInventory(Rect rect, Inventory inventory, ref Vector2 scrollInventory)
    {
        int height = inventory.Size * 50;
        Rect scrollRect = rect.ContractedBy(5);

        Widgets.BeginScrollView(scrollRect, ref scrollInventory, new Rect(0, 0, scrollRect.width - 20, height));
        for (int i = 0; i < inventory.Size; i++)
        {
            Debug.Log(inventory[i].Item);
            Debug.Log(inventory[i].Quantity);
            Int2 offset = new Int2(0, 50 * i);
            //Si la quantité de l'objet est de 0 
            if (inventory[i].Quantity < 1)
            {
                //On le remove et on affiche "rupture de stock !"
                inventory.RemoveItem(inventory[i].Item, inventory[i].Quantity);
                Widgets.DrawBoxWithColors(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 25, 40), new Color(255, 0, 0), Widgets.buttonBorderColor);
                Widgets.Label(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 25, 40),"Out of Stock !", TextAnchor.MiddleCenter);

                //Sinon on affiche l'objet
            } else { 
            
            Widgets.DrawBoxWithColors(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 25, 40), Widgets.buttonBackgroundColor, Widgets.buttonBorderColor);

            //Si on passe la souris sur le rectangle
            if (IsMouseOver(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 25, 40)))
            {
                //La couleur de fond est rose
                Widgets.DrawBoxWithColors(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 25, 40), new Color(255, 0, 255), Widgets.buttonBorderColor);
            }

             //Si Click sur le rectangle
            if(ButtonInvisible(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 25, 40))){
                if(Find.Party.Money >= inventory[i].Item.Price){
                    //Si assez d'argent
                    Find.Party.Money -= inventory[i].Item.Price;
                    Find.Party.CommualInventory.AddItems(new ItemStack(inventory[i].Item));
                    inventory[i].ReduceQuantity(1);
                    }
                    else{
                    Debug.Log("Pas assez d'argent");
                }
            }
            //Prix de l'objet
            Widgets.Label(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 40, 40), $"{inventory[i].Item.Price.ToString()} $", TextAnchor.MiddleRight);
            Widgets.Label(new Rect(5 + offset.x, 5 + offset.y, scrollRect.width - 40, 40), $"x{inventory[i].Quantity} ", TextAnchor.MiddleCenter);
                //Sprite de l'objet
                Widgets.DrawItem(new Rect(10 + offset.x, 10 + offset.y, 30, 30), inventory[i].Item);
            int offY = 0;
            if (inventory[i].Quantity != 1)
            {
                offY += 20;
                Widgets.Label(new Rect(60 + offset.x, 10 + offset.y, 30, 30), $"x{inventory[i].Quantity.ToString()}", TextAnchor.MiddleLeft);
            }
            Widgets.Label(new Rect(60 + offset.x + offY, 10 + offset.y, 20, 30), inventory[i].Item.Name);
        }
        }

        Widgets.EndScrollView();
    }
}
