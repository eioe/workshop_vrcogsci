using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using LSL4Unity.Utils;

namespace LSL4Unity.Samples.Complex
{

    public class PoseOutletLocal : AFloatOutlet
    {
        public PoseFormatLocal PoseFormat = PoseFormatLocal.PosQuat7D;

        public enum PoseFormatLocal { PosEul6D, PosQuat7D }

        public void Reset()
        {
            StreamName = "Unity.Pose";
            StreamType = "Unity.Transform";
            moment = MomentForSampling.FixedUpdate;
        }

        public override List<string> ChannelNames
        {
            get
            {
                List<string> chanNames = new List<string>();
                if ((PoseFormat == PoseFormatLocal.PosEul6D) || (PoseFormat == PoseFormatLocal.PosQuat7D))
                {
                    chanNames.AddRange(new string[] { "PosX", "PosY", "PosZ" });

                    if (PoseFormat == PoseFormatLocal.PosEul6D)
                    {
                        chanNames.AddRange(new string[] { "Pitch", "Yaw", "Roll" });
                    }
                    else
                    {
                        chanNames.AddRange(new string[] { "RotX", "RotY", "RotZ", "RotW" });
                    }
                }
                return chanNames;
            }
        }

        protected override void ExtendHash(Hash128 hash)
        {
            hash.Append(PoseFormat.ToString());
        }

        protected override bool BuildSample()
        {
            if ((PoseFormat == PoseFormatLocal.PosEul6D) || (PoseFormat == PoseFormatLocal.PosQuat7D)) {
                var position = gameObject.transform.localPosition;
                sample[0] = position.x;
                sample[1] = position.y;
                sample[2] = position.z;
                
                if (PoseFormat == PoseFormatLocal.PosEul6D)
                {
                    var rotation = gameObject.transform.localEulerAngles;
                    sample[3] = rotation.x;
                    sample[4] = rotation.y;
                    sample[5] = rotation.z;
                }
                else
                {
                    var rotation = gameObject.transform.localRotation;
                    sample[3] = rotation.x;
                    sample[4] = rotation.y;
                    sample[5] = rotation.z;
                    sample[6] = rotation.w;
                }
            }
            return true;
        }
    }
}