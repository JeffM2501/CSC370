using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;

namespace Models
{
    public class ModelReader
    {
        public static Vector3 ReadVector3(string[] input, int start)
        {
            Vector3 v = new Vector3();

            if (input.Length > start)
                float.TryParse(input[start], out v.X);

            if (input.Length > start + 1)
                float.TryParse(input[start + 1], out v.Y);

            if (input.Length > start + 2)
                float.TryParse(input[start + 2], out v.Z);

            return v;
        }

        public static Vector2 ReadVector2(string[] input, int start)
        {
            Vector2 v = new Vector2();

            if (input.Length > start)
                float.TryParse(input[start], out v.X);

            if (input.Length > start + 1)
                float.TryParse(input[start + 1], out v.Y);

            return v;
        }

        public virtual Model Read(string name, FileInfo file)
        {
            return Model.NewModel(name);
        }
    }
}
