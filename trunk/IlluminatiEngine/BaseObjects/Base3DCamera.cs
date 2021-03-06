﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IlluminatiEngine
{
    public class Base3DCamera : GameComponent, ICameraService
    {
        /// <summary>
        /// Position
        /// </summary>
        protected Vector3 position;
        /// <summary>
        /// Scale
        /// </summary>
        protected Vector3 scale;
        /// <summary>
        /// Rotation
        /// </summary>
        protected Quaternion rotation;
        /// <summary>
        /// World
        /// </summary>
        public Matrix world;
        #region I3DCamera Members

        /// <summary>
        /// Position
        /// </summary>
        public Vector3 Position
        {
            get
            { return position; }
            set
            { position = value; }
        }
        /// <summary>
        /// Scale
        /// </summary>
        public Vector3 Scale
        {
            get
            { return scale; }
            set
            { scale = value; }
        }
        /// <summary>
        /// Rotation
        /// </summary>
        public Quaternion Rotation
        {
            get
            { return rotation; }
            set
            { rotation = value; }
        }
        /// <summary>
        /// World
        /// </summary>
        public Matrix World
        {
            get { return world; }
        }

        #endregion

        /// <summary>
        /// View
        /// </summary>
        protected Matrix view;
        /// <summary>
        /// View
        /// </summary>
        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }
        /// <summary>
        /// Projection
        /// </summary>
        protected Matrix projection;
        /// <summary>
        /// Projection
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
        }
        /// <summary>
        /// View port
        /// </summary>
        protected Viewport viewport;
        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        /// <summary>
        /// Frustum
        /// </summary>
        public BoundingFrustum Frustum
        {
            get
            {
                return new BoundingFrustum(Matrix.Multiply(View, Projection));
            }
        }

        /// <summary>
        /// View ports min depth
        /// </summary>
        protected float minDepth;
        /// <summary>
        /// Viewports max depth.
        /// </summary>
        protected float maxDepth;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="minDepth"></param>
        /// <param name="maxDepth"></param>
        public Base3DCamera(Game game, float minDepth, float maxDepth)
            : base(game)
        {
            position = Vector3.Zero;
            scale = Vector3.One;
            rotation = Quaternion.Identity;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;

            game.Components.Add(this);
            game.Services.AddService(typeof(ICameraService), this);
        }
        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize()
        {
            viewport = Game.GraphicsDevice.Viewport;
            viewport.MinDepth = minDepth;
            viewport.MaxDepth = maxDepth;
        }
        /// <summary>
        /// Method to turn the camera at a target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="speed"></param>
        public virtual void LookAt(Vector3 target, float speed, Vector3 fwd)
        {
            GameComponentHelper.LookAt(target, speed, Position, ref rotation, fwd);
        }

        public virtual void LookAtLockRotation(Vector3 target, float speed, Vector3 fwd, Vector3 lockedRots)
        {
            GameComponentHelper.LookAtLockRotation(target, speed, Position, ref rotation, fwd, lockedRots);
        }
        /// <summary>
        /// Method to update.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            world = Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(Position);
            view = Matrix.Invert(world);

            //projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3.0f, (float)Viewport.Width / (float)Viewport.Height, Viewport.MinDepth, Viewport.MaxDepth);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Viewport.AspectRatio, Viewport.MinDepth, Viewport.MaxDepth);
            base.Update(gameTime);
        }
        public virtual Vector3 GetIntPosition()
        {
            return new Vector3((int)(Position.X + .5f), (int)(Position.Y + .5f), (int)(Position.Z + .5f));
        }
        /// <summary>
        /// Method to translate object
        /// </summary>
        /// <param name="distance"></param>
        public virtual void Translate(Vector3 distance)
        {
            Position += GameComponentHelper.Translate3D(distance, rotation);
        }
        public virtual void TranslateAA(Vector3 distance)
        {
            Position += GameComponentHelper.Translate3D(distance);
        }
        /// <summary>
        /// Method to rotate object
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        public virtual void Rotate(Vector3 axis, float angle)
        {
            GameComponentHelper.Rotate(axis, angle, ref rotation);
        }

        public virtual void Dispose()
        {
            base.Dispose();

            Game.Components.Remove(this);
            Game.Services.RemoveService(typeof(ICameraService));
        }

    }
}
