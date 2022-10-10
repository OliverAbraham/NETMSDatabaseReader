namespace HomenetBase
{
	public class ControllerCapabilities
	{
		public string Name { get; set; }
		public string Version { get; set; }
		public int Fid { get; set; }
		public HardwareDescriptor[] Inputs { get; set; }
		public HardwareDescriptor[] Outputs { get; set; }
	}
}
