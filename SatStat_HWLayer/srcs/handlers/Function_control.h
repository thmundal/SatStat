#pragma once

class Function_control
{
public:
	virtual ~Function_control() {};
	static bool is_available();
	static void run();
protected:
	static void reserve(void(*func)(void));
	static void release();
private:
	static bool m_available;
	static void(*m_func)(void);
};

