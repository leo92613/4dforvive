using UnityEngine;
using System.Collections;

namespace Holojam {
    public class WiiGlobalReceiver : MonoBehaviour {

        public WiiMoteModule module;

        protected virtual void OnEnable() {
            WiiMoteModule.receivers.Add(this);
        }

        protected virtual void OnDisable() {
            WiiMoteModule.receivers.Remove(this);
        }
    }
}

