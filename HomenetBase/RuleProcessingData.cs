using System;
using Abraham.DateAndTime;
using Homenet;

namespace HomenetBase
{
	public class RuleProcessingData
	{
		#region ------------- Properties ----------------------------------------------------------
		public DataObjectRules Rules           { get; private set; }
		public RuleTimer       IndividualTimer { get; private set; }
		public RuleTimer       AutomationTimer { get; private set; }
		#endregion



		#region ------------- Fields --------------------------------------------------------------
        // Wir müssen uns merken, welches DO uns ausgelöst hat,
        // weil das für die Verarbeitung der AutoAus-Regeln nötig ist.
        public DataObject Auslöser;

		private DataObject _Do;
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		public RuleProcessingData(DataObject Do)
		{
			_Do = Do;
			UpdateTimer();
			UpdateRules();
		}
		#endregion



		#region ------------- Methods -------------------------------------------------------------
		public string SerializeRules()
		{
			return Rules.Serialize();
		}

		public void UpdateTimer()
		{
			IndividualTimer = new RuleTimer();
			IndividualTimer.Deserialize(_Do.TimerSettings); 
			
			AutomationTimer = new RuleTimer();
			AutomationTimer.Deserialize(_Do.AutomationTimerSettings); 
		}
		#endregion



		#region ------------- Implementation ------------------------------------------------------
		private void UpdateRules()
		{
			Rules = new DataObjectRules();
			Rules.Deserialize(_Do.RuleSettings);
		}
		#endregion
	}
}
