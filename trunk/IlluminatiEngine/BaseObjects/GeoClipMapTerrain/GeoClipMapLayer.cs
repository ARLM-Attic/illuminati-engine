﻿#if WINDOWS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if KINECT
using IlluminatiEngine.Kinect;
#endif
namespace IlluminatiEngine
{
    public class GeoClipMapLayer : BaseDeferredObject
    {
        protected GeoClipMapFootPrintBlock[] blocks = new GeoClipMapFootPrintBlock[12];
        protected GeoClipMapFootPrintFixUp[] fixup = new GeoClipMapFootPrintFixUp[4];
        protected GeoClipMapFootPrintTrim[] trim = new GeoClipMapFootPrintTrim[4];

        protected Effect effect;

        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Quaternion Orientation = Quaternion.Identity;

        public Matrix World = Matrix.Identity;

        public Vector3 CorseScale = Vector3.One;

        public string TerrainMap;
        
        public GeoClipMapLayer(Game game, short n, string terrainAsset)
            : base(game)
        {
            TerrainMap = terrainAsset;

            short m = (short)((n + 1) / 4);
            short edge = (short)((m * 2) - 1);

            for (int b = 0; b < blocks.Length; b++)
            {
                blocks[b] = new GeoClipMapFootPrintBlock(game, m);

                switch (b)
                {
                    case 0:
                        blocks[b].Position = new Vector3(-edge, 0, -edge);
                        blocks[b].BlockType = BlockTypes.TopLeft;
                        break;
                    case 1:
                        blocks[b].Position = new Vector3(-m, 0, -edge);
                        blocks[b].BlockType = BlockTypes.Top;
                        break;
                    case 2:
                        blocks[b].Position = new Vector3(1, 0, -edge);
                        blocks[b].BlockType = BlockTypes.Top;
                        break;
                    case 3:
                        blocks[b].Position = new Vector3(m, 0, -edge);
                        blocks[b].BlockType = BlockTypes.TopRight;
                        break;
                    case 4:
                        blocks[b].Position = new Vector3(-edge, 0, -m);
                        blocks[b].BlockType = BlockTypes.Left;
                        break;
                    case 5:
                        blocks[b].Position = new Vector3(m, 0, -m);
                        blocks[b].BlockType = BlockTypes.Right;
                        break;
                    case 6:
                        blocks[b].Position = new Vector3(-edge, 0, 1);
                        blocks[b].BlockType = BlockTypes.Left;
                        break;
                    case 7:
                        blocks[b].Position = new Vector3(m, 0, 1);
                        blocks[b].BlockType = BlockTypes.Right;
                        break;
                    case 8:
                        blocks[b].Position = new Vector3(-edge, 0, m);
                        blocks[b].BlockType = BlockTypes.BottomLeft;
                        break;
                    case 9:
                        blocks[b].Position = new Vector3(-m, 0, m);
                        blocks[b].BlockType = BlockTypes.Bottom;
                        break;
                    case 10:
                        blocks[b].Position = new Vector3(1, 0, m);
                        blocks[b].BlockType = BlockTypes.Bottom;
                        break;
                    case 11:
                        blocks[b].Position = new Vector3(m, 0, m);
                        blocks[b].BlockType = BlockTypes.BottomRight;
                        break;
                }
                blocks[b].Orientation *= Orientation;
            }

            for (int b = 0; b < fixup.Length; b++)
            {
                switch (b)
                {
                    case 0:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, false);
                        fixup[b].Position = new Vector3(-1, 0, -edge);
                        fixup[b].FixUpType = FixUpTypes.Top;
                        break;
                    case 1:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, false);
                        fixup[b].Position = new Vector3(-1, 0, m);
                        fixup[b].FixUpType = FixUpTypes.Bottom;
                        break;
                    case 2:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, true);
                        fixup[b].Position = new Vector3(-edge, 0, -1);
                        fixup[b].FixUpType = FixUpTypes.Left;
                        break;
                    case 3:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, true);
                        fixup[b].Position = new Vector3(m, 0, -1);
                        fixup[b].FixUpType = FixUpTypes.Right;
                        break;
                }
                fixup[b].Orientation *= Orientation;
            }

            for (int b = 0; b < trim.Length; b++)
            {
                switch (b)
                {
                    case 0:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, false);
                        trim[b].Position = new Vector3(-m, 0, -m);
                        break;
                    case 1:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, false);
                        trim[b].Position = new Vector3(0, 0, -m);
                        break;
                    case 2:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, true);
                        trim[b].Position = new Vector3(m - 1, 0, -m);
                        break;
                    case 3:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, true);
                        trim[b].Position = new Vector3(m - 1, 0, 0);
                        break;
                }
                trim[b].Orientation *= Orientation;
            }
        }

        public GeoClipMapLayer(Game game, short n)
            : base(game)
        {
            short m = (short)((n + 1) / 4);
            short edge = (short)((m * 2) - 1);

            for (int b = 0; b < blocks.Length; b++)
            {
                blocks[b] = new GeoClipMapFootPrintBlock(game, m);

                switch (b)
                {
                    case 0:
                        blocks[b].Position = new Vector3(-edge, 0, -edge);
                        blocks[b].BlockType = BlockTypes.TopLeft;
                        break;
                    case 1:
                        blocks[b].Position = new Vector3(-m, 0, -edge);
                        blocks[b].BlockType = BlockTypes.Top;
                        break;
                    case 2:
                        blocks[b].Position = new Vector3(1, 0, -edge);
                        blocks[b].BlockType = BlockTypes.Top;
                        break;
                    case 3:
                        blocks[b].Position = new Vector3(m, 0, -edge);
                        blocks[b].BlockType = BlockTypes.TopRight;
                        break;
                    case 4:
                        blocks[b].Position = new Vector3(-edge, 0, -m);
                        blocks[b].BlockType = BlockTypes.Left;
                        break;
                    case 5:
                        blocks[b].Position = new Vector3(m, 0, -m);
                        blocks[b].BlockType = BlockTypes.Right;
                        break;
                    case 6:
                        blocks[b].Position = new Vector3(-edge, 0, 1);
                        blocks[b].BlockType = BlockTypes.Left;
                        break;
                    case 7:
                        blocks[b].Position = new Vector3(m, 0, 1);
                        blocks[b].BlockType = BlockTypes.Right;
                        break;
                    case 8:
                        blocks[b].Position = new Vector3(-edge, 0, m);
                        blocks[b].BlockType = BlockTypes.BottomLeft;
                        break;
                    case 9:
                        blocks[b].Position = new Vector3(-m, 0, m);
                        blocks[b].BlockType = BlockTypes.Bottom;
                        break;
                    case 10:
                        blocks[b].Position = new Vector3(1, 0, m);
                        blocks[b].BlockType = BlockTypes.Bottom;
                        break;
                    case 11:
                        blocks[b].Position = new Vector3(m, 0, m);
                        blocks[b].BlockType = BlockTypes.BottomRight;
                        break;
                }
                blocks[b].Orientation *= Orientation;
            }

            for (int b = 0; b < fixup.Length; b++)
            {
                switch (b)
                {
                    case 0:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, false);
                        fixup[b].Position = new Vector3(-1, 0, -edge);
                        fixup[b].FixUpType = FixUpTypes.Top;
                        break;
                    case 1:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, false);
                        fixup[b].Position = new Vector3(-1, 0, m);
                        fixup[b].FixUpType = FixUpTypes.Bottom;
                        break;
                    case 2:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, true);
                        fixup[b].Position = new Vector3(-edge, 0, -1);
                        fixup[b].FixUpType = FixUpTypes.Left;
                        break;
                    case 3:
                        fixup[b] = new GeoClipMapFootPrintFixUp(game, m, true);
                        fixup[b].Position = new Vector3(m, 0, -1);
                        fixup[b].FixUpType = FixUpTypes.Right;
                        break;
                }
                fixup[b].Orientation *= Orientation;
            }

            for (int b = 0; b < trim.Length; b++)
            {
                switch (b)
                {
                    case 0:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, false);
                        trim[b].Position = new Vector3(-m, 0, -m);
                        break;
                    case 1:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, false);
                        trim[b].Position = new Vector3(0, 0, -m);
                        break;
                    case 2:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, true);
                        trim[b].Position = new Vector3(m - 1, 0, -m);
                        break;
                    case 3:
                        trim[b] = new GeoClipMapFootPrintTrim(game, m, true);
                        trim[b].Position = new Vector3(m - 1, 0, 0);
                        break;
                }
                trim[b].Orientation *= Orientation;
            }
        }


        public override void Initialize()
        {
            base.Initialize();

            for (int b = 0; b < blocks.Length; b++)
                blocks[b].Initialize();

            for (int b = 0; b < fixup.Length; b++)
                fixup[b].Initialize();

            for (int b = 0; b < trim.Length; b++)
                trim[b].Initialize();

        }
        public Texture2D heightMap;
        protected override void LoadContent()
        {
            //effect = Game.Content.Load<Effect>("Shaders/Terrain/GeoClipMapLayer");
            effect = AssetManager.GetAsset<Effect>("Shaders/Terrain/GeoClipMapLayer");

        }

        public override void Update(GameTime gameTime)
        {
            World = Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Orientation) * Matrix.CreateTranslation(Position);
            base.Update(gameTime);

            for (int b = 0; b < blocks.Length; b++)
                blocks[b].Update(gameTime);

            for (int b = 0; b < fixup.Length; b++)
                fixup[b].Update(gameTime);

            for (int b = 0; b < trim.Length; b++)
                trim[b].Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (heightMap == null)
            {
                if (TerrainMap != null)
                    heightMap = AssetManager.GetAsset<Texture2D>(TerrainMap);
                else
                {
#if KINECT
                    heightMap = ((KinectComponent)Game.Services.GetService(typeof(KinectComponent))).depthMap;
#endif

                    if (heightMap == null)
                        return;
                }
                //heightMap = Game.Content.Load<Texture2D>("Textures/TerrainMaps/BigTest");

                Color[] data = new Color[heightMap.Width * heightMap.Height];
                heightMap.GetData<Color>(data);

                heightMap = new Texture2D(Game.GraphicsDevice, heightMap.Width, heightMap.Height, true, SurfaceFormat.Vector4);
                Vector4[] data2 = new Vector4[data.Length];
                for (int x = 0; x < data.Length; x++)
                    data2[x] = new Vector4(data[x].R / 255f, data[x].G / 255f, data[x].B / 255f, data[x].A / 255f);
                heightMap.SetData<Vector4>(data2);

                //FileStream f = new FileStream("height.jpg", FileMode.Create);
                //heightMap.SaveAsJpeg(f, heightMap.Width, heightMap.Height);
                //f.Close();

                if (effect.Parameters["heightMap"] != null)
                {
                    effect.Parameters["heightMap"].SetValue(heightMap);

                    effect.Parameters["scale"].SetValue(Scale);
                    effect.Parameters["scale2"].SetValue(CorseScale);

                    effect.Parameters["sqrt"].SetValue((new Vector2(heightMap.Width, heightMap.Height) / 2));

                    effect.Parameters["halfPixel"].SetValue(-new Vector2(.5f / (float)Game.GraphicsDevice.Viewport.Width,
                                             .5f / (float)Game.GraphicsDevice.Viewport.Height));
                    effect.Parameters["hp"].SetValue(new Vector2(heightMap.Width, heightMap.Height) / 2);
                    effect.Parameters["OneOverWidth"].SetValue(1f / heightMap.Width);
                }

                if (effect.Parameters["LayerMap0"] != null)
                {
                    effect.Parameters["LayerMap0"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/dirt"));
                    effect.Parameters["BumpMap0"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/dirtNormal"));

                    effect.Parameters["LayerMap1"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/grass"));
                    effect.Parameters["BumpMap1"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/grassNormal"));

                    effect.Parameters["LayerMap2"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/stone"));
                    effect.Parameters["BumpMap2"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/stoneNormal"));

                    effect.Parameters["LayerMap3"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/snow2"));
                    effect.Parameters["BumpMap3"].SetValue(AssetManager.GetAsset<Texture2D>("Textures/Terrain/snow2Normal"));

                    effect.Parameters["mod"].SetValue((heightMap.Width + heightMap.Height) / 2);

                }
            }

            effect.Parameters["world"].SetValue(World);
            effect.Parameters["wvp"].SetValue(World * Camera.View * Camera.Projection);

            if (effect.Parameters["vp"] != null)
                effect.Parameters["vp"].SetValue(Camera.View * Camera.Projection);

            if (effect.Parameters["EyePosition"] != null)
                effect.Parameters["EyePosition"].SetValue(Camera.Position);

            effect.CurrentTechnique.Passes[0].Apply();

            for (int b = 0; b < blocks.Length; b++)
            {
                if (Camera.Frustum.Intersects(blocks[b].getBounds(World)))
                    blocks[b].Draw(gameTime);
            }

            for (int b = 0; b < fixup.Length; b++)
            {
                if (Camera.Frustum.Intersects(fixup[b].getBounds(World)))
                    fixup[b].Draw(gameTime);
            }

            for (int b = 0; b < trim.Length; b++)
            {
                if (Camera.Frustum.Intersects(trim[b].getBounds(World)))
                    trim[b].Draw(gameTime);
            }
        }

        public void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Orientation));
            Orientation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * Orientation);
        }
    }
}
#endif