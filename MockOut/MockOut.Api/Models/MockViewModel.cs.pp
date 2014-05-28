using System.Runtime.Serialization;

namespace $rootnamespace$.Models
{
	[DataContract(Namespace="$rootnamespace$")]
	[KnownType(typeof(MockViewModel))]
    public class MockViewModel
    {
		[DataMember(Order = 1)]
        public string Value { get; set; }
    }
}
