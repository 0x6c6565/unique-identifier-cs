#if USING_NETCODE_FOR_GAMEOBJECTS
using Unity.Netcode;

namespace UniqueIdentifiers
{
	/// <summary>
	/// Netcode for GameObjects support for <see cref="Guid128"/>.
	///
	/// <see cref="INetworkSerializeByMemcpy"/> is a marker interface with no members: it tells
	///		Netcode that this type is an unmanaged, fixed-size struct whose bytes can be copied
	///		straight onto the wire. That is exactly what a guid is, so this costs the same 16
	///		bytes as writing each field by hand, with no per-field work.
	///
	/// Implementing it makes <see cref="Guid128"/> valid as:
	///		- an RPC parameter, on its own or as a field of an INetworkSerializable struct,
	///		- a <see cref="NetworkVariable{T}"/> type,
	///		- an element of a NativeArray/NativeList sent through an RPC.
	///
	/// Note: memcpy writes the struct's in-memory layout, so a big-endian peer talking to a
	///		little-endian one would disagree. Every platform Unity ships to is little-endian,
	///		and field-by-field serialization in Netcode does not byte-swap either, so this is
	///		not a practical difference. If you ever need byte-order independence, replace this
	///		with an explicit INetworkSerializable implementation that reverses on demand.
	/// </summary>
	public partial struct Guid128 : INetworkSerializeByMemcpy { }
}
#endif
