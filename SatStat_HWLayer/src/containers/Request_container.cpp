#pragma once
#include "Request_container.h"

Request_container::Request_container()
{
	append("subscribe", Request_functions::subscribe);
	append("unsubscribe", Request_functions::unsubscribe);
	append("reset", Request_functions::reset);
}

bool Request_container::exists(const String& key)
{
	if (no_param.contains(key) || with_param.contains(key))
	{
		return true;
	}

	return false;
}

bool Request_container::has_params(const String& key)
{
	return with_param.contains(key);
}

Func_ptr<void>& Request_container::get_no_param(const String& key)
{
	return no_param.get(key);
}

Func_ptr<void, Json_container<JsonObject>&>& Request_container::get_with_param(const String& key)
{
	return with_param.get(key);
}

void Request_container::append(const String& key, Func_ptr<void> func)
{
	no_param.append(key, func);
}

void Request_container::append(const String& key, Func_ptr<void, Json_container<JsonObject>&> func)
{
	with_param.append(key, func);
}