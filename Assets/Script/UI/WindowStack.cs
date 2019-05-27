using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStack
{
    //Fenêtre actuellement affiché
    public Window currentWindow;

    //Ancienne résolution
    private Int2 oldResolution = new Int2(ScreenUtility.width, ScreenUtility.height);
    //Liste des fenêtres
    private List<Window> windows = new List<Window>();
    //Fenêtre en premier plan
    private Window focusedWindow;
    //ID actuel (pour ne pas faire de doublons => erreurs)
    private static int currentWindowID;
    private bool updateOrder = false;

    //Nombre de fenêtre
    public int Count
    {
        get
        {
            return this.windows.Count;
        }
    }

    //Permet de gérer WindowStack comme un tableau
    public Window this[int index]
    {
        get
        {
            return this.windows[index];
        }
    }

    //Pour obtenir la liste des fenêtres (en lecture seule)
    public IList<Window> Windows
    {
        get
        {
            return this.windows.AsReadOnly();
        }
    }

    /// <summary>
    /// Appelée pour mettre à jour les fenêtres
    /// </summary>
    public void WindowsUpdate()
    {
        ResizeWindowsOnResolutionChanged();
        for (int i = 0; i < this.windows.Count; i++)
        {
            this.windows[i].WindowUpdate();
        }
    }

    /// <summary>
    /// Appelée pour afficher les fenêtres
    /// </summary>
    public void WindowsOnGUI()
    {
        for (int i = 0; i < this.windows.Count; i++)
        {
            this.windows[i].WindowOnGUI();
        }
        if(this.updateOrder)
        {
            UpdateWindowsOrder();
        }
    }

    /// <summary>
    /// Une fenêtre du type est-elle déjà ouverte ?
    /// </summary>
    /// <typeparam name="WindowType">Type de fenêtre</typeparam>
    /// <returns>Oui/Non</returns>
    public bool IsWindowOpen<WindowType>() where WindowType : Window
    {
        for (int i = 0; i < this.windows.Count; i++)
        {
            if(this.windows[i] is WindowType)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsWindowOpen(Type type)
    {
        for (int i = 0; i < this.windows.Count; i++)
        {
            if (this.windows[i].GetType() == type)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Ajoute une nouvelle fenêtre et gère les fonctions Pre/Post
    /// </summary>
    /// <param name="window">Fenêtre à ajouter</param>
    public void Add(Window window)
    {
        if (!window.OnlyOneWindowOfThisType || !IsWindowOpen(window.GetType()))
        {
            this.RemoveWindowsOfType(window.GetType());
            window.ID = WindowStack.currentWindowID++;
            window.PreOpen();
            windows.Add(window);
            focusedWindow = window;
            UpdateWindowsOrder();
            window.PostOpen();
        }
    }

    /// <summary>
    /// Retire toute les fenêtres du type WindowType
    /// </summary>
    /// <typeparam name="WindowType">Type de fenêtre</typeparam>
    public void RemoveWindows<WindowType>() where WindowType : Window
    {
        Window[] win = windows.ToArray();
        for (int i = 0; i < win.Length; i++)
        {
            if(win[i].OnlyOneWindowOfThisType && win[i] is WindowType)
            {
                TryRemove(win[i]);
            }
        }
    }

    public void RemoveWindowsOfType(Type type)
    {
        Window[] win = windows.ToArray();
        for (int i = 0; i < win.Length; i++)
        {
            if (win[i].OnlyOneWindowOfThisType && win[i].GetType() == type)
            {
                TryRemove(win[i]);
            }
        }
    }

    /// <summary>
    /// Retire une fenêtre présente
    /// </summary>
    /// <param name="window">Fenêtre à retirer</param>
    /// <returns>Fenêtre retiré ?</returns>
    public bool TryRemove(Window window)
    {
        //On vérifie que la fenêtre existe
        bool windowExist = false;
        for (int i = 0; i < this.windows.Count; i++)
        {
            if(windows[i] == window)
            {
                windowExist = true;
                break;
            }
        }
        if(!windowExist)
        {
            return false;
        }

        //On applique les fonctions Pre/Post
        window.PreClose();
        this.windows.Remove(window);
        window.PostClose();
        
        //On met a jour le focus des fenêtres
        if(this.focusedWindow == window)
        {
            if(this.windows.Count > 0)
            {
                this.focusedWindow = this.windows[this.windows.Count - 1];
            }
            else
            {
                this.focusedWindow = null;
            }
        }
        UpdateWindowsOrder();
        return true;
    }

    /// <summary>
    /// Récupère la fenêre la plus en premier plan à la position
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Fenêtre (si présente) en position désiré</returns>
    public Window GetWindowAt(Vector2 pos)
    {
        for (int i = this.windows.Count - 1; i >= 0; i--)
        {
            if(this.windows[i].windowRect.Contains(pos))
            {
                return this.windows[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Met à jour la liste des fenêtres et les ordonnes dans les plans
    /// </summary>
    private void UpdateWindowsOrder()
    {
        for (int i = 0; i < this.windows.Count; i++)
        {
            GUI.BringWindowToFront(this.windows[i].ID);
        }
        if (this.focusedWindow != null)
        {
            GUI.FocusWindow(this.focusedWindow.ID);
        }
    }

    private void ResizeWindowsOnResolutionChanged()
    {
        Int2 newResolution = new Int2(ScreenUtility.width, ScreenUtility.height);
        if(newResolution != oldResolution)
        {
            this.oldResolution = newResolution;
            for (int i = 0; i < this.windows.Count; i++)
            {
                this.windows[i].On_ResolutionChanged();
            }
        }
    }

    public bool MouseNotOnWindow()
    {
        return GetWindowAt(MouseUtility.MousePos) != this.currentWindow;
    }

    public bool CurrentInFocus()
    {
        return this.currentWindow == this.focusedWindow;
    }

    public void OnClickInsideWindow(Window window)
    {
        windows.Remove(window);
        focusedWindow = window;
        windows.Add(window);
        this.updateOrder = true;
    }
}
