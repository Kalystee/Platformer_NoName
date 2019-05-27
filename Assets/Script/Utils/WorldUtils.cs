using UnityEngine;

public class WorldUtils
{
    private static Plane plane = new Plane(Vector3.up, Vector3.zero);
    
    /// <summary>
    /// Retourne la position visée par la souris
    /// </summary>
    /// <returns>Position visée par la souris</returns>
    public static Int2 GetWorldMousePosition()
    {
        //On crée un rayon partant de la caméra...
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //(Cet variable définit la distance)
        float enter = 0.0f;

        //On vérifie que on touche le plan XZ
        if (plane.Raycast(ray, out enter))
        {
            //On récupère le point et on le modifie pour qu'il soient en XY
            Vector3 vec = ray.GetPoint(enter);
            vec.y = vec.z;
            vec.z = 0;
            return vec;
        }
        else
        {
            //On ne devrais pas se retrouver ici mais au cas où on retourne la valeur par défault -1,-1
            Debug.LogWarning("Un problème avec la gestion de la position de la souris a eu lieu ! Retourne la valeur -1,-1");
            return Vector3.zero;
        }
    }

    public static bool HasLineOfSightTo(Int2 pos, Int2 impact)
    {
        Vector2 differences = new Vector2(impact.x - pos.x, impact.y - pos.y);

        int signX = (int)Mathf.Sign(differences.x);
        int signY = (int)Mathf.Sign(differences.y);

        float slope = differences.x / differences.y;
        if (differences.y == 0)
            slope = 0;

        float invSlope = differences.y / differences.x;
        if (differences.x == 0)
            invSlope = 0;

        if (Mathf.Abs(differences.x) > Mathf.Abs(differences.y))
        {
            for (int x = 0; x < Mathf.Abs(differences.x); x++)
            {
                int X = signX * x;
                int y1 = Mathf.FloorToInt(invSlope * X);
                int y2 = Mathf.CeilToInt(invSlope * X);

                TileBase t1 = Level.singleton.arena.GetTile(new Int2(X, y1) + pos);
                if (t1 != null && t1.IsBlockingSight)
                {
                    return false;
                }

                TileBase t2 = Level.singleton.arena.GetTile(new Int2(X, y2) + pos);
                if (t2 != null && t2.IsBlockingSight)
                {
                    return false;
                }
            }
        }
        else if (Mathf.Abs(differences.x) < Mathf.Abs(differences.y))
        {
            for (int y = 0; y < Mathf.Abs(differences.y); y++)
            {
                int Y = signY * y;
                int x1 = Mathf.FloorToInt(slope * Y);
                int x2 = Mathf.CeilToInt(slope * Y);

                TileBase t1 = Level.singleton.arena.GetTile(new Int2(x1, Y) + pos);
                if (t1 != null && t1.IsBlockingSight)
                {
                    return false;
                }

                TileBase t2 = Level.singleton.arena.GetTile(new Int2(x2, Y) + pos);
                if (t2 != null && t2.IsBlockingSight)
                {
                    return false;
                }
            }
        }
        else
        {
            for (int i = 0; i < Mathf.Abs(differences.x); i++)
            {
                TileBase t = Level.singleton.arena.GetTile(new Int2(i * signX, i * signY) + pos);
                if (t != null && t.IsBlockingSight)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
