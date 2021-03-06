﻿using BulletXNA;
using BulletXNA.BulletCollision;
using BulletXNA.BulletDynamics;
using BulletXNA.LinearMath;

namespace BulletXNADemos.Demos
{
    public class IslandStressDemo : DemoApplication
    {
        const float SCALING = 1.0f;
        public override void InitializeDemo()
        {
            base.InitializeDemo();
            SetCameraDistance(SCALING * 50f);

            //string filename = @"C:\users\man\bullett\xna-largemesh-output.txt";
            //FileStream filestream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read);
            //BulletGlobals.g_streamWriter = new StreamWriter(filestream);

            ///collision configuration contains default setup for memory, collision setup
            m_collisionConfiguration = new DefaultCollisionConfiguration();

            ///use the default collision dispatcher. For parallel processing you can use a diffent dispatcher (see Extras/BulletMultiThreaded)
            m_dispatcher = new CollisionDispatcher(m_collisionConfiguration);

            m_broadphase = new DbvtBroadphase();
            IOverlappingPairCache pairCache = null;
            //pairCache = new SortedOverlappingPairCache();

            //m_broadphase = new SimpleBroadphase(10000, pairCache);

            ///the default constraint solver. For parallel processing you can use a different solver (see Extras/BulletMultiThreaded)
            SequentialImpulseConstraintSolver sol = new SequentialImpulseConstraintSolver();
            m_constraintSolver = sol;

            m_dynamicsWorld = new DiscreteDynamicsWorld(m_dispatcher, m_broadphase, m_constraintSolver, m_collisionConfiguration);

            IndexedVector3 gravity = new IndexedVector3(0, -10, 0);
            m_dynamicsWorld.SetGravity(ref gravity);

            m_profileManager = new BasicProfileManager();
            BulletGlobals.g_profileManager = m_profileManager;
            m_profileIterator = m_profileManager.getIterator();

            ///create a few basic rigid bodies
            IndexedVector3 halfExtents = new IndexedVector3(50, 50, 50);
            //IndexedVector3 halfExtents = new IndexedVector3(10, 10, 10);
            //CollisionShape groundShape = new BoxShape(ref halfExtents);
            CollisionShape groundShape = new StaticPlaneShape(new IndexedVector3(0,1,0), 0);
            LocalCreateRigidBody(0f, IndexedMatrix.Identity, groundShape);
            //CollisionShape groundShape = BuildLargeMesh();
            m_collisionShapes.Add(groundShape);
            CollisionShape sphereShape = new SphereShape(0.2f);
            int size = 16;// 5; // 16
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    IndexedMatrix m = IndexedMatrix.CreateTranslation(new IndexedVector3(i, 1, j));
                    RigidBody rb = LocalCreateRigidBody(1f, m, sphereShape);
                    rb.SetActivationState(ActivationState.ISLAND_SLEEPING);
                }
            }

            ClientResetScene();

        }


        static void Main(string[] args)
        {
            using (IslandStressDemo game = new IslandStressDemo())
            {
                game.Run();
            }
        }

    }
}
