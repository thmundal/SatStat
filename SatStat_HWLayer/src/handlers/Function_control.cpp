#include "Function_control.h"

// Declaration of static members
String Function_control::m_name;
bool Function_control::m_available = true;
Func_ptr<void> Function_control::m_func;

/**
*	Checks if Function_control is currently available.
*/
bool Function_control::is_available()
{
	return m_available;
}

/**
*	Executes the function currently reserving Function_control.
*/
void Function_control::run()
{
	if (!m_available)
	{
		m_func();
	}
}

/**
*	Reserve Function_control with the given function.
*	Function_control will become unavailable.
*/
void Function_control::reserve(const String& name, const Func_ptr<void>& func)
{
	m_func = func;
	m_name = name;
	m_available = false;
}

/**
*	Releases the function currently reserving Function_control.
*	Function_control will become available.
*/
void Function_control::release()
{
	m_func = nullptr;
	m_available = true;

	Json_container<JsonObject> obj;
	obj->set("instruction_done", m_name);
	obj->printTo(Serial);
	Serial.print("\r\n");
}
