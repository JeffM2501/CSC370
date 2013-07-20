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

namespace Models
{
    public class Material
    {
        public UInt32 ID = 0;
        public string Name = string.Empty;
        public string TextureName = string.Empty;
        public Color DiffuseColor = Color.White;

        // material manager
        public static List<Material> Materials = new List<Material>();

        public static int FindMaterial(string name)
        {
            return Materials.FindIndex(delegate(Material m) { return m.Name == name; });
        }

        public delegate Material MaterialCreator(string name);

        public static MaterialCreator Factory = null;

        public static Material New(string name)
        {
            if (Factory == null)
                return new Material(name);
            else
                return Factory(name);
        }

        protected Material(string name)
        {
            Name = name;
            Materials.Add(this);
        }
    }
}
