using System.Collections.Generic;

namespace HomenetBase
{
    public delegate RC SendPacketHandler_Type(byte controller, byte[] daten, bool waitForReply, out byte[] output);
    public delegate bool SendEthernetPacketHandler_Type(string ipaddress, string daten, string method, out string antwort);

    public interface ITreiber
    {
        void SetSendCommand(SendPacketHandler_Type sendCommand);
		void SetSendEthernetCommand(SendEthernetPacketHandler_Type sendCommand);

        RC Reset();
        RC GetName(out string Name);
        RC GetFids(out List<int> Fids);
        bool Statusabfrage();
        RC GetAllValues(out byte[] Rückgabe);
        RC GetSensorCount(Fid fid, out uint RückgabeAnzahlSensoren);
        RC GetTypeAndValue(Fid fid, out string rückgabe);
        RC GetValue(Fid fid, byte index, out string rückgabe);
        RC GetEepromContents(uint address, uint count, out string Name);
        RC GetEepromContents(uint address, uint count, out byte[] eepromInhalt);
        RC GetEepromContents_One_Chunk(uint address, uint count, out byte[] eepromInhalt);
        RC SendResetCommand(Fid fid, out byte[] Rückgabe);
        RC BetriebsparameterHolen();
        RC EreignisVerarbeiten(IDataObjectRepository dovw, byte ctrID, byte[] rohdaten, byte[] nutzdaten);
        void MerkeLetzteReaktion();
        void SetValue(byte schalterID, string wert);
	}
}
