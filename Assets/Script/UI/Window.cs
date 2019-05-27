using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window
{
    //Identificateur utilisé par Unity (GUI)
    public int ID;

    //Texte affiché en en-tête
    public virtual string Header
    {
        get
        {
            return "";
        }
    }

    public virtual Sprite HeaderIcon
    {
        get
        {
            return null;
        }
    }

    //Fenêtre unique?
    public virtual bool OnlyOneWindowOfThisType
    {
        get
        {
            return true;
        }
    }

    //Petit X rouge pour fermer la fenêtre ?
    public virtual bool DoXClose
    {
        get
        {
            return true;
        }
    }

    //Se ferme en appuyant sur Echap?
    public virtual bool CloseOnEscapeKey
    {
        get
        {
            return true;
        }
    }

    //Se ferme si clique en dehors?
    public virtual bool CloseOnClickedOutside
    {
        get
        {
            return false;
        }
    }

    public virtual Vector2 InitialPosition
    {
        get
        {
            return new Vector2(-InitialSize.x / 2 + ScreenUtility.width / 2, -InitialSize.y / 2 + ScreenUtility.height / 2);
        }
    }

    //Affiche le fond de fenêtre?
    public virtual bool DoWindowBackground
    {
        get
        {
            return true;
        }
    }

    //Déplaceable?
    public virtual bool Draggable
    {
        get
        {
            return true;
        }
    }

    //Redimensionnable?
    public virtual bool Resizeable
    {
        get
        {
            return false;
        }
    }

    public virtual bool FollowMouse
    {
        get
        {
            return false;
        }
    }

    //Rectangle définissant la fenêtre
    public Rect windowRect;

    //Module pour redimensionner
    private WindowResizer resizer;
    private bool resizeLater;
    private Rect resizeLaterRect;

    //Marge autour de la fenêtre
    protected virtual float Margin
    {
        get
        {
            return 10f;
        }
    }

    //Taille initiale de la fenêtre
    public virtual Vector2 InitialSize
    {
        get
        {
            return new Vector2(250f, 250f);
        }
    }

    /// <summary>
    /// Constructeur de base
    /// </summary>
    public Window()
    {

    }

    /// <summary>
    /// Méthode de mise à jour de la fenêtre
    /// </summary>
    public virtual void WindowUpdate()
    {

    }

    /// <summary>
    /// Méthode du contenu de la fenêtre
    /// </summary>
    /// <param name="inRect">Interieur de la fenêtre</param>
    public abstract void DoWindowContents(Rect inRect);

    /// <summary>
    /// Appelée avant l'ouverture
    /// </summary>
    public virtual void PreOpen()
    {
        this.SetDefaultPositionAndSize();
    }

    /// <summary>
    /// Appelée après l'ouverture
    /// </summary>
    public virtual void PostOpen()
    {

    }

    /// <summary>
    /// Appelée avant fermeture
    /// </summary>
    public virtual void PreClose()
    {

    }

    /// <summary>
    /// Appelée après fermeture
    /// </summary>
    public virtual void PostClose()
    {

    }

    /// <summary>
    /// Affichage la fenêtre
    /// </summary>
    public virtual void WindowOnGUI()
    {
        //Si redimensionnable...
        if(this.Resizeable)
        {
            //...on regarde si le module existe...
            if(this.resizer == null)
            {
                this.resizer = new WindowResizer();
            }
            //...et on modifie la taille si elle est différente
            if(this.resizeLater)
            {
                this.resizeLater = false;
                this.windowRect = this.resizeLaterRect;
            }
        }
        this.windowRect = this.windowRect.IntRect();
        Rect winRect = this.windowRect.AtDefaultPosition();
        //On affiche une nouvelle fenêtre GUI tel que...
        this.windowRect = GUI.Window(this.ID, this.windowRect, delegate (int x)
        {
            Find.WindowStack.currentWindow = this;
            //...on affiche le fond (?)...
            if (this.DoWindowBackground)
            {
                Widgets.DrawWindowBackground(winRect);
            }
            if (Event.current.type == EventType.MouseDown)
            {
                Find.WindowStack.OnClickInsideWindow(this);
            }
            //...et le titre (?)...
            if (this.HeaderIcon != null)
            {
                Widgets.DrawSprite(new Rect(this.Margin / 2, this.Margin / 2, 15f + this.Margin, 15f + this.Margin), HeaderIcon);
                if (!this.Header.NullOrEmpty())
                {
                    Widgets.Label(new Rect(this.Margin * 2f + 15f, this.Margin, this.windowRect.width - this.Margin * 2f + 15f, 15f), this.Header, TextAnchor.MiddleLeft);
                }
            }
            else
            {
                if (!this.Header.NullOrEmpty())
                {
                    Widgets.Label(new Rect(this.Margin, this.Margin, this.windowRect.width - this.Margin * 2f, 15f), this.Header, TextAnchor.MiddleCenter);
                }
            }
            if (this.DoXClose && Widgets.CloseButtonFor(new Rect(this.windowRect.width - this.Margin * 2, this.Margin / 2, 15f, 15f)))
            {
                this.Close();
            }
            //...on gère le redimensionnement avant affichage (?)...
            if (this.Resizeable && Event.current.type != EventType.Repaint)
            {
                Rect newRect = this.resizer.DoResizeControl(this.windowRect);
                if(newRect != this.windowRect)
                {
                    this.resizeLater = true;
                    this.resizeLaterRect = newRect;
                }
            }
            //...on prend la partie d'affichage du contenu...
            Rect content = winRect.ContractedBy(this.Margin);
            if(!this.Header.NullOrEmpty() || this.HeaderIcon != null)
            {
                content.yMin += this.Margin + 15f;
            }
            if (this.DoWindowBackground)
            {
                //On affiche le carré du contenu
                Widgets.DrawWindowContent(content);
            }
            //... que l'on actualise...
            GUI.BeginGroup(content);
            try
            {
                this.DoWindowContents(content.AtDefaultPosition());
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error for window '{this.GetType().ToString()}' :  {exception}");
            }
            GUI.EndGroup();
            //...on gère de nouveau le redimensionnement après affichage (?)...
            if (this.Resizeable && Event.current.type == EventType.Repaint)
            {
                this.resizer.DoResizeControl(this.windowRect);
            }
            //...on gère si l'on ferme la fenêtre avec Echap (?)...
            if(this.CloseOnEscapeKey && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape))
            {
                this.Close();
                Event.current.Use();
            }
            //...on gère le déplacement de la fenêtre...
            if(this.Draggable)
            {
                GUI.DragWindow();
            }
            if(this.FollowMouse)
            {
                SetPositionToMouse();
            }
            //...sinon on applique les click de souris
            else if(Event.current.type == EventType.MouseDown)
            {
                Event.current.Use();
            }
            Find.WindowStack.currentWindow = null;
        }, string.Empty, GUIStyle.none);
    }

    /// <summary>
    /// Appelée pour la fermeture
    /// </summary>
    public virtual void Close()
    {
        Find.WindowStack.TryRemove(this);
    }

    /// <summary>
    /// Remet la fenêtre à sa position d'origine
    /// </summary>
    protected virtual void SetDefaultPositionAndSize()
    {
        if(FollowMouse)
            this.windowRect = new Rect(Input.mousePosition.x, ScreenUtility.height - Input.mousePosition.y, InitialSize.x, InitialSize.y);
        else
            this.windowRect = new Rect(InitialPosition, InitialSize);
    }

    private void SetPositionToMouse()
    {
        this.windowRect = new Rect(Input.mousePosition.x, ScreenUtility.height - Input.mousePosition.y, windowRect.width, windowRect.height);
    }

    public virtual void On_ResolutionChanged()
    {
        this.SetDefaultPositionAndSize();
    }
}
