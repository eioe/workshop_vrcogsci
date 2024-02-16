using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using eDIA;
using UXF;
using Unity.XR.CoreUtils;

namespace eDia {
    public class TaskPosner : EBlock {

        public GameObject CueFixManager;
        public GameObject TargetManager;
        public GameObject VrCam;
        public GameObject StimulusRig;
        public GameObject GazeVisualizer;

        private CueFixManager _cueFixManager;
        private TargetManager _targetManager;
        private LslTriggerManager _lslTriggerManager;
        string _curTargetDir;
        string _curCueDir;
        string _curCueCond;
        int _curTargetObjIdx;
        float _curEcc;
        Ray _gazeRay;

        IDisposable _ButtonPressEventListener;

        void Awake() {
            trialSteps.Add(TrialStep0);
            trialSteps.Add(TrialStep1);
            trialSteps.Add(TrialStep2);
            trialSteps.Add(TrialStep3);
            trialSteps.Add(TrialStep4);
            trialSteps.Add(TrialStep5);
            trialSteps.Add(TrialStep6);

            // TODO: Show fixation cross
            if (!CueFixManager.activeSelf) {
                CueFixManager.SetActive(false);
            }
            _cueFixManager = CueFixManager.GetComponent<CueFixManager>();
            _cueFixManager.ShowFixationCross(true);

            _targetManager = TargetManager.GetComponent<TargetManager>();
            _lslTriggerManager = GetComponent<LslTriggerManager>();
        }


        void Start() {
			
        }


        private void Update() {
            //Vector3 targetPos = _targetManager.GetComponent<TargetManager>().GetTargetPosition("right");
            //Vector3 targPosLocal = VrCam.transform.InverseTransformPoint(targetPos);
            //Vector3 fixCrossPosLocal = VrCam.transform.InverseTransformPoint(_cueFixManager.FixationCross.transform.position);
            //float ang = Vector3.Angle(targPosLocal, fixCrossPosLocal);
            //Debug.Log($"Angle: {ang}");
        }


        private void OnEnable() {

        }


        private void OnDisable() {
            if (_ButtonPressEventListener != null) {
                _ButtonPressEventListener.Dispose();
            }
        }

        private void OnDestroy() {
            if (_ButtonPressEventListener != null) {
                _ButtonPressEventListener.Dispose();
            }
        }


        public void TrialStep0() {

            // Load trial settings
            _curCueDir = Session.instance.CurrentTrial.settings.GetString("cueDir");
            _curTargetDir = Session.instance.CurrentTrial.settings.GetString("targetDir");
            _curCueCond = Session.instance.CurrentTrial.settings.GetString("cueCond");
            _curEcc = Session.instance.CurrentTrial.settings.GetFloat("ecc");
            _curTargetObjIdx = Session.instance.CurrentTrial.settings.GetInt("targetObjIdx");
            _targetManager.GetComponent<TargetManager>().SetObject(_curTargetObjIdx, "target", _curTargetDir);

            // Start Trial only if participant is fixating on the fixation cross (important for placement of stimuli in next step)
            if (!_cueFixManager.FixationCross.activeSelf) {
                _cueFixManager.ShowFixationCross(true);
            }
            StartCoroutine(WaitForTargetHit("Fix"));

            //TODO: check that head is in right pos and orientation
        }


        public void TrialStep1() {

            _lslTriggerManager.PushTriggerAtEndOfFrame("trial_start");

            Vector3 targetPos = _targetManager.GetComponent<TargetManager>().GetTargetPosition(_curTargetDir);
            Vector3 fixCrossPos = _cueFixManager.FixationCross.transform.position;

            float xOff = 0f;
            foreach (string side in new string[] { "left", "right" }) {
                if (side == "left") {
                    setStimulusEccentricity(_curEcc, fixCrossPos, targetPos, side, out xOff);
                } else {
                    setStimulusEccentricity(-xOff, side, fixCrossPos);
                }
                targetPos = _targetManager.GetComponent<TargetManager>().GetTargetPosition(side);
                float xOffset = Mathf.Abs((targetPos.x - fixCrossPos.x)) / 2f;
                Vector3 fixCrossPosN = _cueFixManager.FixationCross.transform.position;
                Vector3 targetPosN = _targetManager.GetComponent<TargetManager>().GetTargetPosition(side);
                // Debug.Log($"Angle ({side}): {Vector3.Angle(targetPosN - GazeTracker.transform.position, fixCrossPosN - GazeTracker.transform.position)}");
            }

            // Show fixation cross for 1 second before we proceed
            Experiment.Instance.ProceedWithDelay(.4f);
        }


        void TrialStep2() {
            // Show cue (ext or endo) for 0.2 second

            if (_curCueCond == "endo") {
                _cueFixManager.SetCueDir(_curCueDir);
                _cueFixManager.ShowFixationCross(false);
                _cueFixManager.ShowCue(true);
            } else {
                _targetManager.ToggleHighlightFrame(_curCueDir);
            }
            
            _lslTriggerManager.PushTriggerAtEndOfFrame($"cue_onset_{_curCueCond.ToLower()}_{_curCueDir.ToLower()}");

            float cueDur = Session.instance.CurrentTrial.settings.GetFloat("cueDur");
            Experiment.Instance.ProceedWithDelay(cueDur);
            return;
        }


        void TrialStep3() {
            // Hide cue and wait for XX secs
            if (_curCueCond == "endo") {
                _cueFixManager.ShowCue(false);
                _cueFixManager.ShowFixationCross(true);
            } else {
                _targetManager.ToggleHighlightFrame(_curCueDir);
            }
            _lslTriggerManager.PushTriggerAtEndOfFrame($"cue_offset");
            float isiDur = Session.instance.CurrentTrial.settings.GetFloat("isiDur");
            Experiment.Instance.ProceedWithDelay(isiDur);
            return;
        }


        void TrialStep4() {
            // Show target 
            // Wait for response of participant

            _targetManager.GetComponent<TargetManager>().ShowObject(true, "target", _curTargetDir);
            _cueFixManager.ShowFixationCross(false);

            Vector3 targetPos = _targetManager.GetComponent<TargetManager>().GetTargetPosition(_curTargetDir);
            _lslTriggerManager.PushTriggerAtEndOfFrame($"stimulus_onset_{_curTargetDir}_{_curEcc}");
            _lslTriggerManager.PushTriggerAtEndOfFrame($"target_pos_x-{targetPos.x}_y-{targetPos.y}_z-{targetPos.z}");

            // ProceedOnInput();

            // Experiment.Instance.ProceedWithDelay(1.0f);
            // Experiment.Instance.WaitOnProceed();
            // Experiment.Instance.Proceed();
            
            StartCoroutine(WaitForTargetHit("Target"));
            return;
        }


        void TrialStep5() {
            _lslTriggerManager.PushTriggerAtEndOfFrame("target_hit");
            // Wait 0.5s before we remove the target and proceed
            Experiment.Instance.ProceedWithDelay(0.5f);
            return;
        }


        void TrialStep6() {
            // On response
            // hide the target
            // wait for 1 sec
            // start next trial

            _lslTriggerManager.PushTriggerAtEndOfFrame("trial_end");
            
            _targetManager.GetComponent<TargetManager>().ShowObject(false, "target", _curTargetDir);
            
            // Show fixation cross which also makes it detectable by the gaze raycast
            if (!_cueFixManager.FixationCross.activeSelf) {
                _cueFixManager.ShowFixationCross(true);
            }
            Experiment.Instance.ProceedWithDelay(1.0f);
            return;
        }

        string ProceedOnInput() {
            var response = "";
            _ButtonPressEventListener = InputSystem.onAnyButtonPress.CallOnce(
                (ctrl) => {
                    InterpretInputNew(ctrl, out response, CheckLogResultsProceed);
                }
            );

            return response;
        }



        // vvvvvvvvvvvvvvvvvvvvvv  HELPERS  vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv

        // raycast till target hit
        IEnumerator WaitForTargetHit(string trigger) {
            while (true) {
                yield return new WaitForEndOfFrame();
                _gazeRay = new Ray(GazeVisualizer.transform.position, GazeVisualizer.transform.forward);
                var hit = new RaycastHit();
                if (Physics.Raycast(_gazeRay, out hit, 100.0f)) {
                    // check if the object is a target
                    if (hit.transform.gameObject.name.Contains(trigger)) {
                        Debug.Log($"{trigger} hit!");
                        Experiment.Instance.WaitOnProceed();
                        Experiment.Instance.Proceed();
                        break;
                    }
                }
                yield return null;
            }
            yield return null;
        }


        void CheckLogResultsProceed(string response) {

            _ButtonPressEventListener.Dispose();

            if (response == _curTargetDir) {
                Debug.Log("Correct");
            } else {
                Debug.Log($"Incorrect --- Correct answer: {_curTargetDir} --- Your answer: {response}");
            }

            // Log results
			Experiment.Instance.AddToTrialResults("response", response);
            Experiment.Instance.AddToTrialResults("response_correct", (response == _curTargetDir).ToString());

            // Proceed
            Experiment.Instance.WaitOnProceed();
			Experiment.Instance.Proceed();

            return;
        }


        void InterpretInputNew(InputControl control, out string response, Action<string> callback) {
            var input = control.displayName.ToLower();
            switch (input) {
                case "nach-rechts":
                    response = "right";
                    break;
                case "nach-links":
                    response = "left";
                    break;
                default:
                    response = "";
                    ProceedOnInput();
                    break;
            }
            if (response != "") {
                callback(response);
            }

        }



        float getTargetPosXLocal(Vector3 posFix, float w_y, float w_z, float eccDeg, string side, bool pickBestAbsFit = true) {
            float v = posFix.x;
            float a = posFix.y * w_y;
            float b = posFix.z * w_z;
            float c = posFix.magnitude * posFix.magnitude;
            float d = w_y * w_y;
            float e = w_z * w_z;
            float f = Mathf.Pow(Mathf.Cos(eccDeg * Mathf.Deg2Rad), 2);

            // Solution from https://www.symbolab.com/solver/equation-calculator/
            // prompt: "  solve\:for\:w,\:\left(v\cdot \:\:w\:+\:a\:+\:b\right)^2\:-\:c\:\cdot \left(w^2\:+\:d\:+\:e\right)\:\cdot \:\:f\:=\:0  "
            // substitutions see above; this is derived from the formula for calculating the angle between two 3D vectors (cos(theta) = (v . w) / (|v| * |w|))
            // full link: https://www.symbolab.com/solver/equation-calculator/solve%20for%20w%2C%20%5Cleft(v%5Ccdot%20%20w%20%2B%20a%20%2B%20b%5Cright)%5E%7B2%7D%20-%20c%20%5Ccdot%5Cleft(w%5E%7B2%7D%20%2B%20d%20%2B%20e%5Cright)%20%5Ccdot%20%20f%20%3D%200?or=input

            float solution_1 = (-a * v - b * v + Mathf.Sqrt(c * f * (d * v * v + e * v * v + a * a + 2 * a * b + b * b - c * d * f - e * c * f))) / (v * v - c * f);
            float solution_2 = (-a * v - b * v - Mathf.Sqrt(c * f * (d * v * v + e * v * v + a * a + 2 * a * b + b * b - c * d * f - e * c * f))) / (v * v - c * f);
            // Debug.Log($"Solution 1: {solution_1} --- Solution 2: {solution_2}");

            float w_x;

            if (pickBestAbsFit) {
                float angError(float solution) => (getAngle3d(GazeVisualizer.transform.InverseTransformPoint(new Vector3(solution, w_y, w_z)), posFix) - eccDeg * Mathf.Deg2Rad);
                w_x = Mathf.Abs((angError(solution_1) < angError(solution_2)) ? solution_1 : solution_2);
                w_x = (side == "left") ? -w_x : w_x;

            } else {
                if (side == "left") {
                    w_x = solution_1 < 0f ? solution_1 : solution_2;
                } else {
                    w_x = solution_1 < 0f ? solution_2 : solution_1;
                }
            }

            if (float.IsNaN(w_x)) {
                w_x = Mathf.Sin(eccDeg * Mathf.Deg2Rad) * posFix.magnitude;
                Debug.LogWarning("NaN solution, using sin(ecc) * |posFix| for placement.");
            }

            return w_x;
        }


        void setStimulusEccentricity(float ecc, Vector3 fixCrossPosWorld, Vector3 targetPosWorld, string side, out float xOffset) {
            Vector3 targetPosLocal = GazeVisualizer.transform.InverseTransformPoint(targetPosWorld);
            Vector3 fixCrossPosLocal = GazeVisualizer.transform.InverseTransformPoint(fixCrossPosWorld);
            float w_x = getTargetPosXLocal(fixCrossPosLocal, targetPosLocal.y, targetPosLocal.z, ecc, side);
            //Change to world coordinates
            Vector3 targetPosWorldNew = GazeVisualizer.transform.TransformPoint(new Vector3(w_x, targetPosLocal.y, targetPosLocal.z));

            // Move the whole stimulus center (incl machine):
            string stimIdentifier = $"StimulusMachine{char.ToUpper(side[0])}{side[1..]}"; // GO naming is "CenterRight" or "CenterLeft"
            GameObject stimulusCenter = StimulusRig.GetNamedChild(stimIdentifier);
            Vector3 stimCentPosWorld = stimulusCenter.transform.position;
            stimulusCenter.transform.position = new Vector3(targetPosWorldNew.x, stimCentPosWorld.y, stimCentPosWorld.z);

            xOffset = targetPosWorldNew.x - fixCrossPosWorld.x;

            return;
        }


        void setStimulusEccentricity(float xOff, string side, Vector3 fixCrossPosWorld) {
            string stimIdentifier = $"StimulusMachine{char.ToUpper(side[0])}{side[1..]}"; // GO naming is "CenterRight" or "CenterLeft"
            // Move the whole stimulus center (incl machine):
            GameObject stimulusCenter = StimulusRig.GetNamedChild(stimIdentifier);
            Vector3 stimCentPosWorld = stimulusCenter.transform.position;
            stimulusCenter.transform.position = new Vector3(fixCrossPosWorld.x + xOff, stimCentPosWorld.y, stimCentPosWorld.z);
            return;
        }


        float getAngle3d(Vector3 vec1, Vector3 vec2) {
            float cos = Vector3.Dot(vec1, vec2) / (vec1.magnitude * vec2.magnitude);
            return Mathf.Acos(cos);
        }


    }

}


