#pragma once
#include "Request_container.h"

/**
*	Appends available requests to the correct list.
*/
Request_container::Request_container()
{
	append("subscribe", Request_functions::subscribe);
	append("unsubscribe", Request_functions::unsubscribe);
	append("reset", Request_functions::reset);
}

/**
*	Checks if the given key is present in either of the lists.
*/
bool Request_container::exists(const String& key)
{
	if (no_param.contains(key) || with_param.contains(key))
	{
		return true;
	}

	return false;
}

/**
*	Checks if the given key is present in the with_params list.
*	In other words, if it has parameters.
*/
bool Request_container::has_params(const String& key)
{
	return with_param.contains(key);
}

/**
*	Returns the given object from the no_param list.
*/
Func_ptr<void>& Request_container::get_no_param(const String& key)
{
	return no_param.get(key);
}

/**
*	Returns the given object from the with_param list.
*/
Func_ptr<void, Json_container<JsonObject>&>& Request_container::get_with_param(const String& key)
{
	return with_param.get(key);
}

/**
*	Appends the given Func_ptr to the no_param list.
*/
void Request_container::append(const String& key, const Func_ptr<void>& func)
{
	no_param.append(key, func);
}

/**
*	Appends the given Func_ptr to the with_param list.
*/
void Request_container::append(const String& key, const Func_ptr<void, Json_container<JsonObject>&>& func)
{
	with_param.append(key, func);
}