using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace LibSM64
{
    internal static class Interop
    {
        public const float SCALE_FACTOR = 2500.0f; //oriignal 100

        public const int SM64_TEXTURE_WIDTH  = 64 * 11;
        public const int SM64_TEXTURE_HEIGHT = 64;
        public const int SM64_GEO_MAX_TRIANGLES = 1024;

        public const int SM64_MAX_HEALTH = 8;

        [StructLayout(LayoutKind.Sequential)]
        public struct SM64Surface
        {
            public short type;
            public short force;
            public ushort terrain;
            public short v0x, v0y, v0z;
            public short v1x, v1y, v1z;
            public short v2x, v2y, v2z;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SM64MarioInputs
        {
            public float camLookX, camLookZ;
            public float stickX, stickY;
            public byte buttonA, buttonB, buttonZ;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SM64MarioState
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] position;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] velocity;
            public float faceAngle;
            public short health;

            public Vector3 unityPosition {
                get { return position != null ? new Vector3( -position[0], position[1], position[2] ) / SCALE_FACTOR : Vector3.zero; }
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        struct SM64MarioGeometryBuffers
        {
            public IntPtr position;
            public IntPtr normal;
            public IntPtr color;
            public IntPtr uv;
            public ushort numTrianglesUsed;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct SM64ObjectTransform
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            float[] position;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            float[] eulerRotation;

            static public SM64ObjectTransform FromUnityWorld( Vector3 position, Quaternion rotation )
            {
                float[] vecToArr( Vector3 v )
                {
                    return new float[] { v.x, v.y, v.z };
                }

                float fmod( float a, float b )
                {
                    return a - b * Mathf.Floor( a / b );
                }
                
                float fixAngle( float a )
                {
                    return fmod( a + 180.0f, 360.0f ) - 180.0f;
                }

                var pos = SCALE_FACTOR * Vector3.Scale( position, new Vector3( -1, 1, 1 ));
                var rot = Vector3.Scale( rotation.eulerAngles, new Vector3( -1, 1, 1 ));

                rot.x = fixAngle( rot.x );
                rot.y = fixAngle( rot.y );
                rot.z = fixAngle( rot.z );

                return new SM64ObjectTransform {
                    position = vecToArr( pos ),
                    eulerRotation = vecToArr( rot )
                };
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        struct SM64SurfaceObject
        {
            public SM64ObjectTransform transform;
            public uint surfaceCount;
            public IntPtr surfaces;
        }
        private const string LIBRARY_NAME = "sm64";

        [DllImport(LIBRARY_NAME)]
        static extern void sm64_global_init( IntPtr rom, IntPtr outTexture, IntPtr debugPrintFunctionPtr );
        [DllImport(LIBRARY_NAME)]
        static extern void sm64_global_terminate();

        [DllImport(LIBRARY_NAME)]
        static extern void sm64_static_surfaces_load( SM64Surface[] surfaces, ulong numSurfaces );

        [DllImport(LIBRARY_NAME)]
        static extern uint sm64_mario_create( short marioX, short marioY, short marioZ );
        [DllImport(LIBRARY_NAME)]
        static extern void sm64_mario_tick( uint marioId, ref SM64MarioInputs inputs, ref SM64MarioState outState, ref SM64MarioGeometryBuffers outBuffers );
        [DllImport(LIBRARY_NAME)]
        static extern void sm64_mario_delete( uint marioId );

        [DllImport(LIBRARY_NAME)]
        static extern uint sm64_surface_object_create( ref SM64SurfaceObject surfaceObject );
        [DllImport(LIBRARY_NAME)]
        static extern void sm64_surface_object_move( uint objectId, ref SM64ObjectTransform transform );
        [DllImport(LIBRARY_NAME)]
        static extern void sm64_surface_object_delete( uint objectId );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void DebugPrintFuncDelegate(string str);

        static public Texture2D marioTexture { get; private set; }
        static public bool isGlobalInit { get; private set; }
        [AOT.MonoPInvokeCallback (typeof(DebugPrintFuncDelegate))]
        static void debugPrintCallback(string str)
        {
            Debug.Log("libsm64: " + str);
        }

        public static void GlobalInit( byte[] rom )
        {
            var callbackDelegate = new DebugPrintFuncDelegate( debugPrintCallback );
            var romHandle = GCHandle.Alloc( rom, GCHandleType.Pinned );
            var textureData = new byte[ 4 * SM64_TEXTURE_WIDTH * SM64_TEXTURE_HEIGHT ];
            var textureDataHandle = GCHandle.Alloc( textureData, GCHandleType.Pinned );

            sm64_global_init( romHandle.AddrOfPinnedObject(), textureDataHandle.AddrOfPinnedObject(), Marshal.GetFunctionPointerForDelegate( callbackDelegate ));

            Color32[] cols = new Color32[ SM64_TEXTURE_WIDTH * SM64_TEXTURE_HEIGHT ];
            marioTexture = new Texture2D( SM64_TEXTURE_WIDTH, SM64_TEXTURE_HEIGHT );
            for( int ix = 0; ix < SM64_TEXTURE_WIDTH; ix++)
            for( int iy = 0; iy < SM64_TEXTURE_HEIGHT; iy++)
            {
                cols[ix + SM64_TEXTURE_WIDTH*iy] = new Color32(
                    textureData[4*(ix + SM64_TEXTURE_WIDTH*iy)+0],
                    textureData[4*(ix + SM64_TEXTURE_WIDTH*iy)+1],
                    textureData[4*(ix + SM64_TEXTURE_WIDTH*iy)+2],
                    textureData[4*(ix + SM64_TEXTURE_WIDTH*iy)+3]
                );
            }
            marioTexture.SetPixels32( cols );
            marioTexture.Apply();

            romHandle.Free();
            textureDataHandle.Free();

            isGlobalInit = true;
        }

        public static void GlobalTerminate()
        {
            sm64_global_terminate();
            marioTexture = null;
            isGlobalInit = false;
        }

        public static void StaticSurfacesLoad( SM64Surface[] surfaces )
        {
            sm64_static_surfaces_load( surfaces, (ulong)surfaces.Length );
        }

        public static uint MarioCreate( Vector3 marioPos )
        {
            return sm64_mario_create( (short)marioPos.x, (short)marioPos.y, (short)marioPos.z );
        }

        public static SM64MarioState MarioTick( uint marioId, SM64MarioInputs inputs, Vector3[] positionBuffer, Vector3[] normalBuffer, Vector3[] colorBuffer, Vector2[] uvBuffer )
        {
            SM64MarioState outState = new SM64MarioState();

            var posHandle = GCHandle.Alloc( positionBuffer, GCHandleType.Pinned );
            var normHandle = GCHandle.Alloc( normalBuffer, GCHandleType.Pinned );
            var colorHandle = GCHandle.Alloc( colorBuffer, GCHandleType.Pinned );
            var uvHandle = GCHandle.Alloc( uvBuffer, GCHandleType.Pinned );

            SM64MarioGeometryBuffers buff = new SM64MarioGeometryBuffers
            {
                position = posHandle.AddrOfPinnedObject(),
                normal = normHandle.AddrOfPinnedObject(),
                color = colorHandle.AddrOfPinnedObject(),
                uv = uvHandle.AddrOfPinnedObject()
            };

            sm64_mario_tick( marioId, ref inputs, ref outState, ref buff );

            posHandle.Free();
            normHandle.Free();
            colorHandle.Free();
            uvHandle.Free();

            return outState;
        }

        public static void MarioDelete( uint marioId )
        {
            sm64_mario_delete( marioId );
        }

        public static uint SurfaceObjectCreate( Vector3 position, Quaternion rotation, SM64Surface[] surfaces )
        {
            var surfListHandle = GCHandle.Alloc( surfaces, GCHandleType.Pinned );
            var t = SM64ObjectTransform.FromUnityWorld( position, rotation );

            SM64SurfaceObject surfObj = new SM64SurfaceObject
            {
                transform = t,
                surfaceCount = (uint)surfaces.Length,
                surfaces = surfListHandle.AddrOfPinnedObject()
            };

            uint result = sm64_surface_object_create( ref surfObj );

            surfListHandle.Free();

            return result;
        }

        public static void SurfaceObjectMove( uint id, Vector3 position, Quaternion rotation )
        {
            var t = SM64ObjectTransform.FromUnityWorld( position, rotation );
            sm64_surface_object_move( id, ref t );
        }

        public static void SurfaceObjectDelete( uint id )
        {
            sm64_surface_object_delete( id );
        }
    }
}
