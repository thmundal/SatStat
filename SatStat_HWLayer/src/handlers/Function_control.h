#pragma once
#include "../containers/Json_container.h"

class Function_control
{
public:
	virtual ~Function_control() {};
	static bool is_available();
	static void run();

protected:
	static void reserve(const String& name, void(*func)(void));
	static void release();

private:
	static String m_name;
	static bool m_available;
	static void(*m_func)(void);
};

