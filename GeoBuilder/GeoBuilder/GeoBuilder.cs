using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using OpenTK;

namespace GeoBuilder
{
    public class GeoBuilder
    {
        public class RoomDef
        {
            public Size Bounds = Size.Empty;
       //     public List<int> ClipedCorners = new List<int>();

            public List<int> Exits = new List<int>();

            public float WallHeight = 2;
            public float SouthWallParam = 0.5f;
            public float WallWidth = 0.5f;

            public string FloorObjectName = "Floor";
            public string WallObjectName = "Walls";
            public string UpperWallObjectName = "SouthWall";

            public string WallMaterialName = "walls";
            public string FloorMaterialName = "floors";
            public string UpperWallMaterialName = "walls";
            public string CapMaterialName = "wallCap";
            public string InnerCapMaterialName = "innerCap";
    
        }

        public List<Vector3> Verts = new List<Vector3>();
        public List<Vector3> Normals = new List<Vector3>();
        public List<Vector2> UVs = new List<Vector2>();
        

        //   0 ---------- 1
        //   |            |
        //   |            |
        //   |            |
        //   |            |
        //   |            |
        //   3 ---------- 2
        StringBuilder Builder;

        RoomDef Room;

        public void Build(FileInfo outFile, RoomDef room)
        {
            Room = room;

            Builder = new StringBuilder();

            // build the floor
            Verts.Add(new Vector3(0,0,room.Bounds.Height));
            Verts.Add(new Vector3(room.Bounds.Width, 0, room.Bounds.Height));
            Verts.Add(new Vector3(room.Bounds.Width, 0, 0));
            Verts.Add(new Vector3(0, 0, 0));

            Normals.Add(new Vector3(0, 1, 0));

            UVs.Add(new Vector2(0,room.Bounds.Height));
            UVs.Add(new Vector2(room.Bounds.Width, room.Bounds.Height));
            UVs.Add(new Vector2(room.Bounds.Width, 0));
            UVs.Add(new Vector2(0, 0));

            Builder.AppendLine("g " + room.FloorObjectName);
            Builder.AppendLine("usemtl " + room.FloorMaterialName);
            Builder.AppendLine("usemap " + room.FloorMaterialName);
            Builder.AppendLine("f 0//0//0 1//1//0 2//2//0 3//3//0");



            // build low walls
            // north

        }
    }
}
