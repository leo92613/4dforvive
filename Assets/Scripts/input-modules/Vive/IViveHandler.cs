using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Holojam.IO {
	public interface IViveHandler : IPointerViveHandler, IGlobalViveHandler { }

	public interface IPointerViveHandler : IApplicationMenuHandler, IGripHandler, ITouchpadHandler, ITriggerHandler { }

	//APPLICATION MENU HANDLER
	public interface IApplicationMenuHandler : IApplicationMenuPressDownHandler, IApplicationMenuPressHandler, IApplicationMenuPressUpHandler { }

	public interface IApplicationMenuPressDownHandler : IEventSystemHandler {
		void OnApplicationMenuPressDown(ViveEventData eventData);
	}

	public interface IApplicationMenuPressHandler : IEventSystemHandler {
		void OnApplicationMenuPress(ViveEventData eventData);
	}

	public interface IApplicationMenuPressUpHandler : IEventSystemHandler {
		void OnApplicationMenuPressUp(ViveEventData eventData);
	}

	//GRIP HANDLER
	public interface IGripHandler : IGripPressDownHandler, IGripPressHandler, IGripPressUpHandler { }

	public interface IGripPressDownHandler : IEventSystemHandler {
		void OnGripPressDown(ViveEventData eventData);
	}

	public interface IGripPressHandler : IEventSystemHandler {
		void OnGripPress(ViveEventData eventData);
	}
	public interface IGripPressUpHandler : IEventSystemHandler {
		void OnGripPressUp(ViveEventData eventData);
	}

	//TOUCHPAD HANDLER
	public interface ITouchpadHandler : ITouchpadPressSetHandler, ITouchpadTouchSetHandler { }
	public interface ITouchpadPressSetHandler : ITouchpadPressDownHandler, ITouchpadPressHandler, ITouchpadPressUpHandler { }
	public interface ITouchpadTouchSetHandler : ITouchpadTouchDownHandler, ITouchpadTouchHandler, ITouchpadTouchUpHandler { }

	public interface ITouchpadPressDownHandler : IEventSystemHandler {
		void OnTouchpadPressDown(ViveEventData eventData);
	}

	public interface ITouchpadPressHandler : IEventSystemHandler {
		void OnTouchpadPress(ViveEventData eventData);
	}

	public interface ITouchpadPressUpHandler : IEventSystemHandler {
		void OnTouchpadPressUp(ViveEventData eventData);
	}

	public interface ITouchpadTouchDownHandler : IEventSystemHandler {
		void OnTouchpadTouchDown(ViveEventData eventData);
	}

	public interface ITouchpadTouchHandler : IEventSystemHandler {
		void OnTouchpadTouch(ViveEventData eventData);
	}

	public interface ITouchpadTouchUpHandler : IEventSystemHandler {
		void OnTouchpadTouchUp(ViveEventData eventData);
	}

	//TRIGGER HANDLER
	public interface ITriggerHandler : ITriggerPressSetHandler, ITriggerTouchSetHandler { }
	public interface ITriggerPressSetHandler : ITriggerPressDownHandler, ITriggerPressHandler, ITriggerPressUpHandler { }
	public interface ITriggerTouchSetHandler : ITriggerTouchDownHandler, ITriggerTouchHandler, ITriggerTouchUpHandler { }

	public interface ITriggerPressDownHandler : IEventSystemHandler {
		void OnTriggerPressDown(ViveEventData eventData);
	}

	public interface ITriggerPressHandler : IEventSystemHandler {
		void OnTriggerPress(ViveEventData eventData);
	}

	public interface ITriggerPressUpHandler : IEventSystemHandler {
		void OnTriggerPressUp(ViveEventData eventData);
	}

	public interface ITriggerTouchDownHandler : IEventSystemHandler {
		void OnTriggerTouchDown(ViveEventData eventData);
	}

	public interface ITriggerTouchHandler : IEventSystemHandler {
		void OnTriggerTouch(ViveEventData eventData);
	}

	public interface ITriggerTouchUpHandler : IEventSystemHandler {
		void OnTriggerTouchUp(ViveEventData eventData);
	}


	//GLOBAL VIVE HANDLER: ALL Global BUTTON SETS
	public interface IGlobalViveHandler : IGlobalGripHandler, IGlobalTriggerHandler, IGlobalApplicationMenuHandler, IGlobalTouchpadHandler { }

	/// GLOBAL GRIP HANDLER
	public interface IGlobalGripHandler : IGlobalGripPressDownHandler, IGlobalGripPressHandler, IGlobalGripPressUpHandler { }

	public interface IGlobalGripPressDownHandler : IEventSystemHandler {
		void OnGlobalGripPressDown(ViveEventData eventData);
	}

	public interface IGlobalGripPressHandler : IEventSystemHandler {
		void OnGlobalGripPress(ViveEventData eventData);
	}

	public interface IGlobalGripPressUpHandler : IEventSystemHandler {
		void OnGlobalGripPressUp(ViveEventData eventData);
	}


	//GLOBAL TRIGGER HANDLER
	public interface IGlobalTriggerHandler : IGlobalTriggerPressSetHandler, IGlobalTriggerTouchSetHandler { }
	public interface IGlobalTriggerPressSetHandler : IGlobalTriggerPressDownHandler, IGlobalTriggerPressHandler, IGlobalTriggerPressUpHandler { }
	public interface IGlobalTriggerTouchSetHandler : IGlobalTriggerTouchDownHandler, IGlobalTriggerTouchHandler, IGlobalTriggerTouchUpHandler { }

	public interface IGlobalTriggerPressDownHandler : IEventSystemHandler {
		void OnGlobalTriggerPressDown(ViveEventData eventData);
	}

	public interface IGlobalTriggerPressHandler : IEventSystemHandler {
		void OnGlobalTriggerPress(ViveEventData eventData);
	}

	public interface IGlobalTriggerPressUpHandler : IEventSystemHandler {
		void OnGlobalTriggerPressUp(ViveEventData eventData);
	}

	public interface IGlobalTriggerTouchDownHandler : IEventSystemHandler {
		void OnGlobalTriggerTouchDown(ViveEventData eventData);
	}

	public interface IGlobalTriggerTouchHandler : IEventSystemHandler {
		void OnGlobalTriggerTouch(ViveEventData eventData);
	}

	public interface IGlobalTriggerTouchUpHandler : IEventSystemHandler {
		void OnGlobalTriggerTouchUp(ViveEventData eventData);
	}

	//GLOBAL APPLICATION MENU
	public interface IGlobalApplicationMenuHandler : IGlobalApplicationMenuPressDownHandler, IGlobalApplicationMenuPressHandler, IGlobalApplicationMenuPressUpHandler { }

	public interface IGlobalApplicationMenuPressDownHandler : IEventSystemHandler {
		void OnGlobalApplicationMenuPressDown(ViveEventData eventData);
	}

	public interface IGlobalApplicationMenuPressHandler : IEventSystemHandler {
		void OnGlobalApplicationMenuPress(ViveEventData eventData);
	}

	public interface IGlobalApplicationMenuPressUpHandler : IEventSystemHandler {
		void OnGlobalApplicationMenuPressUp(ViveEventData eventData);
	}

	//GLOBAL TOUCHPAD 
	public interface IGlobalTouchpadHandler : IGlobalTouchpadPressSetHandler, IGlobalTouchpadTouchSetHandler { }

	public interface IGlobalTouchpadPressSetHandler : IGlobalTouchpadPressDownHandler, IGlobalTouchpadPressHandler, IGlobalTouchpadPressUpHandler { }
	public interface IGlobalTouchpadTouchSetHandler : IGlobalTouchpadTouchDownHandler, IGlobalTouchpadTouchHandler, IGlobalTouchpadTouchUpHandler { }

	public interface IGlobalTouchpadPressDownHandler : IEventSystemHandler {
		void OnGlobalTouchpadPressDown(ViveEventData eventData);
	}

	public interface IGlobalTouchpadPressHandler : IEventSystemHandler {
		void OnGlobalTouchpadPress(ViveEventData eventData);
	}

	public interface IGlobalTouchpadPressUpHandler : IEventSystemHandler {
		void OnGlobalTouchpadPressUp(ViveEventData eventData);
	}

	public interface IGlobalTouchpadTouchDownHandler : IEventSystemHandler {
		void OnGlobalTouchpadTouchDown(ViveEventData eventData);
	}

	public interface IGlobalTouchpadTouchHandler : IEventSystemHandler {
		void OnGlobalTouchpadTouch(ViveEventData eventData);
	}

	public interface IGlobalTouchpadTouchUpHandler : IEventSystemHandler {
		void OnGlobalTouchpadTouchUp(ViveEventData eventData);
	}
}
