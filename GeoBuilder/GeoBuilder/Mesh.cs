/*
        Copyright (C) 2013 Jeffry Myers
        Author: Jeffry Myers
 * 
        Note this code file does NOT derive from any previous work.
 * 
	    This is free software; you can redistribute it and/or modify
        it under the terms of the GNU General Public License as published by
        the Free Software Foundation; either version 2 of the License, or
        (at your option) any later version.

        This software is distributed in the hope that it will be useful,
        but WITHOUT ANY WARRANTY; without even the implied warranty of
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        GNU General Public License for more details.

        You should have received a copy of the GNU General Public License
        along with Spacenerds in Space; if not, write to the Free Software
        Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using OpenTK;

namespace Models
{
    public class Mesh
    {
        public string MaterialName = string.Empty;

        public Model.GeometryData Geometry = new Model.GeometryData();

        public override int GetHashCode()
        {
            return MaterialName.GetHashCode() ^ Geometry.Vertecies.GetHashCode() ^ Geometry.Normals.GetHashCode() ^ Geometry.UVs.GetHashCode();
        }

        public class Face
        {
            public class Indecies
            {
                public UInt16 Vertex = UInt16.MinValue;
                public UInt16 Normal = UInt16.MinValue;
                public UInt16 UV = UInt16.MinValue;

                public Indecies() { }
                public Indecies(UInt16 v, UInt16 n)
                {
                    Vertex = v;
                    Normal = n;
                }
                public Indecies(UInt16 v, UInt16 n, UInt16 u)
                {
                    Vertex = v;
                    Normal = n;
                    UV = u;
                }
            }
            public List<Indecies> Vertecies = new List<Indecies>();
        }

        public List<Face> Faces = new List<Face>();

        public UInt16 FindVert(Vector3 v)
        {
            UInt16 index = (UInt16)Geometry.Vertecies.FindIndex(delegate(Vector3 p) { return v.Equals(p); });
            if (index >= 0)
                return index;

            Geometry.Vertecies.Add(v);
            return (UInt16)(Geometry.Vertecies.Count - 1);
        }

        public UInt16 FindNormal(Vector3 v)
        {
            UInt16 index = (UInt16)Geometry.Normals.FindIndex(delegate(Vector3 p) { return v.Equals(p); });
            if (index >= 0)
                return index;

            Geometry.Normals.Add(v);
            return (UInt16)(Geometry.Normals.Count - 1);
        }

        public UInt16 FindUV(Vector2 v)
        {
            UInt16 index = (UInt16)Geometry.UVs.FindIndex(delegate(Vector2 p) { return v.Equals(p); });
            if (index >= 0)
                return index;

            Geometry.UVs.Add(v);
            return (UInt16)(Geometry.UVs.Count - 1);
        }

        public virtual void GeometryFinished() // for derived classes to build lists or VBOs
        {
        }
    }
}
