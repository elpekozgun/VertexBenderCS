﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static partial class ObjectLoader
    {
        public static Mesh LoadOff(string path)
        {
            string[] a = new string[1];
            try
            {
                a = File.ReadLines(path).ToArray();
            }
            catch (Exception)
            {
                throw new FileNotFoundException("file not found");
            }

            if (a[0].ToUpper() != "OFF")
            {
                throw new FileLoadException("Incorrect OFF format");
            }
            var line = a[1].Split(new char[]{ ' '}, StringSplitOptions.RemoveEmptyEntries);

            Mesh mesh = new Mesh();

            int nVerts =  int.Parse(line[0]);
            int nTris =  int.Parse(line[1]);
            int useless = int.Parse(line[2]);
            int i = 2;

            while (i < nVerts + 2)
            {
                line = a[i].Split(new char[]{ ' '}, StringSplitOptions.RemoveEmptyEntries);

                var v1 = float.Parse(line[0]);
                var v2 = float.Parse(line[2]);
                var v3 = float.Parse(line[1]);

                mesh.AddVertex(v1 / 100, v2 / 100, v3 / 100);
                i++;
            }

            while (i < nVerts + nTris + 2)
            {
                line = a[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var v1 = int.Parse(line[1]);
                var v2 = int.Parse(line[2]);
                var v3 = int.Parse(line[3]);

                mesh.AddTriangle(v1, v2, v3);
                i++;
            }

            mesh.CalculateVertexNormals();
            mesh.Center();
            return mesh;
        }

        public static Mesh LoadObj(string path)
        {
            return null;
        }

        public static Mesh LoadDae(string path)
        {
            return null;
        }

        public static Mesh LoadVol(string path, out List<int> colorBuffer)
        {
            string expectedHeader = "KRETZFILE 1.0";

            var firstLine = File.ReadLines(path).First();
            var headerTexts = firstLine.Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
            var header = $"{headerTexts[0]} {headerTexts[1]}";

            colorBuffer = new List<int>();
            if (expectedHeader != header)
            {
                Logger.Log("Not a kretzfile");
                return null;
            }
            

            // KretzFile 1.0 Specific Tags in binary
            KretzTag patientTag             = new KretzTag(0x0110, 0x0002);
            KretzTag dimensionXTag          = new KretzTag(0xc000, 0x0001);
            KretzTag dimensionYTag          = new KretzTag(0xc000, 0x0002);
            KretzTag dimensionZTag          = new KretzTag(0xc000, 0x0003);
            KretzTag resolutionTag          = new KretzTag(0xc100, 0x0001);
            KretzTag offset1Tag             = new KretzTag(0xc200, 0x0001);
            KretzTag offset2Tag             = new KretzTag(0xc200, 0x0002);
            KretzTag anglesPhiTag           = new KretzTag(0xc300, 0x0001);
            KretzTag anglesThetaTag         = new KretzTag(0xc300, 0x0002);
            KretzTag cartesianSpacingTag    = new KretzTag(0x0010, 0x0022);
            KretzTag voxelTag               = new KretzTag(0xd000, 0x0001);
            //KretzTag imageTag           = new KretzTag(0xd000, 0x0001);
            //KretzTag image4dTag = new KretzTag(0xd600, 0x0001);
            //KretzTag cineFramesTag = new KretzTag(0xd400, 0x0001);
            //KretzTag sizeFramesTag = new KretzTag(0xd400, 0x0002);
            //KretzTag timingFramesTag = new KretzTag(0xd400, 0x0005);
            //KretzTag dimensionXTagDoppler   = new KretzTag(0xc000, 0x0201);
            //KretzTag dimensionYTagDoppler   = new KretzTag(0xc000, 0x0202);
            //KretzTag dimensionZTagDoppler   = new KretzTag(0xc000, 0x2003);
            //KretzTag resolutionTagDoppler   = new KretzTag(0xc100, 0x0201);
            //KretzTag offset1TagDoppler      = new KretzTag(0xc200, 0x0201);
            //KretzTag offset2TagDoppler      = new KretzTag(0xc200, 0x0202);
            //KretzTag anglesPhiTagDoppler    = new KretzTag(0xc300, 0x0201);
            //KretzTag anglesThetaTagDoppler  = new KretzTag(0xc300, 0x0202);
            //KretzTag imageTagDoppler        = new KretzTag(0xd000, 0x0201);
            //KretzTag image4dTagDoppler      = new KretzTag(0xd600, 0x0201);
            using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                b.BaseStream.Seek(5, SeekOrigin.End);
                var endofTag = b.BaseStream.Position;
                b.BaseStream.Seek(16, SeekOrigin.Begin);

                
                List<double> thetaAngles = new List<double>();
                List<double> phiAngles = new List<double>();
                List<KeyValuePair<OpenTK.Vector3, int>> intensities = new List<KeyValuePair<OpenTK.Vector3, int>>();
                byte[] buffer = new byte[b.BaseStream.Length];

                ushort dimensionX = 0;
                ushort dimensionY = 0;
                ushort dimensionZ = 0;

                double offset1 = 0.0;
                double offset2 = 0.0;
                double resolution = 1.0;
                double cartesianSpacing = 1.0;

                ushort tagShort1 = 0;
                ushort tagShort2 = 0;
                uint tagLength = 0;


                while (b.BaseStream.Position < endofTag - 8 && b.BaseStream.Position > -1)
                {
                    int shortStart = (int)b.BaseStream.Position;
                    int lengthStart = (int)b.BaseStream.Position + 2 * sizeof(ushort);

                    b.BaseStream.Read(buffer, (int)b.BaseStream.Position, 2 * sizeof(ushort));
                    b.BaseStream.Read(buffer, (int)b.BaseStream.Position, sizeof(uint));

                    tagShort1 = BitConverter.ToUInt16(buffer, shortStart);
                    tagShort2 = BitConverter.ToUInt16(buffer, shortStart + sizeof(ushort));
                    tagLength = BitConverter.ToUInt32(buffer, lengthStart);

                    KretzTag tag = new KretzTag(tagShort1, tagShort2);

                    int startIndex = (int)b.BaseStream.Position;
                    if (tag.Equals(patientTag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        byte[] name = new byte[tagLength];
                        for (int i = 0; i < tagLength; i++)
                        {
                            name[i] = buffer[startIndex + i];
                        }

                        Logger.Log($"Patient: {Encoding.Default.GetString(name)}");
                    }
                    else if (tag.Equals(dimensionXTag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        dimensionX = BitConverter.ToUInt16(buffer, startIndex);

                        Logger.Log($"Dimension X: {dimensionX}");
                    }
                    else if (tag.Equals(dimensionYTag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        dimensionY = BitConverter.ToUInt16(buffer, startIndex);

                        Logger.Log($"Dimension Y: {dimensionY}");
                    }
                    else if (tag.Equals(dimensionZTag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        dimensionZ = BitConverter.ToUInt16(buffer, startIndex);

                        Logger.Log($"Dimension Z: {dimensionZ}");
                    }
                    else if (tag.Equals(resolutionTag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        resolution = BitConverter.ToDouble(buffer, startIndex);

                        Logger.Log($"Resolution : {resolution}");
                    }
                    else if (tag.Equals(offset1Tag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        offset1 = BitConverter.ToDouble(buffer, startIndex);

                        Logger.Log($"offset1 : {offset1}");
                    }
                    else if (tag.Equals(offset2Tag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        offset2 = BitConverter.ToDouble(buffer, startIndex);

                        Logger.Log($"offset2 : {offset2}");
                    }
                    else if(tag.Equals(anglesPhiTag))
                    {
                        int len = (int)(tagLength / sizeof(double));
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        for (int i = 0; i < len; i++)
                        {
                            phiAngles.Add(BitConverter.ToDouble(buffer, startIndex + i * 8));
                        }
                    }
                    else if (tag.Equals(anglesThetaTag))
                    {
                        int len = (int)(tagLength / sizeof(double));
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        for (int i = 0; i < len; i++)
                        {
                            thetaAngles.Add(BitConverter.ToDouble(buffer, startIndex + i * 8));
                        }
                    }
                    else if (tag.Equals(voxelTag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);

                        for (int z = 0; z < dimensionZ; z++)
                        {
                            for (int y = 0; y < dimensionY; y++)
                            {
                                for (int x = 0; x < dimensionX; x++)
                                {
                                    var intensity = buffer[startIndex + (z * dimensionX * dimensionY) + (y * dimensionX) + x];
                                    {
                                        intensities.Add(new KeyValuePair<OpenTK.Vector3, int>(new OpenTK.Vector3(x, y, z), intensity));
                                    }
                                }
                            }
                        }
                    }
                    else if (tag.Equals(cartesianSpacingTag))
                    {
                        b.BaseStream.Read(buffer, (int)b.BaseStream.Position, (int)tagLength);
                        cartesianSpacing = BitConverter.ToDouble(buffer, startIndex);

                        Logger.Log($"Cartesian Spacing: {cartesianSpacing}");
                    }
                    else
                    {
                        b.BaseStream.Seek(tagLength, SeekOrigin.Current);
                    }

                }

                Mesh mesh = new Mesh();
                for (int i = 0; i < intensities.Count; i++)
                {
                    mesh.AddVertex(new Vertex(i, intensities[i].Key * -(float)resolution / (float)cartesianSpacing));
                    colorBuffer.Add(intensities[i].Value);
                }
                return mesh;
            }
        }

        public static Mesh LoadDicom(string path)
        {
            return null;
        }

    }
}
