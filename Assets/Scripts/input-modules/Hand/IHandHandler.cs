using UnityEngine;
using UnityEngine.EventSystems;

namespace Holojam {
	public interface IHandHandler : IPointerHandHandler, IGlobalHandHandler { };

	public interface IGlobalHandHandler : IGlobalHandOnePoseHandler, IGlobalHandTwoPoseHandler, IGlobalHandThreePoseHandler, IGlobalHandFourPoseHandler,
								   IGlobalHandOpenPoseHandler, IGlobalHandClosedPoseHandler, IGlobalHandRockinPoseHandler { };

	public interface IGlobalHandOnePoseHandler : IGlobalHandOneDownHandler, IGlobalHandOneHandler, IGlobalHandOneUpHandler { };
	public interface IGlobalHandOneDownHandler : IEventSystemHandler { void OnGlobalHandOneDown(HandEventData eventData); }
	public interface IGlobalHandOneHandler : IEventSystemHandler { void OnGlobalHandOne(HandEventData eventData); }
	public interface IGlobalHandOneUpHandler : IEventSystemHandler { void OnGlobalHandOneUp(HandEventData eventData); }

	public interface IGlobalHandTwoPoseHandler : IGlobalHandTwoDownHandler, IGlobalHandTwoHandler, IGlobalHandTwoUpHandler { };
	public interface IGlobalHandTwoDownHandler : IEventSystemHandler { void OnGlobalHandTwoDown(HandEventData eventData); }
	public interface IGlobalHandTwoHandler : IEventSystemHandler { void OnGlobalHandTwo(HandEventData eventData); }
	public interface IGlobalHandTwoUpHandler : IEventSystemHandler { void OnGlobalHandTwoUp(HandEventData eventData); }

	public interface IGlobalHandThreePoseHandler : IGlobalHandThreeDownHandler, IGlobalHandThreeHandler, IGlobalHandThreeUpHandler { };
	public interface IGlobalHandThreeDownHandler : IEventSystemHandler { void OnGlobalHandThreeDown(HandEventData eventData); }
	public interface IGlobalHandThreeHandler : IEventSystemHandler { void OnGlobalHandThree(HandEventData eventData); }
	public interface IGlobalHandThreeUpHandler : IEventSystemHandler { void OnGlobalHandThreeUp(HandEventData eventData); }

	public interface IGlobalHandFourPoseHandler : IGlobalHandFourDownHandler, IGlobalHandFourHandler, IGlobalHandFourUpHandler { };
	public interface IGlobalHandFourDownHandler : IEventSystemHandler { void OnGlobalHandFourDown(HandEventData eventData); }
	public interface IGlobalHandFourHandler : IEventSystemHandler { void OnGlobalHandFour(HandEventData eventData); }
	public interface IGlobalHandFourUpHandler : IEventSystemHandler { void OnGlobalHandFourUp(HandEventData eventData); }

	public interface IGlobalHandOpenPoseHandler : IGlobalHandOpenDownHandler, IGlobalHandOpenHandler, IGlobalHandOpenUpHandler { };
	public interface IGlobalHandOpenDownHandler : IEventSystemHandler { void OnGlobalHandOpenDown(HandEventData eventData); }
	public interface IGlobalHandOpenHandler : IEventSystemHandler { void OnGlobalHandOpen(HandEventData eventData); }
	public interface IGlobalHandOpenUpHandler : IEventSystemHandler { void OnGlobalHandOpenUp(HandEventData eventData); }

	public interface IGlobalHandClosedPoseHandler : IGlobalHandClosedDownHandler, IGlobalHandClosedHandler, IGlobalHandClosedUpHandler { };
	public interface IGlobalHandClosedDownHandler : IEventSystemHandler { void OnGlobalHandClosedDown(HandEventData eventData); }
	public interface IGlobalHandClosedHandler : IEventSystemHandler { void OnGlobalHandClosed(HandEventData eventData); }
	public interface IGlobalHandClosedUpHandler : IEventSystemHandler { void OnGlobalHandClosedUp(HandEventData eventData); }

	public interface IGlobalHandRockinPoseHandler : IGlobalHandRockinDownHandler, IGlobalHandRockinHandler, IGlobalHandRockinUpHandler { };
	public interface IGlobalHandRockinDownHandler : IEventSystemHandler { void OnGlobalHandRockinDown(HandEventData eventData); }
	public interface IGlobalHandRockinHandler : IEventSystemHandler { void OnGlobalHandRockin(HandEventData eventData); }
	public interface IGlobalHandRockinUpHandler : IEventSystemHandler { void OnGlobalHandRockinUp(HandEventData eventData); }


	public interface IPointerHandHandler : IHandOnePoseHandler, IHandTwoPoseHandler, IHandThreePoseHandler, IHandFourPoseHandler,
							   IHandOpenPoseHandler, IHandClosedPoseHandler, IHandRockinPoseHandler { };

	public interface IHandOnePoseHandler : IHandOneDownHandler, IHandOneHandler, IHandOneUpHandler { };
	public interface IHandOneDownHandler : IEventSystemHandler { void OnHandOneDown(HandEventData eventData); }
	public interface IHandOneHandler : IEventSystemHandler { void OnHandOne(HandEventData eventData); }
	public interface IHandOneUpHandler : IEventSystemHandler { void OnHandOneUp(HandEventData eventData); }

	public interface IHandTwoPoseHandler : IHandTwoDownHandler, IHandTwoHandler, IHandTwoUpHandler { };
	public interface IHandTwoDownHandler : IEventSystemHandler { void OnHandTwoDown(HandEventData eventData); }
	public interface IHandTwoHandler : IEventSystemHandler { void OnHandTwo(HandEventData eventData); }
	public interface IHandTwoUpHandler : IEventSystemHandler { void OnHandTwoUp(HandEventData eventData); }

	public interface IHandThreePoseHandler : IHandThreeDownHandler, IHandThreeHandler, IHandThreeUpHandler { };
	public interface IHandThreeDownHandler : IEventSystemHandler { void OnHandThreeDown(HandEventData eventData); }
	public interface IHandThreeHandler : IEventSystemHandler { void OnHandThree(HandEventData eventData); }
	public interface IHandThreeUpHandler : IEventSystemHandler { void OnHandThreeUp(HandEventData eventData); }

	public interface IHandFourPoseHandler : IHandFourDownHandler, IHandFourHandler, IHandFourUpHandler { };
	public interface IHandFourDownHandler : IEventSystemHandler { void OnHandFourDown(HandEventData eventData); }
	public interface IHandFourHandler : IEventSystemHandler { void OnHandFour(HandEventData eventData); }
	public interface IHandFourUpHandler : IEventSystemHandler { void OnHandFourUp(HandEventData eventData); }

	public interface IHandOpenPoseHandler : IHandOpenDownHandler, IHandOpenHandler, IHandOpenUpHandler { };
	public interface IHandOpenDownHandler : IEventSystemHandler { void OnHandOpenDown(HandEventData eventData); }
	public interface IHandOpenHandler : IEventSystemHandler { void OnHandOpen(HandEventData eventData); }
	public interface IHandOpenUpHandler : IEventSystemHandler { void OnHandOpenUp(HandEventData eventData); }

	public interface IHandClosedPoseHandler : IHandClosedDownHandler, IHandClosedHandler, IHandClosedUpHandler { };
	public interface IHandClosedDownHandler : IEventSystemHandler { void OnHandClosedDown(HandEventData eventData); }
	public interface IHandClosedHandler : IEventSystemHandler { void OnHandClosed(HandEventData eventData); }
	public interface IHandClosedUpHandler : IEventSystemHandler { void OnHandClosedUp(HandEventData eventData); }

	public interface IHandRockinPoseHandler : IHandRockinDownHandler, IHandRockinHandler, IHandRockinUpHandler { };
	public interface IHandRockinDownHandler : IEventSystemHandler { void OnHandRockinDown(HandEventData eventData); }
	public interface IHandRockinHandler : IEventSystemHandler { void OnHandRockin(HandEventData eventData); }
	public interface IHandRockinUpHandler : IEventSystemHandler { void OnHandRockinUp(HandEventData eventData); }
}

