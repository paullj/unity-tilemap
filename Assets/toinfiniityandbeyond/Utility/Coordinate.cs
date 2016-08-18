using UnityEngine;

[System.Serializable]
public struct Coordinate
{
	public static Coordinate zero = new Coordinate (0, 0);
	public static Coordinate one = new Coordinate (1, 1);

	public static Coordinate up = new Coordinate (0, 1);
	public static Coordinate down = new Coordinate (0, -1);
	public static Coordinate left = new Coordinate (1, 0);
	public static Coordinate right = new Coordinate (-1, 0);
	
	public int x;
	public int y;

	public Coordinate (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString ()
	{
		return "[" + x + "," + y + "]";
	}
	public override bool Equals (object obj)
	{
		return obj is Coordinate && this == (Coordinate)obj;
	}
	public override int GetHashCode ()
	{
		return x.GetHashCode () ^ y.GetHashCode ();
	}

	public static explicit operator Coordinate (Vector2 v)
	{
		return new Coordinate (Mathf.FloorToInt (v.x), Mathf.FloorToInt (v.y));
	}
	public static explicit operator Coordinate (Vector3 v)
	{
		return new Coordinate (Mathf.FloorToInt(v.x), Mathf.FloorToInt (v.y));
	}
	public static explicit operator Vector2 (Coordinate c)
	{
		return new Vector2 (c.x, c.y);
	}
	public static explicit operator Vector3 (Coordinate c)
	{
		return new Vector3 (c.x, c.y);
	}
	public static Coordinate operator + (Coordinate a, Coordinate b)
	{
		return new Coordinate (a.x + b.x, a.y + b.y);
	}
	public static Coordinate operator - (Coordinate a, Coordinate b)
	{
		return new Coordinate (a.x - b.x, a.y - b.y);
	}
	public static Coordinate operator * (Coordinate a, int b)
	{
		return new Coordinate (a.x * b, a.y * b);
	}

	public static bool operator == (Coordinate a, Coordinate b)
	{
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator != (Coordinate a, Coordinate b)
	{
		return !(a == b);
	}
}