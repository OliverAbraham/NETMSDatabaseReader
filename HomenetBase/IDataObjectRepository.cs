using System.Collections.Generic;

namespace HomenetBase
{
    public delegate void OnEvent_Handler(Event e);
    public delegate void OnDeviceChanged_Handler (HNEventType e, Device item);
    public delegate bool CallControllerSetValue_Handler (DataObject Do, string value, ref string errorMessage);

    public interface IDataObjectRepository
    {
		#region Events
		OnEvent_Handler OnEvent { get; set; }
        CallControllerSetValue_Handler OnCallControllerSetValue { get; set; }
        #endregion

		#region Methods
		List<DataObject> GetAll();
        DataObject Get(byte ctrID, byte id);
        DataObject Get(string doName);
        DataObject TryGet(string doName);
        DataObject TryGet(int doid);
        void Change(DataObject Do);
        void Change(DataObject Do, double newValue, double originalValue = 0);
        void ChangeFromBitArray(byte CtrID, byte[] newDataObjects);
        void UpdateViaRule(string name, string value);
		void UpdateViaWebApi(DataObject existingDataObject, DataObject newValues);
        void UpdateViaWebApi_ValueOnly(string name, string value);
        DataObject AddIfNew(string doName);
        void Add(DataObject Do);
        void Delete(int doid);
        object GetAccessLock();
        void LoadAll();
        void SaveAll();
		void SetNetSimulatorIsActive(bool active);
		void SetControllerInitialisationIsActive(bool active);
		List<DataObject> GetOldRepository();
		#endregion
	}
}
