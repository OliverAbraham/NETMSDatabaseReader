using System;
using System.Collections.Generic;
using System.Text;

namespace HomenetBase
{
	/// <summary>
	/// Fehlercodes (Returncodes)
	/// </summary>
	public enum RC
	{
		OK = 0,
		SendenFehlgeschlagen,
		SendenFehlgeschlagenModusNurSenden,
		SendenFehlgeschlagenEventAck,
		SendenFehlgeschlagenNullObject,
		KeineAntwort,
		UngenügendeAntwort,
		FalscherParameter,
		UngültigerBefehl_Nack,
		Beschäftigt_Busy,
		NichtGefunden,
		ExceptionBeiCommandIntern
	}
}
