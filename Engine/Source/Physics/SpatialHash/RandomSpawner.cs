
/*
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Random = Unity.Mathematics.Random;


namespace Test.CollisionDetector
{
    public class RandomSpawner:MonoBehaviour
    {
        public GameObject prefab;
        public int waveCount;
        public int perWave;
        public float interval;
        public float rad = 150;

        private WaitForSeconds _waitForSeconds;
        private List<EntityObj> _objs = new();
        
        // 存储所有物体的位置数据
        private NativeArray<float3> _positions;
        private NativeArray<bool> _shouldUpdate;
        private PositionUpdateJob _positionJob;
        private JobHandle _jobHandle;
        private JobHandle _applyPosHandle;

        private TransformAccessArray _transformAccessArray;
        public void Start()
        {
            int objectCount = waveCount * perWave;
            _transformAccessArray = new TransformAccessArray(objectCount);
            _positions = new NativeArray<float3>(objectCount, Allocator.Persistent);
            _shouldUpdate = new NativeArray<bool>(objectCount, Allocator.Persistent);
            StartCoroutine(Gen());
        }

        // 每帧调度Job
        private void FixedUpdate()
        {
            // 初始化Job参数
            _positionJob = new PositionUpdateJob
            {
                positions = _positions,
                randomSeed = (uint)Time.frameCount, // 用帧数作为随机种子
                radius = rad,
                shouldUpdate = _shouldUpdate,
                time = Time.realtimeSinceStartup
            };

            // 调度并行Job
            _jobHandle = _positionJob.Schedule(_positions.Length, 32);
            var job = new ApplyTransformJob()
            {
                shouldUpdate = _shouldUpdate,
                positions = _positions,
            };
            _applyPosHandle = job.Schedule(_transformAccessArray,_jobHandle);
            _applyPosHandle.Complete();

            int count = _objs.Count;
            for (int i = 0; i < _shouldUpdate.Length; i++)
            {
                if (_shouldUpdate[i] && i < count)
                {
                    _objs[i]?.SetPositionWithOutTransform(_objs[i].transform.position);
                }
            }
        }


        private void LateUpdate()
        {
            _applyPosHandle.Complete();
        }

        // 清理NativeArray
        private void OnDestroy()
        {
            _positions.Dispose();
            _shouldUpdate.Dispose();
            _transformAccessArray.Dispose();
        }

        IEnumerator Gen()
        {
            _waitForSeconds = new WaitForSeconds(interval);
            for (int i = 0; i < waveCount; i++)
            {
                for (int j = 0; j < perWave; j++)
                {
                    var obj = Instantiate(prefab);
                    var pos = UnityEngine.Random.insideUnitCircle * rad;
                    var com = obj.GetComponent<EntityObj>();
                    com.SetPosition(pos);
                    _objs.Add(com);
                    _transformAccessArray.Add(com.transform);
                }

                yield return _waitForSeconds;
            }
            
            
        }
        
        
    }
    
    
    // 定义并行Job
    [BurstCompile]
    public struct PositionUpdateJob : IJobParallelFor
    {
        public NativeArray<float3> positions;
        public NativeArray<bool> shouldUpdate; // 标记是否需要更新位置
        public uint randomSeed;
        public float radius;
        public float time;

        public void Execute(int index)
        {
            Random random = new Random(randomSeed + (uint)index * 2); // 确保每个索引独立随机

            int a = random.NextInt(0, 2);
            if (a == 1)
            {
                // 生成随机位置
                float3 pos2 = SimulateMacroRotation(positions[index],Vector3.zero,radius,10,1,1,ref time);
                //float3 pos2 = CalculateCircularPosition(positions[index], Vector3.zero, radius, 10, time);
                /*float2 pos = random.NextFloat2Direction() * radius * random.NextFloat();#1#
                positions[index] = pos2; //new float3(pos,0);
                shouldUpdate[index] = true;
            }
            else
            {
                shouldUpdate[index] = false;
            }
        }
        
        Vector3 SimulateMacroRotation(
            Vector3 currentPos,
            Vector3 center,
            float baseRadius,
            float resetRadius,
            float angleSpeed,
            float moveSpeed,
            ref float elapsedTime)
        {
            // 转换为极坐标
            Vector3 offset = currentPos - center;
            float currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            float currentRadius = offset.magnitude;

            // 动态参数计算
            float newAngle = currentAngle + angleSpeed * elapsedTime;
            float newRadius = Mathf.MoveTowards(currentRadius, 0, moveSpeed * 0.02f);

            // 重置机制（进入核心区后回到边缘）
            if (newRadius < resetRadius)
            {
                newRadius = baseRadius;
                newAngle = Random.CreateFromIndex(randomSeed).NextFloat(); // 随机新角度
                elapsedTime = 0; // 重置计时器
            }

            // 计算新坐标
            float radian = newAngle * Mathf.Deg2Rad;
            return new Vector3(
                center.x + newRadius * Mathf.Cos(radian),
                center.y + newRadius * Mathf.Sin(radian),
                currentPos.z
            );
        }
    }


    [BurstCompile]
    public struct ApplyTransformJob : IJobParallelForTransform
    {
        [ReadOnly] public NativeArray<bool> shouldUpdate;
        [ReadOnly] public NativeArray<float3> positions;
        
        public void Execute(int index, TransformAccess transform)
        {
            if (shouldUpdate[index] )
            {
                transform.localPosition = positions[index];
            }
            
        }
    }
}*/