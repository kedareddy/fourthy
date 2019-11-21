using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;

[RequireComponent(typeof(RectGrid))]
[RequireComponent(typeof(Parallelepiped))]
public class InitializeGrid : MonoBehaviour
{
	
	void Awake()
    {
		var grid = gameObject.GetComponent<RectGrid>();
		var para = gameObject.GetComponent<Parallelepiped>();
		GridWorld.InitializePuzzle(grid, para);
	}

    // visualizes the matrix in text form to let you see what's going on
    //void OnGUI()
    //{
    //    const int w = 500;
    //    const int h = 250;
    //    const int x = 10;

    //    var y = Screen.height - x - h;

    //    GUI.TextArea(new Rect(x, y, w, h), GridWorld.MatrixToString());
    //}
}
