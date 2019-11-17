using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using GridFramework.Extensions.Nearest;
using System.Collections;
using System.Collections.Generic;

using CoordinateSystem = GridFramework.Grids.RectGrid.CoordinateSystem;


public static class GridWorld
{
	#region  Private variables
	/// <summary>
	///   Private member variable for the renderer of the grid.
	/// </summary>
	public static Parallelepiped _para;

	/// <summary>
	///   This is where we store our information; all game logic uses this
	///   matrix.
	/// </summary>
	private static bool[,] levelMatrix;

	/// <summary>
	///   Size of the level matrix, first component is horizontal, second
	///   vertical.
	/// </summary>
	private static int[] levelMatrixSize;
	#endregion  // Private variables

	#region  Public variables
	/// <summary>
	///   Private member variable for the grid.
	/// </summary>
	public static RectGrid _grid;
	#endregion  // Public variables

	#region  Private methods
	/// <summary>
	///   Takes the grid's rendering range and builds a matrix based on
	///   that. All entries are set to true.
	/// </summary>
	private static void BuildLevelMatrix()
	{
		// Amount of rows and columns, either based on size or rendering
		// range (first entry rows, second one columns).
		ComputeMatrixSize();
		var w = levelMatrixSize[0];
		var h = levelMatrixSize[1];

		levelMatrix = new bool[w, h];

		// Set all entries to true, all squares allowed initially.
		for (var i = 0; i < w; ++i)
		{
			for (var j = 0; j < h; ++j)
			{
				levelMatrix[i, j] = true;
			}
		}
	}

	/// <summary>
	///   How large the matrix should be. For the sake of simplicity we
	///   only use the rendering range here.
	/// </summary>
	private static void ComputeMatrixSize()
	{
		var from = _para.From;
		var to = _para.To;

		// If there is no matrix yet create it, otherwise we can just
		// overwrite its values.
		levelMatrixSize = levelMatrixSize ?? new int[2];

		for (var i = 0; i < 2; ++i)
		{
			// Get the distance between both ends (in world units), divide
			// it by the spacing (to get grid units) and round down to the
			// nearest integer
			var lower = Mathf.CeilToInt(from[i]);
			var upper = Mathf.CeilToInt(to[i]);
			levelMatrixSize[i] = upper - lower;
		}
	}

	/// <summary>
	///   Take world coodinates and find the corresponding square. The
	///   result is returned as an int array that contains that square's
	///   position in the matrix.
	/// </summary>
	public static int[] GetSquare(Vector3 vec)
	{
		const CoordinateSystem system = CoordinateSystem.Grid;

		var cell = _grid.NearestCell(vec, system);
		var shift = .5f * Vector3.one;
		var square = cell - shift;
		var indices = new int[2];

		for (var i = 0; i < 2; ++i)
		{
			// Remember, boxes don't have whole coordinates, that's why we
			// use a little shift to turn e.g. (3.5, 2.5, 1.5) into (3, 2,
			// 1).
			indices[i] = Mathf.RoundToInt(square[i]);
		}

		return indices;
	}

    private static Vector3 SquareToGrid(int[] sq)
    {
        Vector3 gridPoint = new Vector3(sq[0], sq[1], 0f);
        var shift = .5f * Vector3.one;
        gridPoint += shift;
        return gridPoint;
    }
	#endregion  // Private methods

	#region  Public methods
	/// <summary>
	///   Initialize the puzzle using a grid and renderer.
	/// </summary>
	public static void InitializePuzzle(RectGrid grid, Parallelepiped para)
	{
		_grid = grid;
		_para = para;
		BuildLevelMatrix();
	}

	/// <summary>
	///   Takes world coodinates, finds the corresponding square and sets
	///   that entry to either true or false. Use it to disable or enable
	///   squares.
	/// </summary>
	public static void RegisterObstacle(Transform obstacle, bool state)
	{

        var sq = GetSquare(obstacle.position);
        levelMatrix[sq[0], sq[1]] = state;
        // First break up the obstacle into several 1x1 obstacles.
  //      var parts = BreakUpObstacle(obstacle);

		//// Now find the square of each part and set it to true or false.
		//for (var i = 0; i < parts.GetLength(0); ++i)
		//{
		//	for (var j = 0; j < parts.GetLength(1); ++j)
		//	{
		//		var square = GetSquare(parts[i, j]);
		//		levelMatrix[square[0], square[1]] = state;
		//	}
		//}
	}



	

	/// <summary>
	///   This returns the matrix as a string so you can read it yourself,
	///   like in a GUI for debugging (nothing grid-related going on here,
	///   feel free to ignore it).
	/// </summary>
	public static string MatrixToString()
	{
		const string vacant = "_";
		const string occupied = "X";

		var text = "";
		for (var j = levelMatrix.GetLength(1) - 1; j >= 0; --j)
		{
			for (var i = 0; i < levelMatrix.GetLength(0); ++i)
			{
				text = text + (levelMatrix[i, j] ? vacant : occupied) + " ";
			}
			text += "\n";
		}
		return text;
	}


    public static List<GameObject> GetColumnObjects(Vector3 pos)
    {
        List<GameObject> foundGOs = new List<GameObject>();
        var sq = GetSquare(pos);
        //Debug.Log("My Pos: " + sq[0] + " : " + sq[1]);
        

        for (var j = levelMatrix.GetLength(1) - 1; j >= 0; --j)
        {
            //Debug.Log("EMPTY?: " + levelMatrix[sq[0], j]);

            if(levelMatrix[sq[0], j] == false)
            {
                Vector3 gridPoint = SquareToGrid(new int[] { sq[0], j });
                Vector3 worldPoint = _grid.GridToWorld(gridPoint);
                //Debug.Log("worldPoint: " + worldPoint);
                Collider2D col = Physics2D.OverlapPoint(new Vector2(worldPoint.x, worldPoint.y));
                if (col != null)
                {
                    foundGOs.Add(col.gameObject);
                }
            }
        }

        return foundGOs;
    }


    // This method was used at some point but now it is of no use; I left it in though for you if you are interested
    //takes world coodinates, finds the corresponding square and returns the value of that square. Use it to cheack if a square is forbidden or not
    public static bool CheckObstacle(Transform obstacle)
    {
        var sq = GetSquare(obstacle.position);
        return levelMatrix[sq[0], sq[1]];
        ////first break up the obstacle into several 1x1 obstacles
        //Vector3[,] parts = BreakUpObstacle(obstacle);
        ////now find the square of each part and set it to true or false
        //for (int i = 0; i < parts.GetUpperBound(0) + 1; i++)
        //{
        //    for (int j = 0; j < parts.GetUpperBound(1) + 1; j++)
        //    {
        //        int[] square = GetSquare(parts[i, j]);
        //        free = free && levelMatrix[square[0], square[1]]; // add all the entries, returns true if and only if all are true
        //    }
        //}
        //return free;
    }

    #endregion  // Public methods
}


//public class ColumnData: MonoBehaviour
//{
//    public int numOfObjs = 0;
//    public List<GameObject> gameObjects;
//    public int hahah = 0; 
//}

