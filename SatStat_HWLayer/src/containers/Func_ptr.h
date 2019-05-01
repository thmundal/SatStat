#pragma once

template<typename TRet, typename... TParams>
struct Func_ptr
{
	typedef TRet(*t_func)(TParams...);

	Func_ptr()
		: m_func(nullptr), m_set(false) {}

	Func_ptr(t_func func)
		: m_func(func), m_set(true) {}

	Func_ptr& operator=(t_func func)
	{
		m_func = func;
		m_set = true;
		return *this;
	}

	TRet operator()(TParams... args) { return m_func(args...); }

	bool is_set() { return m_set; }

private:
	t_func m_func;
	bool m_set;
};