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

using OpenTK;

namespace Models
{
    public class Model
    {
        public string Name = string.Empty;

        public class GeometryData
        {
            public List<Vector3> Vertecies = new List<Vector3>();
            public List<Vector3> Normals = new List<Vector3>();
            public List<Vector2> UVs = new List<Vector2>();
        }

        public GeometryData Geometry = new GeometryData();

        public Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
       
        protected Model(string name)
        {
            ModelList.Add(this);
        }

        public virtual void GeometryFinished() // for derived classes to build lists or VBOs
        {
            foreach (Mesh mesh in Meshes.Values)
                mesh.GeometryFinished();
        }

        // model manager
        public static List<Model> ModelList = new List<Model>();

        public static int FindModel(string name)
        {
            return ModelList.FindIndex(delegate(Model m) { return m.Name == name; });
        }

        public delegate Model ModelCreator(string name);
        public static ModelCreator ModelFactory = null;


        public delegate Mesh MeshCreator();
        public static MeshCreator MeshFactory = null;

        public Mesh NewMesh()
        {
            Mesh m = null;
            if (MeshFactory == null)
                m = new Mesh();
            else
                m = MeshFactory();

            m.Geometry = Geometry;
            return m;
        }

        public static Model NewModel(string name)
        {
            if (ModelFactory == null)
                return new Model(name);
            else
                return ModelFactory(name);
        }

    }
}
