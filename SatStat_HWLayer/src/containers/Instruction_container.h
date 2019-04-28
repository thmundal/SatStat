#pragma once
#include "../../lib/LinkedList/LinkedList.h"
#include "SADM_functions.h"
#include "Subscriber_functions.h"

/*
*	Struct holding the identifier and type of a parameter.
*	Uf stands for unformatted, which indicates that the parameter is not JSON formatted.
*/
struct Uf_param
{
	/**
	*	Constructor setting identifier and type of the parameter.
	*/
	Uf_param(const String& identifier, const String& type)
		: identifier(identifier), type(type) {}

	String identifier;
	String type;
};

/**
*	Struct holding a function and it's signature.
*	Here the signature is JSON formatted.
*/
struct Instruction
{
	/**
	*	Constructor setting the signature and function pointer.
	*/
	Instruction(const Json_container<JsonObject>& signature, void(*func)(Json_container<JsonObject>&))
	{
		m_signature = signature;
		m_func = func;
	}

	/**
	*	Calls the the function the fcuntion pointer holds.
	*/
	void run(Json_container<JsonObject>& doc) { m_func(doc); }

	/**
	*	Returns the signature of the instruction as Json_container<JsonObject>.
	*/
	Json_container<JsonObject>& get_signature() { return m_signature; }

private:
	Json_container<JsonObject> m_signature;
	void(*m_func)(Json_container<JsonObject>&);
};

/**
*	Class containing a list of available instructions.
*/
class Instruction_container
{
public:
	/**
	*	Constructor appending every available instruction to the list.
	*/
	Instruction_container();	

	/**
	*	Returns a spesific instruction in the list.
	*/
	Instruction* get(const String& ins);

	/**
	*	Returns the entire list of available instructions.
	*/
	LinkedList<String, Instruction*>& get_available_instructions();
private:
	/**
	*	Returns JSON formatted signature of a function.
	*	Called in the constructor to when creating the Instruction object which will be added to the list.
	*	First argument has to be a string, and represents the name of the function.
	*	Consecutive arguments has to be of type Uf_param.
	*/
	template<typename TFirst, typename... TParams>
	Json_container<JsonObject> parse_signature(const String& ins_name, TFirst t, TParams... args) const;

	/**
	*	Called by parse_signature or itself when the parameter pack is not empty.	
	*/
	template<typename TFirst, typename... TParams>
	void format_params(JsonObject& dest, TFirst t, TParams... args) const;

	/**
	*	Base format method.
	*	Called by parse_signature or the other format_params method when the parameter pack is empty.
	*/
	template<typename TFirst>
	void format_params(JsonObject& dest, TFirst t) const;
	
	LinkedList<String, Instruction*> m_available_instructions;
};

// Initial
template<typename TFirst, typename ...TParams>
inline Json_container<JsonObject> Instruction_container::parse_signature(const String& ins_name, TFirst t, TParams ...args) const
{	
	Json_container<JsonObject> obj;
	JsonObject& nested = obj->createNestedObject(ins_name);	
	format_params(nested, t, args...);
	return obj;
}

// Consecutive
template<typename TFirst, typename... TParams>
inline void Instruction_container::format_params(JsonObject& dest, TFirst t, TParams... args) const
{
	dest.set(t.identifier, t.type);
	format_params(dest, args...);
}

// Last
template<typename TFirst>
inline void Instruction_container::format_params(JsonObject& dest, TFirst t) const
{		
	dest.set(t.identifier, t.type);
}
