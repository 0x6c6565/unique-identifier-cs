#if USING_NETCODE_FOR_GAMEOBJECTS
using Unity.Netcode;

namespace UniqueIdentifiers
{
	/// <summary>
	/// Netcode for GameObjects support for <see cref="Guid64"/>.
	///
	/// <see cref="INetworkSerializeByMemcpy"/> is a marker interface with no members: it tells
	///		Netcode that this type is an unmanaged, fixed-size struct whose bytes can be copied
	///		straight onto the wire. <see cref="Guid64"/> is eight bytes that all overlap
	///		<see cref="Guid64.value"/>, so that is the whole of it.
	///
	/// Implementing it makes <see cref="Guid64"/> valid as:
	///		- an RPC parameter, on its own or as a field of an INetworkSerializable struct,
	///		- a <see cref="NetworkVariable{T}"/> type,
	///		- an element of a NativeArray/NativeList sent through an RPC.
	///
	/// See the note in Guid128.Netcode.cs about byte order.
	/// </summary>
	public partial struct Guid64 : INetworkSerializeByMemcpy { }
}
#endif
