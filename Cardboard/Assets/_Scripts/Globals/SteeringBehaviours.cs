using UnityEngine;
using System.Collections.Generic;

namespace AI
{
    public class BaseAgent : MonoBehaviour
    {
        [Header("Steering Settings")]
        public bool TwoD = true;
        public float maxForce = 1;
        public float maxSteer = 0.05f;
        public bool autoCalculate = true;

        [HideInInspector]
        public Vector3 velocity, desiredVector, steer, randomTarget;

        [HideInInspector]
        public float randomTimer = 100, wanderAngle = 0;

        public void addSeek(Vector3 target, float power = 1)
        {
            desiredVector += SteeringBehaviours.Seek(this, target) * power;
        }
        public void addFlee(Vector3 target, float range, float power = 1)
        {
            desiredVector += SteeringBehaviours.Flee(this, target, range) * power;
        }
        public void addRandom2D(Vector3 referencePosition, float changeTargetCD = .1f, float randomRadius = 4, float power = 1)
        {
            desiredVector += SteeringBehaviours.Random2D(this, referencePosition, changeTargetCD, randomRadius, power);
        }

        public void addSeparate(List<BaseAgent> agents, float range, float power = 1)
        {
            desiredVector += SteeringBehaviours.Separate(this, agents, range) * power;
        }

        public void addSeparate(List<Transform> transf, float range, float power = 1)
        {
            desiredVector += SteeringBehaviours.SeparateT(this, transf, range) * power;
        }

        public void addWander(float wanderDistance, float wanderRadius, float angleDelta, float maxAngle, float power = 1)
        {
            desiredVector += SteeringBehaviours.Wander2D(this, wanderDistance, wanderRadius, Mathf.Deg2Rad * angleDelta, Mathf.Deg2Rad * maxAngle) * power;
        }

        public void addCohesion(List<BaseAgent> agents, float range, float power = 1)
        {
            desiredVector += SteeringBehaviours.Cohesion(this, agents, range) * power;
        }

        public void addAlign(List<BaseAgent> agents, float range, float power = 1)
        {
            desiredVector += SteeringBehaviours.Align(this, agents, range) * power;
        }



        public void calculateMovement()
        {
            steer = Vector3.ClampMagnitude(desiredVector - velocity, maxSteer);
            desiredVector = Vector3.zero;

            velocity += steer;
            if (TwoD) velocity.z = 0;

            transform.position += velocity * Time.deltaTime;
        }

        void LateUpdate()
        {
            if (autoCalculate) calculateMovement();
        }

    }

    static public class SteeringBehaviours
    {
        static public Vector3 Seek(BaseAgent agent, Vector3 targetPosition, float arriveRange = 1)
        {
            Vector3 desiredVector = targetPosition - agent.transform.position;
            desiredVector = Arrive(desiredVector, arriveRange);
            desiredVector *= agent.maxForce;
            return desiredVector;
        }

        static Vector3 Arrive(Vector3 desiredVector, float arriveRange)
        {
            if (desiredVector.magnitude > arriveRange)
                return desiredVector.normalized;
            else return desiredVector;
        }

        static public Vector3 Flee(BaseAgent agent, Vector3 targetPosition, float range)
        {
            Vector3 desiredVector;
            if (Vector3.Distance(targetPosition, agent.transform.position) < range)
            {
                desiredVector = agent.transform.position - targetPosition;
                return desiredVector.normalized * agent.maxForce;
            }
            else
            {
                return Vector3.zero;
            }
        }


        static public Vector3 Random2D(BaseAgent agent, Vector3 referencePosition, float changeTargetCD, float randomRadius, float power)
        {

            agent.randomTimer += Time.deltaTime;
            if (agent.randomTimer > changeTargetCD)
            {
                agent.randomTarget = (Vector3)Random.insideUnitCircle * randomRadius + referencePosition;
                agent.randomTimer = 0;
            }

            return Seek(agent, agent.randomTarget, power);
        }

        static public Vector3 Separate(BaseAgent agent, List<BaseAgent> neighbors, float range)
        {
            Vector3 desiredVector = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i].gameObject != agent.gameObject)
                    desiredVector += Flee(agent, neighbors[i].transform.position, range);
            }
            if (desiredVector.magnitude > 0)
            {
                return desiredVector.normalized * agent.maxForce;
            }
            else
                return Vector3.zero;
        }

        static public Vector3 SeparateT(BaseAgent agent, List<Transform> transf, float range)
        {
            Vector3 desiredVector = Vector3.zero;
            for (int i = 0; i < transf.Count; i++)
            {
                if (transf[i].gameObject != agent.gameObject)
                    desiredVector += Flee(agent, transf[i].transform.position, range);
            }
            if (desiredVector.magnitude > 0)
            {
                return desiredVector.normalized * agent.maxForce;
            }
            else
                return Vector3.zero;
        }

        static public Vector3 Cohesion(BaseAgent agent, List<BaseAgent> neighbors, float range)
        {
            Vector3 averagePos = Vector3.zero;
            int count = 0;

            for (int i = 0; i < neighbors.Count; i++)
            {
                float dist = Vector3.Distance(agent.transform.position, neighbors[i].transform.position);
                if (dist > 0 && dist < range)
                {
                    averagePos += neighbors[i].transform.position;
                    count++;
                }
            }
            if (count > 0)
            {
                averagePos /= count;
                return Seek(agent, averagePos);
            }
            return Vector3.zero;
        }

        static public Vector3 Align(BaseAgent agent, List<BaseAgent> neighbors, float range)
        {
            Vector3 desiredVector = Vector3.zero;
            int count = 0;

            for (int i = 0; i < neighbors.Count; i++)
            {
                float dist = Vector3.Distance(agent.transform.position, neighbors[i].transform.position);
                if (dist > 0 && dist < range)
                {
                    desiredVector += neighbors[i].velocity;
                    count++;
                }
            }
            if (count > 0)
            {
                return desiredVector.normalized * agent.maxForce;
            }
            else
                return Vector3.zero;
        }

        static public Vector3 Wander2D(BaseAgent agent, float wanderDistance, float wanderRadius, float angleDelta, float maxAngle)
        {
            Vector3 wanderCenter = agent.velocity.normalized * wanderDistance /* + agent.transform.position*/;
            Vector3 desiredRef = agent.velocity.normalized * wanderRadius;
            Vector3 desiredVector = wanderCenter + rotateVector2D(desiredRef, Mathf.Clamp(agent.wanderAngle + Random.Range(-angleDelta, angleDelta), -maxAngle, maxAngle)) /* - agent.transform.position*/;
            return desiredVector.normalized * agent.maxForce;
        }

        static Vector3 rotateVector2D(Vector3 iniV, float angle)
        {
            Vector3 newV = Vector3.zero;
            newV.Set(iniV.x * Mathf.Cos(angle) - iniV.y * Mathf.Sin(angle),
                     iniV.x * Mathf.Sin(angle) + iniV.y * Mathf.Cos(angle),
                     iniV.z);
            return newV;
        }
    }


}
