#pragma once
#include "../containers/Json_container.h"
#include "../containers/Func_ptr.h"

/**
*	Function_control is a wrapper around a function.
*	A function can reserve Function_control, by passing itself in.
*	When reserved, no other function can enter until it's released.
*	This class is created to make protothreading of functions easier.
*/
class Function_control
{
public:
	/**
	*	Pure virtual destructor to make the class abstract.
	*/
	virtual ~Function_control() = 0;

	static bool is_available();
	static void run();

protected:
	static void reserve(const String& name, const Func_ptr<void>& func);
	static void release();

private:
	static String m_name;
	static bool m_available;
	static Func_ptr<void> m_func;
};

