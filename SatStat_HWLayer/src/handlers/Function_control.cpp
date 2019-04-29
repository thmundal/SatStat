#include "Function_control.h"

String Function_control::m_name;
bool Function_control::m_available = true;
void(*Function_control::m_func)(void);

bool Function_control::is_available()
{
	return m_available;
}

void Function_control::run()
{
	if (!m_available)
	{
		m_func();
	}
}

void Function_control::reserve(const String& name, void(*func)(void))
{
	m_name = name;
	m_func = func;
	m_available = false;
}

void Function_control::release()
{
	m_func = nullptr;
	m_available = true;

	Json_container<JsonObject> obj;
	obj->set("instruction_done", m_name);
	obj->printTo(Serial);
	Serial.print("\r\n");
}
