using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using OpenTK;

namespace Models
{
    public class OBJReader : ModelReader
    {
        public override Model Read(string name, FileInfo file)
        {
            Model model = base.Read(name, file);

            try
            {
                StreamReader reader = file.OpenText();

                List<string> matFiles = new List<string>();

                Mesh mesh = model.NewMesh();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (line == string.Empty || line[0] == '#')
                        continue;

                    string[] lineParts = line.Split(" ".ToCharArray());
                    if (lineParts.Length == 1)
                        continue;

                    string code = lineParts[0].ToLower();

                    if (code == "v")
                        model.Geometry.Vertecies.Add(ReadVector3(lineParts, 1));
                    else if (code == "vn")
                        model.Geometry.Normals.Add(ReadVector3(lineParts, 1));
                    else if (code == "vt")
                    {
                        Vector2 uv = ReadVector2(lineParts, 1);
                        uv.Y = 1 - uv.Y;
                        model.Geometry.UVs.Add(uv);
                    }
                    else if (code == "usemtl")
                    {
                        string matName = lineParts[1];
                        if (mesh.MaterialName == string.Empty)
                        {
                            mesh.MaterialName = matName;
                            model.Meshes.Add(matName, mesh);
                        }
                        else
                        {
                            if (model.Meshes.ContainsKey(matName))
                                mesh = model.Meshes[matName];
                            else
                            {
                                mesh = model.NewMesh();
                                mesh.MaterialName = matName;
                                model.Meshes.Add(matName, mesh);
                            }
                        }
                    }
                    else if (code == "f")
                    {
                        if (lineParts.Length < 4)
                            continue;

                        Mesh.Face face = new Mesh.Face();

                        for (int i = 1; i < lineParts.Length; i++)
                        {
                            if (lineParts[i].Contains('#'))
                                continue;

                            string[] vertParts = lineParts[i].Trim().Split("/".ToCharArray());

                            int vert = 0;
                            int norm = 0;
                            int uv = 0;

                            int.TryParse(vertParts[0], out vert);
                            if (vert < 0)
                                vert = model.Geometry.Vertecies.Count + vert;
                            else
                                vert--;

                            if (vertParts.Length == 2)
                            {
                                int.TryParse(vertParts[1], out uv);
                                if (norm < 0)
                                    norm = model.Geometry.UVs.Count + uv;
                                else
                                    norm--;
                            }
                            else
                            {
                                int.TryParse(vertParts[2], out norm);
                                if (norm < 0)
                                    norm = model.Geometry.Normals.Count + norm;
                                else
                                    norm--;

                                int.TryParse(vertParts[1], out uv);
                                if (uv < 0)
                                    uv = model.Geometry.UVs.Count + uv;
                                else
                                    uv--;
                            }


                            face.Vertecies.Add(new Mesh.Face.Indecies((UInt16)vert, (UInt16)norm, (UInt16)uv));
                        }

                        if (face.Vertecies.Count > 2)
                            mesh.Faces.Add(face);
                    }
                }

                reader.Close();
            }
            catch (System.Exception /*ex*/)
            {
                return null;
            }
            return model;
        }

        public static void ReadMaterials(string relativePath, FileInfo file)
        {
            StreamReader reader = file.OpenText();

            List<string> matFiles = new List<string>();

            Material mat = null;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().Trim();

                if (line == string.Empty || line[0] == '#')
                    continue;

                string[] lineParts = line.Split(" ".ToCharArray());
                if (lineParts.Length == 1)
                    continue;

                string code = lineParts[0].ToLower();

                if (code == "newmtl")
                    mat = Material.New(lineParts[1]);
                else if (code == "kd")
                {
                    Vector3 c = ReadVector3(lineParts, 1);
                    mat.DiffuseColor = Color.FromArgb(Byte.MaxValue, (int)(Byte.MaxValue * c.X), (int)(Byte.MaxValue * c.Y), (int)(Byte.MaxValue * c.Z));
                }
                else if (code == "map_kd")
                    mat.TextureName = relativePath + "/" + lineParts[1];
            }

            reader.Close();
        }
    }
}
