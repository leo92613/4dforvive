using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
namespace Holojam {
    public interface IWiiMoteHandler : IPointerWiiMoteHandler, IGlobalWiiMoteHandler { };

    public interface IPointerWiiMoteHandler : IWiiMoteAHandler, IWiiMoteBHandler, IWiiMoteLeftHandler, IWiiMoteRightHandler,
                                              IWiiMoteUpHandler, IWiiMoteDownHandler, IWiiMotePlusHandler, IWiiMoteMinusHandler,
                                              IWiiMoteHomeHandler, IWiiMoteOneHandler { };

    public interface IWiiMoteAHandler : IWiiMoteAPressDownHandler, IWiiMoteAPressHandler, IWiiMoteAPressUpHandler { }
    public interface IWiiMoteAPressDownHandler : IEventSystemHandler { void OnAPressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteAPressHandler : IEventSystemHandler { void OnAPress(WiiMoteEventData eventData); }
    public interface IWiiMoteAPressUpHandler : IEventSystemHandler { void OnAPressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteBHandler : IWiiMoteBPressDownHandler, IWiiMoteBPressHandler, IWiiMoteBPressUpHandler { }
    public interface IWiiMoteBPressDownHandler : IEventSystemHandler { void OnBPressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteBPressHandler : IEventSystemHandler { void OnBPress(WiiMoteEventData eventData); }
    public interface IWiiMoteBPressUpHandler : IEventSystemHandler { void OnBPressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteLeftHandler : IWiiMoteLeftPressDownHandler, IWiiMoteLeftPressHandler, IWiiMoteLeftPressUpHandler { }
    public interface IWiiMoteLeftPressDownHandler : IEventSystemHandler { void OnLeftPressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteLeftPressHandler : IEventSystemHandler { void OnLeftPress(WiiMoteEventData eventData); }
    public interface IWiiMoteLeftPressUpHandler : IEventSystemHandler { void OnLeftPressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteRightHandler : IWiiMoteRightPressDownHandler, IWiiMoteRightPressHandler, IWiiMoteRightPressUpHandler { }
    public interface IWiiMoteRightPressDownHandler : IEventSystemHandler { void OnRightPressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteRightPressHandler : IEventSystemHandler { void OnRightPress(WiiMoteEventData eventData); }
    public interface IWiiMoteRightPressUpHandler : IEventSystemHandler { void OnRightPressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteUpHandler : IWiiMoteUpPressDownHandler, IWiiMoteUpPressHandler, IWiiMoteUpPressUpHandler { }
    public interface IWiiMoteUpPressDownHandler : IEventSystemHandler { void OnUpPressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteUpPressHandler : IEventSystemHandler { void OnUpPress(WiiMoteEventData eventData); }
    public interface IWiiMoteUpPressUpHandler : IEventSystemHandler { void OnUpPressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteDownHandler : IWiiMoteDownPressDownHandler, IWiiMoteDownPressHandler, IWiiMoteDownPressUpHandler { }
    public interface IWiiMoteDownPressDownHandler : IEventSystemHandler { void OnDownPressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteDownPressHandler : IEventSystemHandler { void OnDownPress(WiiMoteEventData eventData); }
    public interface IWiiMoteDownPressUpHandler : IEventSystemHandler { void OnDownPressUp(WiiMoteEventData eventData); }

    public interface IWiiMotePlusHandler : IWiiMotePlusPressDownHandler, IWiiMotePlusPressHandler, IWiiMotePlusPressUpHandler { }
    public interface IWiiMotePlusPressDownHandler : IEventSystemHandler { void OnPlusPressDown(WiiMoteEventData eventData); }
    public interface IWiiMotePlusPressHandler : IEventSystemHandler { void OnPlusPress(WiiMoteEventData eventData); }
    public interface IWiiMotePlusPressUpHandler : IEventSystemHandler { void OnPlusPressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteMinusHandler : IWiiMoteMinusPressDownHandler, IWiiMoteMinusPressHandler, IWiiMoteMinusPressUpHandler { }
    public interface IWiiMoteMinusPressDownHandler : IEventSystemHandler { void OnMinusPressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteMinusPressHandler : IEventSystemHandler { void OnMinusPress(WiiMoteEventData eventData); }
    public interface IWiiMoteMinusPressUpHandler : IEventSystemHandler { void OnMinusPressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteHomeHandler : IWiiMoteHomePressDownHandler, IWiiMoteHomePressHandler, IWiiMoteHomePressUpHandler { }
    public interface IWiiMoteHomePressDownHandler : IEventSystemHandler { void OnHomePressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteHomePressHandler : IEventSystemHandler { void OnHomePress(WiiMoteEventData eventData); }
    public interface IWiiMoteHomePressUpHandler : IEventSystemHandler { void OnHomePressUp(WiiMoteEventData eventData); }

    public interface IWiiMoteOneHandler : IWiiMoteOnePressDownHandler, IWiiMoteOnePressHandler, IWiiMoteOnePressUpHandler { }
    public interface IWiiMoteOnePressDownHandler : IEventSystemHandler { void OnOnePressDown(WiiMoteEventData eventData); }
    public interface IWiiMoteOnePressHandler : IEventSystemHandler { void OnOnePress(WiiMoteEventData eventData); }
    public interface IWiiMoteOnePressUpHandler : IEventSystemHandler { void OnOnePressUp(WiiMoteEventData eventData); }


    public interface IGlobalWiiMoteHandler : IGlobalWiiMoteAHandler, IGlobalWiiMoteBHandler, IGlobalWiiMoteLeftHandler, IGlobalWiiMoteRightHandler,
                                              IGlobalWiiMoteUpHandler, IGlobalWiiMoteDownHandler, IGlobalWiiMotePlusHandler, IGlobalWiiMoteMinusHandler,
                                              IGlobalWiiMoteHomeHandler, IGlobalWiiMoteOneHandler { };

    public interface IGlobalWiiMoteAHandler : IGlobalWiiMoteAPressDownHandler, IGlobalWiiMoteAPressHandler, IGlobalWiiMoteAPressUpHandler { }
    public interface IGlobalWiiMoteAPressDownHandler : IEventSystemHandler { void OnGlobalAPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteAPressHandler : IEventSystemHandler { void OnGlobalAPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteAPressUpHandler : IEventSystemHandler { void OnGlobalAPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteBHandler : IGlobalWiiMoteBPressDownHandler, IGlobalWiiMoteBPressHandler, IGlobalWiiMoteBPressUpHandler { }
    public interface IGlobalWiiMoteBPressDownHandler : IEventSystemHandler { void OnGlobalBPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteBPressHandler : IEventSystemHandler { void OnGlobalBPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteBPressUpHandler : IEventSystemHandler { void OnGlobalBPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteLeftHandler : IGlobalWiiMoteLeftPressDownHandler, IGlobalWiiMoteLeftPressHandler, IGlobalWiiMoteLeftPressUpHandler { }
    public interface IGlobalWiiMoteLeftPressDownHandler : IEventSystemHandler { void OnGlobalLeftPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteLeftPressHandler : IEventSystemHandler { void OnGlobalLeftPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteLeftPressUpHandler : IEventSystemHandler { void OnGlobalLeftPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteRightHandler : IGlobalWiiMoteRightPressDownHandler, IGlobalWiiMoteRightPressHandler, IGlobalWiiMoteRightPressUpHandler { }
    public interface IGlobalWiiMoteRightPressDownHandler : IEventSystemHandler { void OnGlobalRightPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteRightPressHandler : IEventSystemHandler { void OnGlobalRightPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteRightPressUpHandler : IEventSystemHandler { void OnGlobalRightPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteUpHandler : IGlobalWiiMoteUpPressDownHandler, IGlobalWiiMoteUpPressHandler, IGlobalWiiMoteUpPressUpHandler { }
    public interface IGlobalWiiMoteUpPressDownHandler : IEventSystemHandler { void OnGlobalUpPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteUpPressHandler : IEventSystemHandler { void OnGlobalUpPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteUpPressUpHandler : IEventSystemHandler { void OnGlobalUpPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteDownHandler : IGlobalWiiMoteDownPressDownHandler, IGlobalWiiMoteDownPressHandler, IGlobalWiiMoteDownPressUpHandler { }
    public interface IGlobalWiiMoteDownPressDownHandler : IEventSystemHandler { void OnGlobalDownPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteDownPressHandler : IEventSystemHandler { void OnGlobalDownPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteDownPressUpHandler : IEventSystemHandler { void OnGlobalDownPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMotePlusHandler : IGlobalWiiMotePlusPressDownHandler, IGlobalWiiMotePlusPressHandler, IGlobalWiiMotePlusPressUpHandler { }
    public interface IGlobalWiiMotePlusPressDownHandler : IEventSystemHandler { void OnGlobalPlusPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMotePlusPressHandler : IEventSystemHandler { void OnGlobalPlusPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMotePlusPressUpHandler : IEventSystemHandler { void OnGlobalPlusPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteMinusHandler : IGlobalWiiMoteMinusPressDownHandler, IGlobalWiiMoteMinusPressHandler, IGlobalWiiMoteMinusPressUpHandler { }
    public interface IGlobalWiiMoteMinusPressDownHandler : IEventSystemHandler { void OnGlobalMinusPressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteMinusPressHandler : IEventSystemHandler { void OnGlobalMinusPress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteMinusPressUpHandler : IEventSystemHandler { void OnGlobalMinusPressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteHomeHandler : IGlobalWiiMoteHomePressDownHandler, IGlobalWiiMoteHomePressHandler, IGlobalWiiMoteHomePressUpHandler { }
    public interface IGlobalWiiMoteHomePressDownHandler : IEventSystemHandler { void OnGlobalHomePressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteHomePressHandler : IEventSystemHandler { void OnGlobalHomePress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteHomePressUpHandler : IEventSystemHandler { void OnGlobalHomePressUp(WiiMoteEventData eventData); }

    public interface IGlobalWiiMoteOneHandler : IGlobalWiiMoteOnePressDownHandler, IGlobalWiiMoteOnePressHandler, IGlobalWiiMoteOnePressUpHandler { }
    public interface IGlobalWiiMoteOnePressDownHandler : IEventSystemHandler { void OnGlobalOnePressDown(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteOnePressHandler : IEventSystemHandler { void OnGlobalOnePress(WiiMoteEventData eventData); }
    public interface IGlobalWiiMoteOnePressUpHandler : IEventSystemHandler { void OnGlobalOnePressUp(WiiMoteEventData eventData); }

}
