using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.Utils;
using LSL;

public class LslTriggerManager: AStringOutlet
{
    [System.NonSerialized]
    private bool UniqueFromInstanceId;

    string _triggerQueued = null;

    public override List<string> ChannelNames {
        get {
                List<string> chanNames = new List<string>{ "Trigger" };
                return chanNames;
            }
    }


    // Start is called before the first frame update
    protected override void Start(){
        
        sample = new string[ChannelCount];

        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(moment.ToString());
        if (UniqueFromInstanceId)
            hash.Append(gameObject.GetInstanceID());
        ExtendHash(hash);

        double dataRate = LSL.LSL.IRREGULAR_RATE;
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, ChannelCount, dataRate, Format, hash.ToString());

        // Build XML header. See xdf wiki for recommendations: https://github.com/sccn/xdf/wiki/Meta-Data
        XMLElement acq_el = streamInfo.desc().append_child("acquisition");
        acq_el.append_child_value("manufacturer", "LSL4Unity");
        XMLElement channels = streamInfo.desc().append_child("channels");
        FillChannelsHeader(channels);

        base.outlet = new StreamOutlet(streamInfo);
    }

    protected override void Update() {
        if (_triggerQueued != null) {
            sendTrigger(_triggerQueued);
            _triggerQueued = null;
        }
        base.Update();

    }

    private void OnDisable() {
        if (outlet != null) {
            outlet.Dispose();
            outlet = null;
        }
    }


    public void PushTriggerAtEndOfFrame(string trigger) {
        sample[0] = trigger;
        StartCoroutine(pushSampleAtEndOfFrame(trigger));
    }

    IEnumerator pushSampleAtEndOfFrame(string trigger) {
        yield return new WaitForEndOfFrame();
        sendTrigger(trigger);
    }

    public void PushTriggerNow(string trigger) {
        sample[0] = trigger;
        pushSample();
    }

    void sendTrigger(string trigger) {
        sample[0] = trigger;
        pushSample();
    }

    public void PushTriggerNextFrame(string trigger) {
        _triggerQueued = trigger;
    }

    protected override bool BuildSample() {
        return false;
    }
}
