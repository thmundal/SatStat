#pragma once

/**
*	Generic function pointer implementation.
*/
template<typename TRet, typename... TParams>
struct Func_ptr
{	
	typedef TRet(*t_func)(TParams...);

	/**
	*	Default constructor. Setting m_func to nullptr and m_set to false.
	*/
	Func_ptr()
		: m_func(nullptr), m_set(false) {}

	/**
	*	Constructor. Takes a raw function pointer, sets m_function to point to that function and sets m_set to true.
	*/
	Func_ptr(t_func func)
		: m_func(func), m_set(true) {}

	/**
	*	Copy constructor. Utilizing the assignment operator to perform a deep copy from other to this.
	*/
	Func_ptr(const Func_ptr& other)
	{
		*this = other;
	}

	/**
	*	Assignment operator overload. Takes a raw function pointer, sets m_function to point to that function and sets m_set to true.
	*	Also returns a reference to this to allow chaining.
	*/
	Func_ptr& operator=(t_func func)
	{
		m_func = func;
		m_set = true;
		return *this;
	}

	/**
	*	Assignment operator overload. Takes a Func_ptr and performs a deep copy from that object to this.
	*	Also returns a reference to this to allow chaining.
	*/
	Func_ptr& operator=(const Func_ptr& other)
	{
		m_func = other.m_func;
		m_set = other.m_set;
		return *this;
	}

	/**
	*	Function call operator overload. Allow direct call to the function.
	*/
	TRet operator()(TParams... args) { return m_func(args...); }

	/**
	*	Checks if the function pointer actually points to a function or not.
	*/
	bool is_set() { return m_set; }

private:
	t_func m_func;
	bool m_set;
};