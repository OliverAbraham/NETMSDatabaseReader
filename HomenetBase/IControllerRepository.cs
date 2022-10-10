using System;
using System.Collections.Generic;
using System.Text;

namespace HomenetBase
{
	public interface IControllerRepository
	{
		#region Events
        #endregion

		#region Methods
		ControllerFE GetById(byte id);
		ControllerFE Add(byte id, string name, List<int> fids, 
                                          string ipaddress = "", string capabilities = "");
		void Remove(byte id);
		void Save();
		IEnumerable<ControllerFE> GetAll();
		IEnumerable<ControllerFE> GetAllEthernetControllers();
		ControllerFE GetByIpAddress(string ip);
		int GetCount();
		void UpdateStatistics(byte ctrID, string errorMessage);
		void SetName(ControllerFE device, string v);
		void SetState(ControllerFE device, bool v);
		#endregion
	}
}
