using System.Numerics;

internal class Program
{
    class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        //Only use implicit when there are NO loss of data, so vect2 -> vect3 should be implicit, the opposite should be explicit
        //This is why float can be implicitly converted to double but double can only exlicitly converted to float as double is more accurate than float
        //The reason why we use explicit is that if you are going to cast data, the compliler is actually asking you "Are you sure? there is gonna be loss of data"
        public static implicit operator Vector3(Vector2 vector2)
        {
            return new Vector3(vector2.X, vector2.Y, 0);
        }
        public static explicit operator Vector2(Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }
        //operater overloading
        public static Vector3 operator + (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        //for the same operator, the order of input is important, as below the same *
        //but vector3 before int will return vector3
        //and int before vector3 will return string
        public static Vector3 operator * (Vector3 v1, int x) 
        {
            return new Vector3(v1.X * x, v1.Y * x, v1.Z * x);
        }
        public static string operator *(int x, Vector3 v1)
        {
            return "Hey";
        }

        //make object to be like a boolean: we need both true and false
        public static bool operator true(Vector3 v)
        {
            return true;
        }
        public static bool operator false(Vector3 v)
        {
            return false;
        }

    }
    private static void Main(string[] args)
    {
        var v1 = new Vector3(1,2,3);
        var v2 = new Vector3(2,3,4);
        var sum = v1 + v2;
        Console.WriteLine(sum.X);

        //vector3 before int will return vector3
        var v3 = v1 * 2;
        Console.WriteLine(v3.X);

        //int before vector3 will return string
        var v4 = 3 * v1;
        Console.WriteLine(v4);

        //Test boolean operator
        if (v1)
            Console.WriteLine("Hey");
    }
}