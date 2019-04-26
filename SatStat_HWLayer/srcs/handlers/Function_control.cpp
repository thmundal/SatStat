#include "Function_control.h"

bool Function_control::m_available = true;
void(*Function_control::m_func)(void);

bool Function_control::is_available()
{
	return m_available;
}

void Function_control::run()
{
	m_func();
}

void Function_control::reserve(void(*func)(void))
{
	m_func = func;
	m_available = false;
}

void Function_control::release()
{
	m_func = nullptr;
	m_available = true;
}
